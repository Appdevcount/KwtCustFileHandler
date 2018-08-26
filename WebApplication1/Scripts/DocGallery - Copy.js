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

var ReviewStatusImages=[{ Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.NotReviewed, App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.Reviewed   , App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.Rejected,    Rev: ImagesUsed.NotReviewed, App: ImagesUsed.NotApproved },
                        { Rej: ImagesUsed.NotRejected, Rev: ImagesUsed.NotReviewed, App: ImagesUsed.Approved }
];

var CurrentDocument = {
    DocTypeIdx : null,
    DocNameIdx: null  
}

function renderDocType(docset) {
    docGallerySet = docset;
    var docTypesContainer = document.getElementById("DocTypesContainer");
    var docTypesHTML = "";
    for (var docTypeIdx in docset) {

        var docTypes = docset[docTypeIdx];
        var autoViewDoc = 'viewDoc(\'' + docTypes.doctype + '\',\'' + docTypes.uploadedDocuments[0].name + '\',' + docTypeIdx + ',0,\'' + docTypes.uploadedDocuments[0].FileNameId + '\')';
        var countsHtml = '<br/>' + '<div id="DocTypeCounts_' + docTypeIdx + '" style="width:100%;">' +
            renderDocTypeCounts(docTypeIdx) +
            '</div>'
        docTypesHTML += '<tr><td id="doctype_' + docTypeIdx + '" class="DOCTYPE" onclick="renderDocNames(\'' + docTypeIdx + '\');' + autoViewDoc + '; ">' + docTypes.doctype + countsHtml + '</td></tr>';

    }
    docTypesHTML = '<table class="DOCTYPETABLE">' + docTypesHTML + '</table>';
    docTypesContainer.innerHTML = docTypesHTML;

    if (docset.length > 0) {
        renderDocNames(0);
        documentid = "'"+docset[0].uploadedDocuments[0].FileNameId+"'";
        viewDoc(docset[0].doctype, docset[0].uploadedDocuments[0].name, 0, 0, docset[0].uploadedDocuments[0].FileNameId);
    }
}


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
    for (var docNameIdx in docTypes.uploadedDocuments) {
        id =  docTypes.uploadedDocuments[docNameIdx].FileNameId ;
        var reviewedMark = RenderReviewStatus(docTypeIdx, docNameIdx) + '<br/>';
        var docser = eval(docNameIdx) + 1;
        var shortname = docTypes.uploadedDocuments[docNameIdx].name;
        if(shortname.length>docnamelength) {
            shortname = shortname.substr(0, docnamelength) + " ...";
        }
        docNamesHTML += '<td id="DOCNAMECELL_' + docTypeIdx + '_' + docNameIdx + '" class="DOCNAMECELL">' + reviewedMark + '<span class="DOCNAME" onclick="viewDoc(\'' + docTypes.doctype + '\',\'' + docTypes.uploadedDocuments[docNameIdx].name + '\',' + docTypeIdx + ',' + docNameIdx + ',\'' + id + '\')" title="' + docTypes.uploadedDocuments[docNameIdx].name + '">' + docser + ':' + shortname + '</span></td>';
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
        }
        else {
            viewerIF.src = ".." + documentid;
        }
    }
}

function RenderReviewStatus(docTypeIdx, docNameIdx)
{
    if (bEnableReviewStatus) {
        var docTypes = docGallerySet[docTypeIdx];
        var docRec = docTypes.uploadedDocuments[docNameIdx];

        var results = '<img id="img_Rej_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rej + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' +
            '<img id="img_Rev_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].Rev + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>' +
            '<img id="img_App_' + docTypeIdx + '_' + docNameIdx + '" src="' + ReviewStatusImages[docRec.ReviewStatus].App + '" class="REVIEWSTATUS" onclick="UpdateReviewStatus(' + docTypeIdx + ',' + docNameIdx + ',this)"/>';
        return results;
    }
    else return "";
}


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
        imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
        // added latest 
        imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
      //  notApproved

    }
    else if (imgElRev.id == imgEl.id) {
        if (docRec.ReviewStatus != enumReviewStatus.Reviewed) {
            imgEl.src = ImagesUsed.Reviewed;
            docRec.ReviewStatus = enumReviewStatus.Reviewed;
        }
        else {
            imgEl.src = ImagesUsed.NotReviewed;
            docRec.ReviewStatus = enumReviewStatus.NotSeen;
        }
        imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;

        imgElApp.src = ReviewStatusImages[enumReviewStatus.NotSeen].App;
    }


    else if (imgElApp.id == imgEl.id) {
        if (docRec.ReviewStatus != enumReviewStatus.Approved) {
            imgEl.src = ImagesUsed.Approved;
            docRec.ReviewStatus = enumReviewStatus.Approved;
        }
        else {
            imgEl.src = ImagesUsed.NotApproved;
            docRec.ReviewStatus = enumReviewStatus.NotSeen;
        }
        imgElRej.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rej;
        imgElRev.src = ReviewStatusImages[enumReviewStatus.NotSeen].Rev;
    }

    KeepStatus(docRec.FileNameId, docRec.ReviewStatus);

//    KeepStatus(docRec.DocumentId, docRec.ReviewStatus);

    UpdateDocTypeCounts(docTypeIdx);
}

function renderDocTypeCounts(docTypeIdx)
{
    if (bEnableReviewStatus) {

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

        countsHtml += '<img src="' + ReviewStatusImages[enumReviewStatus.Rejected].Rej + '" width="10" height="10" style="padding-left:5px; padding-right:5px;" />' + countRejected +
            '<img src="' + ReviewStatusImages[enumReviewStatus.Reviewed].Rev + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countReviewed +
            '<img src="' + ReviewStatusImages[enumReviewStatus.Approved].App + '" width="8" height="10" style="padding-left:5px; padding-right:5px;"/>' + countApproved +

            ' | ' + (countAll - (countRejected + countReviewed + countApproved))
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
    for(var i=0; i<arr.length; i++)
    {
        if (arr[i].DocumentId == pDocid)
        {
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
    
    //var filevalue = document.getElementById("DocNamesContainerTitle").getAttribute("value");

    var strArrIdx = new Array();
    var strArrStatus = new Array();

    for (var i = 0; i < arr.length; i++) {
        strArrIdx[strArrIdx.length] = arr[i].DocumentId;
        strArrStatus[strArrStatus.length] = arr[i].ReviewStatus;

        //strArr[strArr.length] = '{DocumentId:"' + arr[i].DocumentId + '",ReviewStatus:"' + arr[i].ReviewStatus + '"}';
    }

    if (strArrIdx.length > 0) {
        //call for saving
        form1.txtArrayIdx.value = strArrIdx.join(',');
        form1.txtArrayStatus.value = strArrStatus.join(',');
        //on success
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


//function scrollIt(dir1) {
//    var docNamesContainer = document.getElementById("DocNamesContainer");
    
//    var f = 10;
//    var docNamesContainer = document.getElementById("DocNamesContainer");
//    var docNamesTable = document.getElementById("docNamesTable");
    
//    firstEl = document.getElementById("DOCNAMECELL_" + CurrentDocument.DocTypeIdx + "_" + 0);
    
//    if (!docNamesTable.cells && document.dir.toLowerCase() == "rtl") {
//        //handle chrome
//        dir1 = -1 * dir1
//    }
//    if(firstEl)
//    {
//        //if (docNamesContainer && docNamesTable && docNamesTable.cells.length > 0) {
//        //var firstEl = docNamesTable.cells[0];

//        var ms = docNamesContainer.scrollWidth / (firstEl.clientWidth / 2);
//        for (var i = ms; i > 1; i = i - 1) {
//            f = Math.cos(1 / i * i)
//            docNamesContainer.scrollLeft = docNamesContainer.scrollLeft + dir1 * Math.round(i / (f * 2));
//        }
//    }
//}


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
    //scrolling to the selected document, scrollIntoView

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