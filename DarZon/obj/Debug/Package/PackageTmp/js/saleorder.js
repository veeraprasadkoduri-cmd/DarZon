function additemfromlist(Itemcode, Itemname, Unitpeice, noofdays) {

    //var Itemcode = $(this).closest("tr").find('span.Itemcode').text();
    //var Itemname = $(this).closest("tr").find('td.Itemname').text();
    //var Unitpeice = parseFloat($(this).closest("tr").find('td.Unitprice').text());
    var category = $("#ddlFristitem option:selected").text();
    var data = { category: category, Itemcode: Itemcode, Itemname: Itemname, Unitpeice: Unitpeice, noofdays: noofdays };
    var url = "../SaleOrder/AddItem";
    // alert(data);
    ajaxcallloadtable(url, data, 'SaleordLineItems');

}
function deleterow(a) {
    var amount = parseInt($(a).closest("tr").find('input.totalamount').val());
    var TOBFDIS = 0;
    if ($.isNumeric($("#TxtTotalBefDis").val())) {

        TOBFDIS = parseInt($("#TxtTotalBefDis").val());
    }
    var total = TOBFDIS - amount;
    $("#TxtTotalBefDis").val(Math.round(total));
    var id = parseInt($(a).closest("tr").find('input.ID').val());
    var valUrl = "../SaleOrder/deleteItem";
    var data = { id: id };
    var result = ajaxcallRedirection(valUrl, data);
    if (result === "Success") {
        $("#TxtTotalBefDis").val(Math.round(total));
        $(a).closest('tr').remove();
        sumoftotal();

    }

}
function deldatepicker(e) {
    $(e).closest("tr").find('span.deliverymsg').fadeOut(300);
    $(e).closest("tr").find('span.deliverymsg').empty();
    $(e).datepicker({
        format: 'dd/mm/yyyy',
        numberOfMonths: 1,
        defaultDate: "",
        minDate:  function() {
            return $('#saledate').val();
        },
        
     

//        onClose: function (dateText, inst) { alert('hai'); }

    });

    //    .on('change', function () {
    //    var deliverydate = $(e).val();

    //    var saleorderdate = $("#objsaleorderHeader_DocDate").val();
    //    var fromdate = saleorderdate.split('/');
    //    var todate = deliverydate.split('/');



    //    var id = $(e).closest("tr").find('input.ID').val();
    //    var noofdays = $(e).closest("tr").find('input.noofdays').val();
    //    if (todate[0] > fromdate[0] && todate[1] >= fromdate[1] && todate[2] >= fromdate[2]) {

    //        var url = "../SaleOrder/getpercentage";
    //        var data = { fromdate: saleorderdate, todate: deliverydate, lineid: id, Nodays: noofdays };
    //        var percertage = ajaxcallRedirection(url, data);
       
    //        if (percertage !== "0") {
    //            var total = parseFloat($("#TxtTotalBefDis").val()) + parseFloat(percertage);
    //            $("#TxtTotalBefDis").val(total);
    //            sumoftotal();
    //        }

    //    }
    //    else {

    //        $(e).closest("tr").find('span.deliverymsg').fadeIn(300);
    //        $(e).closest("tr").find('span.deliverymsg').empty();
    //        $(e).closest("tr").find('span.deliverymsg').append("select proper date");
    //        $(e).css('border-color', 'red');

    //    }
    //});
}
function Quantitychange(a) {
    var quantity = $(a).closest("tr").find('input:Quantity').text();
    //  alert(quantity);
}
function Measurement(a) {
    var ID = $(a).closest("tr").find('input.ID').val();
    var category = $(a).closest("tr").find('input.category').val();
    var Noofdays = $(a).closest("tr").find('input.noofdays').val();
    var deliverydate = $(a).closest("tr").find('input.deliverydate').val();
    var custatus = $(a).closest("tr").find('input.curstatus').val();
    var cate = parseInt(category);

    window.location.href = "../Measurmentdetails/Measurmentdetails?id=" + ID + "&category=" + cate + "&saleorder=" + $("#objsaleorderHeader_DocEntry").val() + "&custId=" + $("#CustomerNo").val() + "&custdetails=" + $("#CustomerName").val() + "&salorderdate=" + $("#saledate").val() + "&nodays=" + Noofdays + "&pickupuser=" + $("#ddlprdesign").val() + "&deliverydate=" + deliverydate + "&cusphonenumber=" + $("#PhoneNo").val() + "&currentstatus=" + custatus;

}
function AddNewItem() {
    var category = $("#ddlFristitem option:selected").val();
    var Noofdays = 0;
    if (category.length > 0) {
        var cate = parseInt(category);
        //var data = {
        //    saleorder: $("#objsaleorderHeader_DocEntry").val(),
        //    itemcode: '',
        //    id: 0,
        //    category: cate,
        //    custId: $("#objsaleorderHeader_DocEntry").val(),
        //custdetails: $("#objsaleorderHeader_DocEntry").val()
        //};
       
        window.location.href = "../Measurmentdetails/Measurmentdetails?id=" + "0" + "&category=" + cate + "&saleorder=" + $("#objsaleorderHeader_DocEntry").val() + "&custId=" + $("#CustomerNo").val() + "&custdetails=" + $("#CustomerName").val() + "&salorderdate=" + $("#saledate").val() + "&pickupuser=" + $("#ddlprdesign").val() + "&nodays=" + Noofdays + "&cusphonenumber=" + $("#PhoneNo").val();
        // ajaxcallRedirection("../Measurmentdetails/Measurmentdetails/", data);

    }
    else {
        alert('Please select category');
    }
}
function sumofadvance() {
    var cardamount = 0;
    var cash = 0;
    var other = 0;
    var totaladvance = 0;
    var totalamount = 0;
    var totalbalance = 0;
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
   


    totaladvance = parseFloat(cardamount) + parseFloat(cash) + parseFloat(other);
   totalbalance = totalamount - totaladvance;
 
    $("#TxtBalance").val(Math.round(totalbalance));
   

    $("#TxtAdvance").val(Math.round(totaladvance));



}
function proceed() {

    $("#paymentdetails").show();
    $("#divsave").show();
    $("#divitemlist :input").prop('disabled', true);
    $("span").off("click");
    $("#btnprcepay").hide();
    $("#btnClaerItems").hide();

    $("#btnMdfOrder").show();

}
function ModifyOrder() {
    $("#paymentdetails").hide();
    $("#divsave").hide();


    $("#divitemlist :input").prop('disabled', false);
    $("span").on("click");
    $("#btnprcepay").show();
    $("#btnClaerItems").show();
    $("#btnMdfOrder").hide();
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
        if (discountpercent <= 15 && discountpercent>=0) {
            // alert(discountpercent);
            sumoftotal();
        }
        else {
            $("#Txtdiscount").val("");
            alert('Discount should be  0 - 15%');
        }

    }
}
function getpercentage(e) {
    var deliverydate = "";
    var saleorderdate = "";
    saleorderdate = $("#saledate").val();
    deliverydate = $(e).val();
    var todate = deliverydate.split("/");
    var fromdate = saleorderdate.split("/");
   
    var id = $(e).closest("tr").find('input.ID').val();
    var noofdays = $(e).closest("tr").find('input.noofdays').val();
    var rate = 0; 
    var total = 0;

    //if ( todate[2] >= fromdate[2] && todate[1] >= fromdate[1]  ) {
      //  $("#CustomerNo").val(todate[0] + "-" + fromdate[0]);
    //    if (todate[0] > fromdate[0] || (todate[0] < fromdate[0] && todate[2] > fromdate[2] && todate[1] > fromdate[1])) {
           
            var valUrl = "../SaleOrder/getpercentage";
        var valData = { fromdate: saleorderdate, todate: deliverydate, lineid: id, Nodays: noofdays };

        $.ajax({
            type: "POST",
            url: valUrl,
            cache: false,
            data: valData,
            async: false,
            success: function (data) {

                if (data.peramount !== "0") {

                    $("#TxtTotalBefDis").val(data.totalamount);
                    sumoftotal();
                    $(e).closest("tr").find('input.unitprice').val(data.peramount);
                    $(e).closest("tr").find('input.totalamount').val(data.linetotal);
                   // $("#CustomerNo").val(data.peramount);

                }
            }
        });


        


  //  }
    //else {

    //    $(e).closest("tr").find('span.deliverymsg').fadeIn(300);
    //    $(e).closest("tr").find('span.deliverymsg').empty();
    //    $(e).closest("tr").find('span.deliverymsg').append("select proper date");
    //    $(e).css('border-color', 'red');

    //}
    //}

}
function CheckValidation() {
  

    var rowCount = $('#SaleordLineItems tr').length;
     if (rowCount > 2) {
        
         var amount = 0;
         var advance = 0;
         if ($("#Txtamount").val().length > 0) {
             amount =parseInt($("#Txtamount").val());
         }
         if ($("#TxtAdvance").val().length > 0)
         {
             advance = parseInt($("#TxtAdvance").val());
         }

         if (advance >= parseInt((amount / 2))) {
             if ((parseInt($("#CashAmount").val()) >= 0 || $("#CashAmount").val().length <= 0) && (parseInt($("#CardAmount").val()) >= 0 || $("#CardAmount").val().length <= 0) && (parseInt($("#OtherPaymentAmount").val()) >= 0 || $("#OtherPaymentAmount").val().length <= 0)) {
                 if (parseFloat($("#TxtBalance").val()) >= 0) {
                     if (required('CustomerNo') && required("saledate")) {
                         beforesave();
                         var message = "Dear Customer otp is XXXX";
                         var url = "../OTPCreation/createOTP";
                         var data = { saleorder: $("#OTPSaleorderNo").val(), mobileno: $("#PhoneNo").val(), message: message };
                         var result = ajaxcallRedirection(url, data);
                         if (result !== "Fail") {
                             $("#saleOTP").val(result);
                             $('#Savedetails').modal('show');
                             return true;
                         }
                         else {
                             return false;
                         }
                     }
                     else {


                         return false;
                     }

                 } else {
                     alert('Balance amount should be zero or greater than zero');
                     return false;
                 }

             }
             else {

                 alert('enter proper payment details');
                 return false;
             }
         }
         else {
             alert('Atleast 50% advance is required');
             return false;

         }


     }
    else {
        alert('No Items are selected');
        return false;
    }


}
function selectCustomer(e) {
    var custcode = $(e).closest("tr").find('span.custcode').text();
    var custname = $(e).closest("tr").find('td.custname').text();
    var custphno = $(e).closest("tr").find('td.custphone').text();
    debugger;
    $("#PhoneNo").val(custphno);
    $("#CustomerNo").val(custcode);
    //$("#BmBinLocDescr").val(TypeDesc);

    $("#CustomerName").val(custname);

    $("#CustomerName").attr('readonly', 'readonly');

}
function beforesave() {
    $("#divitemlist :input").prop('disabled', false);
}


function refreshpage() {
    window.location.href = '../SaleOrder/SaleOrder';
}
// END SALE ORDER SCRIPTS