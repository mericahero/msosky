using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using PanGu;
using PanGu.HighLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using COM.CF;
using Lucene.Net.Analysis.Tokenattributes;
using Utility;

namespace MSOSKY.BL
{


	public class ATS
	{
        /// <summary>
        /// 索引文件存放路径
        /// </summary>
		private static string INDEX_STORE_PATH;
        /// <summary>
        /// 索引路径
        /// </summary>
		private static string INDEX_PATH;
        /// <summary>
        /// 一次添加或更新索引的数量
        /// </summary>
		private static int INDEX_SYNC_CAPACITY;

        private static Lucene.Net.Util.Version Version;

        private const int PageSize=15;

		static ATS()
		{
			INDEX_STORE_PATH = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\index");
			INDEX_PATH = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\doc");
            INDEX_SYNC_CAPACITY = 5000000; //PubFunc.GetInt(CWS.CWConfig.Appset["MSOIndexCapcity"]);
            Version = Lucene.Net.Util.Version.LUCENE_30;
		}

		public ATS()
		{
		}
        /// <summary>
        /// 创建或更新索引
        /// </summary>
        /// <param name="absoluteCreate">是否强制重建索引文件</param>
        /// <param name="msg">提示信息，out参数</param>
        /// <returns>返回操作是否成功</returns>
		public static bool CreateOrUpdateIndex(bool absoluteCreate, out string msg)
		{
			bool flag;
			msg = "";
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(ATS.INDEX_STORE_PATH), new NativeFSLockFactory());
			bool isExist = IndexReader.IndexExists(directory);
			if (isExist)
			{
				if (IndexWriter.IsLocked(directory))
				{
					IndexWriter.Unlock(directory);
				}
			}
			IndexWriter writer = null;
			try
			{
				try
				{
					writer = new IndexWriter(directory, new PanGuAnalyzer(),absoluteCreate || !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
					DataRow curIndexRow = MSODB.oDB.GetSQLSingleRow("select top 1 * from IndexSync where imodify=0 order by id desc");
					int curhashId = (curIndexRow == null ? 0 : PubFunc.GetInt(curIndexRow["hashId"]));
                    DataTable dt = MSODB.oDB.GetSQLTab(string.Format("select top {0} * from dht_sum where id>{1}", ATS.INDEX_SYNC_CAPACITY, curhashId));
					if ((dt == null || dt.Rows == null ? false : dt.Rows.Count > 0))
					{
						foreach (DataRow sRow in dt.Rows)
						{
							Document document = new Document();
							document.Add(new Field("hashId", sRow["id"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("keyContent", sRow["keyContent"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
							document.Add(new Field("keyWords", sRow["keyWords"].ToString(), Field.Store.YES, Field.Index.ANALYZED));
							document.Add(new Field("recvTime", sRow["recvTime"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("updateTime", sRow["updateTime"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("totalsize", sRow["totalsize"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("hashKey", sRow["hashKey"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
							document.Add(new Field("fileCnt", sRow["fileCnt"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("type", sRow["type"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							document.Add(new Field("iCOrE", sRow["iCOrE"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
							writer.AddDocument(document);
						}
						writer.Optimize();
						MSODB.oDB.ExecuteNonQuery(string.Format("insert into IndexSync(hashId) values({0})", (
							from x in (IEnumerable<DataRow>)dt.Select()
							select PubFunc.GetBigInt(x["id"])).Max()));
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception exception)
				{
					Exception e = exception;
					msg = string.Concat(e.Message, "<br /> ", e.StackTrace);
					flag = false;
				}
			}
			finally
			{
				if (writer != null)
				{
					writer.Dispose();
				}
				if (directory != null)
				{
					directory.Dispose();
				}
			}
			return flag;
		}

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="msg">执行结果</param>
        /// <returns>返回是否成功</returns>
		public static bool DelIndex(out string msg)
		{
			bool flag;
			msg = "";
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(ATS.INDEX_STORE_PATH), new NativeFSLockFactory());
			bool isExist = IndexReader.IndexExists(directory);
			if (isExist)
			{
				if (IndexWriter.IsLocked(directory))
				{
					IndexWriter.Unlock(directory);
				}
			}
			IndexWriter writer = null;
			try
			{
				try
				{
					writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
					writer.DeleteAll();
                    flag = writer.HasDeletions();
				}
				catch (Exception exception)
				{
					Exception e = exception;
					msg = string.Concat(e.Message, "<br />", e.StackTrace);
					flag = false;
				}
			}
			finally
			{
				writer.Close();
			}
			return flag;
		}

        /// <summary>
        /// 使用关键词搜索
        /// </summary>
        /// <param name="kw"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<SearchResult> DoSearch(string kw, int pi, out int totalCount,out int pagetCount,out int msecs )
		{
            var t1 = DateTime.Now;
            msecs = 0;
            pagetCount = 0;
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(ATS.INDEX_STORE_PATH), new NoLockFactory());
			List<SearchResult> list = new List<SearchResult>();
            if (string.IsNullOrWhiteSpace(kw) || pi<=0)
            {
                totalCount = 0;
                return list;
            }
			string[] strArrays = new string[] { "keyContent", "keyWords" };
            //解析
			MultiFieldQueryParser parser = new MultiFieldQueryParser(Version, strArrays, new PanGuAnalyzer());
			Query query = parser.Parse(kw);
            BooleanQuery bq = new BooleanQuery();
            bq.Add(query,Occur.SHOULD);
            IndexSearcher searcher = new IndexSearcher(IndexReader.Open(directory, true));
            //TODO 此处false是什么意思
            //TopScoreDocCollector collector = TopScoreDocCollector.Create(pi * ps, false);            
            //searcher.Search(query, collector);
            var hits = searcher.Search(bq, null, 10000, new Sort(new SortField("hashId", SortField.LONG, true))).ScoreDocs;
            var comparer = new ScoreDocComparer(searcher);
            hits=hits.Distinct(comparer).ToArray();
            if (hits == null || hits.Length == 0)
            {
                totalCount = 0;
                return list;
            }
			//ScoreDoc[] docs = collector.TopDocs(ps * (pi-1), ps).ScoreDocs;
            var docs = hits.Skip(PageSize * (pi - 1)).Take(PageSize).ToArray();
            msecs = (DateTime.Now - t1).Milliseconds;
            totalCount = hits.Length;
            pagetCount = totalCount / PageSize + 1;
            return docs.Select(x =>{
                var doc=searcher.Doc(x.Doc);
                return new SearchResult
                {
                    HashId = PubFunc.GetBigInt(doc.Get("hashId")),
                    KeyContent = doc.Get("keyContent"),
                    KeyWords = doc.Get("keyWords"),
                    RecvTime = PubTypes.GetDate(doc.Get("recvTime"), "yyyy-MM-dd"),
                    UpdateTime = PubTypes.GetDate(doc.Get("updateTime"), "yyyy-MM-dd"),
                    HashKey = doc.Get("hashKey"),
                    FileCnt = Convert.ToInt32(doc.Get("fileCnt")),
                    Type = Convert.ToInt32(doc.Get("type")),
                    ICOrE = (Convert.ToInt32(doc.Get("iCOrE")) == 1 ? true : false),
                    TotalSize = PubClass.FormatFileSize(PubFunc.GetBigInt(doc.Get("totalsize"))),
                    BodyPreview = ATS.Preview(doc.Get("keyContent"), kw)
                };
            }).ToList();

            //foreach (ScoreDoc item in docs)
            //{
            //    var doc = searcher.Doc(item.Doc);
            //    list.Add(new SearchResult
            //    {
            //        HashId = PubFunc.GetBigInt(doc.Get("hashId")),
            //        KeyContent = doc.Get("keyContent"),
            //        KeyWords = doc.Get("keyWords"),
            //        RecvTime = PubTypes.GetDate(doc.Get("recvTime"),"yyyy-MM-dd"),
            //        UpdateTime = PubTypes.GetDate(doc.Get("updateTime"),"yyyy-MM-dd"),
            //        HashKey = doc.Get("hashKey"),
            //        FileCnt = Convert.ToInt32(doc.Get("fileCnt")),
            //        Type = Convert.ToInt32(doc.Get("type")),
            //        ICOrE = (Convert.ToInt32(doc.Get("iCOrE")) == 1 ? true : false),
            //        TotalSize =PubClass.FormatFileSize(PubFunc.GetBigInt(doc.Get("totalsize"))),
            //        BodyPreview = ATS.Preview(doc.Get("keyContent"), kw)
            //    });
            //}

            //return list;
		}


        #region 分词和预览     
   


        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        private static string Preview(string body, string keyword)
        {
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            Highlighter highlighter = new Highlighter(new SimpleHTMLFormatter("<strong style=\"color:#f00;\">", "</strong>"), new Segment())
            {
                //设置每个摘要段的字符数
                FragmentSize = 100
            };
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, body);
        }

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public List<string> SplitWords(string inStr)
        {
            List<string> l = new List<string>();
            TokenStream tokenStream = (new PanGuAnalyzer()).TokenStream("", new StringReader(inStr));
            while (tokenStream.IncrementToken())
            {
                l.Add(tokenStream.GetAttribute<TermAttribute>().Term);
                //Lucene.Net.Analysis.Token token1 = tokenStream.Next();
                
                //if (token1 == null)
                //{
                //    break;
                //}
                //l.Add(token1.TermText());
            }
            return l;
        }        
        
        #endregion


    }
}