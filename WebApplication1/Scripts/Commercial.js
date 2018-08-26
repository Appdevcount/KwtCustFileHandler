


$(document).ready(function () {

    $("#SaveItems").click(function ()
    {
       
        var totalcheckbox = $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").length
        alert(olddata);
       // if (olddata.length == 0 && newaddition.length==0)
            {
        if (totalcheckbox != '0')
            {

            $("#DocumentsItemAssocaition").find(".selected input:checkbox:checked").each(function () {
            var y = $(this).attr('value');
        
            if (y.indexOf("|") != -1)
            {
                newaddition.push($(this).attr('value'));

                alert(newaddition.join('@'));
                document.getElementById('SelectedItemAssocationId').value = newaddition.join('|');

            }

            else
            {
                olddata.push($(this).attr('value'));

                alert(olddata.join('@'));

                document.getElementById('SelectedItemAssocationId').value = olddata.join('|');
            }

        });


        }
        else
            {
        alert('select An Item To Save');
        }
        }
       // else
        {
            var number = 1;
            alert('ajax call' + olddata);
            $.ajax({
                type: "POST",
                url: " ItemAssociation.aspx/AssociateandDisassociate",
              //  data: { 'olddata': olddata, 'newdata': newaddition },
                data: '{olddata: "' + olddata + '",newdata: "' + newaddition + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                  //  alert("Valid number.");
                    OnError
                },
                error: OnError
            });
      
        }


        });
    


    function OnError(xhr, errorType, exception)
    {
        alert('err');
        debugger;
        var responseText;
        $("#dialog").html("");
        try {
            responseText = jQuery.parseJSON(xhr.responseText);
            $("#dialog").append("<div><b>" + errorType + " " + exception + "</b></div>");
            $("#dialog").append("<div><u>Exception</u>:<br /><br />" + responseText.ExceptionType + "</div>");
            $("#dialog").append("<div><u>StackTrace</u>:<br /><br />" + responseText.StackTrace + "</div>");
            $("#dialog").append("<div><u>Message</u>:<br /><br />" + responseText.Message + "</div>");
        } catch (e)
        {
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
