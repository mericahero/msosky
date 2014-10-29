using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Utility;

public partial class upload : System.Web.UI.Page
{


    private BaiduOAuth2 _baiduObj;
    protected BaiduOAuth2 BaiduObj
    {
        get
        {
            if (_baiduObj == null)
            {
                var RequestForm = new NameValueCollection();
                RequestForm.Add(Request.Form);
                RequestForm.Add(Request.QueryString);
                _baiduObj = (BaiduOAuth2)OAuth2.GetObj(1, RequestForm);
            }
            return _baiduObj;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //AllSheng.upload UpFiles = new AllSheng.upload();


        //HttpPostedFile File = FileUpload1.PostedFile;
        // AllSheng.UploadObj.PhotoSave("/", FileUpload1);
        HttpFileCollection files = HttpContext.Current.Request.Files;
        var firstFile=files[0];
        var bytes=new Byte[firstFile.ContentLength];
        firstFile.InputStream.Read(bytes, 0, bytes.Length);

        var temp = new BaiduOAuth2();

        temp.PCSUploadSingleFile("a.txt", bytes);

        //UpFiles.Path = "../UpLoadfiles";
        //String ReStr = UpFiles.SaveAs(files).ToString();
        //Label1.Text = ReStr;
        //UpFiles = null;
    }
}