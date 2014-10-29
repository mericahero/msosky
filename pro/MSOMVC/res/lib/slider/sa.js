$(function () {
    SFbest.Slide.init();
})
var SFbest = {};
(function (d) {
    SFbest.Slide = new function () {
        this.init = function () {
            Q();
            S();
            T();
        };
        function Q() {
            var f = $("#lunbo_1");
            var li = $("ul>li", "#slide_show");
            if (f.length > 0 && li.length > 1) {
                setTimeout(function () {
                    $("#lunboNum").show();
                    m("#slide_show");
                }, 1000)
            }
            e();
            function e() {
                var p = p || {};
                p.u_hover = function (r) {
                    var q = $(r);
                    q.hover(function () {
                        $(this).removeClass("hovers").siblings().addClass("hovers")
                    }, function () {
                        $(this).siblings().removeClass("hovers")
                    })
                };
                p.initFun = function () {
                    p.u_hover("#index_slide .mini_pic a")
                };
                p.initFun()
            }
            function m(q) {
                p();
                function p() {
                    var t = $("ol", "#slide_show").width(),
					B = $("#slide_show>ul>li"),
					A = $("#index_slide>ol"),
					D = $("#index_slide>ol>li"),
					x = D.length,
					z;
                    var r = D.first();
                    D.last().clone().prependTo(A);
                    A.width(t * (x + 2) + 100).css("left", "-" + t + "px");
                    $("#slide_show").hover(function () {
                        $(this).children("a").show();
                        clearInterval(z)
                    }, function () {
                        $(this).children("a").hide();
                        clearInterval(z);
                        z = setInterval(function () {
                            s(y())
                        }, 5000)
                    }).trigger("mouseout");
                    B.hover(function () {
                        var E = B.index(this);
                        $(this).addClass("cur").siblings().removeClass("cur");
                        $("ol", "#index_slide").stop(true).animate({
                            left: "-" + (E + 1) * t + "px"
                        }, 360);
                    });
                    $(".show_next,.show_pre", "#slide_show").click(function () {
                        var E = y();
                        if ($("ol", "#index_slide").is(":animated")) {
                            return
                        }
                        if ($(this).hasClass("show_pre")) {
                            $("ol", "#index_slide").animate({
                                left: "+=" + t + "px"
                            }, 360, function () {
                                if (E > 0) {
                                    B.eq(E - 1).addClass("cur").siblings().removeClass("cur");
                                } else {
                                    if (E == 0) {
                                        $("ol", "#index_slide").css("left", "-" + t * (x) + "px");
                                        B.eq(-1).addClass("cur").siblings().removeClass("cur");
                                    }
                                }
                            })
                        } else {
                            s(E)
                        }
                        return false
                    });
                    function s(E) {
                        if (E == x - 1) {
                            r.addClass("cur").css("left", t * x)
                        }
                        $("ol", "#index_slide").stop(true, true).animate({
                            left: "-=" + t + "px"
                        }, 360, function () {
                            if (E < x - 1) {
                                B.eq(E + 1).addClass("cur").siblings().removeClass("cur");
                            } else {
                                if (E == x - 1) {
                                    r.removeClass("cur").css("left", -t);
                                    $("ol", "#index_slide").css("left", "-" + t + "px");
                                    B.eq(0).addClass("cur").siblings().removeClass("cur")
                                }
                            }
                        })
                    }
                    function y() {
                        return $("ul>li", "#slide_show").index($("ul>li.cur", "#slide_show"))
                    }

                }
            }
        }
        function S() {
            $(".slide").each(function () {
                var f = $(this);
                var w = f.width();
                var l = f.find("ul li").length;
                var i = 0;
                var b = "<div class='slideControls'>";
                if (l > 1) {
                    var b = "<div class='slideControls'>";
                    for (var i = 0; i < l; i++) {
                        b += "<span>" + (i + 1) + "</span>";
                    }
                    b += "</div>";
                    f.append(b);
                    f.hover(function () { f.children("a").show(); }, function () { f.children("a").hide(); });
                }
                f.find(".slideControls span").removeClass("cur").eq(0).addClass("cur");
                f.find(".slideControls span").mouseenter(function () {
                    i = f.find(".slideControls span").index(this);
                    if (i == l) { g(); i = 0; } else { h(); }
                });
                f.find("ul").css("width", w * (l + 1));
                f.find(".btn_next").click(function () {
                    i = f.find(".slideControls span.cur").index();
                    i++;
                    if (i == l) { g(); } else { h(); }
                });
                f.find(".btn_prev").click(function () {
                    i = f.find(".slideControls span.cur").index();
                    if (i == 0) {
                        f.find("ul").prepend(f.find("ul li:last").clone());
                        f.find("ul").css("left", -w);
                        f.find("ul").stop(true, false).animate({ "left": 0 }, 360, function () {
                            f.find("ul").css("left", -(l - 1) * w);
                            f.find("ul li:first").remove();
                        });
                        f.find(".slideControls span").removeClass("cur").eq(l - 1).addClass("cur");
                    } else { i--; h(); }
                });
                function g() {
                    f.find("ul").append(f.find("ul li:first").clone());
                    var nowLeft = -l * w;
                    f.find("ul").stop(true, false).animate({ "left": nowLeft }, 360, function () {
                        f.find("ul").css("left", "0");
                        f.find("ul li:last").remove();
                    });
                    f.find(".slideControls span").removeClass("cur").eq(0).addClass("cur");
                }
                function h() {
                    n();
                    f.find(".slideControls span").removeClass("cur").eq(i).addClass("cur");
                }
                function n() {
                    var nowLeft = -i * w;
                    f.find("ul").stop(true, false).animate({ "left": nowLeft }, 360);
                }
            })
        }
        function T() {
            var i = 0;
            var li = $(".lpscroll li");
            var n = li.length - 1;
            var speed = 300;
            if (n > 0) {
                li.not(":first").css({ left: "218px" });
                li.eq(n).css({ left: "-218px" });
                $(".lpleftbtn").show();
                $(".lprightbtn").show();
            }
            $(".lpleftbtn").click(function () {
                if (!li.is(":animated")) {
                    if (i >= n) {
                        i = 0; li.eq(n).animate({ left: "-218px" }, speed);
                        li.eq(i).animate({ left: "0px" }, speed);
                    } else {
                        i++;
                        li.eq(i - 1).animate({ left: "-218px" }, speed);
                        li.eq(i).animate({ left: "0px" }, speed);
                    }
                    li.not("eq(i)").css({ left: "218px" });
                    $("i").text(i + 1);
                }
            });
            $(".lprightbtn").click(function () {
                if (!li.is(":animated")) {
                    if (i <= 0) {
                        i = n;
                        li.eq(0).animate({ left: "218px" }, speed);
                        li.eq(n).animate({ left: "0px" }, speed);
                    } else {
                        i--;
                        li.eq(i + 1).animate({ left: "218px" }, speed);
                        li.eq(i).animate({ left: "0px" }, speed);
                    }
                    li.not("eq(i)").css({ left: "-218px" });
                    $("i").text(i + 1);
                }
            });
        }
    };
})(jQuery);

