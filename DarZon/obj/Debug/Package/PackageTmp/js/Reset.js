function clickreset(e) {

  
    if (required("Password") === true && required("ConfiremPWd") === true) {
        var finalstsus;
        var textbox = document.getElementById("Password");
        //   var textbox1 = document.getElementById("txtpwd2");
        textbox.focus();
        //if (CheckValidation() === true) {
        var LoginUser =
        {
            newpwd: $("#Password").val(),



        };
        var valUrl = '../Resetpassword/Resetpwd/';

        var Redirectstatus = ajaxcallRedirection(valUrl, LoginUser);

        if (Redirectstatus === 'Success') {
            // RedirectToAction("Resetpwd", "Resetpassword");
            //  window.location.href = '../Resetpassword/Index';
            alert('Your password Sucessfully changed');
            window.location.href = '../Login/Login';
            document.getElementById('Password').value = "";
            document.getElementById('ConfiremPWd').value = "";
        }
        else {
            alert('failed');
            document.getElementById('Password').value = "";
            document.getElementById('ConfiremPWd').value = "";
            //  window.location.href = '../Resetpassword/Index';
        }
        //  }
    }

}