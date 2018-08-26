<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentGallery.aspx.cs" Inherits="WebApplication1.DocumentGallery" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" dir="<%=LocRM.GetString("dir")%>">

<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10">
    <link rel="stylesheet" href="style5.css"/>
    <link href="Content/DocGallery.css" rel="stylesheet" />
<%--  <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>--%>

   <script src="Scripts/Jquery-1.12.0.min.js"></script>
  
        <%--    <link rel="stylesheet" href="style5.css">--%>
<%--    <link href="Content/Chk.css" rel="stylesheet" />--%>
    
    <% if (string.IsNullOrEmpty(uploadedDocumentsSet))
       { 
           //data file for testing
           %>


 <%--   <script src="Scripts/DocGalleryData.js"></script>
 
 --%>  
    
    <script type="text/javascript">
        var inProduction = false;
        var languageid = "";
      </script>
    <% }
       else
       { %>
      <script type="text/javascript">

    

          var uploadedDocumentsSet = <%=uploadedDocumentsSet%>;
          var inProduction= true;
      </script>
    <% 
    }
     %>
    <script type="text/javascript">
        //switch for enabling/disabling review status
          var bEnableReviewStatus = <%=bEnableReviewStatus.ToString().ToLower()%>;

       
          var bEnableApproveStatus = <%=bEnableApproveStatus.ToString().ToLower()%>;

          var bEnableRejectStatus = <%=bEnableRejectStatus.ToString().ToLower()%>;

          var bEnableItemAssociationStatus = <%=bEnableItemAssociationStatus.ToString().ToLower()%>;

          var AdditionalDocumentFlag = "<%=AdditionalDocumentFlag.ToString().ToLower()%>";
          var bEnableDeletedRecordsStatus = "<%=bEnableDeletedRecordsStatus.ToString().ToLower()%>";
          

        var msgSelectADocument = '<%=LocRM.GetString("msgSelectADocument")%>';
        var msgSaved = '<%=LocRM.GetString("msgSaved")%>';
        var bSaved = <%=bSaved.ToString().ToLower()%>;
        var errMsg = "<%=errMsg%>"

        var ImagesUsed= {
            Rejected: "Images/RS_Rejected.png",
            NotRejected: "Images/RS_NotRejected.png",
            Reviewed:"Images/RS_Reviewed.png",
            NotReviewed: "Images/RS_NotReviewed.png",
            Approved: "Images/RS_Approveds.png",
            NotApproved: "Images/RS_NotApproved.png"
      
        }

    </script>

    <script type="text/javascript">
        //alert('hicheck');
        //$('#sidebarCollapse').on('click', function () {
        //    alert('hi');
        //    $('#sidebar').toggleClass('active');
        //    $(this).toggleClass('active');
        //});

              //$('table tr td ').click(function () {
            //// alert(event.target.id);
            //  //  alert('td');
            //    alert($(this).find('.DOCNAMECELL').children[0]);//find('img').trigger('click');
            //    //$(this).find('span').trigger('click');
            //    return false;
            //}
                  //$("td").click(function () {
            //    alert($(this).text());
            //});

        $('#docNamesTable tr td').find(".DOCNAMECELL").click(function () {
            var cid = $(this).attr('id');
          //  $(this).find('.REVIEWSTATUS')[0]
            $(this).find('.REVIEWSTATUS').trigger('click');
            return false;
            event.stopPropagation();
        });


        if(!document.getElementsByClassName) {
            document.getElementsByClassName = function(className) {
                return this.querySelectorAll("." + className);
            };
            Element.prototype.getElementsByClassName = document.getElementsByClassName;
        }

        window.onresize = function () {
            var reservedWidth = 55;
            var docNamesContainer = document.getElementById("DocNamesContainer");
            var documentViewer = document.getElementById("documentViewer");

            var display1 = docNamesContainer.style.display;
            docNamesContainer.style.display = "none";

            docNamesContainer.style.width = (documentViewer.offsetWidth - reservedWidth )+ "px";
            docNamesContainer.style.display = display1;
        }

        function pageLoad() {
            

                if ( typeof uploadedDocumentsSet === 'undefined')
                {
         
                }
                else
                {
                    renderDocType(uploadedDocumentsSet);
                }
            if (document.getElementById("GetDirection").innerHTML == 'ar')
            {
             
                document.body.style.direction = "rtl";
            }
            else
            {
                document.body.style.direction = "ltr";

            }
            window.onresize.apply();
            if(bSaved) {
                alert(msgSaved);
               // window.close();

            }
            if(errMsg.length>0) {
                alert(errMsg);
            }
        }

    </script>
</head>

<body onload="pageLoad()">


    <table class="GALLERY"  border1="1">

      <tr>
           
       
                <td rowspan="3" id="DocTypesContainer" style="vertical-align:top; min-width: 230px; width:150px " class="GALLERYCELLS">..</td>
         <%--     <td rowspan="3" id="DocTypesContainer" style="vertical-align:top; min-width: 272px; width:150px " class="GALLERYCELLS">..</td>
        --%>
   
          <td style=" width: 150px; padding-right: 70%; float: right; min-width: 165px; ">
                
           <u>   <asp:Label ID="LabelSelectedFilterDisplay"  runat="server">

              </asp:Label>

               </u>
            </td>
         

        </tr>
        <tr>
    
          
            <td id="tdDocumentViewer" width="90%" height="80%" colspan="4" class="GALLERYCELLS"><iframe id="documentViewer" class="DOCVIEWER" src="#"></iframe></td>
        
      
            </tr>
  
     <%--      <tr>
              <td rowspan="3" id="DocTypesContainer" style="vertical-align:top; min-width: 165px; width:150px " class="GALLERYCELLS">..</td>
            
            <td id="tdDocumentViewer" width="90%" height="80%" colspan="4" class="GALLERYCELLS"><iframe id="documentViewer" class="DOCVIEWER" src="#"></iframe></td>
        </tr>--%>


        <tr style="height:15px">
            <td style="width:65px" colspan="4" class="IMAGEBTNCELL">
                <form id="form1" runat="server">
                     <asp:ImageButton ID="SaveBtn" ImageUrl="images/save-icon.png" style="display:none" BorderStyle="NotSet" BorderWidth="1" class="IMAGEBTN"  runat="server" OnClientClick="return SaveReviewStatus();" OnClick="SaveBtn_Click"/>                                       
                   
                    <input type="image" src="images/close-icon.png" class="IMAGEBTN" value="<%= LocRM.GetString("Close")%>" onclick=" return CloseGallery()" />
                   
                    <asp:TextBox ID="txtArrayIdx" runat="server" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="txtArrayStatus" runat="server" style="display:none"></asp:TextBox>                    
                    <asp:TextBox ID="txtJobId" runat="server" style="display:none" Visible="false"></asp:TextBox> 
                    <div id="DocNamesContainerTitle"  value=""; style="display:inline; vertical-align:central;" onclick="scrollto()" class="DocNamesContainerTitle">..</div>                   
                    <asp:Label ID="GetDirection" runat="server" style="display:none"></asp:Label>
                
      <%--              <asp:Label ID="LabelSelectedFilterDisplay"  runat="server">

              </asp:Label>--%>
                    <input type="hidden" runat="server" id="DeleteFlagTest" />
                    <input type="hidden" runat="server" id="TypeOfFilter" />
                            <input type="hidden" runat="server" id="labelText" />
                         <input type="hidden" runat="server" id="AllDocsIdForDeclaration" />
                   
              <%--      <asp:CheckBox ID="CheckBox1" runat="server" />--%>
      <%--                   <div class="pretty p-switch p-fill">
            <input type="checkbox" />
         <div class="state">
              <label>Fill</label>
             </div>
          </div>--%>
                  
                    </form> 
            </td>
        </tr>
        <tr> 
                 <%if (languageid == "eng")
        {%>
            <td  style="float:right" class="GALLERYCELLS"><img src="images/<%= LocRM.GetString("scrollbk")%>" class="SCROLLBTN" onclick="scrollIt(-1)" /></td>         
            <%} %>

                      <%if (languageid == "ara")
        {%>
             <td  style="float:left" class="GALLERYCELLS"><img src="images/<%= LocRM.GetString("scrollbk")%>" class="SCROLLBTN" onclick="scrollIt(-1)" /></td>         
            <%} %>

            <td id="tdDocNamesContainer" colspan="2" style="min-width: 222px;" class="GALLERYCELLS" width="100%" height="62">
                <div id="DocNamesContainer" class="DOCNAMESDIV"></div>
                
              
            </td>
       

            <td class="GALLERYCELLS"><img src="images/<%= LocRM.GetString("scrollfd")%>" class="SCROLLBTN" onclick="scrollIt(1)"/></td>         
      
            </tr>
    <tr>
             <td>              
                <%=System.Configuration.ConfigurationManager.AppSettings["UploadServer"]%>

                </td>
     
    </tr>
    </table>  
       <%if (languageid == "ara")
        {%>
   <%--     <div class="wrapper"  id="FilterDiv" >
            <!-- Sidebar Holder -->
            <nav id="sidebar" style="max-height:400px;">
                <div class="sidebar-header">
                    <h3>Gallery Filters</h3>
                </div>

                <ul class="list-unstyled components">
              
                   
                    <li> <a href="#">View Deleted Documents</a>    </li>
                       
                
                      <!-- <li class="active">
                        <a href="#homeSubmenu" data-toggle="collapse" aria-expanded="false">Home</a>
                     <ul class="collapse list-unstyled" id="homeSubmenu">
                            <li><a href="#">Home 1</a></li>
                            <li><a href="#">Home 2</a></li>
                            <li><a href="#">Home 3</a></li>
                        </ul>
                    </li>-->
                    <li> <a href="#">Additional Documents </a>  </li>
                       
                  
                    <li>   <a href="#pageSubmenu" data-toggle="collapse" aria-expanded="false">OGA</a>
                    
                    
                         <ul class="collapse list-unstyled" id="pageSubmenu">  <li><a href="#">MOH</a></li> <li><a href="#">MOI</a></li>
                          
                           
                        </ul></li></ul> </nav>
                    
            
                

                <!--<ul class="list-unstyled CTAs">
                    <li><a href="https://bootstrapious.com/tutorial/files/sidebar.zip" class="download">Download source</a></li>
                    <li><a href="https://bootstrapious.com/p/bootstrap-sidebar" class="article">Back to article</a></li>
                </ul>-->
           

            <!-- Page Content Holder -->
            <div id="content">

                <nav class="navbar navbar-default">
                    <div class="container-fluid">

                        <div class="navbar-header">
                            <button type="button" id="sidebarCollapse" class="navbar-btn">
                                <span></span>  <span></span>     <span></span>    </button>  </div>
                         
                      

                        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                       
                        </div></div>      </nav>    </div> </div>--%>
                    
          

                      
       
        <%} %>

<%--    <script type="text/javascript">

        $(document).ready(function () {
            $('#sidebarCollapse').on('click', function () {
                alert('hi');
                $('#sidebar').toggleClass('active');
                $(this).toggleClass('active');
            });
        });
    </script>--%>
</body>
    
    
    <script type="text/javascript" src="Scripts/DocGallery4.js"></script>

   
</html>