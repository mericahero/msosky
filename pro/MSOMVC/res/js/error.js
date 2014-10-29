window.onload = function () {
    //定义用户显示错误信息的元素
    var e1 = document.getElementById("errortype");
    var e2 = document.getElementById("otip");
    var e3 = document.getElementById("oinfo");
    var e4 = document.getElementById("errAct");
    //    var errorObj = {
    //        error:XH.QueryString.error,
    //        errtype:XH.QueryString.errtype,
    //        othertishi: XH.QueryString.othertishi,
    //        otherHTML: XH.QueryString.otherHTML
    //    }
    var autogo = window.location.href;
    if (errorObj.error == "notlogin") {
        //显示错误信息
        e1.innerHTML = "<b>错误等级：</b>一般错误，应用抛出的错误";
        e2.innerHTML = "<b>错误提示：</b>" + "您访问的页面需要登录才能继续操作";
        e3.innerHTML = "<b>其他信息：</b>" + "即将为您跳转到登录页";
        e4.innerHTML = "<p>☉ 如果您对本站有任何疑问、意见、建议、咨询，请联系管理员QQ:123456</p>";

        setTimeout(function () {
            window.location.href = "/login.aspx?autogo=" + encodeURIComponent(autogo);
        }, 0);
        return false;
    } else if (errorObj.errcode == "NotLogined") {
        window.history.go(-1);
        return false;
    } else {
        //$(".cwymbox").show();
        switch (errorObj.errtype) {
            case "error":
                e1.innerHTML = "<b>错误等级：</b>一般错误，应用抛出的错误";
                break;
            case "sys":
                e1.innerHTML = "<b>错误等级：</b>系统错误";
                break;
            case "busy":
                e1.innerHTML = "<b>错误等级：</b>系统繁忙";
                break;
            default:
                break;
        }
        e2.innerHTML = "<b>错误提示：</b>" + errorObj.otip;
        e3.innerHTML = "<b>其他信息：</b>" + (errorObj.ohtml ? errorObj.ohtml : "");

        var actStr = '<hr /><p>☉ 请尝试以下操作：</p>'
                   + '<ul>'
                   + '    <li>单击<a href="javascript:history.back(1)"><span style="color:Red">返回</span></a>重新操作。</li>'
                   + '    <li>前往<a href="/"><span style="color:Red;">鲜活365</span></a>体验品质生活。</li>'
                   + '    <li>前往<a href="javascript:void(0)"><span style="color:Red;">个人中心</span></a>把握个人动态。</li>'
                   + '</ul>'
                   + '<hr />'
                   + '<p>☉ 如果您对本站有任何疑问、意见、建议、咨询，请联系管理员</p>';

        e4.innerHTML = actStr;
    }

}
