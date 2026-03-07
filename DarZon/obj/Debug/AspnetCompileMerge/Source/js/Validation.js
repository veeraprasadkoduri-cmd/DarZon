/// <reference path="signup.js" />


function Irequired(controlId, displyId, displaytext) {

    var PWD = $("#" + controlId).val();


    if (PWD === 0 || PWD === "") {

        error(controlId);
      
    }
    else {

        return true;
    }
}

function required(controlId)
{

    var PWD = $("#" + controlId).val();
   

    if (!PWD)
    {
        error(controlId);
    }
    else
    { 
        $("#" + controlId).css({ "background":""});
        return true;
    }
}
function error(controlId) {
   //$("#" + controlId).css('border-color', '');

    $("#" + controlId).css({ "background": "#FFCECE" });


    
    return false;
}

function errordisplay(displyId, displaytext) {
    $("#" + displyId).fadeIn(300);
    $("#" + displyId).empty();
    $("#" + displyId).append(displaytext);
    $("#" + displyId).fadeOut(8000);

    $("#" + displyId).focus();
    return false;
}

function checkEmail(controlId, displayId, displayText) {
    var email = $('#' + controlId).val();
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (!filter.test(email)) {
        errordisplay(displayId, displayText);

        return false;
    }
    else {
        return true;
    }


}

function ajaxcallloadtable(valUrl, valData, divid) {
    var Redtstatus = '';
    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {
         //   alert(data);
            //$('#' + divid).append(data);

            $('#' + divid).find('tr:last').before(data);

        }
    });
}
function ajaxcallloadtableWithoutemptyrow(valUrl, valData, divid) {
    var Redtstatus = '';
    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {
            //   alert(data);
            $('#' + divid).append(data);

            //$('#' + divid).find('tr:last').before(data);

        }
    });
}
function datepicker(e) {
    $(e).datepicker({
        format: 'dd/mm/yyyy',
        numberOfMonths: 1,
        defaultDate: "",
    });
}

function getExtensions(controlId, displayId, displayText) {
    var fileextens = $('#' + controlId).val();
    var ext = fileextens.split('.').pop().toLowerCase();
    if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) === -1) {
        $(this).val("");
        errordisplay(displayId, displayText);

        return false;
    }
}

function checkNotRegEmail(controlId, displayId, displayText, valUrl, valData) {
    checkDuplicateMobile(controlId, displayId, displayText, valUrl, valData);
    //$.ajax({
    //    type: "POST",
    //    url: valUrl,
    //    cache: false,
    //    data: valData,
    //    success: function (data) {
           
    //        if (data.value == 'Success') {
    //            //alert(displayId);
    //            errordisplay(displayId, displayText);
    //        }
    //        else {
    //            clearErrorDisplay(controlId, displayId);
    //        }

    //    },
    //});
}

function isNumber(controlId) {
    evt = (controlId) ? controlId : window.event;


    var charCode = (controlId.which) ? controlId.which : controlId.keyCode;
    
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {

        return false;
    }
    return true;
}

function checkDuplicateEmail(controlId, displayId, displayText, valUrl, valData) {
    checkDuplicateMobile(controlId, displayId, displayText, valUrl, valData);
    //$.ajax({
    //    type: "POST",
    //    url: valUrl,
    //    cache: false,
    //    data: valData,
    //    success: function (data) {
    //         if (data.value == "Success") {
    //            errordisplay(displayId, displayText);
    //        }
    //        else {
    //            clearErrorDisplay(controlId, displayId);
    //        }

    //    },
    //});
}
function Mobile(id, srvtype) {
    var phone = $("#" + id).val();
    var IndNum = /^\d{10}$/;
    var value = $("#" + id).val();
    if (value.length > 0) {
        if (IndNum.test(phone)) {
            $("#" + id).css('border-color', '');
            $("#" + id + "MobilePara").fadeOut(1000);
           
           // CheckDublicateMobileNumber(phone, srvtype);
            //$("#MobilePara").fadeIn(300);
            //$("#MobilePara").empty();
            //$("#MobilePara").append("OTP Sent to your mobile number.");
            //$("#MobilePara").fadeOut(12000);
            return true;
        }
        else {
            $("#" + id).addClass("error");
            $("#" + id).val('');
            $("#"+id+"MobilePara").fadeIn(300);
            $("#" + id + "MobilePara").empty();
            $("#" + id + "MobilePara").append("mobile number should be 10 digits.");
            $("#" + id).css('border-color', 'red');
            $("#" + id).focus();
        }
    }
    else {
        $("#" + id).removeClass("error");
        return true;
    }
}

function Pincode(id, srvtype) {
    var phone = $("#" + id).val();
    var IndNum = /^[0-9]{1,6}$/;
    var value = $("#" + id).val();
    if (value.length > 0) {
        if (IndNum.test(phone)) {
            $("#" + id).css('border-color', '');
            $("#" + id + "MobilePara").fadeOut(1000);

            // CheckDublicateMobileNumber(phone, srvtype);
            //$("#MobilePara").fadeIn(300);
            //$("#MobilePara").empty();
            //$("#MobilePara").append("OTP Sent to your mobile number.");
            //$("#MobilePara").fadeOut(12000);
            return true;
        }
        else {
            $("#" + id).addClass("error");
            $("#" + id).val('');
            $("#" + id + "MobilePara").fadeIn(300);
            $("#" + id + "MobilePara").empty();
            $("#" + id + "MobilePara").append("mobile number should be 10 digits.");
            $("#" + id).css('border-color', 'red');
            $("#" + id).focus();
        }
    }
    else {
        $("#" + id).removeClass("error");
        return true;
    }
}


function checkDuplicateMobile(controlId, displayId, displayText, valUrl, valData) {

    var Redirectstatus=  ajaxcallRedirection(valUrl, valData)
    if (Redirectstatus === "Success") {
                errordisplay(displayId, displayText);
            }
            else {
                clearErrorDisplay(controlId, displayId);
            }

      //$.ajax({
    //    type: "POST",
    //    url: valUrl,
    //    cache: false,
    //    data: valData,
    //    success: function (data) {
    //         if (data.value == "Success") {
    //            errordisplay(displayId, displayText);
    //        }
    //        else {
    //            clearErrorDisplay(controlId, displayId);
    //        }

    //    },
    //});
}
function clearErrorDisplay(controlId, displayId) {
    $("#" + displayId).fadeOut(2000);
    $("#" + controlId).css({ "background": "#FFFFFF" });
}
function ajaxcallRedirection(valUrl, valData)
{
    var Redtstatus = '';
    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {

         
            Redtstatus = data.value;

        }
    });
      return Redtstatus;
}
function ajaxcallloaddiv(valUrl,valData,divid)
{
    debugger;
    var Redtstatus = '';
    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {
          
            $('#' + divid).html(data); 

        },
    });
}


function CheckConfiremPWD() {
    var PWD = $("#Password").val();
    $("#EmailPara").fadeIn(100);
    $("#EmailPara").empty();
    if (!PWD) {
        $("#PasswordPara").fadeIn(300);
        $("#PasswordPara").empty();
        $("#PasswordPara").append("Enter Password.");
        $("#PasswordPara").fadeOut(3000);
        $("#Password").css({ "background": "#FFCECE" });
        $("#ConfiremPWd").val("");
    }
    else if ($("#Password").val() !== $("#ConfiremPWd").val()) {
        $("#ConfirePWDPara").fadeIn(300);
        $("#ConfirePWDPara").empty();
        $("#ConfirePWDPara").append("Confirm  Password should be same as a Password .");
        $("#ConfiremPWd").css({ "background": "#FFCECE" });
        $("#ConfirePWDPara").fadeOut(3000);
        $("#ConfiremPWd").val("");
    }
    else {
        $("#ConfiremPWd").fadeIn(100);
        $("#ConfiremPWd").empty();
        $("#ConfiremPWd").css({ "background": "#FFFFFF" });
    }
}

function Validate() {
    var e = document.getElementById(""+id+"");
    var strUser = e.options[e.selectedIndex].value;

    var strUser1 = e.options[e.selectedIndex].text;
    if (strUser === 0) {
        $("#" + id).css('background-color', 'white');
        //$("#EmailPara").fadeOut(1000);
      
    }
}
function checkOTP()
{

    if ($("#txtOTP").length) {
        var data = { saleorder: $("#OTPSaleorderNo").val(), OTP: $("#txtOTP").val() };
        var url = "../OTPCreation/checkOPT";
        var result = ajaxcallRedirection(url, data);
        if (result === "Fail") {
            errordisplay('spnopt', 'Incorrect OTP');
            return false;

        }
        else {
            return true;
        }

    } else {

        return true;
    }

}

function resedotp()
{
    var message="XXXX is your's new otp";
    var data = { saleorder: $("#OTPSaleorderNo").val(), mobileno: $("#PhoneNo").val(), message: message };
    var url = "../OTPCreation/ResendOTP";
    var result = ajaxcallRedirection(url, data);
}

///--------Start: Email validation---------------------------///
function checkEmail1(id) {
   
    var inputvalue = $("#" + id).val();
    var re = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (re.test(inputvalue)) {
        $("#" + id).css('background-color', 'white');
        //$("#EmailPara").fadeOut(1000);
        $("#EmailPara").fadeIn(100);
       $("#EmailPara").empty();
        // $("#EmailPara").append("Email activation link sent to your mail id, please check.");
        //$("#EmailPara").fadeOut(12000);
        return true;
    }
    else {
        $("#EmailPara").fadeIn(300);
        //$("#EmailPara").empty();
        //$("#EmailPara").fadeIn('slow');
        $("#EmailPara").empty();     
        $("#EmailPara").append("Enter valid Email Id.");
        $("#" + id).css('background-color', "#FFCECE");
        $("#" + id).val('');
        $("#EmailPara").fadeOut(8000);
        return false;
    }
}

function filtertable(a, TableId) {


    var value = $(a).val().toLowerCase();
    $("#" + TableId + " tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
    });



}
function ajaxcallRedirectiontail(valUrl, valData) {
    var Redtstatus = '';
    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {


            Redtstatus = data.value;
            Redtstatus = data.ttdate;
            Redtstatus = data.ffdate;
        }
    });
    return Redtstatus;
}