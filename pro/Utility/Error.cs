using System;
using System.Web;

namespace Utility
{
	public class Error
	{
		public Error()
		{
		}

		public static void GotoError(string msg)
		{
			HttpContext.Current.Response.Write(msg);
			HttpContext.Current.Response.End();
		}
	}
}