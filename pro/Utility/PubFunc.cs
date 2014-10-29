using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Utility
{
	public class PubFunc
	{
		private static IDictionary<string, string> _sysConfig;

		private static Regex regexsj;

		public static IDictionary<string, string> SysConfig
		{
			get
			{
				if (PubFunc._sysConfig == null)
				{
					PubFunc.InitSysConfig();
				}
				return PubFunc._sysConfig;
			}
		}

		static PubFunc()
		{
			PubFunc.regexsj = new Regex("^(13[0-9]{9})|(15[012356789][0-9]{8})|(18[056789][0-9]{8})$", RegexOptions.IgnoreCase);
		}

		public PubFunc()
		{
		}

		public static bool CheckInts(string s)
		{
			bool flag;
			Regex CheckIntsRegEx = new Regex("^(\\s*\\-?\\d+\\s*,)*\\s*\\-?\\d+\\s*$", RegexOptions.Compiled);
			flag = (!(s == "") ? CheckIntsRegEx.IsMatch(s) : false);
			return flag;
		}

		public static bool CheckMobile(string s)
		{
			bool flag;
			if (!string.IsNullOrEmpty(s))
			{
				s = s.Replace("+86", "");
				flag = Regex.IsMatch(s, "^\\d{11}$");
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public static string DictionaryToJson(IDictionary<string, string> dic)
		{
			return (new JavaScriptSerializer()).Serialize(dic);
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

		public static string GetAitePagesLinkHTML(string url, int pageCount, int pageIndex)
		{
			url = url.Replace("?", "").Replace("a=1&", "").Insert(url.IndexOf(".aspx") + 5, "?a=1&");
			string url1 = url.Substring(0, (url.IndexOf("p=") == -1 ? url.Length : url.IndexOf("p=") - 1));
			StringBuilder sb = new StringBuilder();
			if (pageCount > 1)
			{
				if (pageIndex > 0)
				{
					sb.Append(string.Format("<div class=\"cer_pre\"><a class=\"nprev\" href=\"{0}\"><img src=\"images/certification_pre.jpg\"></a></div>", string.Concat(url1, "&p=", pageIndex - 1)));
				}
				sb.Append("<ul class=\"cer_ul\">");
				for (int i = 0; i < pageCount; i++)
				{
					if (i != pageIndex)
					{
						sb.Append(string.Format("<li class=\"num\"><a href=\"{0}\" class=\"page_a\"> {1} </a></li>", string.Concat(url1, "&p=", i), i + 1));
					}
					else
					{
						sb.Append(string.Concat("<li class=\"num\"><a class=\"current_a\">", i + 1, "</a></li>"));
					}
				}
				sb.Append("</ul>");
				if (pageIndex < pageCount - 1)
				{
					sb.Append(string.Format("<div class=\"cer_nest\"><a href=\"{0}\" style=\"cursor:pointer;\" class=\"ynext\"><img src=\"images/certification_next.jpg\"></a></div>", string.Concat(url1, "&p=", pageIndex + 1)));
				}
			}
			return sb.ToString();
		}

		public static DateTime GetDate(string s)
		{
			DateTime dateTime;
			DateTime dt = DateTime.Now;
			dateTime = (DateTime.TryParse(s, out dt) ? dt : DateTime.Now);
			return dateTime;
		}

		public static int GetInt(object o)
		{
			int num;
			int num1;
			if (o != null)
			{
				string s = o.ToString();
				if (string.IsNullOrWhiteSpace(s))
				{
					num1 = 0;
				}
				else if (s.StartsWith("0x"))
				{
					num1 = int.Parse(s.Substring(2), NumberStyles.AllowHexSpecifier);
				}
				else
				{
					num1 = (int.TryParse(s, out num) ? num : 0);
				}
			}
			else
			{
				num1 = 0;
			}
			return num1;
		}

		public static string GetLeftStr(string s, int n, string link)
		{
			string str;
			str = (s != null ? string.Format("<a href=\"{0}\" {1}>{2}</a>", link, string.Concat("title=", s), (s.Length <= n ? s : string.Concat(s.Substring(0, n), "..."))) : "");
			return str;
		}

		public static long GetLong(object o)
		{
			long n;
			long num;
			if (o != null)
			{
				string s = o.ToString();
				if (string.IsNullOrWhiteSpace(s))
				{
					num = (long)0;
				}
				else if (s.StartsWith("0x"))
				{
					num = (!long.TryParse(s.Substring(2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out n) ? (long)0 : n);
				}
				else
				{
					num = (!long.TryParse(s, out n) ? (long)0 : n);
				}
			}
			else
			{
				num = (long)0;
			}
			return num;
		}

		public static List<string> GetMobileAry(string s)
		{
			List<string> l = new List<string>();
			string[] strArrays = Regex.Split(s.Replace("+86", ""), "\\D");
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				if (PubFunc.CheckMobile(str))
				{
					l.Add(str);
				}
			}
			return l;
		}

		public static string GetMobiles(string s)
		{
			return string.Join(",", PubFunc.GetMobileAry(s));
		}

		public static string GetMoneyStr(object money)
		{
			string str;
			if (money != null)
			{
				double md = 0;
				str = (double.TryParse(money.ToString(), out md) ? Convert.ToInt32(md).ToString() : "");
			}
			else
			{
				str = "";
			}
			return str;
		}

		public static string GetPagedSql(string sql, int pageIndex, int pageSize)
		{
			string str;
			if (!string.IsNullOrEmpty(sql))
			{
				string start = "0";
				if (pageIndex > 0)
				{
					start = Convert.ToString(pageIndex * pageSize);
				}
				int select = sql.GetSymPosition("select");
				string end = Convert.ToString((pageIndex + 1) * pageSize);
				string orderSql = sql.GetStrBySym("order by");
				string fromSql = sql.GetStrBySym("from");
				string selectSql = string.Concat("select ", sql.Substring(select, sql.Length - select - fromSql.Length));
				if (string.IsNullOrEmpty(orderSql))
				{
					throw new Exception(" sql2005 怎么着也得弄个order by啊");
				}
				fromSql = fromSql.Substring(0, fromSql.Length - orderSql.Length);
				string strSql = string.Concat("select * from (", selectSql);
				string rownum = "row_number()";
				string str1 = strSql;
				string[] strArrays = new string[] { str1, ",", rownum, " over (", orderSql, ") as rn ", fromSql, ") as data where rn>", start, " and rn<=", end };
				strSql = string.Concat(strArrays);
				if (!string.IsNullOrEmpty(fromSql.GetStrBySym("group by")))
				{
					fromSql = string.Concat(" from (select count(*) totalCount ", fromSql, ") tbl");
				}
				str = string.Concat(strSql, ";select count(*) as totalCount ", fromSql);
			}
			else
			{
				str = null;
			}
			return str;
		}

		public static string GetPageLink(string url, int pageIndexNum)
		{
			string linkChar = (url.Contains(".aspx?") ? "&" : "?");
			string url1 = url.Substring(0, (url.IndexOf("p=") == -1 ? url.Length : url.IndexOf("p=") - 1));
			object[] objArray = new object[] { url1, linkChar, "p=", pageIndexNum - 1 };
			string str = string.Format("<a href=\"{0}\"> {1} </a>", string.Concat(objArray), pageIndexNum);
			return str;
		}

		public static string GetPagesLinkHTML(string url, int pageCount, int pageIndex)
		{
			url = url.Replace("?", "").Replace("a=1&", "").Insert(url.IndexOf(".aspx") + 5, "?a=1&");
			string url1 = url.Substring(0, (url.IndexOf("p=") == -1 ? url.Length : url.IndexOf("p=") - 1));
			StringBuilder sb = new StringBuilder();
			if (pageCount > 1)
			{
				if (pageIndex > 0)
				{
					sb.Append(string.Format("<a href=\"{0}\">上一页</a>", string.Concat(url1, "&p=", pageIndex - 1)));
				}
				for (int i = 0; i < pageCount; i++)
				{
					if (i != pageIndex)
					{
						sb.Append(string.Format("<a href=\"{0}\"> {1} </a>", string.Concat(url1, "&p=", i), i + 1));
					}
					else
					{
						sb.Append(string.Concat("<span class=\"current\">", i + 1, "</a></span>"));
					}
				}
				if (pageIndex < pageCount - 1)
				{
					sb.Append(string.Format("<a href=\"{0}\">下一页</a>", string.Concat(url1, "&p=", pageIndex + 1)));
				}
			}
			return sb.ToString();
		}

		public static void InitSysConfig()
		{
		}

		public static T JsonToDictionary<T>(string inputstr)
		{
			return (new JavaScriptSerializer()).Deserialize<T>(inputstr);
		}

		public static string SendHTTP(string url)
		{
			Encoding encoding;
			string str = "";
			HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(url)).GetResponse();
			try
			{
				encoding = (response.ContentType.ToLower().IndexOf("utf-8") <= 0 ? Encoding.Default : Encoding.UTF8);
				str = (new StreamReader(response.GetResponseStream(), encoding)).ReadToEnd();
			}
			finally
			{
				response.Close();
			}
			return str;
		}
	}
}