
//实现图片往左滚动的轮播，如果一个页面出现多个轮播的时候HTML的class名字不能相同
function slider(boxClass, boxNumClass, boxWidth, boxHeight, sliderTime){
    var objId=$(boxClass);//轮播的外层class
    var objImgBox = $(boxClass + ' ul:first');//轮播的ul,注意$(字符串)
    var objBtn=$(boxNumClass+" li");//鼠标经过实习轮播的按钮
    var objWidth=boxWidth;//每次滚动的宽带
    var len=objBtn.length;//获取轮播的数量
    var index=0;//初始化
    var speed=sliderTime;//轮播间隔时间
    objId.width(boxWidth);
    objId.height(boxHeight);
    objImgBox.width(boxWidth*len);//设置图片外层div的总宽
    //鼠标经过移动
    objBtn.hover(function(){
        clearInterval(adTimer);
        objImgBox.stop(true);
        index=objBtn.index(this);
        sliderImg(index);
    },function(){
        adTimer = setInterval(au_ad, sliderTime)
    });
    //定时器
    var adTimer = setInterval(au_ad, sliderTime);
    //实现index自动增加
    function au_ad(){
        sliderImg(index);
        index++;
        if (index >= len) {
            index = 0;
        }
    }
    //图片轮播函数
    function sliderImg(index) {
        objImgBox.animate({ left: -objWidth * index},600);
        objBtn.removeClass("on").eq(index).addClass("on");
    }
  }