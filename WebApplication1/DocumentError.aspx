﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentError.aspx.cs" Inherits="WebApplication1.DocumentError" %>

<html >

    <head>
    <link href="~/Content/Blue.css" rel="stylesheet" />
        </head>
<body>
    <div style="width:500px; height:100px; border:3px solid #ccc; margin:200px auto; padding:10px; background:#f2f2f2;">

        <p style="font-size:20px; font-family:Arial; font-weight:bold; padding:0; margin:0; color:#D9232F;">Authentication alert!</p>

        <p style="font-family:Arial; font-size:14px;">
            You are not authorized to view this Page …..Or Your Session has expired .…..   
             أنت غير مخول لعرض هذه الصفحة ... أو انتهت صلاحية الجلسة.…..
        </p>
    
        <button class="mcbutton" onclick="closeWin()">Close</button>
           <p>
           
                 <%=System.Configuration.ConfigurationManager.AppSettings["UploadServer"]%>

        </p>
        </div>
    <script>
    


        function closeWin() {
            self.close();
        }
    </script>


</body>


</html>


