<%@ Page Language="C#" Inherits="CFTL.CFPage" %>


<!DOCTYPE html />
<html>
    <head>		
        <title>MAN搜 MAN搜乐园<%=PubClass.MSOTitleKey %></title>
        <meta name="baidu_union_verify" content="1e83bc9dca5215a5d5f9889afe5bd0d5">
        <meta name="google-site-verification" content="PtmiY8qJ0MrpOhDrsGtbbdhQ-HvUqiNhinF7I46AVzs" />
        <meta name="keywords" content="<%=PubClass.MSOKeyWords %>" />
        <meta name="description" content="<%=PubClass.MSODescription %>" />
        <meta name="baidu-site-verification" content="PVb4VNNOom" />
        <meta http-equiv="X-UA-Compatible" content="IE=Edge">
        <meta charset="UTF-8">
        <!--#include file="/res/inc/staticfile.html"-->     
    </head>
    <body>
        <!--#include file="/res/inc/header.aspx"-->
        <div class="container mso-form-container">
            <div class="row">
                <div class="col-lg-4"></div>
                <div class="col-lg-4"><img alt="" src="/res/img/logo1.png" width="270" height="129" /></div>
                <div class="col-lg-4"></div>
            </div>
            <div class="row">
                <div class="col-lg-2"></div>
                <div class="col-lg-7">
                    <ul class="nav nav-tabs" id="srhTypeNavigater">
                        <li class="active"><a href="javascript:setSearchType(0)">全部</a></li>
                        <li><a href="javascript:setSearchType(1)">视频</a></li>
                        <li><a href="javascript:setSearchType(2)">图片</a></li>
                        <li><a href="javascript:setSearchType(3)">音乐</a></li>
                        <li><a href="javascript:setSearchType(4)">电子书</a></li>
                        <li><a href="javascript:setSearchType(5)">程序</a></li>
                        <li><a href="javascript:setSearchType(6)">其它</a></li>
                    </ul>
                </div>
                <div class="col-lg-3"></div>
            </div>
            <div class="row">
                <div class="col-lg-2"></div>
                <div class="tab-content mso-form-srhblock col-lg-7">
                    <form action="list.aspx" class="form-search" onsubmit="return checkSrhForm()">
                    <input type="hidden" name="t" id="srhType" value="0" />
                        <div class="form-group">
                            <input type="search"  placeholder="您想看啥电影？" class="mso-form-control input-lg" id="mso_srh_wd" name="kw" style="width:500px;" />
                            <button type="submit" class="btn btn-danger btn-lg"><i class="glyphicon glyphicon-search"></i><!--搜一下--></button>
                        </div>                            
                    </form>
                </div>
                <div class="col-lg-3"></div>
            </div>
        </div>

        <div class="container mso-recommend">
            <div class="col-lg-offset-1 col-lg-10">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="panel-title">推荐你看</div>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <dl class="dl-horizontal">
                                <dt>电影</dt>
                                <dd>吸血鬼日记</dd>
                                <dd>私人定制</dd>
                                <dd>小时代2</dd>
                            </dl>
                            <dl class="dl-horizontal">
                                <dt>剧集</dt>
                                <dd>宫锁沉香</dd>
                                <dd>时光恋人</dd>
                                <dd>最美的时光</dd>
                            </dl>
                            <dl class="dl-horizontal">
                                <dt>动漫</dt>
                                <dd>火影忍者</dd>
                                <dd>海贼王</dd>
                                <dd>死神</dd>
                            </dl>
                            <dl class="dl-horizontal">
                                <dt>综艺</dt>
                                <dd>中国好歌曲</dd>
                                <dd>快乐大本营</dd>
                                <dd>爸爸去哪儿</dd>
                            </dl>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <footer class="mso-footer">
            <div class="container">
            <div class="row">
                <div class="col-lg-offset-2 col-lg-7 text-center">
                    本站所有内容系自动搜索生成的结果，排列及分类仅为方便使用，本站与内容的出处无关。
                </div>
            </div>
        </div>
        </footer>

        <!--#include file="/res/inc/righttool.aspx"-->
        <!--#include file="/res/inc/statistic.aspx"-->
    </body>

    <script type="text/javascript">
        $(function () {
            $(".mso-recommend dd").bind("click", function () {
                window.location.href = "/list0/" + $(this).text();
            });
        });
    </script>
    
</html>





