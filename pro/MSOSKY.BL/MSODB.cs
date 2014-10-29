using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;

namespace MSOSKY.BL
{
    public class MSODB
    {
        public static DB oDB;
        static MSODB()
        {
            oDB = new DB(CWS.CWConfig.Appset["dhtDB"]);
        }
    }
}
