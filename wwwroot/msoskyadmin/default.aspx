<%@ Page Language="C#" Inherits="CFTL.CFPage"  %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Utility" %>


<!DOCTYPE html>
<html>
    <head>
        <title>MSOSKY 后台</title>
        <link rel="stylesheet" type="text/css" href="/res/Zebra_Dialog/css/flat/zebra_dialog.css" />
    </head>

    <body>
        <a href="javascript:delIndex()">删除索引</a>
        <a href="javascript:updateIndex()">更新索引</a>
    
    <script type="text/javascript" src="/res/js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/res/Zebra_Dialog/javascript/zebra_dialog.js"></script>
    <script type="text/javascript">
        
        function updateIndex() {
            var uizd = new $.Zebra_Dialog("正在更新索引", { buttons: false });
            $.ajax({
                type: 'post',
                data: {},
                url: "/msoskyadmin/index/updateindex.aspx",
                error: function (XmlHttpRequest, textStatus, errorThrown) { alert(XmlHttpRequest.responseText); },
                success: function (d) {
                    uizd.close();
                    var data;
                    try {
                        data = eval("(" + d + ")");
                        if (data.result == 1) {
                            $.Zebra_Dialog(("索引更新成功！耗时：" + data.time + "毫秒"));
                        } else {
                            $.Zebra_Dialog(("索引更新失败！理由：" + data.description + "，耗时：" + data.time + "毫秒"));
                        }
                    }
                    catch (e) {
                        $.Zebra_Dialog(d, {width:"1000"});
                    }

                }
            });
        }

        function delIndex() {
            var uizd = new $.Zebra_Dialog("正在删除索引", { buttons: false });
            $.ajax({
                type: 'post',
                data: {},
                url: "/msoskyadmin/index/delindex.aspx",
                error: function (XmlHttpRequest, textStatus, errorThrown) { alert(XmlHttpRequest.responseText); },
                success: function (d) {
                    uizd.close();
                    var data;
                    try {
                        data = eval("(" + d + ")");
                        if (data.result == 1) {
                            $.Zebra_Dialog(("索引删除成功！耗时：" + data.time + "毫秒"));
                        } else {
                            $.Zebra_Dialog(("索引删除失败！理由：" + data.description + "，耗时：" + data.time + "毫秒"));
                        }
                    }
                    catch (e) {
                        $.Zebra_Dialog(d, { width: "1000" });
                    }

                }
            });
        }
    </script>

    </body>

</html>