<%@ Page Language="C#" Inherits="CFTL.CFPage" %>
<%@ Register Src="~/inc/srhFromUC.ascx" TagName="SearchFrom" TagPrefix="MSOUC" %>

<script runat="server">
    public string keyWord = "";
</script>

<%
    String hashId = RequestForm["hid"] == null ? "" : RequestForm["hid"].Replace("'", "");
    if (string.IsNullOrWhiteSpace(hashId))
    {
        Response.Redirect("/",false);
    }
    
    DataRow curHashRow = MSOSKY.BL.MSODB.oDB.GetSQLSingleRow("select * from dht_sum where id='" + hashId + "'");
    DataRow detailRow = MSOSKY.BL.MSODB.oDB.GetSQLSingleRow("select * from dht_hashdetail where hashid='" + hashId + "'");

    string detailStr = detailRow["detail"].ToString();
    List<Dictionary<String, String>> detailList=null;
    if(!string.IsNullOrWhiteSpace(detailStr))
    {
        detailList = PubClass.J2T<List<Dictionary<string, string>>>(detailStr);
    }
%>

<!DOCTYPE html>
<html>
    <head>
        <title><%=curHashRow["keycontent"] + PubClass.MSOTitleKey%></title>
        <meta name="keywords" content="<%=PubClass.MSOKeyWords %>" />
        <meta name="description" content="<%=PubClass.MSODescription %>" />
        <meta http-equiv="X-UA-Compatible" content="IE=Edge">
        <meta charset="UTF-8">
        <!--#include file="/res/inc/staticfile.html"-->
        <script type="text/javascript">
            writeComponents("ZeroClipboard/ZeroClipboard");
        </script>
    </head>
    <body>

        <!--#include file="/res/inc/header.aspx"-->
        <MSOUC:SearchFrom ID="SearchFrom1" KeyWord="<%# keyWord%>" runat="server" />
        <div class="container">   
            <div class="col-lg-9">

            <h3><%=curHashRow["keyContent"] %></h3>
            <p>
                创建时间：<%=DateTime.Parse(curHashRow["recvTime"].ToString()).ToString("yyyy-MM-dd") %><br />
                更新时间：<%=DateTime.Parse(curHashRow["updateTime"].ToString()).ToString("yyyy-MM-dd")%><br />
                文件数：<%=curHashRow["fileCnt"]%><br />
                文件大小：<%=PubClass.FormatFileSize(Double.Parse(curHashRow["totalsize"].ToString()))%><br />
                热度：<%=curHashRow["recvTimes"]%><br /><br />
                <abbr title="磁力链接">磁力链接下载</abbr>：<span><a id="t_marglink" href="javascript:void(0)">magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%></a></span><a href="javascript:void(0)" class="btn btn-link btn-sm" id="a_sel_marglink"> 复制磁力链接</a> 
            </p>
            <%if (PubFunc.GetInt(curHashRow["type"]) == 1)
              { %>
            <p>
                试试<abbr title="云播">云播</abbr><br />
                <a target="_blank" href="http://yun.dybeta.com/playapi.php?u=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>&method=high">云播放地址1</a>
                <a target="_blank" href="http://www.weivod.com/#!u=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>">云播放地址1</a>
                <a target="_blank" href="http://www.2yun.net/#!u=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>">云播放地址1</a>
                <a target="_blank" href="http://www.huoyan.tv/api.php#!u=<%=curHashRow["hashKey"]%>">云播放地址1</a>
                <a target="_blank" href="http://www.okdvd.com/okapi.php#!url=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>">云播放地址1</a>   
                <a target="_blank" href="http://vod.7ibt.com/index.php?url=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>">云播放地址1</a>
                <a target="_blank" href="http://api.yundianbo.tv/?u=magnet:?xt=urn:btih:<%=curHashRow["hashKey"]%>">云播放地址1</a>
            </p>
            <%} %>
            <h4>文件列表</h4>

            <div style="">               
                <ul class="list-unstyled">
                    <%
                        if (detailList == null)
                        {
                    %>
                        <li>没有数据！</li>
                    <%
                        }
                        else
                        {
                            foreach (Dictionary<string, string> d in detailList)
                            {
                            %>
                                <li><%=d["n"] %> 大小：<%=PubClass.FormatFileSize(Double.Parse(d["s"])) %> </li>
                            <%
                            }
                        }
                    %>    
                </ul>
            </div>
      

        </div>

        <!--#include file="/res/inc/righttool.aspx"-->
        <!--#include file="/res/inc/ads.aspx"-->
        <!--#include file="/res/inc/statistic.aspx"-->

    </body>
        
        <script type="text/javascript">
            $(function () {
                ZeroClipboard.setMoviePath("/res/ZeroClipboard/ZeroClipboard.swf");
                clip = new ZeroClipboard.Client();
                clip.setHandCursor(true);
                clip.setText($("#t_marglink").text());
                clip.glue("a_sel_marglink");
                clip.addEventListener("complete", function () {
                    $.Zebra_Dialog("复制成功！您可以将磁力链接粘贴到迅雷下载任务中，使用迅雷下载。");
                });
                $(window).resize(function () {
                    clip.reposition();
                });
            });
        </script>

</html>





