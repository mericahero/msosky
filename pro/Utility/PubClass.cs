using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using COM.CF;
using CWS;

namespace Utility
{
	public class PubClass
	{
		private static IDictionary<string, string> _sysConfig;

		private static Regex regexsj;

        public static string MSOTitleKey;

        public static string MSOKeyWords;

        public static string MSODescription;



		static PubClass()
		{
		}

		public PubClass()
		{
            MSOTitleKey = PubFunc.GetDefaultStr(CWConfig.Appset["MSOTitleKey"], " - 专业BT资源搜索");
            MSOKeyWords = PubFunc.GetDefaultStr(CWConfig.Appset["MSOKeyWords"], "搜索 DHT 种子下载 磁力连接 torrent magic 资源");
            MSODescription = PubFunc.GetDefaultStr(CWConfig.Appset["MSODescription"], "MSOSKY是一个最专业的BT种子搜索和磁力链接下载网站,目前已经收录了上千万BT种子的磁力链接,支持磁力链接转换BT种子,BT种子转换磁力链接,提供在线云点播功能，一个MAN的天堂，MAN点搜哦！");
		}



		public static string T2J(object dic)
		{
			return (new JavaScriptSerializer()).Serialize(dic);
		}

        public static T J2T<T>(string inputstr)
        {
            return (new JavaScriptSerializer()).Deserialize<T>(inputstr);
        }

		public static string FormatFileSize(double fileSize)
		{
			string str;
			if (fileSize < 0)
			{
				throw new ArgumentOutOfRangeException("fileSize");
			}
			if (fileSize >= 1073741824)
			{
				str = string.Format("{0:########0.00} GB", (double)fileSize / 1073741824);
			}
			else if (fileSize < 1048576)
			{
				str = (fileSize < 1024 ? string.Format("{0} bytes", fileSize) : string.Format("{0:####0.00} KB", (double)fileSize / 1024));
			}
			else
			{
				str = string.Format("{0:####0.00} MB", (double)fileSize / 1048576);
			}
			return str;
		}


        public static IDictionary<string, string> GetYunBoUrl(string hashkey)
        {
            var magLink="magnet:?xt=urn:btih:" + hashkey;
            var d = new Dictionary<string, string> { 
                //{"迅雷云播",String.Format("http://vod.xunlei.com/nplay.html?uvs=_4_&tryplay=1&from=vodHome&url={0}",magLink)},
                {"火焰云播",String.Format("http://www.huoyan.tv/api.php#!u={0}",hashkey)},
                {"如意云播",String.Format("http://www.huoyan.tv/api.php?ed=1&url={0}",magLink)},
                {"云点播",String.Format("http://vod.dbanklm.com/index.php#!u={0}",magLink)},
                {"超级云点播",String.Format("http://yunplay.sinaapp.com/vod.html#!url={0}",magLink)},
                {"奇爱云播",String.Format("http://vod.7ibt.com/index.php?url={0}",magLink)},
                {"电影离线云播",String.Format("http://yun.dybeta.com/playapi.php?u={0}&method=high",magLink)}
            };
            return d;
        }


	}
}