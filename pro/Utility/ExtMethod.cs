using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Web;
using System.Web.Routing;
using COM.CF;
using System.Data;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
namespace Utility
{
	public static class ExtMethod
	{
        //public static int GetCount(this string str, string sym)
        //{
        //    int num;
        //    num = ((string.IsNullOrEmpty(str) ? false : !string.IsNullOrEmpty(sym)) ? str.Length - str.Replace(sym, "").Length : 0);
        //    return num;
        //}

        //public static string GetStrBySym(this string str, string sym)
        //{
        //    string str1;
        //    if ((string.IsNullOrEmpty(str) ? false : !string.IsNullOrEmpty(sym)))
        //    {
        //        string result = null;
        //        Regex regex = new Regex(string.Concat("^.*?[\\]\\)\\s\\n\\*]+?(?<key>", sym, ")[\\[\\(\\s\\n\\*]+?.*?$"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        //        if (regex.IsMatch(str))
        //        {
        //            bool isHas = false;
        //            result = str;
        //            while (regex.IsMatch(result))
        //            {
        //                result = result.Substring(regex.Match(result).Groups["key"].Index);
        //                if (result.GetCount("(") == result.GetCount(")"))
        //                {
        //                    isHas = true;
        //                    break;
        //                }
        //            }
        //            if (!isHas)
        //            {
        //                result = null;
        //            }
        //        }
        //        str1 = result;
        //    }
        //    else
        //    {
        //        str1 = null;
        //    }
        //    return str1;
        //}

        //public static int GetSymPosition(this string str, string sym)
        //{
        //    int num;
        //    if ((string.IsNullOrEmpty(str) ? false : !string.IsNullOrEmpty(sym)))
        //    {
        //        Regex regex = new Regex(string.Concat("^[\\s\\n]*?(?<key>", sym, ")[\\[\\(\\s\\n]+?.*?$"), RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        //        num = (!regex.IsMatch(str) ? 0 : regex.Match(str).Groups["key"].Index + sym.Length);
        //    }
        //    else
        //    {
        //        num = 0;
        //    }
        //    return num;
        //}


        public static int GetCount(this string str, string sym)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(sym))
            {
                return 0;
            }
            else
            {
                return str.Length - str.Replace(sym, "").Length;
            }
        }
        public static int GetSymPosition(this string str, string sym)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(sym))
            {
                return 0;
            }
            else
            {
                Regex regex = new Regex(@"^[\s\n]*?(?<key>" + sym + @")[\[\(\s\n]+?.*?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
                if (regex.IsMatch(str))
                {
                    return regex.Match(str).Groups["key"].Index + sym.Length;
                }
                else
                {
                    return 0;
                }
            }
        }
        public static string GetStrBySym(this string str, string sym)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(sym))
            {
                return null;
            }
            else
            {
                string result = null;
                Regex regex = new Regex(@"^.*?[\]\)\s\n\*]+?(?<key>" + sym + @")[\[\(\s\n\*]+?.*?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
                if (regex.IsMatch(str))
                {
                    bool isHas = false;
                    result = str;
                    while (regex.IsMatch(result))
                    {
                        result = result.Substring(regex.Match(result).Groups["key"].Index);
                        if (GetCount(result, "(") == GetCount(result, ")"))
                        {
                            isHas = true;
                            break;
                        }
                    }
                    if (!isHas)
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        #region 类型转换
        #region 强转成int 如果失败返回 0
        /// <summary>
        /// 强转成int 如果失败返回 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ToInt(this object o)
        {
            return PubFunc.GetInt(o);
        }
        /// <summary>
        /// 强转成long 如果失败返回 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static long ToBigInt(this object o)
        {
            return PubFunc.GetBigInt(o);
        }
        #endregion
        #region 强转成int 如果失败返回 i
        /// <summary>
        /// 强转成int 如果失败返回 i
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int ToInt(this object o, int i)
        {
            int reval = 0;
            if (o != null && int.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return i;
        }
        #endregion
        #region 强转成double 如果失败返回 0
        /// <summary>
        /// 强转成double 如果失败返回 0
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double ToDou(this object o)
        {
            double reval = 0;
            if (o != null && double.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return 0;
        }
        #endregion
        #region 强转成double 如果失败返回 i
        /// <summary>
        /// 强转成double 如果失败返回 i
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static double ToDou(this object o, int i)
        {
            double reval = 0;
            if (o != null && double.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return i;
        }
        #endregion
        #region 强转成string 如果失败返回 ""
        /// <summary>
        /// 强转成string 如果失败返回 ""
        /// </summary>
        /// <param name="o"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ToConvertString(this object o)
        {
            if (o != null) return o.ToString().Trim();
            return "";
        }
        #endregion
        #region  强转成string 如果失败返回 str
        /// <summary>
        /// 强转成string 如果失败返回 str
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToConvertString(this object o, string str)
        {
            if (o != null) return o.ToString().Trim();
            return str;
        }
        #endregion
        #region  去除字符串中/t/r/n/s
        /// <summary>
        ///去除字符串中/t/r/n/s
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToRemovetsrn(this string o)
        {
            if (o == null || o == "") return o;
            else
            {
                o = Regex.Replace(o, @"\t|\n|\s|\r", "");
            }
            return o;
        }
        /// <summary>
        ///去除字符串中/t/r/n/s
        /// </summary>
        /// <param name="o"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToRemovetsrn(this string o, string str)
        {
            o = o.Replace(" ", "");
            if (o == null || o == "") return str;
            else
            {
                o = Regex.Replace(o, @"\t|\n|\s|\r", "");
            }
            return o;
        }
        #endregion
        #region 格式化时间
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="o"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static string ToDateStr(this object o, string FormatStr = "yyyy-MM-dd HH:mm")
        {
            if (o == null) return "";
            if (string.IsNullOrWhiteSpace(o.ToString())) return "";
            return Convert.ToDateTime(o).ToString(FormatStr);
        }
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="o"></param>
        /// <param name="FormatStr"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string o)
        {
            DateTime reval = DateTime.Now;
            if (o != null && DateTime.TryParse(o.ToString(), out reval))
            {
                return reval;
            }
            return null;
        }
        #endregion
        #region 强转GUID
        /// <summary>
        /// 强转GUID
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object o)
        {
            if (o == null) return Guid.Empty;
            else return Guid.Parse(o.ToString());
        }
        #endregion
        #endregion

        #region IEnumerable通用扩展方法
        #region Private expression tree helpers
        private static LambdaExpression GenerateSelector<TEntity>(String propertyName, out Type resultType) where TEntity : class
        {
            PropertyInfo property;
            Expression propertyAccess;
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");

            if (propertyName.Contains('.'))
            {
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            resultType = property.PropertyType;

            return Expression.Lambda(propertyAccess, parameter);
        }
        private static MethodCallExpression GenerateMethodCall<TEntity>(IQueryable<TEntity> source, string methodName, String fieldName) where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(fieldName, out selectorResultType);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName,
            new Type[] { type, selectorResultType },
            source.Expression, Expression.Quote(selector));
            return resultExp;
        }
        #endregion

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(this IQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "OrderByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }


        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(this IOrderedQueryable<TEntity> source, string fieldName) where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall<TEntity>(source, "ThenByDescending", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }
        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(this IQueryable<TEntity> source, string sortExpression) where TEntity : class
        {
            String[] orderFields = sortExpression.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (int currentFieldIndex = 0; currentFieldIndex < orderFields.Length; currentFieldIndex++)
            {
                String[] expressionPart = orderFields[currentFieldIndex].Trim().Split(' ');
                String sortField = expressionPart[0];
                Boolean sortDescending = (expressionPart.Length == 2) && (expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase));
                if (sortDescending)
                {
                    result = currentFieldIndex == 0 ? source.OrderByDescending(sortField) : result.ThenByDescending(sortField);
                }
                else
                {
                    result = currentFieldIndex == 0 ? source.OrderBy(sortField) : result.ThenBy(sortField);
                }
            }
            return result;
        }
        /// <summary>
        /// IEnumerable通用扩展方法
        /// </summary>     
        public static void ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            if (List == null) throw new ArgumentNullException("集合不能为空");
            if (Action == null) throw new ArgumentNullException("Action不能为空");
            foreach (T item in List)
            {
                Action.Invoke(item);
            }
        }
        /// <summary>
        /// 添加 并且新添元素在索引第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> AddFirstItem<T>(this IEnumerable<T> list, T t)
        {
            List<T> reval = new List<T>();
            reval.Add(t);
            reval.AddRange(list);
            return reval;
        }
        public static void For<T>(this IEnumerable<T> List, int Start, int End, int Step, Action<T> Action)
        {
            for (int i = Start; i < End; i = i + Step)
            {
                Action.Invoke(List.ElementAt(i));
            }
        }
        public static void For<T>(this IEnumerable<T> List, int Start, Func<int, bool> End, int Step, Action<T> Action)
        {
            for (int i = Start; End.Invoke(i); i = i + Step)
            {
                Action.Invoke(List.ElementAt(i));
            }
        }
        public static void For<T>(this IEnumerable<T> List, int Start, Func<int, bool> End, int Step, Func<T, int, int> Action)
        {
            for (int i = Start; End.Invoke(i); i = i + Step)
            {
                i = Action.Invoke(List.ElementAt(i), i);
            }
        }


        #endregion

        #region string扩展方法
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string ToFormat(this string o, params object[] ps)
        {
            return string.Format(o, ps);
        }

        /// <summary>
        /// 调用js用 "alert(1)".ToJavaScript(); 返回的是完整JS对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJavaScript(this string o)
        {
            return ("<script>" + o + "</script>");
        }

        /// <summary>
        ///弹出 alert; 返回的是完整JS对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToAlert(this string o)
        {
            string js = "$.Zebra_Dialog('" + o + "')";
            return js.ToJavaScript();
        }

        /// <summary>
        ///弹出 alert;并且跳转
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToAlertAndHref(this string message, string url)
        {
            string js = "alert(\"" + message + "\");window.location.href=\"" + url + "\"";
            return js.ToJavaScript();
        }
        /// <summary>
        /// 弹出提示并返回
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToAlertBack(this string message)
        {
            return "alert('{0}');history.go(-1)".ToFormat(message).ToJavaScript();
        }

        /// <summary>
        /// 手机格式转换 13812341234 转成138-1234-1234
        /// </summary>
        /// <param name="pn">手机号</param>
        /// <returns></returns>
        public static string ToPhone(this string phoneNum)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNum)) return "";
                return phoneNum.Substring(0, 3) + "-" + phoneNum.Substring(3, 4) + "-" + phoneNum.Substring(7);
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// 将匿名对象转成 SqlParameter[] 
        /// </summary>
        /// <param name="o">如 var o=new {id=1,name="张三"}</param>
        /// <returns>SqlParameter[]</returns>
        public static SqlParameter[] ToPars(this object o)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            RouteValueDictionary rvd = new RouteValueDictionary(o);
            foreach (var r in rvd)
            {
                SqlParameter par;
                if (r.Value == null)
                {
                    par = new SqlParameter("@" + r.Key, DBNull.Value);
                }
                else
                {
                    par = new SqlParameter("@" + r.Key, r.Value);
                }
                pars.Add(par);
            }
            return pars.ToArray();
        }

        /// <summary>
        /// 将匿名对象转成 SqlParameter[] 
        /// </summary>
        /// <param name="o">如 var o=new {id=1,name="张三"}</param>
        /// <returns>SqlParameter[]</returns>
        public static string ToUrl(this object o)
        {
            RouteValueDictionary rvd = new RouteValueDictionary(o);
            string reval = "";
            foreach (var r in rvd)
            {
                if (reval != "") reval += "&";
                reval += (r.Key + "=" + r.Value);

            }
            return reval;
        }

        /// <summary>
        /// sql过滤关键字   
        /// </summary>
        /// <param name="objWord"></param>
        /// <returns></returns>
        public static string ToSqlFilter(this object objWord)
        {
            var str = objWord + "";
            str = str.Replace("'", "‘");
            str = str.Replace(";", "；");
            str = str.Replace(",", ",");
            str = str.Replace("?", "?");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("(", "(");
            str = str.Replace(")", ")");
            str = str.Replace("@", "＠");
            str = str.Replace("=", "＝");
            str = str.Replace("+", "＋");
            str = str.Replace("*", "＊");
            str = str.Replace("&", "＆");
            str = str.Replace("#", "＃");
            str = str.Replace("%", "％");
            str = str.Replace("$", "￥");
            return str;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static List<string> Split(this string str, string spiltStr)
        {
            var reval = str.Split(new string[] { spiltStr }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return reval;
        }

        /// <summary>
        /// htmlEncode一个对象 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static String HtmlEncode(this object o)
        {
            return HttpUtility.HtmlEncode(o) + "";
        }

        /// <summary>
        /// htmlDecode一个字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static String HtmlDecode(this object o)
        {
            if (o == null) return "";
            var s = o.ToString();
            if (string.IsNullOrWhiteSpace(s)) return "";
            return HttpUtility.HtmlDecode(s);
        }

        public static string UrlEncode(this string o)
        {
            return HttpContext.Current.Server.UrlEncode(o);
        }
        public static string UrlDecode(this string o)
        {
            return HttpContext.Current.Server.UrlDecode(o);
        }
        #endregion

        #region json转换
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static TEntity ToModel<TEntity>(this string json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<TEntity>(json);
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj);
        }

        #endregion

        #region 对像转换
        /// <summary>
        /// 将对像转为匿名类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic ToDynamic<T>(this List<T> obj)
        {
            if (obj == null || obj.Count == 0) return null;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj).ToModel<dynamic>();
        }

        /// <summary>
        /// 将对像转为匿名类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this object obj)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj).ToModel<dynamic>();
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = typeof(T).GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将datatable转为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }


        /// <summary>
        /// 将HTML内容转换成纯文本形式，即去除HTML格式
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string HtmlToText(this string source)
        {
            string result;            //remove line breaks,tabs
            result = source.Replace("\r", " ");
            result = result.Replace("\n", " ");
            result = result.Replace("\t", " ");

            //remove the header
            result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);

            result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);

            //remove all styles
            result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase); //clearing attributes
            result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);

            //insert tabs in spaces of <td> tags
            result = Regex.Replace(result, @"<( )*td([^>])*>", " ", RegexOptions.IgnoreCase);

            //insert line breaks in places of <br> and <li> tags
            result = Regex.Replace(result, @"<( )*br( )*>", "\r\n", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<( )*li( )*>", "\r\n", RegexOptions.IgnoreCase);

            //insert line paragraphs in places of <tr> and <p> tags
            result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\n", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\n", RegexOptions.IgnoreCase);

            //remove anything thats enclosed inside < >
            result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

            //replace special characters:
            result = Regex.Replace(result, @"&", "&", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @" ", " ", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<", "<", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @">", ">", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);

            //remove extra line breaks and tabs
            result = Regex.Replace(result, @" ( )+", " ");
            result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r");
            result = Regex.Replace(result, @"(\r\r)+", "\r\n");

            //remove blank
            result = Regex.Replace(result, @"\s", "");

            return result;
        }


        #endregion

        #region 逻辑
        /// <summary>
        /// o==true 返回successVal否则返回errorVal
        /// </summary>
        /// <param name="o"></param>
        /// <param name="successVal"></param>
        /// <param name="errorVal"></param>
        /// <returns></returns>
        public static string IIF(this object o, string successVal, string errorVal)
        {
            return Convert.ToBoolean(o) ? successVal : errorVal;
        }

        /// <summary>
        /// o==true 返回successVal否则返回errorVal
        /// </summary>
        /// <param name="o"></param>
        /// <param name="successVal"></param>
        /// <param name="errorVal"></param>
        /// <returns></returns>
        public static string IIF(this object o, string successVal)
        {
            return Convert.ToBoolean(o) ? successVal : "";
        }

        /// <summary>
        /// o==true 执行success 否则执行 error
        /// </summary>
        /// <param name="o"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public static void IIF(this object o, Action<dynamic> success, Action<dynamic> error)
        {
            if (Convert.ToBoolean(o))
            {
                success(o);
            }
            else
            {
                error(error);
            }
        }

        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNE(this object o)
        {
            if (o == null || o == DBNull.Value) return true;
            return o.ToString() == "";
        }

        public static string IsNE(this string o, string s)
        {
            return o.IsNE() ? s : o;
        }

        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNE_ByGuidNull(this Guid? o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }
        /// <summary>
        /// 是否是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsNE_ByGuid(this Guid o)
        {
            if (o == null) return true;
            return o == Guid.Empty;
        }

        /// <summary>
        /// 是否不是null或""
        /// </summary>
        /// <returns></returns>
        public static bool IsntNE(this object o)
        {
            return !o.IsNE();
        }
        /// <summary>
        /// 是否是INT
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsInt(this object o)
        {
            if (o == null) return false;
            return Regex.IsMatch(o.ToString(), @"^\d+$");
        }
        /// <summary>
        /// 是否不是INT
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsntInt(this object o)
        {
            if (o == null) return true;
            return !Regex.IsMatch(o.ToString(), @"^\d+$");
        }
        #endregion

        #region 数组
        /// <summary>
        /// 将数组以豆号格开
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoin(this object[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => c.ToString().Trim()));
            }
        }
        /// <summary>
        /// 将数组转为 '1','2' 这种格式的字符串 用于 where id in(  )
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoinSqlInVal(this object[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => "'" + c.ToSqlFilter() + "'"));//除止SQL注入
            }
        }
        /// <summary>
        /// 将数组转为 '1','2' 这种格式的字符串 用于 where id in(  )
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToJoinSqlInVal(this Guid[] array)
        {
            if (array == null || array.Length == 0)
            {
                return "";
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(c => "'" + c.ToSqlFilter() + "'"));//除止SQL注入
            }
        }
        #endregion
	}
}



