jQuery(document).ready(function ($) {
    if (document.getElementById('Uploadedfileslist') != null) {
       
            $(function () {
                $('#Uploadedfileslist tr').hover(function () {
                    $(this).addClass('DataGridItem_Row_Selected');
                }, function () {
                    $(this).removeClass('DataGridItem_Row_Selected');
                });
            });

       
    }
});
