; $(function () {
    var opt = {
        smallWidth: 420,
        smallHeight: 420,

        bigWidth: 800,
        bigHeight: 800
    };
    var oDemo = $("#d_pcontainer");
    var oWin = $(window);
    var owraper = $("#d_pwraper")
    var oSmall = $("#d_pshow");
    var oBig = $("#d_pbig");
    var obg = $("#s_bg");
    var oMask = $("#d_pmask");

    var oBigImg = null;
    var oSmallImg = null;
    var oBigImgWidth = opt.bigWidth;
    var oBigImgHeight = opt.bigHeight;

    var iBwidth = oBig.width();
    var iBheight = oBig.height();

    oBig.hide();

    var iTop = owraper.position().top;
    var iLeft = owraper.position().left;
    var iWidth = owraper.width();
    var iHeight = owraper.height();
    var iSpeed = 200;

    var setOpa = function (o) {
        o.style.cssText = "opacity:0;filter:alpha(opacity:0);"
        return o;
    };

    var imgs = function () {
        //if (typeof opt !== "object") return false;
        oBigImg = oBig.children("img");
        oSmallImg = oSmall.children("img");

        return {
            bigImg: setOpa(oBigImg[0]),
            smallImg: setOpa(oSmallImg[0])
        };
    };
    //追加
    var append = function (o, img) {
        o.append(img);
        $(img).animate({ opacity: 1 }, iSpeed * 2, null, function () { this.style.cssText = ""; });
    };
    //移动
    var eventMove = function (e) {
        var e = e || window.event;
        var w = oMask.width();
        var h = oMask.height();
        var x = e.clientX - iLeft + oWin.scrollLeft() - w / 2;
        var y = e.clientY - iTop + oWin.scrollTop() - h / 2;

        var l = iWidth - w - 10;
        var t = iHeight - h - 10;

        if (x < 0) {
            x = 0;
        } else if (x > l) {
            x = l;
        };
        if (y < 0) {
            y = 0;
        } else if (y > t) {
            y = t;
        };
        oMask.css({ left: x < 0 ? 0 : x > l ? l : x, top: y < 0 ? 0 : y > t ? t : y });
        var bigX = x / (iWidth - w);
        var bigY = y / (iHeight - h);
        oBigImg.css({ left: bigX * (iBwidth - oBigImgWidth), top: bigY * (iBheight - oBigImgHeight) });
        return false;
    };

    var eventOver = function () {
        oMask.show();
        obg.stop().animate({ opacity: .1 }, iSpeed);
        oBig.show().stop().animate({ opacity: 1 }, iSpeed / 2);
        return false;
    };

    var eventOut = function () {
        oMask.hide(); obg.stop().animate({ opacity: 0 }, iSpeed / 2);
        oBig.stop().animate({ opacity: 0 }, iSpeed, null, function () { $(this).hide(); });
        return false;
    };

    var _init = function (object, oB, oS, callback) {
        var num = 0;
        oBig.css("opacity", 0);
        append(oB, object.bigImg);
        append(oS, object.smallImg);
        if (num === 0) {
            callback.call(object.smallImg);
        }
        object.bigImg.onload = object.smallImg.onload = function () {
            num += 1;
        };
    };

    // 初始化  继续写
    _init(imgs(), oBig, oSmall, function () {
        //绑定事件
        oWin.resize(function () {
            iTop = owraper.position().top;
            iLeft = owraper.position().left;
            iWidth = owraper.width();
            iHeight = owraper.height();

        });
        oSmall.hover(eventOver, eventOut).mousemove(eventMove);
    });
});





; (function ($) {
    $.extend($.fn, {
        scrollGoodsPicture: function (options) {
            var defaults = {};
            options = $.extend(defaults, options);
            var self = this,
                $bImg = $("#d_pshow img:first"),
                $bbImg = $("#d_pbig img:first"),
                $sContainer = $("#d_psmallc"),
                $rCount = 6,
                $sUl = $("#u_pics"),            
                $sLi = $("#d_psmallc li");
                $sUl.html($sLi.length>$rCount ? $sUl.html()+$sUl.html() : $sUl.html());
                $sLi = $("#d_psmallc li");
            var $sImg = $("#d_psmallc li img"),
                $left = $("#s_psmall_left"),
                $right = $("#s_psmall_right"),
                $singleOne = $("#d_psmallc li:first"),
                $sLiHeight = $singleOne.height() + parseInt($singleOne.css("border-top-width")) * 2,
                $sLiLen = $singleOne.width() + parseInt($singleOne.css("border-left-width")) * 2 + parseInt($singleOne.css("margin-left")) * 2,
                
                $liCount=$("#d_psmallc li").length;
            //var $rSum = Math.ceil($sLi.length / $rCount);
            var S = {
                init: function () {
                    if($liCount>=$rCount * 2){
                        $left.live("click", this.clickOnleft);
                        $right.live("click", this.clickOnRight);
                    }
                    $.each($sLi, function (k, v) {
                        $(v).live("mouseover", function () { S.mouseOnImg(k); });
                    });
                    //$sLi.bind("mouseover",this.mouseOnImg);
                    //图片集外面的DIV宽 高
                    $sContainer.width($sLiLen * $rCount);
                    $sContainer.height($sLiHeight);
                    $sContainer.css({ overflow: "hidden" });

                    //总行数
                    //$rSum = Math.ceil($sLi.length / $rCount);
                    //Ul宽
                    $sUl.width($sLiLen * $liCount);
                    //图片等比例
                    this.scrollResize();
                },
                rNum: function () { return Math.floor($left.attr("showPicNum") / $rCount); },
                clickOnleft: function () {
                    if (parseInt($sUl.css("margin-left")) < -$sUl.width() / 2) {
                        $sUl.css("margin-left", "0");
                    } else if (parseInt($sUl.css("margin-left")) > 0) {
                        $sUl.css("margin-left", '-' + $sul.width() / 2 + 'px');
                    }
                    $sUl.css("margin-left", (parseInt($sUl.css("margin-left")) - $sLiLen) + "px");
                    
                },
                clickOnRight: function () {
                    if (parseInt($sUl.css("margin-left")) >= 0) {
                        $sUl.css("margin-left","-" + $sUl.width() / 2 + "px");
                    } 
                    $sUl.css("margin-left", (parseInt($sUl.css("margin-left")) + $sLiLen) + "px");
                    
                    
                    
                },
                //设置导航,如果不对上面的Img进行操作,那么imgno就要有参数进来
                scrollNav: function (i) {
                    //设置上一个导航
                    $left.attr("showPicNum", i);
                    //设置下一个导航
                    $right.attr("showPicNum", i);

                },
                mouseOnImg: function (m) {
                    var c = $($sLi[m]);
                    //将所有的li样式赋值为空
                    $sLi.removeClass("current");
                    c.addClass("current");
                    $bImg.attr("src", $($sImg[m]).attr("oImg"));
                    $bbImg.attr("src", $($sImg[m]).attr("oImg"));
                    //图片等比例
                    this.scrollResize();
                    //设置导航
                    //this.scrollNav(m);
                    //滚动图片定位                    
                    $sUl.css({ 'margin-left': '-' + this.rNum() * $rCount * $sLiLen });
                },
                scrollResize: function () {
                    var maxWidth = 420;
                    var maxHeight = 420;

                    //将myimg存起来，相当于一个参数，不然异步的时候执行太快，一直是最后一张图
                    var imgNew = $("<img>").attr("src", $bImg.attr("src"))
                                      .attr("preImg", $bImg.attr("src"));

                    //这个是为了防遨游等浏览器，图片宽、高加为0执行
                    if (imgNew.width() == 0 || imgNew.height() == 0) {
                        imgNew.load = function () {
                            this.scrollResizeHd(this, maxWidth, maxHeight, this.preImg);
                        };
                    }
                    else {
                        this.scrollResizeHd($bImg, maxWidth, maxHeight, imgNew);
                    }

                },
                scrollResizeHd: function (imgNew, maxWidth, maxHeight, imgNew) {
                    //var tsImgsBox = document.getElementById("tsImgS");
                }
            };
            S.init();
        }
    });
})(jQuery)




//; $(function () {
//    var oDemo = $("#d_pcontainer");
//    var opt = {
//        smallSrc: "http://www.jq-school.com/upload/small.jpg",
//        smallWidth: 350,
//        smallHeight: 350,

//        bigSrc: "http://www.jq-school.com/upload/big.jpg",
//        bigWidth: 800,
//        bigHeight: 800
//    };
//    var oWin = $(window);
//    var owraper = $("#d_pwraper")
//    var oSmall = $("#d_pshow");
//    var oBig = $("#d_pbig");
//    var obg = $("#s_bg");
//    var oMask = $("#d_pmask");

//    var oBigImg = null;
//    var oBigImgWidth = opt.bigWidth;
//    var oBigImgHeight = opt.bigHeight;

//    var iBwidth = oBig.width();
//    var iBheight = oBig.height();

//    oBig.hide();

//    var iTop = owraper.position().top;
//    var iLeft = owraper.position().left;
//    var iWidth = owraper.width();
//    var iHeight = owraper.height();
//    var iSpeed = 200;

//    var setOpa = function (o) {
//        o.style.cssText = "opacity:0;filter:alpha(opacity:0);"
//        return o;
//    };

//    var imgs = function (opt) {
//        if (typeof opt !== "object") return false;

//        var oBig = new Image();

//        oBig.src = opt.bigSrc;
//        oBig.width = opt.bigWidth;
//        oBig.height = opt.bigHeight;

//        var oSmall = new Image();
//        oSmall.src = opt.smallSrc;
//        oSmall.width = opt.smallWidth;
//        oSmall.height = opt.smallHeight;

//        oBigImg = $(oBig);

//        return {
//            bigImg: setOpa(oBig),
//            smallImg: setOpa(oSmall)
//        };
//    };
//    //追加
//    var append = function (o, img) {
//        o.append(img);
//        $(img).animate({ opacity: 1 }, iSpeed * 2, null, function () { this.style.cssText = ""; });
//    };
//    //移动
//    var eventMove = function (e) {
//        var e = e || window.event;
//        var w = oMask.width();
//        var h = oMask.height();
//        var x = e.clientX - iLeft + oWin.scrollLeft() - w / 2;
//        var y = e.clientY - iTop + oWin.scrollTop() - h / 2;

//        var l = iWidth - w - 10;
//        var t = iHeight - h - 10;

//        if (x < 0) {
//            x = 0;
//        }else if (x > l) {
//            x = l;
//        };
//        if (y < 0) {
//            y = 0;
//        }else if (y > t) {
//            y = t;
//        };
//        oMask.css({left: x < 0 ? 0 : x > l ? l : x,top: y < 0 ? 0 : y > t ? t : y});
//        var bigX = x / (iWidth - w);
//        var bigY = y / (iHeight - h);
//        oBigImg.css({left: bigX * (iBwidth - oBigImgWidth),top: bigY * (iBheight - oBigImgHeight)});
//        return false;
//    };

//    var eventOver = function () {
//        oMask.show();
//        obg.stop().animate({ opacity: .1 }, iSpeed);
//        oBig.show().stop().animate({ opacity: 1 }, iSpeed / 2);
//        return false;
//    };

//    var eventOut = function () {
//        oMask.hide(); obg.stop().animate({ opacity: 0 }, iSpeed / 2);
//        oBig.stop().animate({ opacity: 0 }, iSpeed, null, function () { $(this).hide(); });                
//        return false;
//    };

//    var _init = function (object, oB, oS, callback) {
//        var num = 0;
//        oBig.css("opacity", 0);
//        append(oB, object.bigImg);
//        append(oS, object.smallImg);

//        object.bigImg.onload = function () {
//            num += 1;
//            if (num === 2) {
//                callback.call(object.smallImg);
//            };
//        };

//        object.smallImg.onload = function () {
//            num += 1;
//            if (num === 2) {
//                callback.call(object.smallImg);
//            };
//        };
//    };

//    // 初始化  继续写
//    _init(imgs(opt), oBig, oSmall, function () {
//        //绑定事件
//        oWin.resize(function () {
//            iTop = owraper.position().top;
//            iLeft = owraper.position().left;
//            iWidth = owraper.width();
//            iHeight = owraper.height();

//        });
//        oSmall.hover(eventOver, eventOut).mousemove(eventMove);
//    });
//});

