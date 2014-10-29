using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Collections.Specialized;

namespace Utility
{
    public class OAuth2:IOAuth2
    {
        private static string host = ConfigurationManager.AppSettings["Oauth2_Host"] ?? "http://www.msosky.com";

        public static String GetRedirectUri()
        {
            return HttpUtility.UrlEncode(host + "/oauth2/redirect.aspx");
        }

        protected static IDictionary<String, String> GetDict(string s)
        {
            var o = new Dictionary<String, String>();
            foreach (var t in s.Split('&'))
            {
                var a = t.Split('=');
                if (a.Length == 2)
                {
                    o[a[0]] = HttpUtility.UrlDecode(a[1]);
                }
            }
            return o;
        }


        protected static T GetJsonDict<T>(string s)
        {
            return (new JavaScriptSerializer()).Deserialize<T>(s);
        }

        public static IDictionary<String, String> GetJsonDictionary(string s)
        {
            var dic = GetJsonDict<Dictionary<String, String>>(s);
            dic.Add("resultstr", s);
            return dic;
        }

        public static IOAuth2 GetObj(int otype, NameValueCollection f)
        {
            switch (otype)
            {
                case 1:
                    return new BaiduOAuth2(f);
            }
            Error.GotoError("错误的otype" + otype);
            return null;
        }

        public static string GetRequestAuthCodeURL(int otype, string state, int display)
        {
            switch (otype)
            {
                case 1:
                    return BaiduOAuth2.GetRequestAuthCodeURL(state, display);
            }
            Error.GotoError("");
            return "";
        }


        protected int _otype;
        public int Otype
        {
            get { return _otype; }
        }


        protected string _state;
        public string State
        {
            get { return _state; }
        }

        protected string _access_token;
        public string Access_token
        {
            get { return _access_token; }
        }


        protected string _refresh_token;
        public string Refresh_token
        {
            get { return _refresh_token; }
        }

        protected DateTime _expire;
        public DateTime Expire
        {
            get { return _expire; }
        }

        protected string _ouid;
        public string OUID
        {
            get { return _ouid; }
        }
    }
}
