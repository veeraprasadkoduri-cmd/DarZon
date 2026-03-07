function SelectTrailRecord() {
    // var Docnum = $(this).closest("tr").find('span.custcode').text();
    var frmdate = $('#RDTFromdate').val();
    var tdate = $('#RDTTodate').val();
    var data = { fromdate: frmdate, todate: tdate};
    var url = "../Trail/Trailadditem";
    ajaxcallloaddiv(url, data, 'traildiv');

}
function CheckValidation() {

    if (required('SaleOrderNo') && required('CustomerNo')) {
        return true;
    }
    else {
      
        return false;
    }
}