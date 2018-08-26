


$(document).ready(function () {

    
    



    var flag = "true";
   
    $("#CloseBtn").click(function () { 
        window.close();
    });




    $("#headercheck").click(function () {
        $(".deleteselctedcheckbox").prop('checked', $(this).prop('checked'));
    });
    $("#MultiFiles").click(function () {
   
        document.getElementById("tempDiv").style.display = "block";
        document.getElementById("someId").innerHTML = "";
      //  $("input[type='file']").replaceWith($("input[type='file']").clone(true));
       
    });

    $("#remove").click(function () {
        var selected = new Array();
        var getlanguageforalert = document.getElementById('languageAlert').value;
        $("#someId").find(":checkbox:checked").each(function () {

            $(this).closest('tr').remove();

        });


        if ($("#someId").text().trim() == "") {
         


            if (getlanguageforalert == "eng") {
                alert('No Files To Remove');
            }
            else {
                alert('لا توجد ملفات لإزالتها');
            }



            document.getElementById("MultiFiles").value = "";
            $("input[type='file']").replaceWith($("input[type='file']").clone(true));
        }

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
                var td = "<tr> <td style='width: 38px;'> <input type='checkbox' class='.dynamiccheckbox'  id='" + array[i] + "'/> </td ><td>" + array[i].substring(array[i].lastIndexOf("\\") + 1, array[i].length) +"</td></tr>";

                $("#someId").append(td);

            }
            return false;


        }
    });


    $("#RemoveFile").click(function () {
        document.getElementById("UploadCtrl").value = "";
        if ($("#someId").text().trim()="")
        {
            document.getElementById("MultiFiles").value = "";
        }

    });

});

$("#MultiFiles").click(function () {


});

$("#UploadBtn").click(function () {
  //  debugger;
    var selected = new Array();
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


        $("#someId").find("input:checkbox:not(:checked)").each(function () {
          //  selected.push($(this).attr('id'));


            //set these is production amnd comment earler 
               var text = $(this).attr('id');
               text = text.substring(text.lastIndexOf("\\") + 1, text.length);
            selected.push(text);


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

        document.getElementById('SelectedFileId').value = selected.join('|');

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


    }
        var e = document.getElementById('ddlDocumentTypes');
        var value = e.options[e.selectedIndex].value;
        //  alert(value);

        document.getElementById('SelectedDropdownId').value = value;
        $("[data-valmsg-for=files]").text('');

        $("#UploadBtn").attr("disabled", true);

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
          
            }
            message

        $("#texens").val(dataItem);
        document.getElementById("texens").value = "dataItem";



        $.ajax({
            type: "POST",

      //     url: "/ScanUploadDocument/Home/DeleteFile",
            url: "/Home/DeleteFile",
            data: { 'dataItem': dataItem },
            success: function (response) {

                alert(message);

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

