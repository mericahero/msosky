using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CFTL;
using COM.CF.Web;
using COM.CF;

namespace MSOSKY.Web
{
    public class Hash:CFCtrlPage
    {
        [Page(enPageType.SelfPage, false)]
        private void Search()
        {
            Response.Write(BL.Hash.Search(RequestForm["kw"],PubFunc.GetInt(RequestForm["pi"])));
        }
    }
}
