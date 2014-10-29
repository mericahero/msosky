using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.IO;
using COM.CF;

namespace Utility
{
    public class BaiduOAuth2 : OAuth2
    {
        private static string AppID;
        private static string AppKey;
        private static string Redirect_uri;


        static BaiduOAuth2()
        {
            AppID = ConfigurationManager.AppSettings["BaiduAPIKey"] ?? "";
            AppKey = ConfigurationManager.AppSettings["BaiduSecretKey"] ?? "";
            Redirect_uri = OAuth2.GetRedirectUri();
            
        }

        public static string GetRequestAuthCodeURL(string state, int display)
        {
            var dispDic = new Dictionary<int, string>() { { 0, "page" }, { 1, "mobile" }, { 2, "popup" }, { 3, "dialog" }, { 4, "pad" }, { 5, "tv" } };
            return string.Format("https://openapi.baidu.com/oauth/2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&scope=basic,super_msg,netdisk&state={2}&display={3}",AppID,Redirect_uri,state,dispDic[display]);
        }


        public BaiduOAuth2()
        {
            _access_token = "3.303e179b808b4390e5e1974bfd08d92e.2592000.1388658454.2952962517-1646585";
            _refresh_token = "4.85e0816af778f6828a8751a803d1b462.315360000.1701426454.2952962517-1646585";
        }

        public BaiduOAuth2(NameValueCollection f)
        {
            _otype = 1;
            _state = f["state"];
            if (string.IsNullOrWhiteSpace(_state))
            {
                Error.GotoError("没有获得state");
            }

            var url = string.Format("https://openapi.baidu.com/oauth/2.0/token?grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}", f["code"], AppID, AppKey, System.Web.HttpContext.Current.Request.Url.GetComponents(UriComponents.Path | UriComponents.SchemeAndServer, UriFormat.Unescaped));
            var s = PubFunc.SendHTTP(url);

            var o = OAuth2.GetJsonDict<Dictionary<String,String>>(s);

            if (o == null)
            {
                Error.GotoError("无法获得对象" + s);
            }

            if (!o.TryGetValue("access_token",out  _access_token) || _access_token == "")
            {
                Error.GotoError("无法获得access_token"+s);
            }

            var temp = "";
            if (!o.TryGetValue("expires_in", out  temp) || temp == "")
            {
                Error.GotoError("无法获得expires_in" + s);
            }
            _expire = DateTime.Now.AddSeconds(PubFunc.GetInt(temp));

            if (!o.TryGetValue("refresh_token", out _refresh_token) || _refresh_token == "")
            {
                Error.GotoError("无法获得refresh_token" + s);
            }
        }

        /// <summary>
        /// 获取配额信息
        /// </summary>
        public IDictionary<string,string> PCSGetQuota()
        {
            var url = "https://pcs.baidu.com/rest/2.0/pcs/quota?method=info&access_token=" + Access_token;
            var s = PubFunc.SendHTTP(url);
            return OAuth2.GetJsonDictionary(s);
        }

        /// <summary>
        /// 上传单个文件
        /// http://developer.baidu.com/wiki/index.php?title=docs/pcs/rest/file_data_apis_list
        /// </summary>
        /// <returns></returns>
        public  IDictionary<string, string> PCSUploadSingleFile(string filePath,byte[] fileByte)
        {            

            var boundary=System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(DateTime.Now.ToString(),"MD5");

            var postContent="--" + boundary + "\r\n"
                +"Content-Disposition: form-data; name=\"file\"; filename=\"{$fileName}\"\r\n"
                +"Content-Type: application/octet-stream\r\n\r\n"
                +"aaaaa" + "\r\n"
                +"--" + boundary + "\r\n";


            var basePath = "/apps/msosky/torr/";
            var finalPath = basePath + (filePath.StartsWith("/") ? filePath.Substring(1) : filePath);
            var url = String.Format("https://c.pcs.baidu.com/rest/2.0/pcs/file?method=upload&access_token={0}&path={1}&ondup={2}", Access_token, HttpUtility.UrlEncode(finalPath), "overwrite"); ////newcopy
            var helper=new HttpHelper();
            var item=new HttpItem(){
                Method="post",
                ContentType="multipart/form-data; boundary=" + boundary,
                Postdata=postContent,
                URL=url,
            };
            var s=helper.GetHtml(item).Html;
            return OAuth2.GetJsonDictionary(s);
        }

    }
}
