using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSOSKY.BL;
using System.Data;
using Utility;
using COM.CF;

namespace MSOMVC.Models
{
    /// <summary>
    /// 列表页Model
    /// </summary>
    public class HashListView
    {
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int Mecs { get; set; }
        public List<SearchResult> List { get; set; }
        public int PageIndex { get; set; }
        public SearchUnit SearchParam { get; set; }
    }
    /// <summary>
    /// 搜索单元，包括搜索类型和搜索关键字
    /// </summary>
    public class SearchUnit
    {
        public int SearchType { get; set; }
        public string KeyWord { get; set; }

        public SearchUnit(int type,string word)
        {
            SearchType = type;
            KeyWord = word;
        }
    }
    /// <summary>
    /// Hash 
    /// </summary>
    public class HashFile
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string FileSize { get; set; }
    }
    /// <summary>
    /// 详细的Hash对象
    /// </summary>
    public class HashItem
    {
        public long ID { get; set; }
        public string HashKey { get; set; }
        public DateTime RecvTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string KeyContent { get; set; }
        public string KeyWords { get; set; }
        public int RecvTimes { get; set; }
        public int FileCnt { get; set; }
        public int Level { get; set; }
        public string TotalSize { get; set; }
        public int Type { get; set; }
        public IList<HashFile> Detail { get; set; }
        public SearchUnit SearchParam { get; set; }

        public static HashItem GetOne(long id)
        {
            var one = new HashItem();
            var curHashRow = MSODB.oDB.GetSQLSingleRow("select * from dht_sum where id='" + id + "'");
            if (curHashRow == null) return null;
            var detailRow = MSODB.oDB.GetSQLSingleRow("select * from dht_hashdetail where hashid='" + id + "'");
            if (detailRow == null) return null;

            List<Dictionary<String, String>> detailList = null;
            var detailStr = detailRow["detail"].ToString();
            if (!string.IsNullOrWhiteSpace(detailStr))
            {
                detailList = PubClass.J2T<List<Dictionary<string, string>>>(detailStr);
            }
            one.ID = id;
            one.HashKey = curHashRow["hashKey"].ToString();
            one.RecvTime = DateTime.Parse(curHashRow["recvTime"].ToString());
            one.UpdateTime = DateTime.Parse(curHashRow["updateTime"].ToString());
            one.KeyContent = curHashRow["keyContent"].ToString();
            one.KeyWords = curHashRow["keyWords"].ToString();
            one.RecvTimes = Convert.ToInt32(curHashRow["recvTimes"].ToString());
            one.FileCnt = Convert.ToInt32(curHashRow["fileCnt"].ToString());
            one.Level = Convert.ToInt32(curHashRow["lvl"].ToString());
            one.TotalSize =PubClass.FormatFileSize(double.Parse(curHashRow["totalSize"].ToString()));
            one.Type = Convert.ToInt32(curHashRow["type"].ToString());
            one.Detail=new List<HashFile>();
            foreach (var item in detailList)
            {
                one.Detail.Add(new HashFile {Type=Convert.ToInt32(item["t"].ToString()),Name=item["n"],FileSize=PubClass.FormatFileSize(double.Parse(item["s"])) });
            }
            return one;
        }
    }

    public class Recommend
    {
        public string Title { get; set; }
        public int Category { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }


        public static IList<Recommend> GetAll(int cat = 0, int t = 0)
        {
            var l = new List<Recommend>();
            return MSODB.oDB.GetSQLTab(String.Format("select * from recommend where canuse=1 and category={0} and type={1}", cat, t))
                .Select()
                .Select(
                    x => new Recommend { Title = x["title"].ToString(), Category = PubFunc.GetInt(x["category"]), Type = PubFunc.GetInt(x["type"]), Time = DateTime.Parse(x["time"].ToString()) }
                )
                .ToList();
        }

    }

}