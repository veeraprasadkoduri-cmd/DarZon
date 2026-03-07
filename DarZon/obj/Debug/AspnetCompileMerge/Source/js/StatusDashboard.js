function allocation() {
    if (CheckValidation() === true) {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = { fromdate: fromdate, todate: todate };

        var url = "../StatusDashboard/Orderstatuslistfrmdatetodate";

        ajaxcallloaddiv(url, data, 'mdlitemlist');
    }
}
function searchdata() {
    if (CheckValidation() === true) {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var valUrl = "../StatusDashboard/Orderstatusladditem";
        var valData = { fromdate: fromdate, todate: todate };



        $.ajax({
            type: "POSt",
            url: valUrl,
            cache: false,
            data: valData,
            async: false,
            success: function (data) {
              
             
                window.location.href = '../StatusDashboard/StatusDashboard';
            }
        });
    }
    
}



function CheckValidation() {

    if (required('fromdate') && required('todate')) {
        return true;
    }
    else {

        return false;
    }
}