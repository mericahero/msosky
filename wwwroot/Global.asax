<%@ Application Language="C#" Inherits="CFTL.CFGlobal" %>

<%@ Import Namespace="COM.CF" %>
<%@ Import Namespace="COM.CF.Web" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="log4net.Config" %>
<%@ Import Namespace="System.Threading" %>

<%@ Import Namespace="System.Web.Configuration" %>




<script runat="server">
    

    public void Application_Start(object sender,EventArgs e)
    {
        base.Application_Start(sender, e);
        UpdateIndex();
        
    }
    
	private void UpdateIndex()
	{
		string str = "";
		ThreadPool.QueueUserWorkItem((object obj) => {
			while (true)
			{
				DateTime t1 = DateTime.Now;
				bool flag =MSOSKY.BL.ATS.CreateOrUpdateIndex(false, out str);
				double totalTime = (DateTime.Now - t1).TotalMilliseconds;
				if (!flag)
				{
					Logger.W.Info(string.Format("{{result:'{0}',description:'{1}',time:'{2}'}}", 0, string.Concat("索引更新失败", str), totalTime));
				}
				else
				{
					Logger.W.Info(string.Format("{{result:'{0}',description:'{1}',time:'{2}'}}", 1, "索引更新成功", totalTime));
				}
				Thread.Sleep(28800000);
			}
		});
		ThreadPool.QueueUserWorkItem((object obj) => {
			while (true)
			{
				if (DateTime.Now.Hour != 3)
				{
					Thread.Sleep(1800000);
				}
				else if (Math.Abs(DateTime.Now.Minute - 30) < 2)
				{
					MSOSKY.BL.MSODB.oDB.ExecuteNonQuery("ALTER INDEX [IX_dht_hashdetail] ON [dbo].[dht_hashdetail] REBUILD PARTITION = ALL WITH ( PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, ONLINE = OFF, SORT_IN_TEMPDB = OFF )");
					Thread.Sleep(240000);
					Logger.W.Info("更新数据库索引成功");
				}
				else
				{
					Thread.Sleep(120000);
				}
			}
		});
	}

		


</script>
