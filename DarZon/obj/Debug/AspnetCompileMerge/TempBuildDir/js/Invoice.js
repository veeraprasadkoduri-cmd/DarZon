
function SelectOrderRow(e) {
    debugger;
    $("#Invoicedivprint").hide();
    $("#Invoicedivsave").show();
    var altdate = $(e).closest("tr").find('span.altdate').text();
    var SaleOrderNo = $(e).closest("tr").find('span.SaleOrderNo').text();
    var custNo = $(e).closest("tr").find('span.CustomerNo').text();
    var DelDate = $(e).closest("tr").find('span.DelDate').text();
    var websaleorder = $(e).closest("tr").find('span.WebSaleNo').text();

    var AltDetails = $(e).closest("tr").find('span.AltDetails').text();
    var TotalBefDis = $(e).closest("tr").find('span.TotalBefDis').text();
    //var DiscountP = $(this).closest("tr").find('span.DiscountP').text();
    var Advance = $(e).closest("tr").find('span.advance').text();

    if (Advance.trim().length <= 0) {
        Advance = 0;
    }
    var Tax = $(e).closest("tr").find('span.Tax').text();
    var saleorderdate = $(e).closest("tr").find('td.saleorderdate').text();
    //   var PhoneNo = $(this).closest("tr").find('td.PhoneNo').text();
    var CustomerName = $(e).closest("tr").find('td.CustomerName').text();
    //  var AltPhoneNo = $(this).closest("tr").find('td.AltPhoneNo').text();
    var DocTotal = $(e).closest("tr").find('td.DocTotal').text();
    var balance = parseFloat(DocTotal) - parseFloat(Advance);
    $("#AltDetails").val(AltDetails);
 
    $("#DiscountP").val(Math.round(TotalBefDis),0);
    $("#WebSaleNo").val(websaleorder);

    //  $("#DiscountP").val(DiscountP);
    $("#total").val(Math.round(DocTotal),0);
    $("#TxtAdvance").val(Math.round(Advance),0);
    $("#AltDetails").val(AltDetails);
 
    $("#TxtBalance").val(Math.round(balance),0);

    // $("#DiscountP").val(TotalBefDis);
    //  $("#DiscountP").val(DiscountP);

    $("#Txtamount").val(Math.round(DocTotal),0);


    $("#TxtBalance").val();

    $("#TxtTotalBefDis").val(Math.round(TotalBefDis),0);



    $("#CustomerName").val(CustomerName.trim());
    $("#DelNo").val(SaleOrderNo);
    $("#InvoiceDate").val(DelDate);

    $("#cno").val(custNo);
    $("#tottax").val(Tax);
    var data = { deliveryno: SaleOrderNo };
    var url = "../Invoice/invdetails";

    ajaxcallloaddiv(url, data, 'divsaleorderdetails');


}
function SelectInvoiceRow(e) {
    debugger;
    $("#Invoicedivprint").show();
    $("#Invoicedivsave").hide();
    var altdate = $(e).closest("tr").find('span.altdate').text();
    var InvNo = $(e).closest("tr").find('span.SaleOrderNo').text();
    var custNo = $(e).closest("tr").find('span.CustomerNo').text();
    var DelDate = $(e).closest("tr").find('span.DelDate').text();
    var websaleorder = $(e).closest("tr").find('td.WebSaleorder').text();
    var delNo = $(e).closest("tr").find('span.delve').text();
    var AltDetails = $(e).closest("tr").find('span.AltDetails').text();
    var TotalBefDis = $(e).closest("tr").find('span.TotalBefDis').text();
    $("#WebSaleNo").val(websaleorder);
    var Discount = $(e).closest("tr").find('span.discount').text();
    var Tax = $(e).closest("tr").find('span.Tax').text();
    var saleorderdate = $(e).closest("tr").find('td.saleorderdate').text();
    //   var PhoneNo = $(this).closest("tr").find('td.PhoneNo').text();
    var CustomerName = $(e).closest("tr").find('td.CustomerName').text();
    //  var AltPhoneNo = $(this).closest("tr").find('td.AltPhoneNo').text();
    var DocTotal = $(e).closest("tr").find('td.DocTotal').text();
    var Advance = $(e).closest("tr").find('span.advance').text();
    var curadvance = $(e).closest("tr").find('span.Curadv').text();
    $("#TxtAdvance").val(Math.round(Advance));
    $("#AltDetails").val(AltDetails);
    $("#altdate").val(DelDate);
    $("#CashAmount").attr('readonly', 'readonly');
    $("#CardAmount").attr('readonly', 'readonly');
    $("#CardDetails").attr('readonly', 'readonly');
    $("#OtherPaymentAmount").attr('readonly', 'readonly');
    $("#OtherPaymentDetails").attr('readonly', 'readonly');
    $("#AltDetails").attr('readonly', 'readonly');
    $("#Txtdiscount").attr('readonly', 'readonly');
    $("#Txtdiscount").val(Discount);
   
    // $("#DiscountP").val(TotalBefDis);
    //  $("#DiscountP").val(DiscountP);
    $("#TxtCurAdvance").val(curadvance);

    $("#Txtamount").val(Math.round(TotalBefDis));
    $("#TxtBalance").val(0);
    $("#TxtTotalBefDis").val(Math.round(DocTotal));
    $("#CustomerName").val(CustomerName.trim());
    $("#DelNo").val(delNo);
    $("#InvoiceDate").val(DelDate);
    $("#cno").val(custNo);
    $("#tottax").val(Tax);
    document.getElementById("Print").style.visibility = "visible";
    $("#InvNo").val(InvNo);
    var data = { InvNo: InvNo };
    var url = "../Invoice/invoicedetails";
    ajaxcallloaddiv(url, data, 'divsaleorderdetails');


}
function refreshpage() {
    window.location.href = '../Invoice/Invoice';
}

function sumofadvance() {
    var leng = $("#TblInvList tr").length;

    if (leng > 1) {
        var cardamount = 0;
        var cash = 0;
        var other = 0;
        var totaladvance = 0;
        var totalamount = 0;
        var totalbalance = 0;
        var curAdvance = 0;
        var advance = 0;
        if ($.isNumeric($("#TxtAdvance").val())) {
            advance = parseFloat($("#TxtAdvance").val());
        }

        if ($.isNumeric($("#CashAmount").val())) {
            cash = parseFloat($("#CashAmount").val());
        }
        if ($.isNumeric($("#CardAmount").val())) {
            cardamount = parseFloat($("#CardAmount").val());
        }
        if ($.isNumeric($("#OtherPaymentAmount").val())) {
            other = parseFloat($("#OtherPaymentAmount").val());
        }

        if ($.isNumeric($("#Txtamount").val())) {
            totalamount = parseFloat($("#Txtamount").val());
        }

        if ($.isNumeric($("#TxtCurAdvance").val())) {
            curAdvance = parseFloat($("#TxtCurAdvance").val());
        }


        totaladvance = parseFloat(cardamount) + parseFloat(cash) + parseFloat(other);
        $("#TxtCurAdvance").val(Math.round(totaladvance));

        totalbalance = totalamount - (totaladvance + advance);
        $("#TxtBalance").val(Math.round(totalbalance));


    }
    else {
        refreshpage();
    }


}
function sumoftotal() {

    // alert("hi");
    
   
    var befortax = 0;
    var discountpercent = 0;
    if ($.isNumeric($("#Txtdiscount").val())) {
        discountpercent = parseFloat($("#Txtdiscount").val());
        // alert(discountpercent);
    }

    if ($.isNumeric($("#TxtTotalBefDis").val())) {
        befortax = parseFloat($("#TxtTotalBefDis").val());

    }


    var discount = (100 - discountpercent);
    var tottal = befortax * (discount / 100);
    $("#Txtamount").val(Math.round(tottal));
    sumofadvance();
  


}

function checkdiscount() {
    var discountpercent = 0;
    if ($.isNumeric($("#Txtdiscount").val())) {

        discountpercent = parseFloat($("#Txtdiscount").val());
        if (discountpercent <= 15) {
            // alert(discountpercent);
            sumoftotal();
        }
        else {

            alert('Discount should not exceed more than 15%');
        }

    }
}
function checkbalance() {
    var leng = $("#TblInvList tr").length;
  
    if (leng > 1) { 

    if ($.isNumeric($("#TxtBalance").val())) {
        
            if (parseFloat($("#TxtBalance").val()) === 0) {
                return true;

            }
            else {
                
                alert('Balance Amount Should be zero');
                return false;
            }


        
    }
    else {

        alert('Balance Amount Should be zero');
        return false;
    }
    } else { return false; }

}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
