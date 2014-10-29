<%@ Page Language="C#" AutoEventWireup="true" CodeFile="redirect.aspx.cs" Inherits="upload" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Utility" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%

          var pcsQutota = BaiduObj.PCSGetQuota();
          
          var results="";
          if(!pcsQutota.TryGetValue("resultstr",out results)) results="";
          
           %>


        I am back!<br />

        access_token:<%=BaiduObj.Access_token%><br />
        refresh_token:<%=BaiduObj.Refresh_token%><br />

        pcs:<%=results %>


        
    </div>

        <div style="left: 0px; clip: rect(0px auto auto 0px); position: absolute; top: 0px">
        <asp:FileUpload ID="FileUpload1" runat="server"  />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="上传文件" CssClass="btn2" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </div>

    </form>
</body>
</html>
