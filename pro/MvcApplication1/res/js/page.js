var d = null,
    MSO = {};

MSO.QueryString = (function () {
    var s1;
    var q = {}
    var s = document.location.search.substring(1);
    s = s.split("&");
    for (var i = 0, l = s.length; i < l; i++) {
        s1 = s[i].split("=");
        if (s1.length > 1) {
            var t = s1[1].replace(/\+/g, " ")
            try {
                q[s1[0]] = decodeURIComponent(t)
            } catch (e) {
                q[s1[0]] = unescape(t)
            }
        }
    } //for
    return q
})();



$(function () {
    //移动到顶部
    $("#js-gotop").click(function () {
        window.scrollTo(0, 0);
        return false;
    });
    //移动到底部
    $("#js-gobottom").click(function () {
        window.scrollTo(0, 99999);
        return false;
    });
    //打开扩展 //百度分享的显示与隐藏
    $("#js-addact").mouseenter(function () {
        $("#mso_panel_baidushare").hide();
    });

    $("#js-addact").mouseover(function () {
        $("#mso-sharepanel").removeClass("hidden");
    }).mouseout(function () {
        $("#mso-sharepanel").addClass("hidden");
    });

    $("#mso_btn_share").mouseover(function () { $("#mso_panel_baidushare").show(); });
    //$("#mso_panel_baidushare").mouseout(function () { $(this).hide(); });



    //页面执行
    goTopOrBottom();
    //页面滚动时执行
    $(window).scroll(function () { goTopOrBottom(); });
    $(window).resize(function () { goTopOrBottom(); });


    //在搜索框里输入当前搜索关键字
    //$("#mso_srh_wd").val(MSO.QueryString.kw);



    $("#mso_btn_suggest").click(function () {
        var suggestHtml = '<div class=" form-horizontal"><div class="form-group"><label class="col-sm-10">有啥建议：</label><div class="col-sm-10"><input type="text" class="form-control" size="25" id="suggest_title"  /></div></div><div class="form-group"><label class="col-sm-10">详细说说：</label><div class="col-sm-10"><textarea cols="25" class="form-control" rows="5" id="suggest_detail"></textarea></div></div><div class="form-group pull-right col-sm-10"><input type="button" class="btn btn-default" id="suggest_submit" onclick="suggestionsubmit()" value="提交" /><input type="button" class="btn btn-warning" onclick="dclose()" id="suggest_cancel" value="取消" /></div></div>'
        d = $.Zebra_Dialog("<strong>提出您宝贵的建议</strong>", {
            source: { "inline": suggestHtml },
            type: "information",
            buttons: false,
            width: "600",
            height: "400"
        });
    });
    $("#suggest_cancel").click(function () { });
    $("#suggest_submit").click(function () {
        

    });
});


function suggestionsubmit() {
    $.ajax({
        url: "/class/MSOSKY/Suggestion.aspx",
        data: { act: "AddSuggestion", title: $("#suggest_title").val(), detail: $("#suggest_detail").val() },
        error: function (data) { alert("erorr:" + data); },
        success: function (data) {
            var r = eval("(" + data + ")");
            alert("谢谢您宝贵的建议");
            dclose();
        }
    });
}

function goTopOrBottom() {
    var scrollTop = Math.max((document.documentElement || document.body).scrollTop, document.body.scrollTop);
    var scrollHeight = Math.max((document.documentElement || document.body).scrollHeight, document.body.scrollHeight);
    if (scrollTop > 50) {
        $("#js-gotop").show();
    } else {
        $("#js-gotop").hide();
    }
    if (scrollTop > scrollHeight - $(window).height() - 50) {
        $("#js-gobottom").hide();
    } else {
        $("#js-gobottom").show();
    }
}

function setSearchType(v) {
    $("#srhTypeNavigater").children().removeClass("active").eq(v ? v : 0).addClass("active");
    $("#srhType").val(v);
}

function checkSrhForm() {
    if ($("#mso_srh_wd").val() == "") return false;
    window.location.href = "/list/" + ($("#srhType").val() == 0 ? "" : $("#srhType").val() + "/") + $("#mso_srh_wd").val();
    return false;
}

function dclose() {
    d.close();
}

