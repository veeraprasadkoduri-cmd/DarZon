function clickforgot(e) {
        var finalstsus;
       var textbox = document.getElementById("emailId");
        if (CheckValidation() === true) {
            var LoginUser =
            {
                emailId: $("#EmailId").val(),

            };
            var valUrl = '../Forgotpwd/checkvalemail/';

            var Redirectstatus = ajaxcallRedirection(valUrl, LoginUser);
            // alert(Redirectstatus);
            if (Redirectstatus === 'Success') {
                // RedirectToAction("Resetpwd", "Resetpassword");
                window.location.href = '../Resetpassword/Index';
            }
            else {
                alert('Please enter valid Email ID');
                // window.location.href = '../Resetpassword/Index';
            }



        }
   
}

