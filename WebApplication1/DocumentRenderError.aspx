<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentRenderError.aspx.cs" Inherits="WebApplication1.DocumentRenderError" %>

<html >

    <head>
    <link href="~/Content/Blue.css" rel="stylesheet" />
        </head>
<body>
    <div style="width:500px; height:100px; border:3px solid #ccc; margin:200px auto; padding:10px; background:#f2f2f2;">

        <p style="font-size:20px; font-family:Arial; font-weight:bold; padding:0; margin:0; color:#D9232F;"> Alert!</p>

        <p style="font-family:Arial; font-size:14px;">
             File is Not available for viewing ...
                 الملف غير متاح للعرض...
        
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


