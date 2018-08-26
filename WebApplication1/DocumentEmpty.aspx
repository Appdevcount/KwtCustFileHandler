<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentEmpty.aspx.cs" Inherits="WebApplication1.DocumentEmpty" %>


<html >

    <head>
    <link href="~/Content/Blue.css" rel="stylesheet" />
        </head>
<body>
    <div style="width:500px; height:100px; border:3px solid #ccc; margin:200px auto; padding:10px; background:#f2f2f2;">

        <p style="font-size:20px; font-family:Arial; font-weight:bold; padding:0; margin:0; color:#D9232F;">Authentication alert!</p>

            <%if (TypeOfFilter == "")
                {%>
        <p style="font-family:Arial; font-size:14px;">
               You are having No documents for this declarations …. 
             لا يوجد لديك وثائق لهذا التصريح
        
        </p>
    
        <button class="mcbutton" onclick="closeWin()">Close</button>
        <%} %>
        <%else { %>
        
             <p style="font-family:Arial; font-size:14px;">
               You are having No documents for this Filter Criteria …. 
      ليس لديك وثائق لهذا المرشح معايير
        
        </p>
    
        <button class="mcbutton" onclick="MoveBack()">Back</button>
        
        <%} %>


        </div>
    <script>
    


        function closeWin() {
            self.close();
        }

        function MoveBack()
        {
           // alert('<%=ResponsebackUrl%>');

            window.location.href = '<%=ResponsebackUrl%>'.replace("ScanUploadServicesPP/", "");
            //switch below for production 
//            window.location.href = '<%=ResponsebackUrl%>';
         

        }

    </script>


</body>


</html>
