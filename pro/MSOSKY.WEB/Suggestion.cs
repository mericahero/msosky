
using MSOSKY.BL;
using System;
using System.Web;
using COM.CF.Web;
using CFTL;

namespace MSOSKY.Web
{
	public class Suggestion : CFCtrlPage
	{
		public Suggestion()
		{
		}

		[Page(enPageType.SelfPage, false)]
		private void AddSuggestion()
		{
			if (MSOSKY.BL.Suggestion.AddSuggestion(RequestForm) <= 0)
			{
				Response.Write("{r:0,m:''}");
			}
			else
			{
				Response.Write("{r:1}");
			}
		}
	}
}