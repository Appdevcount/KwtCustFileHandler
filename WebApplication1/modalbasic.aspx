<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modalbasic.aspx.cs" Inherits="WebApplication1.modalbasic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

<script src="~/Scripts/jquery-3.2.1.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           basicWindow

      
            <asp:Button ID="Button1" runat="server" Text="ok" />
            
            <asp:Button ID="Button2" runat="server" onclick="closeWin()" Text="cancel" />


        </div>
    </form>
    <script type="text/javascript">
        string url = "window.open('login.aspx ', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=yes,scrollbars=yes,resizable=yes,titlebar=no' );";
        ClientScript.RegisterStartupScript(this.GetType(), "newWindow", url, true);

    </script>
</body>

</html>
