using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Cls_Upfile 的摘要说明
/// </summary>
///
namespace AllSheng
{
    public class upload1
    {
        #region 变量

        System.Web.HttpPostedFile postedFile;

        protected string localFileName;//原文件名(含扩展名)
        protected string localFileExtension;//原扩展名
        protected long localFileLength;//原文件大小
        protected string localFilePath;//原文件路径

        protected string saveFileName;//保存的文件名(含扩展名)
        protected string saveFileExtension;//保存的扩展名
        //protected long saveFileLength;//保存的文件大小
        protected string saveFilePath;//保存文件的服务器端的完整路径
        protected string saveFileFolderPath;//保存文件的服务器端的文件夹路径


        private string path = null;
        private string fileType = null;
        private int sizes = 0;

        #endregion

        #region upload():初始化变量
        /// <summary>
        /// 初始化变量
        /// </summary>
        public upload1()
        {
            path = @"uploadimages"; //上传路径
            fileType = "jpg|gif|bmp|jpeg|png|rar|doc";
            sizes = 200; //传文件的大小,默认200KB
        }
        #endregion

        #region 设置传入的值:Path/Sizes/FileType
        /// <summary>
        /// 设置上传路径,如:uploadimages
        /// </summary>
        public string Path
        {
            set
            {
                path = @"" + value + @"";
            }
        }

        /// <summary>
        /// 设置上传文件大小,单位为KB
        /// </summary>
        public int Sizes
        {
            set
            {
                sizes = value;
            }
        }

        /// <summary>
        /// 设置上传文件的类型,如:jpg|gif|bmp
        /// </summary>
        public string FileType
        {
            set
            {
                fileType = value;
            }
        }
        #endregion
       
        #region SaveAs()上传文件

      public string  SaveAs(System.Web.HttpFileCollection files)
        {
            string myReturn = "";
            try
            {
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    postedFile = files[iFile];
                    //获得文件的上传的路径
                    localFilePath = postedFile.FileName;
                    //判断上传文件路径是否为空
                    if (localFilePath == null || localFilePath == "")
                    {
                        //message("您没有上传数据呀，是不是搞错了呀!");
                        //break;

                        continue;
                    }
                    else
                    {
                        #region 判断文件大小
                        //获得上传文件的大小
                        localFileLength = postedFile.ContentLength;
                        //判断上传文件大小
                        if (localFileLength >= sizes * 1024)
                        {
                            message("上传的图片不能大于" + sizes + "KB");
                            break;
                        }
                        #endregion

                        #region 文件夹
                        //获取保存文件夹路径
                        saveFileFolderPath = getSaveFileFolderPath(path);
                        #endregion

                        #region 文件名
                        //获得原文件名（含扩展名）
                        localFileName = System.IO.Path.GetFileName(postedFile.FileName);
                       
                        saveFileName = DateTime.UtcNow.ToString("yyyy" + "MM" + "dd" + "HH" + "mm" + "ss" + "ffffff");//"yyyy"+"MM"+"dd"+"HH"+"mm"+"ss"+"ffffff"
                        #endregion

                        #region 扩展名
                        //获取原文件扩展名
                        localFileExtension = getFileExtension(localFileName);

                        //如果为真允许上传,为假则不允许上传
                        if (localFileExtension == "")
                        {
                            message("目前本系统支持的格式为:" + fileType);
                        }
                        //得到保存文件的扩展名,可根据需要更改扩展名
                        saveFileExtension = localFileExtension;
                        #endregion

                        //得到保存文件的完整路径
                        saveFilePath = saveFileFolderPath + saveFileName + saveFileExtension;
                        postedFile.SaveAs(saveFilePath);
                        myReturn = myReturn + ((myReturn == "" || myReturn == null) ? "" : "|") + path.TrimStart(new char[] { '\\' }) + saveFileName + saveFileExtension;
                      //以下对文章的内容进行一些加工

                        System.Web.HttpContext.Current.Response.Write("<script>parent.Article_Content___Frame.FCK.EditorDocument.body.innerHTML+='<img src=" + saveFileName + saveFileExtension + " "+" border=0 />'</SCRIPT>");
                       
                    }
                }
            }
            catch
            {
                //异常
                message("出现未知错误！");
                myReturn = null;
            }
            return myReturn;
        }
        #endregion

        #region getSaveFileFolderPath(...):获得保存的文件夹的物理路径
        /// <summary>
        /// 获得保存的文件夹的物理路径
        /// 返回保存的文件夹的物理路径,若为null则表示出错
        /// </summary>
        /// <param name="format">保存的文件夹路径 或者 格式化方式创建保存文件的文件夹，如按日期"yyyy"+"MM"+"dd":20060511</param>
        /// <returns>保存的文件夹的物理路径,若为null则表示出错</returns>
        private string getSaveFileFolderPath(string format)
        {
            string mySaveFolder = null;
            try
            {
                string folderPath = null;
                //以当前时间创建文件夹,
                //!!!!!!!!!!!!以后用正则表达式替换下面的验证语句!!!!!!!!!!!!!!!!!!!
                if (format.IndexOf("yyyy") > -1 || format.IndexOf("MM") > -1 || format.IndexOf("dd") > -1 || format.IndexOf("hh") > -1 || format.IndexOf("mm") > -1 || format.IndexOf("ss") > -1 || format.IndexOf("ff") > -1)
                {
                    //以通用标准时间创建文件夹的名字
                    folderPath = DateTime.UtcNow.ToString(format);
                    mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + @"" + folderPath + @"";
                }
                else
                {
                    mySaveFolder = System.Web.HttpContext.Current.Server.MapPath(".") + format;
                }
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(mySaveFolder);
                //判断文件夹否存在,不存在则创建
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch
            {
                message("获取保存路径出错");
            }
            return mySaveFolder;
        }

        #endregion

        #region getFileExtension(...):获取原文件的扩展名
        /// <summary>
        /// 获取原文件的扩展名,返回原文件的扩展名(localFileExtension),该函数用到外部变量fileType,即允许的文件扩展名.
        /// </summary>
        /// <param name="myFileName">原文件名</param>
        /// <returns>原文件的扩展名(localFileExtension);若返回为null,表明文件无后缀名;若返回为"",则表明扩展名为非法.</returns>
        private string getFileExtension(string myFileName)
        {
            string myFileExtension = null;
            //获得文件扩展名
            myFileExtension = System.IO.Path.GetExtension(myFileName);//若为null,表明文件无后缀名;
            //分解允许上传文件的格式
            if (myFileExtension != "")
            {myFileExtension = myFileExtension.ToLower();//转化为小写
            }
            string[] temp = fileType.Split('|');
            //设置上传的文件是否是允许的格式
            bool flag = false;
            //判断上传的文件是否是允许的格式
            foreach (string data in temp)
            {
                if (("." + data) == myFileExtension)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                myFileExtension = "";//不能设置成null,因为null表明文件无后缀名;
            }
            return myFileExtension;
        }
        #endregion

        #region message(...):弹出消息框
        /// <summary>
        /// 弹出消息框,显示内容(msg),点击"确定"后页面跳转到该路径(url)
        /// </summary>
        /// <param name="msg">显示内容</param>
        /// <param name="url">跳转路径</param>
        private void message(string msg, string url)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');window.location='" + url + "'</script>");
        }

        /// <summary>
        /// 弹出消息框,显示内容(msg),无跳转
        /// </summary>
        /// <param name="msg">显示内容</param>
        private void message(string msg)
        {
            System.Web.HttpContext.Current.Response.Write("<script language=javascript>alert('" + msg + "');</script>");
        }
        #endregion
    }
}