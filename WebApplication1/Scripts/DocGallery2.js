var docGallarySet;

var ReviewstatusUpdates = new Object();
var arr = new Array();
var docnamelength = 6;


var documentid =0;
var enumReviewStatus = {
    NotSeen: 0,
    Reviewed: 1,
    Rejected: 2,
    Approved:3
}



var buttonvalue;
var reviewButtonValue;

var selectionText;
var checkboxtext;
var listalldocuments;

var listadditionaldocumentsByOGA;
var listadditionaldocumentsByALL;

var listadditionaldocumentsByCustoms;
var Filters;
var textAdditionalDocuments;
var getlanguageforalert = document.getElementById('GetDirection').innerHTML;
if (getlanguageforalert == "en")
{
    // alert('No Files To Remove');
    buttonvalue = "Associate Items";
    selectionText = "Select/Unselect All";
    checkboxtext = "Deleted Documents"
    listadditionaldocumentsByALL = "All Documents";
    listadditionaldocumentsByOGA = "OGA Documents";
    listadditionaldocumentsByCustoms = "Customs Documents ";
    listalldocuments = "All Documents";
    Filters = "Filters";
    textAdditionalDocuments = "Additional Documents";
    reviewButtonValue = "Review Documents";

}
else {

    buttonvalue = "إرفاق";
    selectionText = "حدد / ألغ تحديد الكل";
    checkboxtext = "عرض السجلات المحذوفة";
    checkboxtext = " حذف المستندات"
 
    listadditionaldocumentsByALL = "جميع الوثائق";
    listadditionaldocumentsByOGA = "وثائق حكومية";
    listadditionaldocumentsByCustoms = " المستندات الجمركية";
    listalldocuments = " جميع المستندات";
    Filters = "معايير تصفية";
    textAdditionalDocuments = " مستندات إضافية";
    reviewButtonValue = "مراجعة الوثائق";
}

//$('#sidebarCollapse').on('click', function () {
//    alert('hi');
//    $('#sidebar').toggleClass('active');
//    $(this).toggleClass('active');
//});



var ReviewStatusImages=[{ Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.NotReviewed, App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.Reviewed   , App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.Rejected,    Rev: ImagesUsed.NotReviewed, App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.NotReviewed, App: ImagesUsed.Approved }
];

var CurrentDocument = {
    DocTypeIdx : null,
    DocNameIdx: null  
}



function renderDocType(docset)
{
  //  var filterDiv = document.getElementById("FilterDiv").innerHTML;
    docGallerySet = docset;
    var docTypesContainer = document.getElementById("DocTypesContainer");


    var docTypesHTML = "";
    for (var docTypeIdx in docset)
    {
        var docTypes = docset[docTypeIdx];
        var autoViewDoc = 'viewDoc(\'' + docTypes.doctype + '\',\'' + docTypes.uploadedDocuments[0].name + '\',' + docTypeIdx + ',0,\'' + docTypes.uploadedDocuments[0].FileNameId + '\')';
        var countsHtml = '<br/>' + '<div id="DocTypeCounts_' + docTypeIdx + '" style="width:100%;">' +
            renderDocTypeCounts(docTypeIdx) +
            '</div>'
        var checked = "";
        if (docTypes.Selected) checked = "checked='true'";

        if (bEnableItemAssociationStatus)
            {
        var select = '<input type="checkbox" id="DOCTYPESELL_' + docTypeIdx + '" ' + checked + ' onclick="SelectDocTypes(this,' + docTypeIdx + '); CheckSelectAllDoc();" />';

        docTypesHTML += '<tr><td id="doctype_' + docTypeIdx + '" class="DOCTYPE" onclick="renderDocNames(\'' + docTypeIdx + '\');' + autoViewDoc + '; ">' + select + docTypes.doctype + countsHtml + '</td></tr>';
        }
        else
        {
            docTypesHTML += '<tr><td id="doctype_' + docTypeIdx + '" class="DOCTYPE" onclick="renderDocNames(\'' + docTypeIdx + '\');' + autoViewDoc + '; ">' + docTypes.doctype + countsHtml + '</td></tr>';

        }
    }
    //changes
    // var selectalltypes = '<input type="checkbox" id="DOCTYPESELLALL" ' + checked + ' onclick="SelectAllDocTypes(this)" /> Select/Unselect All';
   
    docTypesHTML = '<table class="DOCTYPETABLE">' + docTypesHTML + '</table>';
    var htmlReviewItems = '<input type="button" value="' + reviewButtonValue + '" OnClientClick="return SaveReviewStatus();"  onclick= "SubmitforReview();" />';

    if (bEnableItemAssociationStatus)
    {
        var selectalltypes = '<input type="checkbox" id="DOCTYPESELLALL" ' + checked + ' onclick="SelectAllDocTypes(this)" /> ' + selectionText + '';

        // var htmlAssociateItems = '<input type="button" value="Associate Items"  style="width:100%" onclick="AssociateItems();"/>';
        var htmlAssociateItems = '<input type="button" value="' + buttonvalue + '"  onclick= "AssociateItems();" />';
 
   
        {

            
            leftTable = '<table class="DOCTYPETABLE" style="height:100%">' +
                '<tr> <td> <div class="wrapper"  id= "FilterDiv" > ' +
                ' <nav id="sidebar" style="max-height:400px;">   <div class="sidebar-header">   <h3>' + Filters +'</h3> </div>   <ul style="padding-left: 7%;" class="list-unstyled components">' +
                '<li><a href="#" onclick="Fn_ViewAllRecords(this)">' + listalldocuments + '</a></li>' +
                '<li><a href="#pageSubmenu" id="main" onmouseout="ToggleSubmenu()" onmouseover="ToggleSubmenu()"  " aria-expanded="false">'+ textAdditionalDocuments+'</a>'+
                '<ul onmouseout="ToggleSubmenu()" class="collapse list-unstyled" id="pageSubmenu">' +
                '<li> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByALL + '</a></li> ' +
                '<li> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByCustoms + '</a></li> ' +
                '<li> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByOGA + '</a></li> ' +
               '<li> <a href="#"   onclick="Fn_ViewDeletedRecords(this)">' + checkboxtext + '</a> </li>' +
                '</ul></li> ' +
                '</ul></nav><div id="content"><nav class="navbar navbar-default"><div class="container-fluid"><div class="navbar-header"> <img width="10" height="10" class="navbar-btn inactive" id="sidebarCollapse" title='+ Filters +' onclick="Toggle()" src="Images/iconFilter.png" >' +
                '<span></span><span></span><span></span> </button >  </div ><div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">' +
                '  </div></div>      </nav>    </div> </div> </td> </tr >'+
                '<tr><td>' + selectalltypes + '</td></tr>' +
                '<tr><td >' + docTypesHTML + '</td></tr>' +
                '<tr><td>' + htmlAssociateItems + '</td></tr>' +
                '<tr><td>' + htmlReviewItems + '</td></tr>' +
                '</table>';

        }
   

        docTypesContainer.innerHTML = leftTable;


        $("#pageSubmenu").parent().find('ul').toggle();

    }
    else
    {
        if (bEnableDeletedRecordsStatus == "true")
        {
      

            leftTable = '<table class="DOCTYPETABLE" style="height:100%">' +
                '<tr> <td> <div class="wrapper"  id= "FilterDiv" > ' +
                ' <nav id="sidebar" style="max-height:400px;">   <div class="sidebar-header">   <h3>' + Filters + '</h3> </div>   <ul style="padding-left: 7%;" class="list-unstyled components">' +
                '<li><a href="#" onclick="Fn_ViewAllRecords(this)">' + listalldocuments + '</a></li>' +
                '<li><a href="#pageSubmenu" id="main" onmouseout="ToggleSubmenu()" onmouseover="ToggleSubmenu()"  " aria-expanded="false">' + textAdditionalDocuments +'</a>' +
                '<ul class="collapse list-unstyled" id="pageSubmenu">' +

                '<li onmouseout="ToggleSubmenu()"> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByALL + '</a></li> ' +
                '<li onmouseout="ToggleSubmenu()"> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByCustoms + '</a></li> ' +
                '<li onmouseout="ToggleSubmenu()"> <a href="#"  onclick="Fn_ViewAllAdditional(this)">' + listadditionaldocumentsByOGA + '</a></li> ' +

                '  <li> <a href="#"   onmouseout="ToggleSubmenu()" onclick="Fn_ViewDeletedRecords(this)">' + checkboxtext + '</a> </li>' +

                '</ul></li> ' +
                //'  <li> <a href="#" onclick="Fn_ViewDeletedRecords(this)">' + checkboxtext + '</a> </li>' +

                '</ul></nav><div id="content"><nav class="navbar navbar-default"><div class="container-fluid"><div class="navbar-header"> <img width="10" height="10" class="navbar-btn inactive" id="sidebarCollapse" title=' + Filters +' onclick="Toggle()" src="Images/iconFilter.png" >' +
                '<span></span><span></span><span></span> </button >  </div ><div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">' +
                '  </div></div>      </nav>    </div> </div> </td> </tr >' +
                '<tr><td >' + docTypesHTML + '</td></tr>' +
                '<tr><td>' + htmlReviewItems + '</td></tr>' +
                '</table>';

       ///     $('#sidebar').toggleClass('active');
                    docTypesContainer.innerHTML = leftTable;
            //    $("#sidebarCollapse").find(".active").removeClass("active");
          //  docTypesContainer.innerHTML = docTypesHTML;
        }

        else
            {
    docTypesContainer.innerHTML = docTypesHTML;
        }
    }
  //  docTypesContainer.innerHTML = docTypesHTML;

    if (docset.length > 0) {
        renderDocNames(0);
        documentid = "'" + docset[0].uploadedDocuments[0].FileNameId + "'";
        viewDoc(docset[0].doctype, docset[0].uploadedDocuments[0].name, 0, 0, docset[0].uploadedDocuments[0].FileNameId);
    }
}


function Toggle()
{
    // alert('toggle');        
    //$("#pageSubmenu").parent().find('ul').toggle();
  //  ToggleSubmenu();
    $('#sidebar').toggleClass('active');
    $(this).toggleClass('active');

}

function ToggleSubmenu() {
   $("#pageSubmenu").parent().find('ul').toggle();

  //  document.getElementById("pageSubmenu").parent().find('ul').toggle();
}
//window.onload = function () {
//    $("#pageSubmenu").parent().find('ul').toggle();
//};

// 
function Fn_ViewDeletedRecords(element)
{
    document.getElementById("labelText").value = element.innerHTML;
 //   document.getElementById("TDLABEL").innerHTML = element.innerHTML;
    document.getElementById("DeleteFlagTest").value = "ScanRequestUploadDocsDeleted";

    document.getElementById("TypeOfFilter").value = element.innerHTML;
    //if (document.getElementById("deleteText").checked == true)
    //{
    //    document.getElementById("DeleteFlagTest").value = "ScanRequestUploadDocsDeleted";
        
    //}
    //else
    //{
    //    document.getElementById("DeleteFlagTest").value = "ScanRequestUploadDocsCreated";

    //}

    document.forms[0].submit();


  //  deleteText
 //   document.getElementById("DeleteFlagTest").value = "undeleteText";
}



function Fn_ViewAllRecords(element)
{
    //document.getElementById("TDLABEL").innerHTML = element.innerHTML;
    document.getElementById("labelText").value = element.innerHTML;
    document.getElementById("DeleteFlagTest").value = "ScanRequestUploadDocsCreated";
    document.getElementById("TypeOfFilter").value = "ALLDOCS";
    document.forms[0].submit();
   // document.getElementById("TDLABEL").innerHTML = element.innerHTML;
   // document.getElementById("labelText").value = element.innerHTML;




}

function Fn_ViewAllAdditional(element) {
    document.getElementById("labelText").value = element.innerHTML;
    document.getElementById("DeleteFlagTest").value = "ScanRequestUploadDocsCreated";
    document.getElementById("TypeOfFilter").value = "AllAdditional";
    document.forms[0].submit();

}

function SubmitforReview()
{
    $("#SaveBtn").trigger("click");
}

function SelectAllDocTypes(elChk)
{
    DocTypesSelected = elChk.checked;

    for (var docTypeIdx in docGallerySet)
    {
        var docTypeCheck = document.getElementById("DOCTYPESELL_" + docTypeIdx);
        docTypeCheck.checked = DocTypesSelected;
        SelectDocTypes(docTypeCheck, docTypeIdx);
    }
}

function CheckSelectAllDoc()
{
    var allDocTypes = true
    for (var docTypeIdx in docGallerySet) {
        if (!docGallerySet[docTypeIdx].Selected)
        {
            allDocTypes = false;
            break;
        }
    }
    document.getElementById("DOCTYPESELLALL").checked = allDocTypes;
}

function SelectDocTypes(elChk, docTypeIdx) {
    docGallerySet[docTypeIdx].Selected = elChk.checked;
    for (var docNameIdx in docGallerySet[docTypeIdx].uploadedDocuments)

    {
        docGallerySet[docTypeIdx].uploadedDocuments[docNameIdx].Selected = elChk.checked;
        if (document.getElementById("DOCSELL_" + docTypeIdx + "_" + docNameIdx) != null)
            document.getElementById("DOCSELL_" + docTypeIdx + "_" + docNameIdx).checked = elChk.checked;
    }
}

//changes
function SelectDocument(elChk, docTypeIdx, docNameIdx)
{
    docGallerySet[docTypeIdx].uploadedDocuments[docNameIdx].Selected = elChk.checked;

    var docTypeSelected = true
    for (var i in docGallerySet[docTypeIdx].uploadedDocuments)
    {
        if (!docGallerySet[docTypeIdx].uploadedDocuments[i].Selected)
        {
            docTypeSelected = false;
            break;
        }
    }

    docGallerySet[docTypeIdx].Selected = docTypeSelected;

    document.getElementById("DOCTYPESELL_" + docTypeIdx).checked = docTypeSelected;

    CheckSelectAllDoc();
}


function AssociateItems() {
    //this will view the associated items for the selected documents if any, or it will show all associated items for all documents if no document selected.
    //iterate through the selected documents.
    var selectedDocId = new Array();
    var c = 0;

    var getlanguageforalert = document.getElementById('GetDirection').innerHTML;
    for (var docTypeIdx in docGallerySet) {
        for (var docNameIdx in docGallerySet[docTypeIdx].uploadedDocuments) {
            if (docGallerySet[docTypeIdx].uploadedDocuments[docNameIdx].Selected) {
                selectedDocId[selectedDocId.length] = docGallerySet[docTypeIdx].uploadedDocuments[docNameIdx].FileNameId;
            }
            c++;
        }
    }

    if (selectedDocId.length == 0) {
        if (getlanguageforalert == "en") {
            // alert('No Files To Remove');
            alert("No Documents Selected to Associate ");
            return false;
        }
        else {

            alert("قم باختيار قائمة المستندات للربط ");
            return false;
        }

    }
    //alert('oyt');
  //  alert('Selected documents:\n' + selectedDocId);
   // if (c > selectedDocId.length && selectedDocId.length > 0)
        {
     //   alert('Selected documents:\n' + selectedDocId);

        var sWinName = '';
        var nWidth = 1200;
        var nHeight = 900;
        var bScroll = true;;
        var bResize = false;
        var full_screen = 'fullscreen=yes,';
        var fromtop = 0;
        var fromleft = 0;
        var oWindow;
 
        var scrolling = 'no';
        var resizeing = 'no';
        if (bScroll) scrolling = 'yes';
        if (bResize) resizeing = 'yes';
     //   sPageURL = 'http://localhost:56770/ItemAssociation.aspx?Documentid=' + selectedDocId;
 // sPageURL = 'http://localhost:56770/ItemAssociation.aspx?Documentid=' + selectedDocId;
   sPageURL = 'http://10.10.26.226/ScanUploadServicesPP/ItemAssociation.aspx?Documentid=' + selectedDocId;
        oWindow = window.open(sPageURL, sWinName, 'directories=no,height=' + nHeight + ',width=' + nWidth + ',left=' + fromleft + ',top=' + fromtop + ',screeny=' + fromtop + ',screenx=' + fromleft + ',location=no,menubar=no,resizable=' + resizeing + ',scrollbars=' + scrolling + ',status=no,toolbar=no', true);

    //window.location.href = "http://localhost:56770/ItemAssociation.aspx?Documentid=" + selectedDocId;

  //      window.open("http://localhost:56770/ItemAssociation.aspx?Documentid=" + selectedDocId, "fullscreen=yes", "resizable=no");
    }

    //else
    //    {
    //    alert('Connsider all documents');
    //}



}




//end changes for item association

function renderDocNames(docTypeIdx) {

    var docNamesContainer = document.getElementById("DocNamesContainer");
    var docNamesContainerTitle = document.getElementById("DocNamesContainerTitle");
    var docTypes = docGallerySet[docTypeIdx];
    docNamesContainerTitle.innerText = docTypes.doctype;
    var oldEls = document.getElementsByClassName('DOCTYPE_SELECTED')
    var newEl = document.getElementById('doctype_' + docTypeIdx);
    if (oldEls.length > 0) oldEls[0].className = 'DOCTYPE';    
    if (newEl) newEl.className = 'DOCTYPE_SELECTED';
    var docNamesHTML = "";
    var id= "";
    for (var docNameIdx in docTypes.uploadedDocuments)
    {
        id = docTypes.uploadedDocuments[docNameIdx].FileNameId;
        var reviewedMark = RenderReviewStatus(docTypeIdx, docNameIdx) + '<br/>';
        
        var docser = eval(docNameIdx) + 1;
        var shortname = docTypes.uploadedDocuments[docNameIdx].name;
        if(shortname.length>docnamelength) {
            shortname = shortname.substr(0, docnamelength) + " ...";
        }
        //changes
        if (typeof docTypes.uploadedDocuments[docNameIdx].AdditionalDocRequestNumber === 'undefined')
        {

        }
        else
            {
        if (docTypes.uploadedDocuments[docNameIdx].AdditionalDocRequestNumber != 0)
        {
            var additionaldociamge = '<img id="img_Add_' + docTypes.uploadedDocuments[docNameIdx].AdditionalDocRequestNumber + '" title="' + docTypes.uploadedDocuments[docNameIdx].AdditionalDocRequestNumber + '|' + docTypes.uploadedDocuments[docNameIdx].RequestedBy + '|' + docTypes.uploadedDocuments[docNameIdx].RequestDateTime + '|' + docTypes.uploadedDocuments[docNameIdx].UploadDatetime + '" class="REVIEWSTATUS" src="Images/Rs_AddDocs.png"  />'
          //  additionaldociamge = additionaldociamge.replace(/ /g, "-");
            reviewedMark = additionaldociamge + reviewedMark;
        }
        }
        var checked = "";
        if (docTypes.uploadedDocuments[docNameIdx].Selected) checked = "checked='true'";
        if (bEnableItemAssociationStatus)
        {
            docNamesHTML += '<td id="DOCNAMECELL_' + docTypeIdx + '_' + docNameIdx + '" class="DOCNAMECELL">' +
                reviewedMark +
                '<input type="checkbox" id="DOCSELL_' + docTypeIdx + '_' + docNameIdx + '" class="DOCSELL" ' + checked + ' onclick="SelectDocument(this,' + docTypeIdx + ',' + docNameIdx + ')" />' +
                '<span class="DOCNAME" onclick="viewDoc(\'' + docTypes.doctype + '\',\'' + docTypes.uploadedDocuments[docNameIdx].name + '\',' + docTypeIdx + ',' + docNameIdx + ',\'' + id + '\')" title="' + docTypes.uploadedDocuments[docNameIdx].name + '">' + docser + ':' + shortname + '</span></td > ';
        }
        else
        {
            docNamesHTML += '<td id="DOCNAMECELL_' + docTypeIdx + '_' + docNameIdx + '" class="DOCNAMECELL">' + reviewedMark + '<span class="DOCNAME" onclick="viewDoc(\'' + docTypes.doctype + '\',\'' + docTypes.uploadedDocuments[docNameIdx].name + '\',' + docTypeIdx + ',' + docNameIdx + ',\'' + id + '\')" title="' + docTypes.uploadedDocuments[docNameIdx].name + '">' + docser + ':' + shortname + '</span></td>';
        }
    }
    docNamesHTML = '<table id="docNamesTable" class="DOCNAMES"><tr>' + docNamesHTML + '</tr></table>';
    docNamesContainer.innerHTML = docNamesHTML;
}

var doccellidx = 0;
function viewDoc(doctype, pPath, docTypeIdx, docNameIdx, documentid) {
    doccellidx = docNameIdx;
    if (CurrentDocument.DocTypeIdx != docTypeIdx || CurrentDocument.DocNameIdx != docNameIdx) {
        CurrentDocument.DocTypeIdx = docTypeIdx;
        CurrentDocument.DocNameIdx = docNameIdx;
        var oldEls = document.getElementsByClassName('DOCNAMECELL_SELECTED')
        var newEl = document.getElementById('DOCNAMECELL_' + docTypeIdx + '_' + docNameIdx);
        if (oldEls.length > 0) oldEls[0].className = 'DOCNAMECELL';
        if (newEl) newEl.className = 'DOCNAMECELL_SELECTED';
        var docNamesContainerTitle = document.getElementById("DocNamesContainerTitle");
        docNamesContainerTitle.innerText = doctype + ":" + pPath;
        docNamesContainerTitle.setAttribute("value", documentid); 
        var viewerIF = document.getElementById("documentViewer");
        if (inProduction)
        {
            var s = documentid;
            viewerIF.src = "RenderFile.aspx?Documentid=" + documentid;
            //viewerIF.title = "test";
            //viewerIF.textContent = "test";
            //viewerIF.textContent.title = "test";
        }
        else {
            viewerIF.src = ".." + documentid;
        }
    }
}

function RenderReviewStatus(docTypeIdx, docNameIdx)
{
    var results="";


    var data1;
    
    if (bEnableReviewStatus || bEnableApproveStatus || bEnableRejectStatus || bEnableItemAssociationStatus)
        {
     //   var docTypes = docGallerySet[docTypeIdx];
  //  var docRec = docTypes.uploadedDocuments[docNameIdx];
    }
    if (bEnableRejectStatus) {
        var docTypes = docGallerySet[docTypeIdx];
        var docRec = docTypes.uploadedDocuments[docNameIdx];
         results = '<img id="img_Rej_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rej + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' 
        //    '<img id="img_Rev_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rev + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' +
       //     '<img id="img_App_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].App + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>';

       
    }
    if (bEnableReviewStatus)
    {
        var docTypes = docGallerySet[docTypeIdx];
        var docRec = docTypes.uploadedDocuments[docNameIdx];
        //if (results != "")
            {
        results = results + '<img id="img_Rev_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rev + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>'
        }

    }

    if (bEnableApproveStatus)
    {
        var docTypes = docGallerySet[docTypeIdx];
        var docRec = docTypes.uploadedDocuments[docNameIdx];
       // if (results != "")
        {
            results = results + '<img id="img_App_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].App + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>'
        }

    }
    //  if(res)
    if (results != "")

    return results ;
    else return "";
}



//function RenderReviewStatus(docTypeIdx, docNameIdx) {

//    if (bEnableReviewStatus) {
//        var docTypes = docGallerySet[docTypeIdx];
//        var docRec = docTypes.uploadedDocuments[docNameIdx];

//        var results = '<img id="img_Rej_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rej + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' +
//            '<img id="img_Rev_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rev + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' +
//            '<img id="img_App_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].App + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>';

//        return results;
//    }
//    else return "";
////}
//function UpdateReviewStatus(docTypeIdx, docNameIdx, imgEl) {
//    //Check if the document in view
//    if (CurrentDocument.DocTypeIdx != docTypeIdx || CurrentDocument.DocNameIdx != docNameIdx) {
//        alert(msgSelectADocument);
//        return;
//    }

//    var docTypes = docGallerySet[docTypeIdx];
//    var docRec = docTypes.uploadedDocuments[docNameIdx];

//    var imgElRej = document.getElementById('img_Rej_' + docTypeIdx + '_' + docNameIdx);
//    var imgElRev = document.getElementById('img_Rev_' + docTypeIdx + '_' + docNameIdx);
//    var imgElApp = document.getElementById('img_App_' + docTypeIdx + '_' + docNameIdx);


//    if (imgElRej != null) {
//        if (imgElRej.id == imgEl.id) {
//            if (docRec.ReviewStatus != enumReviewStatus.Rejected) {
//                imgEl.src = ImagesUsed.Rejected;
//                docRec.ReviewStatus = enumReviewStatus.Rejected;
//            }
//            else {
//                imgEl.src = ImagesUsed.NotRejected;
//                docRec.ReviewStatus = enumReviewStatus.NotSeen;
//            }
//            imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
//            // added latest 
//            imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
//            //  notApproved

//        }

//    }
//     if (imgElRev != null) {
//        if (imgElRev.id == imgEl.id) {
//            if (docRec.ReviewStatus != enumReviewStatus.Reviewed) {
//                imgEl.src = ImagesUsed.Reviewed;
//                docRec.ReviewStatus = enumReviewStatus.Reviewed;
//            }
//            else {
//                imgEl.src = ImagesUsed.NotReviewed;
//                docRec.ReviewStatus = enumReviewStatus.NotSeen;
//            }
//            imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;

//            imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
//        }
//    }
//    if  (imgElApp != null) {
//        if (imgElApp.id == imgEl.id) {
//            if (docRec.ReviewStatus != enumReviewStatus.Approved) {
//                imgEl.src = ImagesUsed.Approved;
//                docRec.ReviewStatus = enumReviewStatus.Approved;
//            }
//            else {
//                imgEl.src = ImagesUsed.NotApproved;
//                docRec.ReviewStatus = enumReviewStatus.NotSeen;
//            }
//            imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;
//            imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
//        }

//        KeepStatus(docRec.FileNameId, docRec.ReviewStatus);


//        UpdateDocTypeCounts(docTypeIdx);
//    }
//}
function UpdateReviewStatus(docTypeIdx, docNameIdx, imgEl)
{
    //Check if the document in view
    if (CurrentDocument.DocTypeIdx != docTypeIdx || CurrentDocument.DocNameIdx != docNameIdx) {
        alert(msgSelectADocument);
        return;
    }

    var docTypes = docGallerySet[docTypeIdx];
    var docRec = docTypes.uploadedDocuments[docNameIdx];

    var imgElRej = document.getElementById('img_Rej_' + docTypeIdx + '_' + docNameIdx);
    var imgElRev = document.getElementById('img_Rev_' + docTypeIdx + '_' + docNameIdx);
    var imgElApp = document.getElementById('img_App_' + docTypeIdx + '_' + docNameIdx);

    if (imgElRej != null)
        {

    if(imgElRej.id == imgEl.id)
    {
        if (docRec.ReviewStatus != enumReviewStatus.Rejected) {
            imgEl.src = ImagesUsed.Rejected;
            docRec.ReviewStatus = enumReviewStatus.Rejected;
        }
        else{
            imgEl.src = ImagesUsed.NotRejected;
            docRec.ReviewStatus = enumReviewStatus.NotSeen;
        }
        if (imgElRev != null) {
            imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
        }
            // added latest
        if (imgElApp != null) {
            imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
        }    //  notApproved

    }
    }
      
     if (imgElRev != null)
        {

     if (imgElRev.id == imgEl.id) {
        if (docRec.ReviewStatus != enumReviewStatus.Reviewed) {
            imgEl.src = ImagesUsed.Reviewed;
            docRec.ReviewStatus = enumReviewStatus.Reviewed;
        }
        else {
            imgEl.src = ImagesUsed.NotReviewed;
            docRec.ReviewStatus = enumReviewStatus.NotSeen;
         }
         if (imgElRej != null){
        imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;
        }
         if (imgElApp != null) {
             imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
         }
     }
    }

    if (imgElApp != null)
        {
     if (imgElApp.id == imgEl.id) {
        if (docRec.ReviewStatus != enumReviewStatus.Approved) {
            imgEl.src = ImagesUsed.Approved;
            docRec.ReviewStatus = enumReviewStatus.Approved;
        }
        else {
            imgEl.src = ImagesUsed.NotApproved;
            docRec.ReviewStatus = enumReviewStatus.NotSeen;
        }
        if (imgElRej != null) {
            imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;
        }
        if (imgElRev != null) {
            imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
        }

     }
    }
  KeepStatus(docRec.FileNameId, docRec.ReviewStatus);
 //   KeepStatus(docRec.FileNameId,'1');


    UpdateDocTypeCounts(docTypeIdx);
}


//function renderDocTypeCounts(docTypeIdx) {
//    if (bEnableReviewStatus) {

//        var docTypes = docGallerySet[docTypeIdx];

//        var countReviewed = 0;
//        var countRejected = 0;
//        var countApproved = 0;

//        var countAll = docTypes.uploadedDocuments.length;

//        var countsHtml = "";
//        for (var docNameIdx in docTypes.uploadedDocuments) {

//            var docTypes = docGallerySet[docTypeIdx];
//            var docRec = docTypes.uploadedDocuments[docNameIdx];
//            if (docRec.ReviewStatus == enumReviewStatus.Rejected) countRejected++;
//            if (docRec.ReviewStatus == enumReviewStatus.Reviewed) countReviewed++;
//            if (docRec.ReviewStatus == enumReviewStatus.Approved) countApproved++;

//        }



//        countsHtml += '<img src="' + ReviewStatusImages[enumReviewStatus.Rejected].Rej + '" width="10" height="10" style="padding-left:5px; padding-right:5px;" />' + countRejected +
//            '<img src="' + ReviewStatusImages[enumReviewStatus.Reviewed].Rev + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countReviewed +
//            '<img src="' + ReviewStatusImages[enumReviewStatus.Approved].App + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countApproved +

//            ' | ' + (countAll - (countRejected + countReviewed + countApproved))



//        return countsHtml;
//    }
//    else {
//        return "";
//    }
//}

function renderDocTypeCounts(docTypeIdx)
{
    if (bEnableReviewStatus || bEnableApproveStatus || bEnableRejectStatus)
    {

        var docTypes = docGallerySet[docTypeIdx];

        var countReviewed = 0;
        var countRejected = 0;
        var countApproved = 0;

        var countAll = docTypes.uploadedDocuments.length;

        var countsHtml = "";
        for (var docNameIdx in docTypes.uploadedDocuments) {

            var docTypes = docGallerySet[docTypeIdx];
            var docRec = docTypes.uploadedDocuments[docNameIdx];
            if (docRec.ReviewStatus == enumReviewStatus.Rejected) countRejected++;
            if (docRec.ReviewStatus == enumReviewStatus.Reviewed) countReviewed++;
            if (docRec.ReviewStatus == enumReviewStatus.Approved) countApproved++;

        }
        if (bEnableApproveStatus && bEnableRejectStatus && bEnableReviewStatus)
        {
            countsHtml += '<img src="' + ReviewStatusImages[enumReviewStatus.Rejected].Rej + '" width="10" height="10" style="padding-left:5px; padding-right:5px;" />' + countRejected +
                '<img src="' + ReviewStatusImages[enumReviewStatus.Reviewed].Rev + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countReviewed +
                '<img src="' + ReviewStatusImages[enumReviewStatus.Approved].App + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countApproved +

                ' | ' + (countAll - (countRejected + countReviewed + countApproved))
        }
        else
            {
        if (bEnableRejectStatus)
            {

        countsHtml += '<img src="' + ReviewStatusImages[enumReviewStatus.Rejected].Rej + '" width="10" height="10" style="padding-left:5px; padding-right:5px;" />' + countRejected +
          
            ' | ' + (countAll - (countRejected ))

        }



        if (bEnableReviewStatus) {

            countsHtml +=       '<img src="' + ReviewStatusImages[enumReviewStatus.Reviewed].Rev + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countReviewed +
               
                ' | ' + (countAll - ( countReviewed ))

        }

        if (bEnableApproveStatus) {

            countsHtml += '<img src="' + ReviewStatusImages[enumReviewStatus.Approved].App + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countReviewed +

                ' | ' + (countAll - (countApproved))

        }
        }

        return countsHtml;
    }
    else {
        return "";
    }
}

function UpdateDocTypeCounts(docTypeIdx) {

    var divEl = document.getElementById("DocTypeCounts_" + docTypeIdx);
    divEl.innerHTML = renderDocTypeCounts(docTypeIdx)
}

function KeepStatus(pDocid, pReviewStatus)
{

    for (var i = 0; i < arr.length; i++) {
        if (arr[i].DocumentId == pDocid) {
            arr[i] = { DocumentId: pDocid, ReviewStatus: pReviewStatus };
            return;
        }
    }

    arr[arr.length] = { DocumentId: pDocid, ReviewStatus: pReviewStatus };
}
function setDocumentDetails()
{
    ButtonDetails.setAttribute("value", documentid); 
    return true;
}


function SaveReviewStatus() {
    var i = 0;
    var strArrIdx = new Array();
    var strArrStatus = new Array();

    for (var i = 0; i < arr.length; i++) {
        strArrIdx[strArrIdx.length] = arr[i].DocumentId;
        strArrStatus[strArrStatus.length] = arr[i].ReviewStatus;

     }

    if (strArrIdx.length > 0) {
        form1.txtArrayIdx.value = strArrIdx.join(',');
        form1.txtArrayStatus.value = strArrStatus.join(',');
    }
    
    else {
        return false;
    }
}


function CloseGallery()
{
    if (arr.length == 0) {
        window.close();
    }
    else
        {
        var r = confirm("You have unsaved Changes!");
        if (r == true) {
            window.close();
        }
        else {
            return false;
        }
    }


}




function scrollIt(dir1) {

    var docNamesContainer = document.getElementById("DocNamesContainer");
    var docNamesContainerScrollwidth = docNamesContainer.scrollWidth;

    if (docNamesContainerScrollwidth < 600) {
        docNamesContainer.scrollLeft = 0;
    }
    else if (dir1 == 1) {
        docNamesContainer.scrollLeft = docNamesContainer.scrollLeft + 600
    }
    else if (dir1 == -1) {
        docNamesContainer.scrollLeft = docNamesContainer.scrollLeft - 600
    }
}


function scrollto() {

    var docNamesContainer = document.getElementById("DocNamesContainer");
    var docNamesTable = document.getElementById("docNamesTable");
    var selectedTrEl = document.getElementById("DOCNAMECELL_" + CurrentDocument.DocTypeIdx + "_" + CurrentDocument.DocNameIdx);
    
    if (selectedTrEl) {
        var scrollTo = 1 * selectedTrEl.offsetLeft;
        //in IE this works just fine
        docNamesContainer.scrollLeft =  docNamesContainer.scrollWidth - (scrollTo + selectedTrEl.clientWidth);
        //or just this statement it works
        selectedTrEl.scrollIntoView(true);
    }
}