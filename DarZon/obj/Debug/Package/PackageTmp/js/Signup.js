$(document).ready(function () {
   
    var textbox = document.getElementById("Name");
    textbox.focus();
    $("#Name").focusout(function () {
        Irequired('Name', 'spnpwd', 'Enter Username');
    });
    $("#Password").focusout(function () {
        Irequired('Password', 'spnpwd', 'Enter Password');
    });
    $("#EmailId").focusout(function () {
        Irequired('EmailId', 'spnpwd', 'Enter Emailid');
    });
    $('#btnLogin').click
        (function () {
        var finalstsus;
       
            if (CheckValidation() === true) {
                var LoginUser =
                {
                    Email: $("#EmailId").val(),
                    UserName: $("#Name").val(),
                    Password: $("#Password").val(),

                };
                var valUrl = '../Signup/Regform/';

                var Redirectstatus = ajaxcallRedirection(valUrl, LoginUser);

                if (Redirectstatus === 'Success') {
                  
                    alert('Your Registration Sucessfully');
                    window.location.href = '../Login/Login';
                    document.getElementById('Name').value = "";
                    document.getElementById('EmailId').value = "";
                    document.getElementById('Password').value = "";
                    document.getElementById('ConfiremPWd').value = "";
                   
                }
                else {
                    alert('Username or Email Id already Registerd');
                    //  window.location.href = '../Resetpassword/Index';
                }

            }
        //$.ajax({
        //    type: "POST",
        //    url: "../Login/LoginSubmit/",
        //    async: false,
        //    data: '{LoginUser: ' + JSON.stringify(LoginUser) + '}',
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",

        //    error: function (xhr, status, error) {
        //        alert(error);

        //    },
        //    success: function (json) {
        //        //alert('Success')
        //        window.location.href = '../ProductListView/ProductListView';
        //    }
        //})



    });
});

