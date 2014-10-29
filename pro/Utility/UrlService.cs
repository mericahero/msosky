using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Utility
{
	public class UrlService : IHttpModule
	{
		public string ModuleName
		{
			get
			{
				return "UrlService";
			}
		}

		public UrlService()
		{
		}

		private void Application_BeginRequest(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			string filePath = context.Request.FilePath;
            Regex listReg = new Regex(@"/list((?<type>\d))?/(?<keyword>[^&/]*)(/(?<page>\d+))?", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
			Regex detailReg = new Regex(@"/detail/(?<hashid>\d*)", RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            Regex htmlPageReg=new Regex(@"/(<page>\w*).html",RegexOptions.IgnoreCase | RegexOptions.ECMAScript);


			Match match = listReg.Match(filePath);
            if(match.Success)
            {
                context.RewritePath(string.Format("/list.aspx?t={0}&kw={1}&pi={2}", match.Groups["type"].Value, HttpUtility.UrlEncode(match.Groups["keyword"].Value), match.Groups["page"].Value));
				context = null;
                return;
            }
            match=detailReg.Match(filePath);
            if(match.Success)
            {
                context.RewritePath(string.Format("/detail.aspx?hid={0}", match.Groups["hashid"].Value));
                context = null;
                return;
            }
            match=htmlPageReg.Match(filePath);
            if(match.Success)
            {
                context.RewritePath("/" + match.Groups["page"].Value + ".aspx" + context.Request.Url.Query);
            }
            context=null;
		}

		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(Application_BeginRequest);
		}
	}
}