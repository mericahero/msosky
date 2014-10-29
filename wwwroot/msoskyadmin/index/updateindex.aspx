<%@ Page Language="C#" %>

<%@ Import Namespace="MSOSKY.BL" %>

<%
    DateTime t1 = DateTime.Now;
    //ATS ats = new ATS();
    string msg = "";
    var flag=ATS.CreateOrUpdateIndex(true,out msg);
    
    double totalTime=(DateTime.Now - t1).TotalMilliseconds;
    if (flag)
    {
        Response.Write(String.Format("{{result:'{0}',description:'{1}',time:'{2}'}}",1,"",PubFunc.GetInt(totalTime)));
    }
    else
    {
        Response.Write(String.Format("{{result:'{0}',description:'{1}',time:'{2}'}}", 0, msg, PubFunc.GetInt(totalTime)));
    }

%>
