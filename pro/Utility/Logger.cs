using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.CF;
using log4net;
using log4net.Config;

namespace Utility
{
    public class Logger 
    {
        private static ILog m_logger;

        public static ILog W
        {
            get
            {
                return m_logger;
            }
        }


        static Logger()
        {
            XmlConfigurator.Configure();
            m_logger = LogManager.GetLogger("defaultLogger");
        }
    }
}
