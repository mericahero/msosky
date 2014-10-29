using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace AllSheng
{
	public class upload
	{
		private HttpPostedFile postedFile;

		protected string localFileName;

		protected string localFileExtension;

		protected long localFileLength;

		protected string localFilePath;

		protected string saveFileName;

		protected string saveFileExtension;

		protected string saveFilePath;

		protected string saveFileFolderPath;

		private string path = null;

		private string fileType = null;

		private int sizes = 0;

		public string FileType
		{
			set
			{
				fileType = value;
			}
		}

		public string Path
		{
			set
			{
				path = value ?? "";
			}
		}

		public int Sizes
		{
			set
			{
				sizes = value;
			}
		}

		public upload()
		{
			path = "uploadimages";
			fileType = "jpg|gif|bmp|jpeg|png|rar|doc";
			sizes = 200;
		}

		private string getFileExtension(string myFileName)
		{
			string myFileExtension = null;
			myFileExtension = System.IO.Path.GetExtension(myFileName);
			if (myFileExtension != "")
			{
				myFileExtension = myFileExtension.ToLower();
			}
			string[] temp = fileType.Split(new char[] { '|' });
			bool flag = false;
			string[] strArrays = temp;
			int num = 0;
			while (num < (int)strArrays.Length)
			{
				if (!(string.Concat(".", strArrays[num]) == myFileExtension))
				{
					num++;
				}
				else
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				myFileExtension = "";
			}
			return myFileExtension;
		}

		private string getSaveFileFolderPath(string format)
		{
			string mySaveFolder = null;
			try
			{
				string folderPath = null;
				if ((format.IndexOf("yyyy") > -1 || format.IndexOf("MM") > -1 || format.IndexOf("dd") > -1 || format.IndexOf("hh") > -1 || format.IndexOf("mm") > -1 || format.IndexOf("ss") > -1 ? false : format.IndexOf("ff") <= -1))
				{
					mySaveFolder = string.Concat(HttpContext.Current.Server.MapPath("."), format);
				}
				else
				{
					folderPath = DateTime.UtcNow.ToString(format);
					mySaveFolder = string.Concat(HttpContext.Current.Server.MapPath("."), folderPath);
				}
				DirectoryInfo dir = new DirectoryInfo(mySaveFolder);
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

		private void message(string msg, string url)
		{
			HttpResponse response = HttpContext.Current.Response;
			string[] strArrays = new string[] { "<script language=javascript>alert('", msg, "');window.location='", url, "'</script>" };
			response.Write(string.Concat(strArrays));
		}

		private void message(string msg)
		{
			HttpContext.Current.Response.Write(string.Concat("<script language=javascript>alert('", msg, "');</script>"));
		}

		public string SaveAs(HttpFileCollection files)
		{
			string myReturn = "";
			try
			{
				for (int iFile = 0; iFile < files.Count; iFile++)
				{
					postedFile = files[iFile];
					localFilePath = postedFile.FileName;
					if ((localFilePath == null ? false : !(localFilePath == "")))
					{
						localFileLength = (long)postedFile.ContentLength;
						if (localFileLength < (long)(sizes * 1024))
						{
							saveFileFolderPath = getSaveFileFolderPath(path);
							localFileName = System.IO.Path.GetFileName(postedFile.FileName);
							saveFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff");
							localFileExtension = getFileExtension(localFileName);
							if (localFileExtension == "")
							{
								message(string.Concat("目前本系统支持的格式为:", fileType));
							}
							saveFileExtension = localFileExtension;
							saveFilePath = string.Concat(saveFileFolderPath, saveFileName, saveFileExtension);
							postedFile.SaveAs(saveFilePath);
							string[] strArrays = new string[] { myReturn, null, null, null, null };
							strArrays[1] = (myReturn == "" || myReturn == null ? "" : "|");
							string str = path;
							char[] chrArray = new char[] { '\\' };
							strArrays[2] = str.TrimStart(chrArray);
							strArrays[3] = saveFileName;
							strArrays[4] = saveFileExtension;
							myReturn = string.Concat(strArrays);
							HttpContext.Current.Response.Write(string.Concat("<script>parent.Article_Content___Frame.FCK.EditorDocument.body.innerHTML+='<img src=", saveFileName, saveFileExtension, "  border=0 />'</SCRIPT>"));
						}
						else
						{
							message(string.Concat("上传的图片不能大于", sizes, "KB"));
							break;
						}
					}
				}
			}
			catch
			{
				message("出现未知错误！");
				myReturn = null;
			}
			return myReturn;
		}
	}
}