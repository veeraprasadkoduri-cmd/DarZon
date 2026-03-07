function searchdata() {
    if (CheckValidation() === true) {
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = { fromdate: fromdate, todate: todate };
        var url = "../TailorAssignedDashboard/searchdata";

        var Redirectstatus = ajaxcallRedirection(url, data);

        if (Redirectstatus === 'Success') {
            window.location.href = '../TailorAssignedDashboard/TailorAssignedDashboard';
        }
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