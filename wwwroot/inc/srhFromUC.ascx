<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.UserControl" %>
<script runat="server">
    public string KeyWord { get; set; }
    public int SearchType { get; set; }
</script>
<%
    DataBind();
 %>
<div class="container mso-form-container">
            <div class="row">
                <div class="col-lg-9" style="height:60px">
                    <a style="float:left;margin-right:15px;" href="/"><img alt="" src="/res/img/logo1.png" height="60" ></a>
                    <div style="display:block;float:left;padding:17px 0 0;height:60px;">
                        <ul class="nav nav-tabs" id="srhTypeNavigater">
                            <li<%=SearchType==0 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(0)">全部</a></li>
                            <li<%=SearchType==1 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(1)">视频</a></li>
                            <li<%=SearchType==2 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(2)">图片</a></li>
                            <li<%=SearchType==3 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(3)">音乐</a></li>
                            <li<%=SearchType==4 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(4)">电子书</a></li>
                            <li<%=SearchType==5 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(5)">程序</a></li>
                            <li<%=SearchType==6 ? " class=\"active\"":"" %>><a href="javascript:setSearchType(6)">其它</a></li>
                        </ul>
                    </div>
                </div>
                <div class="col-lg-3"></div>
            </div>
            <div class="row">
                <div class="tab-content mso-form-srhblock col-lg-9">
                    <form class="form-search" onsubmit="return checkSrhForm()">
                    <input type="hidden" name="t" id="srhType" value="0" />
                        <div class="form-group">
                            <input id="mso_srh_wd" placeholder="您想看啥电影？" type="search" class="mso-form-control input-lg" name="kw" style="width:700px;" value="<%=KeyWord %>" />
                            <button type="submit" class="btn btn-danger btn-lg"><%--<i class="glyphicon glyphicon-search"></i>--%> 搜一下</button>
                        </div>                            
                    </form>
                </div>
                <div class="col-lg-3"></div>
            </div>
        </div>


