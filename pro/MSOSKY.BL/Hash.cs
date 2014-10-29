using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace MSOSKY.BL
{
    public class Hash
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kw"></param>
        /// <param name="pi"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static string Search(string kw, int pi)
        {
            var totalCount=0;
            var pageCount = 0;
            var msecs = 0;
            var l = ATS.DoSearch(kw, pi, out totalCount, out pageCount,out msecs);
            return PubClass.T2J(new { total = totalCount, pages = pageCount,msecs=msecs, list = l });
        }
    }
}
