/* measured image place */
//function readURL(input) {
//  if (input.files && input.files[0]) {
//    var reader = new FileReader();

//    reader.onload = function (e) {
//      $('#measure')
//        .attr('src', e.target.result);
//    };
//    reader.readAsDataURL(input.files[0]);
//  }
//}
/* end of measured image place */


$('.newbtn').bind("click", function () {
    $('#pic').click();
});


$(document).ready(function () {

    if ($("#itemstatus").val() === "Add") {
        $("#divmeasurDetls").hide();
        $("#divAddonDetls").hide();
    }


    var itemcost = 0;
    var addcost = 0;
    var totalamt = 0;
    if ($.isNumeric($("#UnitPrice").val())) {
        itemcost = parseFloat($("#UnitPrice").val());

    }
    if ($.isNumeric($("#ItemPrice").val())) {
        totalamt = $("#ItemPrice").val();

    }
    addcost = totalamt - itemcost;
    $("#AddonAmount").val(addcost);
     $("#Invoicedivprint").hide();
        $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
       

    });
    $("#starttime").timepicker();
    $("#Endtime").timepicker();
    $("#btnprcepay").show();
    $("#btnMdfOrder").hide();

    if ($("#hdnStatus").val() !== 'C') {

        $("#paymentdetails").hide();
        $("#divsave").hide();
    }
    else {

        //$("#divsalhed").children().prop('disabled', true);
        $("input", "#divsalhed").attr('readonly', true);

        //$("#divsaldet").children().prop('disabled', true);
        $("input", "#divsaldet").attr('readonly', true);
    }
    if ($("#hdnStatus").val() === 'O') {

        $("#divrpoceed").hide();

    }
    if ($("#hdnStatus").val() === 'R') {
        $("#hdnStatus").val('O');
    }



});






