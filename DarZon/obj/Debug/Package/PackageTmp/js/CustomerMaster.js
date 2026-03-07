 function CheckValidation() {

        if (Irequired('CustomerName', 'CustomerNamepara', 'Enter Name') && Irequired('PhoneNo', 'PhoneNoMobilePara', 'Enter Phone number')) {
            var LoginUser =
            {
                newpwd: $("#PhoneNo").val(),



            }; 
            var valUrl = '../CustomeDetailsr/checkphno/';

            var Redirectstatus = ajaxcallRedirection(valUrl, LoginUser);

            if (Redirectstatus === 'Success') {

                return true;
            }
                else
            {
                $("#PhoneNo").val("");
                alert("Phone number Already Exist");
                return false;
                }

}
        else {
      
    return false;
}
}
function selectCustomer(e) {
    $("#Savediv").hide();
    debugger;
    // $("#Updatediv").show();
    document.getElementById("Updatediv").style.visibility = "visible";
    var custcode = $(e).closest("tr").find('span.custcode').text();


    var AltrphoneNo = $(e).closest("tr").find('span.AltrphoneNo').text();


    var Area = $(e).closest("tr").find('span.Area').text();
    var Block = $(e).closest("tr").find('span.Block').text();
    var city = $(e).closest("tr").find('span.city').text();
    var CustTYpe = $(e).closest("tr").find('span.CustTYpe').text();
    var DoorNo = $(e).closest("tr").find('span.DoorNo').text();
    var Emailid = $(e).closest("tr").find('span.Emailid').text();
    var Landmark = $(e).closest("tr").find('span.Landmark').text();



    var Pincode = $(e).closest("tr").find('span.Pincode').text();
    var State = $(e).closest("tr").find('span.State').text();
    var Street = $(e).closest("tr").find('span.Street').text();
    var Anniversery = $(e).closest("tr").find('span.Anniversery').text();
    var DBO = $(e).closest("tr").find('span.DBO').text();
    

    var custname = $(e).closest("tr").find('td.custname').text();
    var custphno = $(e).closest("tr").find('td.custphone').text();

    $("#customercode").val(custcode);
    //$("#BmBinLocDescr").val(TypeDesc);

    $("#CustomerName").val(custname);

    $("#CustomerName").attr('readonly', 'readonly');

   
    //FindDetail();\
    $("#PhoneNo").val(custphno);
    $("#PhoneNo").attr('readonly', 'readonly');

    $("#DoorNo").val(DoorNo);

    $("#Street").val(Street);
    $("#Block").val(Block);
    $("#Area").val(Area);
    $("#Landmark").val(Landmark);
    $("#city").val(city);
    $("#Pincode").val(Pincode);
    $("#State").val(State);
    $("#APhoneNo").val(AltrphoneNo);
    $("#Emailid").val(Emailid);
    $("#Anniversery").val(Anniversery);

    $("#DBO").val(DBO);
    //$("#DetailsPopup").dialog('close')
}
function refreshpage() {
    window.location.href = '../CustomeDetailsr/CustomerMaster';
}

function custdatepicker(e) {
    var d = new Date();
    $(e).closest("tr").find('span.deliverymsg').fadeOut(300);
    $(e).closest("tr").find('span.deliverymsg').empty();
    $(e).datepicker({
        format: 'dd/mm/yyyy',
        numberOfMonths: 1,
        defaultDate: "",
        maxDate: function () {
            return d;
        }

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