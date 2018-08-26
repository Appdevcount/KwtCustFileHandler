


$(document).ready(function () {

    
    



    var flag = "true";
   
    $("#CloseBtn").click(function () { 

     //   IEVersion


        var IEVersion = document.getElementById('IEVersion').value;

        if (IEVersion == "Supported")
        {
            window.close();

        }
        else
        {
            window.open('javascript:window.open("", "_self", "");window.close();', '_self'); 

        }

     //      



    });




    $("#headercheck").click(function () {
        $(".deleteselctedcheckbox:visible").prop('checked', $(this).prop('checked'));
    });
    



    $("#MultiFiles").click(function () {
   
        document.getElementById("tempDiv").style.display = "block";
        document.getElementById("someId").innerHTML = "";
      //  $("input[type='file']").replaceWith($("input[type='file']").clone(true));
       
    });

    $("#remove").click(function () {
        var selected = new Array();
        var getlanguageforalert = document.getElementById('languageAlert').value;
      

        if ($("#someId").text().trim() == "") {
          //  document.getElementById("UploadCtrl").value = "";
            document.getElementById("MultiFiles").value = "";
            if (getlanguageforalert == "eng") {
                alert('No Files To Remove');
            }
            else {
                alert('لا توجد ملفات لإزالتها');
            }

        }


        $("#someId").find(":checkbox:checked").each(function () {

                $(this).closest('tr').remove();

            });

            if ($("#someId").text().trim() == "") {

                document.getElementById("MultiFiles").value = "";
                $("input[type='file']").replaceWith($("input[type='file']").clone(true));
            }

         //   document.getElementById("MultiFiles").value = "";
 
     

        return false;
    });
    
    
    $("#MultiFiles").change(function () {
      
        {
            $("#UploadBtn").removeAttr("disabled");
            var getlanguageforalert = document.getElementById('languageAlert').value;
            var array = document.getElementById("MultiFiles").value.split(",");
            if (array.length > 5)
            {

                if (getlanguageforalert == "eng") {
                    alert('You can select only 5 files ');
                    document.getElementById("MultiFiles").value = "";
                    $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                    return false;
                }
                else {
                    alert('يمكنك تحديد 5 ملفات فقط');
                    document.getElementById("MultiFiles").value = "";
                    $("input[type='file']").replaceWith($("input[type='file']").clone(true));
                    return false;
                }
            }



            for (var i = 0; i < array.length; i++) {
              //  var td = "<tr> <td> <input type='checkbox' class='.dynamiccheckbox'  id='" + array[i] + "'/>" + array[i] + "</td ></tr>";
              //  var td = "<tr> <td style='width: 38px;'> <input type='checkbox' class='dynamiccheckbox'  id='" + array[i] + "'/> </td ><td>" + array[i].substring(array[i].lastIndexOf("\\") + 1, array[i].length) +"</td></tr>";
                var td = "<tr> <td style='width: 38px;'> <input type='checkbox' class='dynamiccheckbox'  id='" + array[i].replace(';', '').replace('=', '') + "'/> </td ><td>" + array[i].replace(';', '').replace('=', '').substring(array[i].replace(';', '').replace('=', '').lastIndexOf("\\") + 1, array[i].replace(';', '').replace('=', '').length) + "</td></tr>";

                $("#someId").append(td);

            }
            return false;


        }
    });

    /*
    $("#RemoveFile").click(function () {
        document.getElementById("UploadCtrl").value = "";
        if ($("#someId").text().trim()="")
        {
            document.getElementById("MultiFiles").value = "";
        }

    });

    */

});

$("#MultiFiles").click(function () {


});

$("#UploadBtn").click(function () {
 
    var selected = new Array();
    var selectedfilename = new Array();
    // alert(document.getElementById('IEVersion').value);
    if (document.getElementById('IEVersion').value != "NotSupported")

    {

       
        var rowcount = $('#Uploadedfileslist tr').length - 1;

        var getallowedcount = document.getElementById('rowCountAlert').value - rowcount;

        var getlanguageforalert = document.getElementById('languageAlert').value;


        if (document.getElementById("MultiFiles").value == "") {

            if (getlanguageforalert == "eng") {
                alert('Please Select File');
            }
            else {
                alert('الرجاء إختيار ملف');
            }
        

            $("#UploadBtn").attr("disabled", true);
            return false;
       
        };

        if (document.getElementById("TxtDescrition").value.length > 256) {

            if (getlanguageforalert == "eng") {
                alert('You cannot have more than 256 charectors ');
            }
            else {
                alert('لا يجب ان يتعدى الوصف 256 حرف');
            }
            return false;
        };

        $("#someId").find("input:checkbox:not(:checked)").each(function () {
          //  selected.push($(this).attr('id'));


            //set these is production amnd comment earler 
               var text = $(this).attr('id');
               text = text.substring(text.lastIndexOf("\\") + 1, text.length);
            selected.push(text);

          //  selectedfilename.push($(this).text());

            //  alert($(this).val());
        });


        var arraylengthforcount = selected.length

        
   
        document.getElementById('SelectedFileId').value = selected.join('|');
             
    }


    else
    {

        var getlanguageforalert = document.getElementById('languageAlert').value;

       
        $('input[type=file]').each(function () {
            //  alert(this.value);

            if (this.value != null) {

                var text = this.value;
                text = text.substring(text.lastIndexOf("\\") + 1, text.length);
                selected.push(text);

                // selected.push(this.files[0].name)


            }
        })
        var uniqueArray = selected.filter(function (elem, pos) {
            return selected.indexOf(elem) == pos;
        });

        document.getElementById('SelectedFileId').value = uniqueArray.join('|');

        var getlanguageforalert = document.getElementById('languageAlert').value;


        if (document.getElementById("SelectedFileId").value == "") {

            if (getlanguageforalert == "eng") {
                alert('Please Select File');
            }
            else {
                alert('الرجاء إختيار ملف');
            }
            return false;
        };

        if (document.getElementById("TxtDescrition").value.length > 256)
        {

            if (getlanguageforalert == "eng") {
                alert('You cannot have more than 256 charectors ');
            }
            else {
                alert('لا يجب ان يتعدى الوصف 256 حرف');
            }
            return false;
        };




    }

    var hasSymbols = new RegExp("[!@#$%^&*=;]");
    // if (document.getElementById('SelectedFileId').value)
    var selctedfilenaemstring = selectedfilename.join(',');
    if (hasSymbols.test(document.getElementById('SelectedFileId').value) ) {
        if (getlanguageforalert == "eng") {
            alert('File Tampered ');
        }
        else {
            alert('ملف تم عرضه');
        }
        return false;

    }

    if (hasSymbols.test(document.getElementById("TxtDescrition").value))

    {
        if (getlanguageforalert == "eng") {
            alert('Description has unwanted charectors ');
        }
        else {
            alert('الوصف له حراس غير مرغوب فيهم');
        }
        return false;


    }





    var e = document.getElementById('ddlDocumentTypes');
        var value = e.options[e.selectedIndex].value;
        //  alert(value);

        document.getElementById('SelectedDropdownId').value = value;
        $("[data-valmsg-for=files]").text('');

     //   $("#UploadBtn").attr("disabled", true);

});

$(".deleteselctedcheckbox").click(function () {
  
    $(".deleteselctedcheckbox").each(function () {
        if (!$(this).is(":checked")) {
            $('#headercheck').prop('checked', false);
            return;
        }

    });
});


$("#DeleteBtn").click(function () {
    // $("#Uploadedfileslist").html('');
    var message = '';
    var HostedLocation = location.pathname.split('/')[1];
    var delteselected = new Array();
 //   $('#ddlDocumentTypes').val('');
    //var checkForBroker = document.getElementById('isbroker').value;

   // var checkForTokenCreatedBY = document.getElementById('tokencreatedby').value;
    var count = 0;
    $('#Uploadedfileslist').find('input.deleteselctedcheckbox:checkbox:checked').each(function () {


        {
         //   alert($(this).attr('name'));
            delteselected.push($(this).attr('name'));
            count++;
            $(this).closest('tr').remove();

        }
    });
    if ($("#Uploadedfileslist").find(".deleteselctedcheckbox:checkbox:checked").text().trim() == "")
    {
    $('#headercheck').prop('checked', false);
      }
    if ($("#Uploadedfileslist").find("input:checkbox:not(:checked)"))
    {
        $('#headercheck').prop('checked', false);

    }



   // $("#Uploadedfileslist").find(".deleteselctedcheckbox:checkbox:checked").text().trim();

    {
        var getlanguageforalert = document.getElementById('languageAlert').value;
        document.getElementById('SelectedFileId').value = delteselected.join('|');

        var dataItem = delteselected.join(',');
   


        if (dataItem == "")
        {

            if (getlanguageforalert == "eng")
            {
                
                alert('No File selected to delete')
                $('#headercheck').prop('checked', false);
            return false;
            }
            else
            {
                alert('لم يتم اختيار أي ملفات للحذف')
                $('#headercheck').prop('checked', false);
                return false;
            }

        }
        else
        {
            if (getlanguageforalert == "eng") {

              //  alert('No File selected to delete')
                message = ' File  deleted';
             
          
            }
            else {
                message = " تم حذف الملف";
              //  $("#UploadForm").submit();
          
            }
            var e = document.getElementById('ddlDocumentTypes');
            var value = e.options[e.selectedIndex].value;
            //  alert(value);

            document.getElementById('SelectedDropdownId').value = value;

            var dropdownvalue = document.getElementById('SelectedDropdownId').value;

        $("#texens").val(dataItem);
        document.getElementById("texens").value = "dataItem";



        $.ajax({
            type: "POST",
            // switch over after hosting 
        url: "/ScanUploadServiceAugpp/Home/DeleteFile",
       //    url: "/Home/DeleteFile",
            data: { 'dataItem': dataItem, 'dropdownvalue': dropdownvalue },
            success: function (response) {
                $("#UploadForm").submit();
                alert(message);
                $('#headercheck').prop('checked', false);
            
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
        }

    }
    return false;
});





$("#TextUpload").click(function () {


});

