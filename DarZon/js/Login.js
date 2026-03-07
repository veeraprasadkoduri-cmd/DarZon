$(document).ready(function () {
    var finalstsus;
    var textbox = document.getElementById("UserName");
    textbox.focus();
    $("#UserName").focusout(function () {
        if (Irequired('UserName', 'SpnEmailid', 'Enter User Name')) {
            if ($.isNumeric($("#UserName").val())) {
                IsMobileNumber('UserName', 'SpnEmailid', 'Invalid Mobile Number');

            }
            else {
                var istrue = checkEmail('UserName', 'SpnEmailid', 'Invalid Email Id');
                var param = {
                    emailId: $("#UserName").val()
                }
                if (istrue === true) {

                    checkNotRegEmail('UserName', 'SpnEmailid', 'EmailID/Mobile Number Not Register', '../DALSignUpBase/NotRegisterdEmailId/', param);
                }
                else {
                    Irequired('UserName', 'SpnEmailid', 'Enter User Name');
                }
            }


        }

    });
    $("#Password").focusout(function () {
        Irequired('Password', 'spnpwd', 'Enter Password');
    });
    $("#Password").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode === 13) {
            login();
        }
    });
    $('#btnLogin').click(function () {
        login();
      });
})
function login() {
    var LoginUser =
    {
        U_EMPCODE: $("#UserName").val(),
        U_Password: $("#Password").val()

    };
    var valUrl = '../Login/LoginSubmit/';
    var Redirectstatus = ajaxcallRedirection(valUrl, LoginUser);
    if (Redirectstatus === 'Success') {
        window.location.href = '../Home/Home';
    }
    else {
        alert('Please enter valid Email or Password');
    }
}