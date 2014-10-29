/**
* [MSO public util]
* @author chenchen
* @version 0.0.1
* @time 2014-7-1
*/


/**
* MSO对象
*/
var MSO = {};

MSO.Conf = {
    DefaultDomAry: ["127.0.0.1"],
    DefaultDom: "http://127.0.0.1",
    Img0Host: "http://127.0.0.1",
    Img80Host: "http://127.0.0.1",
    WWWHost: "http://127.0.0.1",
    AdminHost: "http://127.0.0.1",
    LoginHost: "http://127.0.0.1",
    PayHost: "http://127.0.0.1"

};




MSO.Util = {};

MSO.version = "0.0.1"

/*
* 环境，包括浏览器，设备，手机版设置、判断
*/
MSO.Environment = {
    browser: (function () {
        var dataBrowser = [{ string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
                            { string: navigator.userAgent, subString: "OmniWeb", versionSearch: "OmniWeb/", identity: "OmniWeb" },
                            { string: navigator.vendor, subString: "Apple", identity: "Safari", versionSearch: "Version" },
                            { prop: window.opera, identity: "Opera", versionSearch: "Version" },
                            { string: navigator.vendor, subString: "iCab", identity: "iCab" },
                            { string: navigator.vendor, subString: "KDE", identity: "Konqueror" },
                            { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
                            { string: navigator.vendor, subString: "Camino", identity: "Camino" },
                            { subString: "Netscape", identity: "Netscape" }, // for newer Netscapes (6+)string: navigator.userAgent,
                            {string: navigator.userAgent, subString: "MSIE", identity: "Explorer", versionSearch: "MSIE" },
                            { string: navigator.userAgent, subString: "Gecko", identity: "Mozilla", versionSearch: "rv" },
                            { string: navigator.userAgent, subString: "Mozilla", identity: "Netscape", versionSearch: "Mozilla"}], // for older Netscapes (4-)
            dataOS = [{ string: navigator.platform, subString: "Win", identity: "Windows" },
                        { string: navigator.platform, subString: "Mac", identity: "Mac" },
                        { string: navigator.userAgent, subString: "iPhone", identity: "iPhone/iPod" },
                        { string: navigator.platform, subString: "Linux", identity: "Linux"}];
        function searchString(data) {
            for (var i = 0; i < data.length; i++) {
                var dataString = data[i].string;
                var dataProp = data[i].prop;
                this.versionSearchString = data[i].versionSearch || data[i].identity;
                if (dataString) {
                    if (dataString.indexOf(data[i].subString) != -1)
                        return data[i].identity;
                } else if (dataProp)
                    return data[i].identity;
            }
        }

        function searchVersion(dataString) {
            var index = dataString.indexOf(this.versionSearchString);
            if (index == -1) return;
            return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
        }
        return {
            name: searchString(dataBrowser) || "An unknown browser",
            version: searchVersion(navigator.userAgent) || searchVersion(navigator.appVersion) || "an unknown version",
            os: searchString(dataOS) || "an unknown OS"
        };
    } ()),
    device: (function () {
        var u = navigator.userAgent,
            app = navigator.appVersion;
        return { //移动终端浏览器版本信息
            trident: u.indexOf('Trident') > -1, //IE内核
            presto: u.indexOf('Presto') > -1, //opera内核
            webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
            mobile: !!u.match(/AppleWebKit.*Mobile.*/), //|| !!u.match(/AppleWebKit/), //是否为移动终端
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
            iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
            iPad: u.indexOf('iPad') > -1, //是否iPad
            webApp: u.indexOf('Safari') == -1 //是否web应该程序，没有头部与底部
        };
    } ()),
    language: (navigator.browserLanguage || navigator.language).toLowerCase()
};

/*******************************Validate*********************************/

MSO.Validate = {
    /***
    *param:v  Array [{"value":"XXX","name":"XXX"},{"value":"XXX","name":"XXX"}]
    ******/
    checkMoney: function (v) {
        if (v.length > 0) {
            var regs = /^([0-9])+(\.){0,1}([0-9]){0,2}$/g;
            for (var i = 0; i < v.length; i++) {
                var val = HZTG.PUB.trim(v[i].value);
                var nam = HZTG.PUB.trim(v[i].name);
                if (val == "") {
                    alert(nam + "不能为空");
                    return false;
                } else {
                    if (!val.match(regs)) {
                        alert(nam + "允许输入数字和小数点，只能保留2位小数");
                        return false;
                    }
                } //end if val
            } //end for
            return true;
        } else {
            alert("数据错误"); return false;
        }
    }, //end money
    intFill: function (v) {
        var n = $.trim(v),
              regn = /\D/g;
        return n.replace(regn, "");
    },
    card: function (v) {
    },
    isTel: function (v) {
        var flag = /^(13|15|18)[0-9]{9}$/.test($.trim(v));
        if (!flag) {
            return false;
        }
        return true;
    },
    isEmail: function (v) {
        var flag = /^[\w\-\.]+\w@[\w\-]+(\.[\w\-]+)*\.[A-Za-z]{2,}$/.test($.trim(v));
        if (!flag) {
            return false;
        }
        return true;
    },
    compareDate: function (dt1, dt2) {
        if (dt1 == "" || dt2 == "") return true;
        else {
            var reg = /-/g;
            return Date.parse(dt1.replace(reg, "/")) - Date.parse(dt2.replace(reg, "/")) < 0;
        }
    },
    /**
    * 允许小数点后面两位
    * param:c 是否允许为负值，c有值即允许为负值
    * param:w 保留小数的位数
    */
    doubleFill: function (s, w, c) {
        var str = $.trim(s);
        if (c) {
            var reg5 = /^[a-zA-Z\!\@\#\$\%\^\&\*\(\)\+\=\|\\\/\?]/,
                   reg1 = /[a-zA-Z\!\@\#\$\%\^\&\*\(\)\+\=\|\\\/\?\,\;\:]/g,
                   reg2 = /^([\-]?\d*)(\.)(\d{1,2})(.*)$/g,
                   reg4 = /(\.{1,}\D)/g,
                   reg6 = /(\-{1,}\D)/g,
                   reg7 = /[\u4e00-\u9fa5]+/g;
            if (w == 4) reg2 = /^([\-]?\d*)(\.)(\d{1,4})(.*)$/g;
            return (((((str.replace(reg5, "")).replace(reg1, "")).replace(reg2, "$1$2$3")).replace(reg4, ".")).replace(reg6, "-")).replace(reg7, "");
        } else {
            var reg1 = /[a-zA-Z\!\@\#\$\%\^\&\*\(\)\-\+\=\|\\\/\?\,\;\:]/g,
                   reg2 = /^(\d*)(\.)(\d{1,2})(.*)$/g,
                   reg4 = /(\.{1,})/g,
                   reg7 = /[\u4e00-\u9fa5]+/g;
            if (w == 4) reg2 = /^([\-]?\d*)(\.)(\d{1,4})(.*)$/g;
            return (((str.replace(reg1, "")).replace(reg2, "$1$2$3")).replace(reg4, ".")).replace(reg7, "");
        }
    }

}



/*******************************扩展系统类方法*********************************/

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{\{|\{(\d+)\}/g, function (m, i, o, n) {
        if (m == "{{") return "{";
        return args[i];
    });
}

String.prototype.append = function (s) {
    return this + s;
}


/*******************************数组、字符串操作*********************************/

/**
* 如果是数组则返回，否则封装到数组中
* @param a 传入的对象
* @return 数组
*/

MSO.Util.toArray = function (a) {
    if (a.length == null)
        return new Array(a)
    else
        return a
}

/***
* @action 去除字符串左边空格
* @param  str String
* @return String
**/
MSO.Util.lTrim = function (str) {
    return str.replace(/(^\s*)/g, "");
};

/***
* @action 去除字符串右边空格
* @param  str String
* @return String
**/
MSO.Util.rTrim = function (str) {
    return str.replace(/(\s*$)/g, "");
};
/***
* @action 去除字符串左右两边空格
* @param  str String
* @return String
**/
MSO.Util.trim = function (str) {
    return str.replace(/(?:^[ \t\n\r]+)|(?:[ \t\n\r]+$)/g, '');
}
/**
* 判断字符串开头
* @param  外字符串
* @param  内字符串
* @return 返回bool，如果内字符串在外字符串结尾，返回true，否则false
*/
MSO.Util.endWith = function (a, b) {
    var aL = a.length;
    var bL = b.length;
    var start = a.indexOf(b)
    if (aL - start > bL) return false;
    var s = a.substring(start, aL)
    if (s == b)
        return true;
    return false;
}
MSO.Util.startWith = function (a, b) {
    var aL = a.length;
    var bL = b.length;
    var s = a.substring(0, bL)
    if (s == b)
        return true;
    return false;
}
/**
* String.Format方法 类C#
*/
MSO.Util.stringFormat = function () {
    return arguments[0].format(arguments.splice(0, 1));
}

/*******************************域、cookie操作*********************************/

MSO.Util.getDom = function (domain) {
    if (domain) return ";domain=" + domain;
    var d = document.domain.toLowerCase();
    var a = MSO.Conf.DefaultDomAry;
    for (var i in a)
        if (MSO.Util.endWith(d, a[i])) return ";domain=" + a[i];
    return ":domain=" + MSO.Conf.DefaultDom;
}
/***
* @action 设置cookie
* @param  name    String cookie名
* @param  value   String 值
* @param  expires String 有效期
* @param  path    String 可用路径
* @param  domain  String 作用域
* @param  secure  String 是否https才能访问该值;默认为空，都能访问
* @return
**/
MSO.Util.setCookie = function (name, value, expires, path, domain, secure) {
    var str = name + "=" + encodeURI(value);
    if (expires && (typeof expires == 'number' || expires.toUTCString)) {
        var date;
        if (typeof expires == 'number') {
            date = new Date();
            date.setTime(date.getTime() + (expires * 24 * 60 * 60 * 1000)); //按天数控制cookie的有效期
        } else {
            date = expires;
        }
        str += '; expires=' + date.toUTCString();
    }
    path = path ? path : "/"
    str += "; path=" + path;

    str += this.getDom(domain)

    if (secure) {
        str += "; secure";
    }
    document.cookie = str;
};
/*
* @action 获取cookieName 的值
* @param cookieName String
* @return String
*/
MSO.Util.getCookie = function (cookieName) {
    var t = document.cookie.split(";");
    for (var i = t.length - 1; i >= 0; i--) {
        var tt = t[i].split("=");
        if (((tt[0].charAt(0) == " ") ? tt[0].substring(1) : tt[0]) == cookieName)
            return decodeURI(tt[1])
    }
    return null;

}

/**
* @param cookieName String 要删除的cookie名*
**/
MSO.Util.delCookie = function (cookieName) {
    document.cookie = cookieName + "=a" + MSO.Util.getDom() + ";path=/;expires=Mon, 28 Dec 1998 23:59:59 UTC";
};
/**
* 判断当前是否登录
* @return 登录返回true，否则返回false
*/
MSO.Util.logined = function () {
    if (!this.getCookie("account")) return false;
    if (this.getCookie("bz") == "3") return true;
    if (this.getCookie("L") == "1") return true;
    return false;
}

MSO.Util.logOut = function (autogo) {
    if (MSO.Util.getCookie("L") != null)
        MSO.Util.delCookie("L")
    if (MSO.Util.getCookie("bz") == "3")
        MSO.Util.setCookie("bz", "2", 30);
    top.location = MSO.Conf.LoginHost + "/handle/MSO.Main/User.aspx?act=logout&delay=1&autogo=" + encodeURIComponent(autogo ? autogo : "/");
}


MSO.Util.headInfo = function (a) {
    if (a == "" || a == null) {
        a = MSO.Conf.WWWHost + "/user";
    }
    var account = decodeURIComponent(MSO.Util.getCookie("account"));
    var str = "";
    if (MSO.Util.logined())
        str += '您好<a href="{0}" title="{1}">{2}</a>，欢迎来到<span class="green_c">天天鲜活</span>!<a href="javascript:MSO.Util.logOut(\'{3}\');" >退出</a>'.format(MSO.Conf.WWWHost + "/user", "", account, "/"); //encodeURIComponent(location.href)
    else
        str += '您好，欢迎来到<span class="green_c">天天鲜活</span>!<a href="{0}">登录</a> <a href="{1}">注册</a>'.format(MSO.Conf.LoginHost + "/login.aspx?autogo=" + encodeURIComponent(a), MSO.Conf.LoginHost + "/r.aspx?autogo=" + encodeURIComponent(a));
    return str;
}

/*******************************Date操作*********************************/
MSO.Date = {};
MSO.Date.format = function (d) {
    var y = d.getFullYear();
    var m = d.getMonth() + 1;
    var a = d.getDate();

    if (m < 10) {
        m = "0" + m;
    }
    if (a < 10) {
        a = "0" + a;
    }
    return (y + "-" + m + "-" + a);
}


MSO.Date.getMonthDay = function (d) {
    var dt = d ? d : new Date();
    var y = dt.getFullYear();
    var m = dt.getMonth();
    var nd = new Date(y, m, 0);
    return nd.getDate();
}

MSO.Date.getLastMonthToday = function (d) {
    var dt = d ? d : new Date();
    var y = dt.getFullYear();
    var m = dt.getMonth();
    var yn = m > 1 ? y : y - 1;
    var mn = m > 1 ? m - 1 : 12;
    var nd = new Date(yn, mn, 0);
    dt.setDate(dt.getDate() - nd.getDate());
    return dt;
}





/*******************************AJAX*********************************/
/**
* MSO.AJAX操作
* 分为返回xml和json格式的处理
* 其中Post和Get方法用于处理xml格式的请求及返回
*     Run用于处理json格式的请求及返回
*     需要注意的是，在使用Run时，无须指定ajax的dataType了
*/
MSO.AJAX = {
    XMLError: function (oXml) {
        var o = oXml.documentElement;
        if (!o) {
            alert("网络错误，请稍后再试！");
            return true;
        }
        if (o.nodeName != "error") return false;

        var errcode = o.getAttribute("errcode")
        var errstr = o.text || o.textContent;
        alert(errstr);
        return true;
    },
    XMLCallback: function (d, callBack) {
        if (this.XMLError(d)) return;
        if (typeof callBack != "function") return;
        callBack(d);

    },
    Post: function (url, data, callBack) {
        //不缓存
        $.post(url, data, function (d) {
            MSO.AJAX.XMLCallback(d, callBack);
        })
    },
    Send: function (url, callBack) {
        //有缓存
        $.get(url, function (d) {
            MSO.AJAX.XMLCallback(d, callBack)
        })
    },

    JsonError: function (d) {
        if (!d) {
            alert("返回数据错误");
            return true;
        }
        if (d.r && d.r < 0) {
            if (d.r == -100) {
                //alert("尚未登录，确定后进入登录页面！");
                MSO.Msg.Error("尚未登录，确定后进入登录页面！", function () { window.location.href = "/login.aspx?autogo=" + encodeURIComponent(window.location.href); });
                
                return true;
            }
            MSO.Msg.Error(d.msg);
            //alert(d.msg);
            return true;
        }
        return false;
    },
    JsonCallback: function (d, callBack) {
        var j = {};

        try {
            j = eval("(" + d + ")");
        }
        catch (e) {
            MSO.Msg.Error(e.name + ": " + e.message + "\n" + d);
            //alert(e.name + ": " + e.message + "\n" + d);
            return false;
        }
        if (this.JsonError(j)) return;
        if (typeof callBack != "function") return;
        callBack(j);
    },
    Run: function (p, callBack) {
        $.ajax({
            data: p.data || {},
            type: p.type || "post",
            url: p.url || "",
            async: p.async || false,
            dataType: p.dataType || "text",
            success: function (d) {
                MSO.AJAX.JsonCallback(d, callBack);
            }
        })
    }

};

/*******************************操作页面*********************************/

/**
* 功能说明：添加到收藏
* @param surl String
* @param stitle String
**/
MSO.Util.addFavorite = function (surl, stitle) {
    try {
        window.external.addFavorite(surl, stitle);
    } catch (e) {
        try {
            window.sidebar.addpanel(stitle, surl, "");
        } catch (e) {
            alert("加入收藏失败,请使用ctrl+d进行添加");
        }
    }
}

/**
*功能说明：设为首页
*@param obj Object
*@param vrl  String
*/
MSO.Util.setHome = function (obj, vrl) {
    try {
        obj.style.behavior = 'url(#default#homepage)';
        obj.sethomepage(vrl);
    } catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("此操作被浏览器拒绝!\n请在浏览器地址栏输入'about:config'并回车\n然后将[signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
            }
        } else {
            alert("抱歉，您所使用的浏览器无法完成此操作。\n\n您需要手动设置为首页。");
        }
    }
}


//弹出新窗口
MSO.Util.open = function (u, t, w, h, status) {
    window.open(u, t ? t : "_blank", "status={0},scrollbars=yes,menubar=0,resizable=1,width={1},height={2}".format(status ? 1 : 0, w ? w : 400, h ? h : 300));
}

/**************************************标志位***********************************/
MSO.Status = {
    OrderStatus: ["无操作", "下单完成", "部分付款", "付款完成", "商品出库中", "等待收货", "订单完成", "订单取消申请", "订单取消", "订单删除", "退货", "订单关闭", "拒签"],
    PayType: ["账户余额支付", "支付宝", "银联", "网银"],
    DeliveryTimeP: ["工作日和节假日（周末、法定假日）", "只有工作日", "只有节假日"],
    InvoiceType: ["普通发票（纸质）", "普通发票（电子）", "增值税发票"],
    InvoiceHeadType: ["个人", "单位"],
    InvoiceContentType: ["明细", "办公用品", "电脑配件", "耗材"]
};


MSO.Bank = {
    "CCB": "中国建设银行",
    "ICBCB2C": "中国工商银行",
    "GDB": "广发银行",
    "BOCB2C": "中国银行",
    "CITIC": "中信银行",
    "SPABANK": "平安银行",
    "CIB": "兴业银行",
    "CMB": "招商银行",
    "COMM-DEBIT": "交通银行",
    "SPDB": "浦发银行",
    "SHBANK": "上海银行",
    "NBBANK": "宁波银行",
    "HZCBB2C": "杭州银行",
    "CEB-DEBIT": "中国光大银行",
    "CMBC": "中国民生银行",
    "POSTGC": "中国邮政储蓄"
};

/**************************************checkbox***********************************/


/**
* 选中传入的所有的checkbox
* @param  {[type]} cbs [description]
* @return {[type]}     [description]
*/
MSO.Util.selAllCheckBox = function (cbs) {
    if (cbs == null) return
    var ids = this.toArray(cbs);
    for (var i = 0; i < ids.length; i++)
        ids[i].checked = true;
}
/**
* 不选传入的所有的checkbox
* @param  {[type]} cbs [description]
* @return {[type]}     [description]
*/
MSO.Util.unselAllCheckBox = function (cbs) {
    if (cbs == null) return
    var ids = this.toArray(cbs);
    for (var i = 0; i < ids.length; i++) {
        ids[i].checked = false;
    }
}
/**
* 不选传入的所有的checkbox
* @param  {[type]} cbs [description]
* @return {[type]}     [description]
*/
MSO.Util.selFanCheckBox = function (cbs) {
    if (cbs == null) return
    var ids = this.toArray(cbs);
    for (var i = 0, l = ids.length; i < l; i++)
        ids[i].checked = !ids[i].checked;
}
//如果是数组则返回，否则封装到数组中
MSO.Util.toArray = function (a) {
    if (a.length == null)
        return new Array(a)
    else
        return a
}


/**
* 获得选中框的值，以逗号隔开
* @param  {Array} cbs 一组checkbox
* @param  {Bool} t   是否全部选中
* @return {String}     以逗号隔开的选中的checkbox值
*/
MSO.Util.getCheckBoxValue = function (cbs, t) {
    if (cbs == null) return
    var v = new Array;
    var ids = this.toArray(cbs);
    for (var i = 0; i < ids.length; i++) {
        if (t || ids[i].checked) {
            v.push(ids[i].value);
        }
    }
    return v.toString();
}

/**
* 获取传入的checkbox数组的值，以逗号隔开
* @param  {Array} cbs 一组checkbox
* @return {String}     以逗号隔开的选中的checkbox值
*/
MSO.Util.getAllCheckBoxValue = function (cbs) {
    return this.getCheckBoxValue(cbs, true);
}

/**
* 获取一组选中的复选框的值，每个值之间取按位或
* @param  {[type]} cbs [description]
* @return {[type]}     [description]
*/
MSO.Util.getCheckboxValueOr = function (cbs) {
    if (cbs == null) return;
    var v = new Array;
    var ids = this.toArray(cbs);
    var value = 0;
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].checked == true) {
            value |= ids[i].value;
        }
    }
    return value;
}




/*******************************分类操作*********************************/

/**
* 分类对象
*/
MSO.Category = {};

/**
* 是否是一级分类
* @param  int  c 类别id
* @return 是一级分类返回true否则false
*/
MSO.Category.isFirst = function (c) {
    return (c & 0xffff) == 0;
};

/**
* 是否是二级分类
* @param  int  c 类别id
* @return 是二级分类返回true否则false
*/
MSO.Category.isSecond = function (c) {
    return (c & 0xffff) != 0 && (c & 0xff) == 0;
};
/**
* 是否是三级分类
* @param  int  c 类别id
* @return 是三级分类返回true否则false
*/
MSO.Category.isThird = function (c) {
    return (c & 0xff) != 0;
};


/**
* 获取最大的子分类
* @param  {int} c 类别id
* @return {int} 最大的子分类
*/
MSO.Category.getMaxSubCategory = function (c) {
    if ((c & 0xff) != 0) return c;
    if ((c & 0xff00) != 0) return c | 0xff;
    if ((c & 0xff0000) != 0) return c | 0xffff;
    return 0;
};

/**
* 获取下一个同级分类ID
* @param  {int} c 分类ID
* @return 下一个分类的ID
*/
MSO.Category.getNextCategory = function (c) {
    if ((c & 0xff) != 0) return c + 0x1;
    if ((c & 0xff00) != 0) return c + 0x100;
    if ((c & 0xff0000) != 0) return c + 0x10000;
    return 0;
};

/**
* 获取最大直接子分类
* @param  {int} c 分类ID
* @return 最大直接子分类
*/
MSO.Category.getMaxSubDirCategory = function (c) {
    if ((c & 0xff) != 0) return c;
    if ((c & 0xff00) != 0) return (c | 0xff);
    if ((c & 0xff0000) != 0) return ((c | 0xffff) & ~0xff);
    return 0;
};

/**
* 获取分类的级别
* @param  {int} c 分类ID
* @return 返回分类级别
*/
MSO.Category.getLevel = function (c) {
    if ((c & 0xff) != 0) return 3;
    if ((c & 0xff00) != 0) return 2;
    if ((c & 0xff0000) != 0) return 1;
    return 0;
};

/**
* 获取父分类
* @param  {int} c 分类ID
* @return 父分类
*/
MSO.Category.getParent = function (c) {
    if ((c & 0xff) != 0) return cid & ~0xff;
    if ((c & 0xff00) != 0) return cid & ~0xffff;
    return 0;
};


MSO.Category.getCrumbsList = function (c) {
    var l = [];
    var level = this.getLevel(c);
    if (level >= 2) l.push(c & 0xff0000);
    if (level == 3) l.push(c & 0xffff00);
    l.push(c);
    return l;
}



MSO.Goods = {};

MSO.Goods.UnitObj = {
    "0": "瓶",
    "1": "包",
    "2": "箱",
    "3": "袋",
    "4": "盒",
    "5": "斤",
    "6": "个"
}

MSO.Goods.writeUnitSelect = function (u) {
    var s = '<select name="unit" id="unit"><option value="-1">请选择</option>';
    for (var obj in this.UnitObj) {
        s += '<option value="{0}"{1}>{2}</option>'.format(obj, obj == u ? ' selected="selected"' : "", this.UnitObj[obj]);
    }
    s += '</select>';
    document.write(s);
}

MSO.Goods.getUnit = function (u) {
    return this.UnitObj[u] || "";
}

/**
* 分页对象
*/
MSO.Pager = {
    pageSize: 20,
    curPage: 1,
    totalPage: 1,
    orderField: "",
    orderType: 0,
    pagerclass:"mso_pager"
};
MSO.Pager.pageClick = function (pi,f) {
    return true;
};
MSO.Pager.getPageList = function () {
    var cur = this.curPage,
        total = this.totalPage;
    var s = "";
    if (cur > 1) {
        s += "<a class='prev' href='javascript:MSO.Pager.pageClick(\"{0}\");'>上一页</a>".format(parseInt(cur) - 1);
    }
    if (total > 10) {
        if (cur == 1) {
            s += "<span class='current'>{0}</span>".format(1);
        } else {
            s += "<a href='javascript:MSO.Pager.pageClick(\"{0}\");'>{0}</a>".format(1);
        }

        if (cur - 6 > 0) {
            s += "...";
        }
        var min = parseInt(cur) - 4;
        if (min < 2) {
            min = 2;
        }
        var max = (cur < 6 ? 8 : parseInt(cur) + 2);
        if (max > total - 1) {
            max = total;
        }
        for (var i = min; i < max; i++) {
            if (cur == i) {
                s += "<span class='current'>{0}</span>".format(i);
            } else {
                s += "<a href='javascript:MSO.Pager.pageClick(\"{0}\");'>{0}</a>".format(i);
            }

        }
        if (total - cur > 3) {
            s += "...";
        }

        if (cur == total) {
            s += "<span class='current'>{0}</span>".format(total);
        } else {
            s += "<a href='javascript:MSO.Pager.pageClick(\"{0}\");'>{0}</a>".format(total);
        }

    } else {
        for (var i = 1; i <= total; i++) {
            if (cur == i) {
                s += "<span class='current'>{0}</span>".format(i);
            } else {
                s += "<a href='javascript:MSO.Pager.pageClick(\"{0}\");'>{0}</a>".format(i);
            }
        }
    }

    if (cur < total) {
        s += "<a class='next' href='javascript:MSO.Pager.pageClick(\"{0}\");'>下一页</a>".format(parseInt(cur) + 1);
    }
    return s;
};

MSO.Pager.updatePager = function () {
    $("." + this.pagerclass).html(MSO.Pager.getPageList());
};

MSO.Pager.init = function () {
    this.curPage = 1;
    this.pageClick(this.curPage, true);
}





/***************处理请求参数**********************************/
MSO.Util.loadXML = function (xmlStr) {
    var xmlDoc;
    if (window.ActiveXObject) {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.async = false;
        xmlDoc.load(xmlStr);
    } else if (document.implementation && document.implementation.createDocument) {
        var xmlhttp = new window.XMLHttpRequest();
        xmlDoc = xmlStr;
    } else {
        alert('Your   browser   cannot   handle   this   script');
    }
    return xmlDoc;
};

MSO.Util.print = function (id_str) {

    var el = document.getElementById(id_str);
    var iframe = document.createElement('IFRAME');
    var doc = null;
    iframe.setAttribute('style', 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;');
    document.body.appendChild(iframe);
    doc = iframe.contentWindow.document;
    doc.write('<div>' + el.innerHTML + '</div>');
    doc.close();
    iframe.contentWindow.focus();
    iframe.contentWindow.print();
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        document.body.removeChild(iframe);
    }
}


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
    }
    return q;
})();
MSO.QueryStringByUrl = function (url) {
    var s1;
    var q = {}
    var s = url.toString();
    s = s.split("?")[1];
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
    }
    return q;
};
/***************信息处理**********************************/
MSO.Msg = {};
MSO.Msg.Info = function(m, c, t, b, s) {
    new $.Zebra_Dialog(m, { type: t ? t : false, buttons: b ? b : false, auto_close: s ? s : "500", onClose: c });
}

MSO.Msg.Error = function (m, c, t, b, s) {
    this.Info(m, c, t ? t : "error", b, s);
}

MSO.Msg.Confirm = function (m, c, t, b, s) {
    this.Info(m, c, t ? t : "question", b, s);
}
