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
   
    <link href="Content/Blue.css" rel="stylesheet" />
    <link href="Content/datatables.min.css" rel="stylesheet" />
    <script type="text/javascript" charset="utf8" src="http://code.jquery.com/jquery-1.12.4.js"></script>
     <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript" charset="utf8" src="scripts/jquery.dataTables.min.js"></script>

    <script src="Scripts/DataTableMultiselect.js"></script>
    <script src="Scripts/DocumentsData.js"></script>
        <script src="Scripts/Commercial.js"></script>

  <style type="text/css">

              #dialog { height: 600px; overflow: auto; font-size: 10pt !important; font-weight: normal !important; background-color: #FFFFC1; margin: 10px; border: 1px solid #ff6a00; }
        #dialog div { margin-bottom: 15px; }
  </style>

<script type="text/javascript">
  


 

    var clonetr;
    var newaddition = new Array();
    var olddata = new Array();

    $(document).ready(function () {


    
        var DocumentsTable = $('#DocumentsTable').DataTable({
            'scrollY': '10vh',
            'scrollCollapse': true,
            
     
            'ajax': {
                'url': '/data/Checkdata.txt',

                'dataSrc': 'DocumentsData'
            }, 
            'columns': [
                {  
                    "render": function (data, type, row, meta) {
                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            "id": "SelDoc" + row[0],
                            "value":row[0]
                        });
                        if (row[3] === "enable") {
                    
                            checkbox.attr("checked", "checked");

                            $('input:checked').each(updateRowColor);
                        
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
                        return row[4];
                    }
                }


            ],


            'select': {
                'style': 'multi',
                'selector': 'td:first-child input'
            },
            'order': [[1, 'asc']]
        });

    

            function updateRowColor() {
                //debugger;
                $(this).closest('tr').addClass('selected');
            }


        // Handle click on "Select all" control
        $('#DocumentsTable-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = DocumentsTable.rows({ 'search': 'applied' }).nodes();
            // Check/uncheck checkboxes for all rows in the table
            if ($('#DocumentsTable tbody tr').hasClass('selected')) {

                //  clonetr = $(this).closest('tr').clone();
                $('#DocumentsTable tbody tr').removeClass('selected');
            }
            else {
                $('#DocumentsTable tbody tr').addClass('selected');

            }
         //   $("#DocumentsTable-select-all").row.removeClass("selected");

            $('input[type="checkbox"]', rows).prop('checked', this.checked);

            $('#DocumentsTable').find('thead tr').removeClass('selected');
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#DocumentsTable tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#DocumentsTable-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
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
        var DocumentsItemAssocaition = $('#DocumentsItemAssocaition').DataTable({
            'scrollY': '20vh',
            'scrollCollapse': true,
            'ajax': {
                'url': '/data/AssociatedItemData.txt',
                'dataSrc': 'AssociatedItemData'
            },
            'columns': [
                {
                    "render": function (data, type, row, meta) {
                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            "id": "AssociatedItemData" + row[0],
                            "value": row[0]
                        });
                     //   alert(row[0]);
                        if (row[3] === "enable" || row[0].indexOf('|')>0) {
                            // debugger;
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

    
 
           

        // Handle click on "Select all" control
        $('#DocumentsItemAssocaition-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = DocumentsItemAssocaition.rows({ 'search': 'applied' }).nodes();

           // debugger;
            if ($('#DocumentsItemAssocaition tr').hasClass('selected')) {

                //  clonetr = $(this).closest('tr').clone();
                $('#DocumentsItemAssocaition tr').removeClass('selected');
            }
            else
                {
                $('#DocumentsItemAssocaition tr').addClass('selected');
            }
                // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);

            $('#DocumentsItemAssocaition').find('thead tr').removeClass('selected');
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#DocumentsItemAssocaition tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#DocumentsItemAssocaition-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
        });

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

            $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").each(function () {
           // $("#DocumentsItemAssocaition").find(" input:checkbox:checked").each(function () {

                var y = $(this).attr('value');

                if (y.indexOf("|") != -1) {
                   // newaddition.push($(this).attr('value'));

                   

                    DocumentsItemAssocaition.row('.selected').remove().draw(false);

                    document.getElementById('SelectedItemAssocationId').value = newaddition.join('|');

                }

                else {
                    olddata.push($(this).attr('value'));

                   
                    DocumentsItemAssocaition.row('.selected').remove().draw(false);
                    document.getElementById('SelectedItemAssocationId').value = olddata.join('|');
                }

            });

           // DocumentsItemAssocaition.row('.selected').remove().draw(false);

        });




        var ItemTable = $('#ItemTable').DataTable({
            'scrollY': '10vh',
            'scrollCollapse': true,
            'ajax': {
                'url': '/data/ItemHsCodedata.txt',
                'dataSrc': 'ItemHsCodeData'
            },
            'columns': [
                {
                    "render": function (data, type, row, meta) {
                        var checkbox = $("<input/>", {
                            "type": "checkbox",
                            "id": "AssociatedItemData" + row[0],
                            "value": row[0]
                        });
                        if (row[3] === "enable") {
                            // debugger;
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

        // Handle click on "Select all" control
        $('#ItemTable-select-all').on('click', function () {
            // Get all rows with search applied
            var rows = ItemTable.rows({ 'search': 'applied' }).nodes();


            if ($('#ItemTable tr').hasClass('selected')) {

                //  clonetr = $(this).closest('tr').clone();
                $('#ItemTable tr').removeClass('selected');
            }
            else {
                $('#ItemTable tr').addClass('selected');
            }
            // Check/uncheck checkboxes for all rows in the table
            $('input[type="checkbox"]', rows).prop('checked', this.checked);

            $('#ItemTable').find('thead tr').removeClass('selected');
        });

        // Handle click on checkbox to set state of "Select all" control
        $('#ItemTable tbody').on('change', 'input[type="checkbox"]', function () {
            // If checkbox is not checked
            if (!this.checked) {
                var el = $('#ItemTable-select-all').get(0);
                // If "Select all" control is checked and has 'indeterminate' property
                if (el && el.checked && ('indeterminate' in el)) {
                    // Set visual state of "Select all" control
                    // as 'indeterminate'
                    el.indeterminate = true;
                }
            }
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
                alert('select Documents to associate');
                return false;
            }

            $('#DocumentsTable').find('tbody tr.selected').each(function () {
               
                //singledocid = $(this).find(' td:nth-child(1)').find('input').attr('value');
                singledocid = $(this).find(' td:nth-child(1)').find('input').attr('value');

                documentext = $(this).find('td:nth-child(2)').html();
              
               // alert(singledocid);
               // alert(singleid);
                if ($('#ItemTable').find('tr.selected ').length==0)
                {
                    alert('select item to associate');
                    return false;
                }


                $('#ItemTable').find('tbody tr.selected ').each(function () {
                    
                    DocumentsItemAssocaition.row.add([
                        
                        singledocid + '|' + $(this).find('td:nth-child(1)').find('input').attr('value'),
                        documentext,
                        $(this).find('td:nth-child(2)').html(),
                        datetime
                    ]).draw(false).nodes()
                        .to$()
                        .addClass('selected');;

                });

            });
            

            counter++;
        });


    });

 
 



</script>



</head>
<body>
   
    <table id="maincontainer" class="table" width="100%" border="1">
        <tr>
            <td width="30%" style="max-height:40%">
                <input type="hidden" id="SelectedItemAssocationId" runat="server" />
                <table id="DocumentsTable" class="display select"  cellspacing="0">
                    <thead>
                        <tr>
                            <th><input type="checkbox" name="select_all" value="1" id="DocumentsTable-select-all"></th>
                           
                            <th>DocName</th>
                            <th>CreatedDate</th>
                            
                            <th>Test</th>
                    
                        </tr>
                    </thead>
   
                </table>


            </td>

            <td width="35%">


                <table id="DocumentsItemAssocaition"  class="display select"   cellspacing="0">
                    <thead>
                        <tr>
                            <th><input type="checkbox" name="select_all" value="1" id="DocumentsItemAssocaition-select-all"></th>

                            <th >
                                DocName
                            </th>

                            <th >
                                ItemName
                            </th>

                            <th>
                                CreatedDate
                            </th>
                        </tr>

                </table>


            </td>


            <td width="30%">

                <table id="ItemTable" class="display select"  cellspacing="0">
                    <thead>
                        <tr>
                            <th><input type="checkbox" name="select_all" value="1" id="ItemTable-select-all"></th>

                            <th>
                                ItemName
                            </th>

                            <th>
                                Origin
                            </th>

                             <th>
                                 HsCode
                            </th>
                           
<%--
                            
                            <th>
                                 HsCode
                            </th>
                            
                            <th>
                                Manufacturer
                            </th>
                            
                            <th>
                                NoOfPackages
                            </th>
                            
                            <th>
                                Quantity

                            </th>
                            <th>
                                NetWeight
                            </th>
                        
                            <th>
                                grossWeight
                            </th>--%>
                        </tr>

                </table>



            </td>
        </tr>
        <tr>
       
        </tr>
    </table>

        <div id="dialog" style="display: none"></div>
   <%if (checkflag == "true")
       {%>
    <div >
    
                <input type="button" class="mcbutton" id="AssociateDocItems" value="Associate documents and items" />
                <input type="button" class="mcbutton" id="DisassociateDocItems" value="Disassociate documents and items" />
          <input type="button" id="SaveItems" class="mcbutton" value="Save Items " />
        
        </div>

    <%} %>
    
    <div>
 

    </div>
</body>
</html>
