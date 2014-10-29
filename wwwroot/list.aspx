<%@ Page Language="C#" Inherits="CFTL.CFPage" %>
<%@ Register Src="~/inc/srhFromUC.ascx" TagName="SearchFrom" TagPrefix="MSOUC" %>
<%@ Import Namespace="MSOSKY.BL" %>
<script runat="server">
    
    public string keyWord = "";
    public int totalCount = 0;
    public int pageCount = 0;
    public int pageIndex = 1;
    public int mecs = 0;
    public int srhType = 0;
</script>

<%
    Response.Cache.SetMaxAge(TimeSpan.FromHours(1));
    Response.Cache.SetCacheability(HttpCacheability.Public);
    Response.Cache.SetLastModifiedFromFileDependencies();
    Response.Cache.SetValidUntilExpires(false);
    Response.Cache.VaryByParams["*"] = true;
    keyWord = RequestForm["kw"];
    srhType = PubFunc.GetInt(RequestForm["t"]);
    if (string.IsNullOrWhiteSpace(keyWord))
    {
        Response.Redirect("/", false);
    }
    pageIndex = PubFunc.GetInt(RequestForm["pi"]) == 0 ? 1 : PubFunc.GetInt(RequestForm["pi"]);
    
    List<SearchResult> list = ATS.DoSearch(keyWord, pageIndex, out totalCount, out pageCount, out mecs);

%>
<!DOCTYPE html>
<html>
    <head>
        <title><%=keyWord+PubClass.MSOTitleKey%></title>
        <meta name="keywords" content="<%=PubClass.MSOKeyWords %>" />
        <meta name="description" content="<%=PubClass.MSODescription %>" />
        <!--#include file="/res/inc/staticfile.html"-->
        <link href="/res/lib/bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="/res/lib/jPaginate/css/style.css" rel="stylesheet" type="text/css" />
        <script src="/res/lib/jPaginate/jquery.paginate.js" type="text/javascript"></script>
    </head>
    <body>
        <!--#include file="/res/inc/header.aspx"-->
        <MSOUC:SearchFrom KeyWord="<%# keyWord%>" SearchType="<%# srhType %>" runat="server" />

        <div class="container">
            <div class="row alert " id="d_srhsummary">为您找到相关结果<%=totalCount %>个(耗时<%=mecs %>毫秒)</div>
            <div class="row">
                <div class="holder"></div>
                <%--<div class="mso_pager"></div>--%>
                <div id="itemContainer" class="itemContainer">
                    <%foreach (var item in list)
                      {
                    %>
                    <dl>
                    <dt><a target="_blank" href="/detail/<%=item.HashId %>"><%=item.BodyPreview %></a></dt>
                    <dd class="itemShowTitle"><%=item.KeyContent %></dd>
                    <dd class="itemShowDetail">创建时间：<%=item.UpdateTime %> 文件个数：<%=item.FileCnt %> 文件大小：<%=item.TotalSize %></dd>
                    </dl>
                    <%      
                      } %>
                </div>
                <div class="mso_pager"></div>
                <div class="holder"></div>
			</div>
		</div>        
        <!--#include file="/res/inc/righttool.aspx"-->
        <!--#include file="/res/inc/statistic.aspx"-->
    </body>

	<script type="text/javascript">

        $(function(){
        
            $(".mso_pager").paginate({
				count 		: <%=pageCount %>,
				start 		: <%=pageIndex %>,
				display     : 15,
				border					: false,
				text_color  			: '#888',
				background_color    	: '#EEE',	
				text_hover_color  		: 'black',
				background_hover_color	: '#CFCFCF',
                onChange                : function (p){window.location.href="/list/<%=keyWord %>/" + p}
			});
        });


	    

	    function setSearchType(v) {
	        $("#srhTypeNavigater").children().removeClass("active").eq(v ? v : 0).addClass("active");
	        $("#srhType").val(v);
	    }

			
	</script>
</html>





