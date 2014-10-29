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

namespace Utility
{
	public class ATS
	{
		private static string INDEX_STORE_PATH;

		private static string INDEX_PATH;

		private static int INDEX_SYNC_CAPACITY;

		private int PAGESIZE = 20;

		static ATS()
		{
			ATS.INDEX_STORE_PATH = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\index");
			ATS.INDEX_PATH = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\doc");
			ATS.INDEX_SYNC_CAPACITY = PubFunc.GetInt(ConfigurationManager.AppSettings["MSOIndexCapcity"]);
		}

		public ATS()
		{
		}

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
					writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
					DataRow curIndexRow = DB.oDB.GetSQLSingleRow("select top 1 * from IndexSync where imodify=0 order by id desc");
					int curhashId = (curIndexRow == null ? 0 : PubFunc.GetInt(curIndexRow["hashId"]));
					DataTable dt = DB.oDB.GetSQLTab(string.Format("select top {0} * from dht_sum where id>{1}", ATS.INDEX_SYNC_CAPACITY, curhashId));
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
						DB.oDB.ExecuteNonQuery(string.Format("insert into IndexSync(hashId) values({0})", (
							from x in (IEnumerable<DataRow>)dt.Select()
							select PubFunc.GetLong(x["id"])).Max()));
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
					writer.Close();
				}
				if (directory != null)
				{
					directory.Close();
				}
			}
			return flag;
		}

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
					flag = true;
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

		public static List<Utility.SearchResult> DoSearch(string kw, int startRowIndex, int pageSize, out int totalCount)
		{
			List<Utility.SearchResult> searchResults;
			FSDirectory directory = FSDirectory.Open(new DirectoryInfo(ATS.INDEX_STORE_PATH), new NoLockFactory());
			IndexSearcher searcher = new IndexSearcher(IndexReader.Open(directory, true));
			List<Utility.SearchResult> list = new List<Utility.SearchResult>();
			if ((kw == null ? false : !(kw == "")))
			{
				Lucene.Net.Util.Version lUCENE29 = Lucene.Net.Util.Version.LUCENE_29;
				string[] strArrays = new string[] { "keyContent", "keyWords" };
				MultiFieldQueryParser parser = new MultiFieldQueryParser(lUCENE29, strArrays, new PanGuAnalyzer());
				Query query = parser.Parse(kw);
				TopScoreDocCollector collector = TopScoreDocCollector.create(1024, true);
				searcher.Search(query, collector);
				ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;
				totalCount = collector.GetTotalHits();
				for (int i = 0; i < (int)docs.Length; i++)
				{
					Document doc = searcher.Doc(docs[i].doc);
					List<Utility.SearchResult> searchResults1 = list;
					Utility.SearchResult searchResult = new Utility.SearchResult()
					{
						HashId = (long)PubFunc.GetInt(doc.Get("hashId")),
						KeyContent = doc.Get("keyContent"),
						KeyWords = doc.Get("keyWords"),
						RecvTime = Convert.ToDateTime(doc.Get("recvTime")),
						UpdateTime = Convert.ToDateTime(doc.Get("updateTime")),
						HashKey = doc.Get("hashKey"),
						FileCnt = Convert.ToInt32(doc.Get("fileCnt")),
						Type = Convert.ToInt32(doc.Get("type")),
						ICOrE = (Convert.ToInt32(doc.Get("iCOrE")) == 1 ? true : false),
						TotalSize = Convert.ToInt64(doc.Get("totalsize")),
						BodyPreview = ATS.Preview(doc.Get("keyContent"), kw)
					};
					searchResults1.Add(searchResult);
				}
				searchResults = list;
			}
			else
			{
				totalCount = 0;
				searchResults = null;
			}
			return searchResults;
		}

		private static string Preview(string body, string keyword)
		{
			Highlighter highlighter = new Highlighter(new SimpleHTMLFormatter("<font color=\"Red\">", "</font>"), new Segment())
			{
				FragmentSize = 100
			};
			return highlighter.GetBestFragment(keyword, body);
		}

		public List<string> SplitWords(string inStr)
		{
			List<string> l = new List<string>();
			TokenStream tokenStream = (new PanGuAnalyzer()).TokenStream("", new StringReader(inStr));
			while (true)
			{
				Lucene.Net.Analysis.Token token1 = tokenStream.Next();
				Lucene.Net.Analysis.Token token = token1;
				if (token1 == null)
				{
					break;
				}
				l.Add(token.TermText());
			}
			return l;
		}
	}
}