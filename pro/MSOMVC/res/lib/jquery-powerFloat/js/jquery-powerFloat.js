$.fn.extend({
    floatDwon: function (o) {
        var th = this;
        var container = o.container;
        var conwidth = container.width();
        var conheight = container.height()
        var mouseover = o.mouseover;
        var mouseout = o.mouseout;
        var thw = th.width();
        var thOff = th.offset();
        var left = thOff.left - thw;
        var top = thOff.top;
        var arror_r = "<div class=\"arrow_r\"></div>";
        var loadingImg = "  <img  style=\" margin:30px 140px\" src=\"/res/images/loading.gif\" />";
        th.mouseover(function () {
            var thw = th.width();
            var thh = th.height();
            var thOff = th.offset();
            var left = thOff.left + conwidth / 2 - thw - 55;
            var top = thOff.top + thh;
            if (o.func == null) {
                top = top + 8;
            }
            container.css({ top: top })
            if ((left + conwidth) > $(window).width()) {
                container.animate({ left: $(window).width() - conwidth - 10 }, 10);
            } else {
                container.animate({ left: left }, 10);
            }
            if (o.func != null)
                container.html(loadingImg);
            container.show();
            if (o.func != null) {
                o.func(container);
            }
            if (mouseover != null) {
                mouseover();
            }
        })
        $(document).mousemove(function (e) {
            //判段关闭
            function closeJudgment(e, con, th) {
                //鼠标坐标
                var ex = e.pageX;
                var ey = e.pageY;

                //判段是否在th范围内
                var thoff = th.offset();
                var thw = th.width();
                var thh = th.height();
                var thl = thoff.left;
                var tht = thoff.top;
                var isthX = ex >= thl - 80 && ex <= thl + thw;
                var isthY = ey >= tht - 10 && ey <= tht + thh + 180;
                if (isthX && isthY) { return true; }


                //判段是否在container范围内
                var conoff = con.offset();
                var conw = con.width();
                var conh = con.height();
                var conl = conoff.left;
                var cont = conoff.top;
                var isconX = ex >= conl && ex <= conl + conw;
                var isconY = ey >= cont && ey <= cont + conh;
                if (isconX && isconY) { return true; }

                //不配备反回false
                return false;
            }
            if (closeJudgment(e, container, th) == false) {
                container.hide();
                if (mouseout != null) {
                    mouseout();
                }
            }
        })
    },
    float: function (o) {
        var th = this;
        var container = o.container;
        var conwidth = container.width()
        var thw = th.width();
        var thOff = th.offset();
        var left = thOff.left - thw;
        var top = thOff.top;
        var arror_r = "<div class=\"arrow_r\"></div>";
        var loadingImg = "  <img  style=\" margin:30px 140px\" src=\"/res/images/loading.gif\" />";
        th.mouseover(function () {
            var thw = th.width();
            var thOff = th.offset();
            var left = thOff.left - thw;
            var top = thOff.top;
            container.css({ left: left, top: top, width: 0 })
            container.animate({ width: conwidth, left: left - conwidth + 30 }, 10);
            container.html(loadingImg);
            container.append(arror_r);
            container.show();
            $.ajax({
                url: "/ajax/cart.aspx",
                dataType: "html",
                cache: false,
                success: function (msg) {
                    container.html(msg);
                    container.append(arror_r);
                }

            })
        })
        $(document).mousemove(function (e) {
            //判段关闭
            function closeJudgment(e, con, th) {
                //鼠标坐标
                var ex = e.pageX;
                var ey = e.pageY;

                //判段是否在th范围内
                var thoff = th.offset();
                var thw = th.width();
                var thh = th.height();
                var thl = thoff.left;
                var tht = thoff.top;
                var isthX = ex >= thl - 80 && ex <= thl + thw;
                var isthY = ey >= tht - 10 && ey <= tht + thh + 180;
                if (isthX && isthY) { return true; }


                //判段是否在container范围内
                var conoff = con.offset();
                var conw = con.width();
                var conh = con.height();
                var conl = conoff.left;
                var cont = conoff.top;
                var isconX = ex >= conl && ex <= conl + conw;
                var isconY = ey >= cont && ey <= cont + conh;
                if (isconX && isconY) { return true; }

                //不配备反回false
                return false;
            }
            if (closeJudgment(e, container, th) == false) {
                container.hide();
            }
        })
                ;
    }
})