using CWS;
using System;
using System.Collections.Specialized;
using COM.CF;


namespace MSOSKY.BL
{
    public class Suggestion
    {
        public Suggestion()
        {
        }

        public static int AddSuggestion(NameValueCollection f)
        {
            int ip = CWPub.GetIPAsInt32(FWFunc.GetIP());
            string title = PubFunc.GetDefaultStr(f["title"]);
            string detial = PubFunc.GetDefaultStr(f["detail"]);
            int num =MSODB.oDB.ExecuteNonQuery(string.Format("insert into dht_suggestion(title,detail,ip) values('{0}','{1}',{2})", title.Replace("'", ""), detial.Replace("'", ""), ip));
            return num;
        }
    }
}