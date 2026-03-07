function selectrowDeliver(e) {


    var SaleOrderNo = $(e).closest("tr").find('span.SaleOrderNo').text();
    var custNo = $(e).closest("tr").find('span.CustomerNo').text();
    //   var DelDate = $(e).closest("tr").find('span.DelDate').text();
    var AltDetails = $(e).closest("tr").find('span.AltDetails').text();
    //   var TotalBefDis = $(e).closest("tr").find('span.TotalBefDis').text();
    var DiscountP = $(e).closest("tr").find('span.DiscountP').text();
    var Tax = $(e).closest("tr").find('span.Tax').text();

    //       var saleorderdate = $(e).closest("tr").find('td.saleorderdate').text();
    // var PhoneNo = $(e).closest("tr").find('td.PhoneNo').text();
    var CustomerName = $(e).closest("tr").find('td.CustomerName').text();
    //  var AltPhoneNo = $(e).closest("tr").find('td.AltPhoneNo').text();
    var DocTotal = $(e).closest("tr").find('td.DocTotal').text();
    var docentry = $(e).closest("tr").find('span.Docentey').text();

    $("#AltDetails").val(AltDetails);
    // $("#AltDate").val(saleorderdate);
    $("#TotalBefDis").val(Math.round(DocTotal),0);
    $("#DiscountP").val(Math.round(DiscountP),0);
    $("#DocTotal").val(Math.round(DocTotal),0);
    $("#CustomerName").val(CustomerName.trim());
    $("#DevSaleOrderNo").val(SaleOrderNo);
    $("#CustomerNo").val(custNo);
    $("#TotalTax").val(Tax);
    var data = { salorderNo: docentry };
    var url = "../Delivery/orderdetails";
    ajaxcallloaddiv(url, data, 'divsaleorderdetails');





}

function checkvalidation() {

    var leng = $("#orderdetails tr").length;
  
    if (leng > 1) {
        return true;
    }
    else {
        return false;
    }


}







