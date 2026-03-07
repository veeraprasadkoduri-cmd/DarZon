// ACTIVITY SCRIPTS

function Filsaleorder() {


    var cno = $("#custno").val();

    var data = { customerno: cno };
    var url = "../Activity/Orderlist";
    
    ajaxcallloadtable(url, data, 'fillsaleordr');

    //$("#OrderList").show();
}
function CheckValidation() {

    if (required('custno')) {


        return true;
    }
    else {
      
        return false;
    }
}

function selectactivitysaleorderrow(SaleOrderNo, cno, cname, PhoneNo,websaleorder) {
  
  
    //var SaleOrderNo = $(this).closest("tr").find('span.SaleOrderNo').text();
    //var cno = $(this).closest("tr").find('span.CustomerNo').text();
    //var cname = $(this).closest("tr").find('td.CustomerName').text();
    //var PhoneNo = $(this).closest("tr").find('td.PhoneNo').text();


    $("#SaleOrderNo").val(SaleOrderNo);
    $("#custno").val(cno);
    $("#Customername").val(cname);
    $("#mobileno").val(PhoneNo);
    $("#hdnsaleorder").val(websaleorder);

}

//function Validate() {
//    var e = document.getElementById("ddlactivitytype");
//    var strUser = e.options[e.selectedIndex].value;

//    var strUser1 = e.options[e.selectedIndex].text;
//    if (strUser === 0) {
//        $("#" + "ddlactivitytype").css('background-color', 'white');
//        //$("#EmailPara").fadeOut(1000);

//    }
//    var e = document.getElementById("ddlFristitem");
//    var strUser = e.options[e.selectedIndex].value;

//    var strUser1 = e.options[e.selectedIndex].text;
//    if (strUser === 0) {
//        $("#" + "ddlFristitem").css('background-color', 'white');
//        //$("#EmailPara").fadeOut(1000);

//    }
//}


function Avctivitysaleorder() {
    var cno = $("#custno").val();
    var data = { customerno: cno };
    var url = "../Activity/Orderlist";

    ajaxcallloaddiv(url, data, 'fillsaleordr');

}

function activityclose()
{
    window.location.href = '../Activity/Activity';
}
function selectCustomer(e) {
    
    var custcode = $(e).closest("tr").find('span.custcode').text();
    var custname = $(e).closest("tr").find('td.custname').text();
    var custphno = $(e).closest("tr").find('td.custphone').text();

    $("#custno").val(custcode);
    //$("#BmBinLocDescr").val(TypeDesc);

    $("#Customername").val(custname.trim());
    $("#mobileno").val(custphno.trim());
    $("#Customername").attr('readonly', 'readonly');
    $("#mobileno").attr('readonly', 'readonly');
}

function timepicker(e) {
    $(e).timepicker();
}
// ACTIVITY END

