<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemAssociation.aspx.cs" Inherits="WebApplication1.ItemAssociation" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
    <meta http-equiv="Content-type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1">
 <link href="Content/datatables.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="styles/jquery.dataTables.min.css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
 
<%--    <% var DeclarationId = Session["DeclarationId"].ToString();%>--%>
     <%-- var bEnableReviewStatus = <%=ThemeId.ToString()%>;--%>
<%--    <link href="Content/Blue.css" rel="stylesheet" />--%>

    <link href="Content/<%=ThemeId.ToString() %>" rel="stylesheet" />
    

    <script type="text/javascript" charset="utf8" src="http://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript" charset="utf8" src="scripts/jquery.dataTables.min.js"></script>

    <script src="Scripts/DataTableMultiselect.js"></script>

       <script src="Scripts/CheckBoxDataTables.js"></script>
    <%--        <script src="Scripts/Commercial.js"></script>--%>



    <style type="text/css">
        /*th, td {
            white-space: nowrap;
        }

        div.dataTables_wrapper {
            width: 600px;
            margin: 0 auto;
        }*/
        /*tr:nth-child(even) {
    background-color: #dddddd;
}*/
        #dialog {
            height: 600px;
            overflow: auto;
            font-size: 10pt !important;
            font-weight: normal !important;
            background-color: #FFFFC1;
            margin: 10px;
            border: 1px solid #ff6a00;
        }

            #dialog div {
                margin-bottom: 15px;
            }
    </style>

    <style>    
        .table {
        border: 1px solid #ddd;
      
    }
 

        td input[type="checkbox"] {
    float: left;
    margin: 0 auto;
    width: 100%;
}
        .focus {
            background-color: #ff00ff;
            color: #fff;
            cursor: pointer;
            font-weight: bold;
        }

        .selected {
            /*background-color: #ff00ff;*/
            color: #fff;
            font-weight: bold;
        }

        .asc:after {
            content: "\25B2";
        }

        .desc:after {
            content: "\25BC";
        }

        .table {
            border: 1px solid #ddd;
        }


        .pg-normal {
            color: #000000;
            font-size: 15px;
            cursor: pointer;
            background: #D0B389;
            padding: 2px 4px 2px 4px;
        }

        .pg-selected {
            color: #fff;
            font-size: 15px;
            background: #000000;
            padding: 2px 4px 2px 4px;
        }

        table.yui {
            font-family: arial;
            border-collapse: collapse;
            border: solid 3px #7f7f7f;
            font-size: small;
        }

            table.yui td {
                padding: 5px;
                border-right: solid 1px #7f7f7f;
            }

            table.yui .even {
                background-color: #EEE8AC;
            }

            table.yui .odd {
                background-color: #F9FAD0;
            }

            table.yui th {
                border: 1px solid #7f7f7f;
                padding: 5px;
                height: auto;
                background: #D0B389;
            }

                table.yui th a {
                    text-decoration: none;
                    text-align: center;
                    padding-right: 20px;
                    font-weight: bold;
                    white-space: nowrap;
                }

            table.yui tfoot td {
                border-top: 1px solid #7f7f7f;
                background-color: #E1ECF9;
            }

            table.yui thead td {
                vertical-align: middle;
                background-color: #E1ECF9;
                border: none;
            }

            table.yui thead .tableHeader {
                font-size: larger;
                font-weight: bold;
            }

            table.yui thead .filter {
                text-align: right;
            }

            table.yui tfoot {
                background-color: #E1ECF9;
                text-align: center;
            }

            table.yui .tablesorterPager {
                padding: 10px 0 10px 0;
            }

                table.yui .tablesorterPager span {
                    padding: 0 5px 0 5px;
                }

                table.yui .tablesorterPager input.prev {
                    width: auto;
                    margin-right: 10px;
                }

                table.yui .tablesorterPager input.next {
                    width: auto;
                    margin-left: 10px;
                }

            table.yui .pagedisplay {
                font-size: 10pt;
                width: 30px;
                border: 0px;
                background-color: #E1ECF9;
                text-align: center;
                vertical-align: top;
            }
    </style>

    

    <script type="text/javascript">




        var newaddition = new Array();
        var olddata = new Array();
        var searchtext;
        var lengthtext1;

        var lengthtext2;
        var infotext1;
        var infotext2;
        var infotext3;
        var NodocumnetsSelectedToAssociate;
        var SelectDocumentstoassociate;

        var SelectItemtoassociate;

        var infotext4;

        var paginatePrev;

        var paginateNext;
        var sucessmessgaeassociation;
        var sucessmessgaedisassociation;


        $(document).ready(function () {
            if ('<%=languageid%>' != 'eng') {
                //   alert('ar');
                document.body.style.direction = "rtl";
                searchtext = "بحث";
                lengthtext1 = " عرض";
                lengthtext2 = " السجلات";
                infotext1 = " السجلات";
                infotext2 = " من";
                infotext3 = "الى ";
                infotext4 = " عرض";
                paginatePrev = " السابق";
                paginateNext = "التالي ";
                NodocumnetsSelectedToAssociate = "قم باختيار قائمة المستندات للربط ";
                SelectDocumentstoassociate = "قم باختيار المستندات للربط";
                SelectItemtoassociate = "حدد العناصر التي تريد الارتباط بها";
                sucessmessgaeassociation = "تم إنشاء الروابط ";
                sucessmessgaedisassociation = "تم إزالة الروابط ";

            }
            else {
                //  alert('tes');
                document.body.style.direction = "ltr";
                searchtext = "Search";
                lengthtext1 = " Show ";

                lengthtext2 = " Records ";

                infotext1 = " Showing ";
                infotext2 = " to ";
                infotext3 = " of ";
                infotext4 = " Records ";
                paginatePrev = " Previous ";
                paginateNext = " Next ";
                NodocumnetsSelectedToAssociate = "No Documents Selected To Associate";
                SelectDocumentstoassociate = "Select Documents to associate";
                SelectItemtoassociate = "Select Item  to Associate";
                sucessmessgaeassociation = "associations are made and";

                sucessmessgaedisassociation = "associations are removed";
            }

            //  $.fn.dataTableExt.sErrMode = 'throw';
            $.fn.dataTable.ext.classes.sPageButton = 'mcbutton';

    <% var DeclarationId = Session["DeclarationId"].ToString();%>


        <% var decryptedDocid = Session["decryptedDocid"].ToString();%>

            var urlf = 'http://localhost:1555/api/itemassociation/GetDocumentDetails?DocId=' + <%="'"+decryptedDocid.ToString()+"'"%>+'&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';///?id= ' + check +'',


//        var urlf = 'http://10.10.26.226:8024/api/itemassociation/GetDocumentDetails?DocId=' + <%="'"+decryptedDocid.ToString()+"'"%>+'&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';///?id= ' + check +'',

            //     http://localhost:1555/
            //  alert(urlf);
            var DocumentsTable = $('#DocumentsTable').DataTable({
                //"scrollY": '200',
                ////"scrollX": 'true',
                //"scrollX": '10vh',
                //'scrollCollapse': true,
                "scrollY": 260,
                "scrollCollapse": true,
                "scrollX": true,
                'columnDefs': [{
                    'orderable': 'false', 'targets': ['0'], 'checkboxes': {
                        'selectRow': 'true'
                    }
                }],
                'ajax': {
                    //  'url': '/data/Checkdata.txt',

                    'url': urlf,
                    'dataSrc': 'Resp',

                },
                'language': {
                    'paginate': {
                        'next': paginateNext, // or '→'
                        'previous': paginatePrev // or '←' 
                    }, "sSearch": searchtext,
                    "sLengthMenu": "" + lengthtext1 + "_MENU_" + lengthtext2 + "",

                    //    "info": "" + infotext1 + "_START_" + infotext2 + "_END_" + infotext3 + "_TOTAL_" + infotext4 + "",
                    //    "infoEmpty": "",
                },

                'columns': [
                    {
                        "render": function (data, type, row, meta) {
                            // alert(data);

                            var checkbox = $("<input/>", {
                                "type": "checkbox",
                                "id": "SelDoc" + row[0],
                                "class": "Documentsselctedcheckbox",
                                "value": row[0]
                            });



                            if (row[1] === "enable") {

                                checkbox.attr("checked", "checked");

                                //  $('input:checked').each(updateRowColor);

                            } else {
                                checkbox.removeAttr("checked");

                            }
                            return checkbox.prop("outerHTML")
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            return row[1];
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            return row[2];
                        }
                    },
                    {
                        "render": function (data, type, row, meta) {
                            return row[3];
                        }
                    }


                ],


                'select': {
                    'style': 'multi',
                    'selector': 'td:first-child input'
                },
                'order': [[1, 'asc']]
            });


            $('#DocumentsTable-select-all').change(function () {
                $(".Documentsselctedcheckbox:visible").prop('checked', $(this).prop('checked'));
                if ($(this).is(":checked")) {
                    $('.Documentsselctedcheckbox').closest('tr').addClass("selected");
                } else {
                    $('.Documentsselctedcheckbox').closest('tr').removeClass("selected");
                }
            });
            $(".Documentsselctedcheckbox").click(function () {

                $(".Documentsselctedcheckbox").each(function () {
                    if (!$(this).is(":checked")) {
                        $('#DocumentsTable-select-all').prop('checked', false);
                        return;
                    }

                });
            });




            function updateRowColor() {
                //// debugger;
                $(this).closest('tr').addClass('selected');
            }


            //// Handle click on "Select all" control
            //$('#DocumentsTable-select-all').on('click', function () {
            //    // Get all rows with search applied
            //    var rows = DocumentsTable.rows({ 'search': 'applied' }).nodes();
            //    // Check/uncheck checkboxes for all rows in the table
            //    if ($('#DocumentsTable tbody tr').hasClass('selected')) {

            //        //  clonetr = $(this).closest('tr').clone();
            //        $('#DocumentsTable tbody tr').removeClass('selected');
            //    }
            //    else {
            //        $('#DocumentsTable tbody tr').addClass('selected');

            //    }
            //    //   $("#DocumentsTable-select-all").row.removeClass("selected");

            //    $('input[type="checkbox"]:visible', rows).prop('checked', this.checked);

            //    $('#DocumentsTable').find('thead tr').removeClass('selected');

            //});


            $('#DocumentsTable tbody').on('click', 'input[type="checkbox"]', function (e) {
                var $row = $(this).closest('tr');
                console.log($row);
                // Get row data
                var data = DocumentsTable.row($row).data();
                console.log(data);
                // Get row ID
                var rowId = data[0];

                // Determine whether row ID is in the list of selected row IDs
                var index = $.inArray(rowId, rows_selected);

                // If checkbox is checked and row ID is not in list of selected row IDs
                if (this.checked && index === -1) {
                    rows_selected.push(rowId);

                    // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
                } else if (!this.checked && index !== -1) {
                    rows_selected.splice(index, 1);
                }

                if (this.checked) {
                    $row.addClass('selected');
                } else {
                    $row.removeClass('selected');
                }

                // Update state of "Select all" control
                updateDataTableSelectAllCtrl(DocumentsItemAssocaition);

                // Prevent click event from propagating to parent
                e.stopPropagation();
            });
            // Handle form submission event
            $('#frm-DocumentsTable').on('submit', function (e) {
                var form = this;

                // Iterate over all checkboxes in the table
                DocumentsTable.$('input[type="checkbox"]').each(function () {
                    // If checkbox doesn't exist in DOM
                    if (!$.contains(document, this)) {
                        // If checkbox is checked
                        if (this.checked) {
                            // Create a hidden element
                            $(form).append(
                                $('<input>')
                                    .attr('type', 'hidden')
                                    .attr('name', this.name)
                                    .val(this.value)
                            );
                        }
                    }
                });
            });


            var rows_selected = [];
            var urlDocumentsItemAssocaition = 'http://localhost:1555/api/itemassociation/GetDocumentItemAssocDet?DocId=' + <%="'"+decryptedDocid.ToString()+"'"%>+'&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';///?id= ' + check +'',
//        var urlDocumentsItemAssocaition = 'http://10.10.26.226:8024/api/itemassociation/GetDocumentItemAssocDet?DocId=' + <%="'"+decryptedDocid.ToString()+"'"%>+'&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';///?id= ' + check +'',



        //  console.log(urlDocumentsItemAssocaition);
        //.removeAttr('width').
        var DocumentsItemAssocaition = $('#DocumentsItemAssocaition').DataTable({
            //'scrollY': 200,
            //'scrollCollapse': true, "scrollX": true,
            //150vh
            "scrollY": 260,
            "scrollCollapse": true,
            "scrollX": 260,
            'ajax': {
                //'url': '/data/AssociatedItemData.txt',
                //    'dataSrc': 'AssociatedItemData'
                'url': urlDocumentsItemAssocaition,
                'dataSrc': 'Resp'


            },
            "selectRow": "false",

            'language': {
                'paginate': {
                    'next': paginateNext, // or '→'
                    'previous': paginatePrev // or '←' 
                }, "sSearch": searchtext,
                "sLengthMenu": "" + lengthtext1 + "_MENU_" + lengthtext2 + "",

                //   "info": "" + infotext1 + "_START_" + infotext2 + "_END_" + infotext3 + "_TOTAL_" + infotext4 + "",
                //   "infoEmpty": "",
            },
            'columns': [
                {

                    "render": function (data, type, row, meta) {
                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            "id": "AssociatedItemData" + row[0],
                            "style": "text-align: center",
                            "class": "deleteselctedcheckbox",
                            "value": row[0]

                        });
                        // debugger;

                        if (row[3] === "true" || row[0].indexOf('|') > 0) {
                            // // debugger;
                            checkbox.attr("checked", "checked");

                            $('input:checked').each(updateRowColor);

                        } else {
                            checkbox.removeAttr("checked");
                            //    checkbox.addClass("checkbox_unchecked");
                        }
                        return checkbox.prop("outerHTML")
                    }
                },
                {
                    "width": "50%",
                    "render": function (data, type, row, meta) {
                        return row[2];
                    }
                },
                {
                    "width": "30%",
                    "render": function (data, type, row, meta) {
                        return row[4];
                    }
                },
                {
                    "width": "20%",
                    "render": function (data, type, row, meta) {
                        return row[6];
                    }
                }
            ],


            'select': {
                'style': 'multi',
                'selector': 'td:first-child input'
            },
            'order': [[1, 'asc']]
        });


        //$("#DocumentsItemAssocaition-select-all").click(function () {
        //    $(".deleteselctedcheckbox:visible").prop('checked', $(this).prop('checked'));
        //});

        // Handle click on "Select all" control
        //  $('#DocumentsItemAssocaition-select-all').on('click', function () {
        //      // Get all rows with search applied
        //      var rows = DocumentsItemAssocaition.rows({ 'search': 'applied' }).nodes();

        //      // // debugger;
        //      if ($('#DocumentsItemAssocaition tr').hasClass('selected')) {
        //      //    debugger;
        //          //  clonetr = $(this).closest('tr').clone();
        //          $('#DocumentsItemAssocaition tr').removeClass('selected');

        //         // $('#DocumentsItemAssocaition tr input[type=checkbox]').prop("checked", "false");
        //      }
        //      else {
        //          $('#DocumentsItemAssocaition tr').addClass('selected');
        //        //  $('#DocumentsItemAssocaition tr input[type=checkbox]').prop("checked", "true");

        //      }
        //      // Check/uncheck checkboxes for all rows in the table
        //$('input[type="checkbox"]:visible', rows).prop('checked', this.checked);

        //      $('#DocumentsItemAssocaition').find('thead tr').removeClass('selected');

        //  });

        $('#DocumentsItemAssocaition-select-all').change(function () {
            $(".deleteselctedcheckbox:visible").prop('checked', $(this).prop('checked'));
            if ($(this).is(":checked")) {
                $('.deleteselctedcheckbox').closest('tr').addClass("selected");
            } else {
                $('.deleteselctedcheckbox').closest('tr').removeClass("selected");
            }
        });

        $(".deleteselctedcheckbox").click(function () {

            $(".deleteselctedcheckbox").each(function () {
                if (!$(this).is(":checked")) {
                    $('#DocumentsItemAssocaition-select-all').prop('checked', false);
                    return;
                }

            });
        });
        // Handle click on checkbox
        $('#DocumentsItemAssocaition tbody').on('click', 'input[type="checkbox"]', function (e) {
            var $row = $(this).closest('tr');

            // Get row data
            var data = DocumentsItemAssocaition.row($row).data();

            // Get row ID
            var rowId = data[0];

            // Determine whether row ID is in the list of selected row IDs
            var index = $.inArray(rowId, rows_selected);

            // If checkbox is checked and row ID is not in list of selected row IDs
            if (this.checked && index === -1) {
                rows_selected.push(rowId);

                // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
            } else if (!this.checked && index !== -1) {
                rows_selected.splice(index, 1);
            }

            if (this.checked) {
                $row.addClass('selected');
            } else {
                $row.removeClass('selected');
            }

            // Update state of "Select all" control
            updateDataTableSelectAllCtrl(DocumentsItemAssocaition);

            // Prevent click event from propagating to parent
            e.stopPropagation();
        });

        function updateDataTableSelectAllCtrl(table) {
            // debugger;
            var $table = table.table().node();
            var $chkbox_all = $('tbody input[type="checkbox"]', $table);
            var $chkbox_checked = $('tbody input[type="checkbox"]:checked', $table);
            var chkbox_select_all = $('thead input[name="select_all"]', $table).get(0);

            // If none of the checkboxes are checked
            if ($chkbox_checked.length === 0) {
                chkbox_select_all.checked = false;
                if ('indeterminate' in chkbox_select_all) {
                    chkbox_select_all.indeterminate = false;
                }

                // If all of the checkboxes are checked
            } else if ($chkbox_checked.length === $chkbox_all.length) {
                chkbox_select_all.checked = true;
                if ('indeterminate' in chkbox_select_all) {
                    chkbox_select_all.indeterminate = false;
                }

                // If some of the checkboxes are checked
            } else {
                chkbox_select_all.checked = true;
                if ('indeterminate' in chkbox_select_all) {
                    chkbox_select_all.indeterminate = true;
                }
            }
        }








        // Handle form submission event
        $('#frm-DocumentsItemAssocaition').on('submit', function (e) {
            var form = this;

            // Iterate over all checkboxes in the table
            DocumentsItemAssocaition.$('input[type="checkbox"]').each(function () {
                // If checkbox doesn't exist in DOM
                if (!$.contains(document, this)) {
                    // If checkbox is checked
                    if (this.checked) {
                        // Create a hidden element
                        $(form).append(
                            $('<input>')
                                .attr('type', 'hidden')
                                .attr('name', this.name)
                                .val(this.value)
                        );
                    }
                }
            });
        });







        $('#DisassociateDocItems').click(function () {
            // debugger;
            $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").each(function () {
                // $("#DocumentsItemAssocaition").find(" input:checkbox:checked").each(function () {
                // debugger;
                var y = $(this).attr('value');

                if (y.indexOf("|") != -1) {
                    // newaddition.push($(this).attr('value'));

                    //  DocumentsItemAssocaition.row.find(".input:checkbox:checked").remove().draw(false);


                    DocumentsItemAssocaition.row('.selected').remove().draw(false);

                    document.getElementById('SelectedItemAssocationId').value = newaddition.join('|');

                }

                else {
                    olddata.push($(this).attr('value'));

                    // DocumentsItemAssocaition.row.find(".input:checkbox:checked").remove().draw(false);

                    DocumentsItemAssocaition.row('.selected').remove().draw(false);


                    document.getElementById('SelectedItemDisAssocationId').value = olddata.join('|');
                }

            });

            // DocumentsItemAssocaition.row('.selected').remove().draw(false);

        });

        var urlitemf = '  http://localhost:1555/api/itemassociation/GetCommercialInvoiceItems?DeclId=' +<%="'"+DeclarationId.ToString()+"'"%>+'';


//        var urlitemf = 'http://10.10.26.226:8024/api/itemassociation/GetCommercialInvoiceItems?DeclId=' +<%="'"+DeclarationId.ToString()+"'"%>+'';
        http://localhost:1555/

        console.log(urlitemf);

        var ItemTable = $('#ItemTable').removeAttr('width').DataTable({

            //"scrollY": '10vh',
            //"scrollX": '100',

            //"scrollY": '200',
            ////"scrollX": 'true',
            //"scrollX": '10vh',
            //'scrollCollapse': true,
            "scrollY": '50vh',
            "scrollCollapse": true,
            "scrollX": true,
            'ajax': {
                // 'url': '/data/ItemHsCodedata.txt',

                'url': urlitemf,

                // 'dataSrc': 'ItemHsCodeData'
                'dataSrc': 'Resp'

            },
            'language': {
                'paginate': {
                    'next': paginateNext, // or '→'
                    'previous': paginatePrev // or '←' 
                }, "sSearch": searchtext,
                "sLengthMenu": "" + lengthtext1 + "_MENU_" + lengthtext2 + "",

                //   "info": "" + infotext1 + "_START_" + infotext2 + "_END_" + infotext3 + "_TOTAL_" + infotext4 + "",
                // "infoEmpty": "",
            },

            'columns': [
                {

                    "render": function (data, type, row, meta) {

                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            "id": "AssociatedItemData" + row[0],
                            "style": "text-align: center",
                            "class": "Itemselctedcheckbox",
                            "value": row[0]
                        });
                        if (row[3] === "true") {
                            // // debugger;
                            checkbox.attr("checked", "checked");

                            $('input:checked').each(updateRowColor);

                        } else {
                            checkbox.removeAttr("checked");
                            //    checkbox.addClass("checkbox_unchecked");
                        }
                        return checkbox.prop("outerHTML")
                    }
                },

                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[3];
                    }
                },
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        //  alert(row);
                        return row[4];
                    }
                },
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[5];
                    }
                }
                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[6];
                    }
                }
                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[7];
                    }
                }

                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[8];
                    }
                }

                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[9];
                    }
                }

                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[10];
                    }
                }


                ,
                {
                    'width': '40%',
                    "render": function (data, type, row, meta) {
                        return row[11];
                    }
                }




            ],


            'select': {
                'style': 'multi',
                'selector': 'td:first-child input'
            },

            'order': [[1, 'asc']]

        });

        $(".dataTables_info").hide();
        // Handle click on "Select all" control
        //$('#ItemTable-select-all').on('click', function () {
        //    // Get all rows with search applied
        //    var rows = ItemTable.rows({ 'search': 'applied' }).nodes();

        //  if ($('#ItemTable tr').hasClass('selected')) {
        //   // if ($('#ItemTable tr').find('input[type="checkbox"]:not(:checked)')) {


        //        $('#ItemTable tr').removeClass('selected');
        //     //   ItemTable.rows().deselect();
        //    }
        //    else {
        //        $('#ItemTable tr').addClass('selected');
        //    //    ItemTable.rows().select();
        //    }


        //    // Check/uncheck checkboxes for all rows in the table
        //  $('input[type="checkbox"] :visible', rows).prop('checked', this.checked);

        //    $('#ItemTable').find('thead tr').removeClass('selected');
        //    });



        //$('#ItemTable-select-all').on('click', function () {
        //    // Get all rows with search applied
        //    var rows = ItemTable.rows({ 'search': 'applied' }).nodes();
        //    // Check/uncheck checkboxes for all rows in the table
        //    if ($('#ItemTable tbody tr').hasClass('selected')) {

        //        //  clonetr = $(this).closest('tr').clone();
        //        $('#ItemTable tbody tr').removeClass('selected');
        //    }
        //    else {
        //        $('#ItemTable tbody tr').addClass('selected');

        //    }
        //    //   $("#DocumentsTable-select-all").row.removeClass("selected");

        //    $('input[type="checkbox"]:visible', rows).prop('checked', this.checked);

        //    $('#ItemTable').find('thead tr').removeClass('selected');

        //});

        $('#ItemTable-select-all').change(function () {
            $(".Itemselctedcheckbox:visible").prop('checked', $(this).prop('checked'));
            if ($(this).is(":checked")) {
                $('.Itemselctedcheckbox').closest('tr').addClass("selected");
            } else {
                $('.Itemselctedcheckbox').closest('tr').removeClass("selected");
            }
        });



        $(".Itemselctedcheckbox").click(function () {

            $(".Itemselctedcheckbox").each(function () {
                if (!$(this).is(":checked")) {
                    $('#ItemTable-select-all').prop('checked', false);
                    return;
                }

            });
        });

        $(".dataTables_scrollBody").css("height", "220px");
        // Handle click on checkbox to set state of "Select all" control
        $('#ItemTable tbody').on('click', 'input[type="checkbox"]', function (e) {
            var $row = $(this).closest('tr');

            // Get row data
            var data = ItemTable.row($row).data();

            // Get row ID
            var rowId = data[0];

            // Determine whether row ID is in the list of selected row IDs
            var index = $.inArray(rowId, rows_selected);

            // If checkbox is checked and row ID is not in list of selected row IDs
            if (this.checked && index === -1) {
                rows_selected.push(rowId);

                // Otherwise, if checkbox is not checked and row ID is in list of selected row IDs
            } else if (!this.checked && index !== -1) {
                rows_selected.splice(index, 1);
            }

            if (this.checked) {
                $row.addClass('selected');
            } else {
                $row.removeClass('selected');
            }

            // Update state of "Select all" control
            updateDataTableSelectAllCtrl(DocumentsItemAssocaition);

            // Prevent click event from propagating to parent
            e.stopPropagation();
        });


        // Handle form submission event
        $('#frm-ItemTable').on('submit', function (e) {
            var form = this;

            // Iterate over all checkboxes in the table
            ItemTable.$('input[type="checkbox"]').each(function () {
                // If checkbox doesn't exist in DOM
                if (!$.contains(document, this)) {
                    // If checkbox is checked
                    if (this.checked) {
                        // Create a hidden element
                        $(form).append(
                            $('<input>')
                                .attr('type', 'hidden')
                                .attr('name', this.name)
                                .val(this.value)
                        );
                    }
                }
            });
        });


        var counter = 1;
        var tds = new Array();

        var counter = 1;
        var currentdate = new Date();
        //    var strDate = (d.getMonth() + 1) + "/" + d.getfull() + "/" + d.getFullYear();

        var datetime = " " + (currentdate.getMonth() + 1) + "/"
            + currentdate.getDate() + "/"
            + currentdate.getFullYear() + "  ";


        var selcteddocu = new Array();
        var selcteditem = new Array();
        var documentext;
        var itemtext;
        var singledocid;
        var singleitemid;
        var singleid;

        $('#AssociateDocItems').on('click', function () {

            //            $('#DocumentsTable').find('tr.selected td:nth-child(2)').each(function () {
            if ($('#DocumentsTable').find('tr.selected ').length == 0) {
                //   alert('select Documents to associate');
                alert(SelectDocumentstoassociate);
                return false;
            }

            $('#DocumentsTable').find('tbody tr.selected').each(function () {
                // debugger;
                //singledocid = $(this).find(' td:nth-child(1)').find('input').attr('value');
                singledocid = $(this).find(' td:nth-child(1)').find('input').attr('value');

                documentext = $('#DocumentsTable').find('tbody tr.selected').find('td:nth-child(2)').html();


                // alert(singledocid);
                // alert(singleid);
                if ($('#ItemTable').find('tr.selected ').length == 0) {
                    //  alert('select item to associate');
                    alert(SelectItemtoassociate);
                    return false;
                }


                $('#ItemTable').find('tbody tr.selected ').each(function () {

                    DocumentsItemAssocaition.row.add([

                        singledocid + '|' + $(this).find('td:nth-child(1)').find('input').attr('value'), "",
                        documentext,
                        "",
                        $('#ItemTable').find('tbody tr.selected ').find('td:nth-child(2)').html(),
                        "", datetime

                    ]).draw(false).nodes()
                        .to$()
                        .addClass('selected');;

                });

            });

            counter++;
        });




        $("#SaveItems").click(function () {
            // debugger;
            //   alert("saveold" + olddata);
            // alert("savenew" + newaddition);


            var totalcheckbox = $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").length
            if (totalcheckbox != '0') {
                $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").each(function () {
                    var y = $(this).attr('value');

                    if (y.indexOf("|") != -1) {
                        newaddition.push($(this).attr('value'));

                        //  alert(newaddition.join(','));
                        //  document.getElementById('SelectedItemAssocationId').value = newaddition.join('|');

                    }

                });

            }


            {
                if (olddata != null || newaddition != null) {
                    var number = 1;
                //    alert('ajax call' + olddata);
                    // debugger;
                //    urld = "http://10.10.26.226:8024/api/itemassociation/AssociationHandler?Assoc=" + newaddition + "&Disassoc=" + olddata + '&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';
                    //    http://localhost:1555/
                    urld = "http://localhost:1555/api/itemassociation/AssociationHandler?Assoc=" + newaddition + "&Disassoc=" + olddata + '&DeclId=' + <%="'"+DeclarationId.ToString()+"'"%>+'';

                    //  console.log(urld);

                    $.ajax({
                        type: "Get",
                        url: urld,
                        dataType: "json",
                        success: function (r) {
                            //  alert(r);
                            var message = r.slice(0, r.indexOf('|')) + sucessmessgaeassociation + r.slice(r.lastIndexOf('|') + 1) + sucessmessgaedisassociation;
                            alert(message)
                            //  alert(r);
                            var refreshurl = window.location.href;
                            window.location = refreshurl;//"https://www.google.com";//refreshurl;//"//www.aspsnippets.com/";
                            newaddition.length = 0;

                            olddata.length = 0;
                            // OnError
                        },
                        error: OnError
                    });


                }
                else {
                    alert('select An Item To Save');
                }

                // else
                {


                }


            }

        });


        function OnError(xhr, errorType, exception) {
            //  alert('err');
            // debugger;
            var responseText;
            $("#dialog").html("");
            try {
                responseText = jQuery.parseJSON(xhr.responseText);
                $("#dialog").append("<div><b>" + errorType + " " + exception + "</b></div>");
                $("#dialog").append("<div><u>Exception</u>:<br /><br />" + responseText.ExceptionType + "</div>");
                $("#dialog").append("<div><u>StackTrace</u>:<br /><br />" + responseText.StackTrace + "</div>");
                $("#dialog").append("<div><u>Message</u>:<br /><br />" + responseText.Message + "</div>");
            } catch (e) {
                responseText = xhr.responseText;
                $("#dialog").html(responseText);
            }

            $("#dialog").dialog({
                title: "Exception Occured ",
                width: 700,
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                }
            });
        }








        });


    </script>



</head>

<body>
      <%if (languageid == "eng")
        {%>
  





    <div style="width: 60%;padding-left:20%;padding-right:10%">
<div style="border-style:double;border-color:lightgray; padding-top:5%;height:360px">
   <div id="Header" style=" margin-top: 11%; margin-bottom: 0.5%; margin-left: -1.5%;">
                    <table  width="98.5%" cellpadding="0" cellspacing="0" style=" margin-left:12px;margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="27%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                               Associated Items
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>
      <table id="DocumentsItemAssocaition"    class="table"     >
         <thead>
                         <tr>
                             <th>
                                 <input type="checkbox" name="select_all" value="1" id="DocumentsItemAssocaition-select-all"></th>
                             <th>DocName
                             </th>
                             <th>ItemName
                             </th>
                             <th>CreatedDate
                             </th>

                         </tr>
                     </thead>
  
    </table>

</div>
</div>
<div style="width: 100%;overflow:hidden;margin-top:25px">

<!-- <div style="width: 47%; float: left;border-style:double;border-color:black;"> -->
<div style="width: 47%; border-style: double; padding-top:3%; height:360px; border-color: lightgrey; padding-left:1%; float: left;">
       <div id="Header" style=" margin-top: 14%; margin-bottom: 1%;">
                    <table width="98.5%" cellpadding="0" cellspacing="0" style="margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="27%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                              Documents 
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>

      <table id="DocumentsTable"  class="table display select " >
                    <thead>
                        <tr>
                            <th>
                             <input type="checkbox" name="select_all" value="1" class="select-checkbox" id="DocumentsTable-select-all">


                            </th>
                            <th>DocName</th>
                                <th>DocType</th>
                            <th>CreatedDate</th>
                        
                        </tr>
                    </thead>

                </table>
     
  </div>
<!-- <div style="width: 4%;float: left;border-color:red;border-style:double;padding-top: 134px;padding-left:2px"  >  -->
<div style="width: 4%;float: left;padding-top:5%;padding-left:0.75%;"  > 
<!-- 11% -->
<!-- margin-left:4px; -->
<%--<button onclick="myFunction()" class="btc"><img src="connectg.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="connectr.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="saveb.png" class="imgc"/></button>--%>

    <div id="dialog" style="display: none"></div>
    <%if (checkflag == "true")
        {%>
        <button type="button"   class="mcbutton" style="height: 30px;" id="AssociateDocItems" > <img src="Images/connectg.png" style="width: 80%;" class="imgc"/> </button>
        <button type="button"   class="mcbutton" style="height: 30px;" id="DisassociateDocItems" ><img src="Images/connectr.png" style="width: 80%;" class="imgc"/> </button>
        <button type="button"  class="mcbutton" style="height: 30px;" id="SaveItems"  ><img src="Images/saveb.png" style="width: 80%;" class="imgc" /></button>

<%--        <button onclick="myFunction()"  class="btc"><img src="connectg.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="connectr.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="saveb.png" class="imgc"/></button>--%>




    <%} %>


</div>

  <!-- <div style="width: 47%; float: left;border-style:double;border-color:black;" > -->
  <div style="width: 46%; float: left; border-style: double; border-color: lightgrey; height:360px; padding-top:3%" >
                     <div id="Header" style=" margin-top: 14%; margin-bottom: 1%; ">
                    <table width="98.5%" cellpadding="0" cellspacing="0" style="margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="40%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                              Items
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>
 <table id="ItemTable" class=" table display select">
                    <thead>
                        <tr>
                            <th>
                                <input type="checkbox" name="select_all" value="1" id="ItemTable-select-all"></th>
                        
                            <th>Item Name 
                            </th>
                         
                            <th>Item Number
                            </th>
                            <th>Hsccode
                            </th>
                            
                            <th>Good Description
                            </th>
                            <th>Manufacturer
                            </th>
                            <th>NoOfPackages
                            </th>
                            <th>Quantity</th>

                            <th>NetWeight</th>

                            <th>GrossWeight</th>

                        

                        </tr>

                </table>

  
  </div>

</div>

    
    <%} %>
      <%if (languageid == "ara")
        {%>
  <div style="width: 60%;padding-left:20%;padding-right:19%">
<div style="border-style:double;border-color:lightgray;  height:360px; padding-top:3%">
   <div id="Header" dir="rtl" style=" margin-top: 11%; margin-bottom: 0.5%; margin-left: -1.5%;">
                    <table  width="98.5%" cellpadding="0" cellspacing="0" style=" margin-left:12px;margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="27%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                               العناصر المرتبطة
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>
      <table id="DocumentsItemAssocaition" dir="rtl"   class="table"     >
         <thead>
                         <tr>
                             <th>
                                 <input type="checkbox" name="select_all" value="1" id="DocumentsItemAssocaition-select-all"></th>
                             <th style="text-align:right">اسم الملف
                             </th>
                             <th style="text-align:right"><%=Resources.GlobalAra.ItemName%>
                             </th>
                             <th style="text-align:right"><%=Resources.GlobalAra.CreatedDate%>
                             </th>

                         </tr>
                     </thead>
  
    </table>

</div>
</div>
<div style="width: 100%;overflow:hidden;margin-top:25px">

<!-- <div style="width: 47%; float: left;border-style:double;border-color:black;"> -->
<div style="width: 47%; padding-top:3%; border-style: double; height:360px; border-color: lightgrey; padding-left:1%; float: left;">
       <div id="Header"  dir="rtl" style=" margin-top: 14%; margin-bottom: 1%;">
                    <table width="98.5%" cellpadding="0" cellspacing="0" style="margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="27%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                                    <%=Resources.GlobalAra.Documents%>

                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>

      <table id="DocumentsTable" dir="rtl"  class="table display select " >
                    <thead>
                        <tr>
                            <th>
                               <input type="checkbox" name="select_all" value="1" class="select-checkbox" id="DocumentsTable-select-all"></th>
                          
                       
                                <th style="text-align:right">
                                
                                
                                                    <%=Resources.GlobalAra.DocName%>

                             
                            </th>
                                <th style="text-align:right">
                                     <%=Resources.GlobalAra.DocType%>


                                </th>
                            <th style="text-align:right">
                                     <%=Resources.GlobalAra.CreatedDate%>


                            </th>
                        
                        </tr>
                    </thead>

                </table>
     
  </div>
<!-- <div style="width: 4%;float: left;border-color:red;border-style:double;padding-top: 134px;padding-left:2px"  >  -->
<div style="width: 4%;float: left;padding-top:5%;padding-left:0.75%"  > 
<!-- 11% -->
<!-- margin-left:4px; -->
<%--<button onclick="myFunction()" class="btc"><img src="connectg.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="connectr.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="saveb.png" class="imgc"/></button>--%>

    <div id="dialog" style="display: none"></div>
    <%if (checkflag == "true")
        {%>
        <button type="button"   class="mcbutton" style="height: 30px;" id="AssociateDocItems" > <img src="Images/connectg.png" style="width: 80%;" class="imgc"/> </button>
        <button type="button"   class="mcbutton" style="height: 30px;" id="DisassociateDocItems" ><img src="Images/connectr.png" style="width: 80%;" class="imgc"/> </button>
        <button type="button"  class="mcbutton" style="height: 30px;" id="SaveItems"  ><img src="Images/saveb.png" style="width: 80%;" class="imgc" /></button>

<%--        <button onclick="myFunction()"  class="btc"><img src="connectg.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="connectr.png" class="imgc"/></button>
<button onclick="myFunction()" class="btc"><img src="saveb.png" class="imgc"/></button>--%>




    <%} %>


</div>

  <!-- <div style="width: 47%; float: left;border-style:double;border-color:black;" > -->
  <div style="width: 46%; float: left; border-style: double; height:360px;  padding-top:3%; border-color: lightgrey;" >
                     <div id="Header" dir="rtl" style=" margin-top: 14%; margin-bottom: 1%; ">
                    <table width="98.5%" cellpadding="0" cellspacing="0" style="margin-top: -80px;">
                        <tbody>
                            <tr width="100%">
                                <td valign="bottom" width="8">
                                    <div class="div11">
                                    </div>
                                </td>
                                <td valign="bottom" class="SubTitleHeader div12" height="24" width="40%" nowrap align="left">
                                    <table style="padding-bottom: 0px; padding-top: 0px; padding-left: 0px; padding-right: 0px"
                                           cellspacing="0" cellpadding="0" width="100%" type="formheader;">
                                        <tbody>
                                            <tr>
                                                <td style="line-height: 24px;              color: #000; font-weight: bold; font-size: 12px; font-family: Arial,Verdana, Helvetica, sans-serif;">

                                              
                                                    

                                                       <%=Resources.GlobalAra.Items%>
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="bottom" width="35">
                                    <div class="div13">
                                    </div>
                                </td>



                                <td class="TitleHeader" valign="bottom" width="100%">
                                    &nbsp;
                                </td>


                            </tr>
                        </tbody>
                    </table>
                </div>
 <table id="ItemTable" dir="rtl" class=" table display select">
                    <thead>
                        <tr>
                            <th style="text-align:right">
                                <input type="checkbox" name="select_all" value="1" id="ItemTable-select-all"></th>
                        
                            <th style="text-align:right">

                                                  <%=Resources.GlobalAra.ItemName%>
                                 
                                

                            </th>
                         
                            <th style="text-align:right">
                                
                                
                             <%=Resources.GlobalAra.ItemNumber%>
                            
                            </th>
                            <th style="text-align:right">
                                
                             <%=Resources.GlobalAra.Hsccode%>
                            </th>
                            
                            <th style="text-align:right">
                                
                             <%=Resources.GlobalAra.GoodDescription%>
                            </th>
                            <th style="text-align:right">
                                
                             <%=Resources.GlobalAra.Manufacturer%>

                            </th>
                            <th style="text-align:right">
                                
                             <%=Resources.GlobalAra.NoOfPackages%>

                            </th>
                            <th style="text-align:right" >

                                <%=Resources.GlobalAra.Quantity%>
                            </th>

                            <th style="text-align:right">

                                <%=Resources.GlobalAra.NetWeight%>
                            </th>


                            <th style="text-align:right">
                                
                                <%=Resources.GlobalAra.GrossWeight%>
                            </th>

                        

                        </tr>

                </table>

  
  </div>

</div>


    <%} %>
         <input type="hidden" id="SelectedItemAssocationId" runat="server" />

                <input type="hidden" id="SelectedItemDisAssocationId" runat="server" />
              <asp:Label ID="GetDirection" runat="server" style="display:none"></asp:Label>



</body>
</html>
