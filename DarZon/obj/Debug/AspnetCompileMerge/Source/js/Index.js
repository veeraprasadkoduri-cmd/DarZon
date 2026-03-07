


    function additemfromlist(row) {

        debugger;
        //var Itemcode = $(this).closest("tr").find('span.Itemcode').text();
        //var Itemname = $(this).closest("tr").find('td.Itemname').text();
        //var Unitpeice = parseFloat($(this).closest("tr").find('td.Unitprice').text());
        //  var category = $("#ddlFristitem option:selected").text();

       
        var check = "No";
        //$('#SaleordLineItems tr').each(function () {

         
        //    if ($(this).find('input.order').val() === Docnum) {

           
        //        check = "Yes";

        //    }

        //});
       
        //if (check === "No") {

        var Docnum = $(row).closest('tr').find('span.Itemcode').text();
        var Qty = $(row).closest('tr').find('span.Mainqty').text();
        var DocDueDate = $(row).closest('tr').find('span.deldate').text();
        var addqty = $(row).closest('tr').find('span.AddonQty').text();
        var websaleorder = $(row).closest('tr').find('span.websaleorder').text();

            var data = { Orderno: Docnum, Qty: Qty, Schedulefordelivery: DocDueDate, AddQty: addqty, websaleorder: websaleorder };

            var url = "../Transportation/AddItem";


        ajaxcallloadtable(url, data, 'SaleordLineItems');
        $(row).closest('tr').css('display', 'none');

        //} else
        //    //Verify blank value
        //    if (Docnum === "") {
        //        alert('Please select Docnumber ');
        //    }
        //    else {

        //        alert('Docnumber is allready selected');
             
        //    }
        


}
    //$('.delete').click(function () {
   
    //    $(this).closest('tr').remove();

    //});
    function deleterow(e) {

        debugger;
     
        var docnumber = $(e).closest('tr').find('input.order').val();
        $(e).closest('tr').remove();

        var tableRow = $("#tblSaleorderlist td").filter(function () {
            return $(this).text() === docnumber;
        }).closest("tr");
        tableRow.css('display', 'block');

        //$('#tblSaleorderlist tr.item').each(function () {


        //    if ($(this).find('span.Itemcode').val() === docnumber) {


        //        $(this).css('display', 'block');

        //    }

        //});
       


    }

function sendotp()
{
    var pickuser = $("#ddlFristitem option:selected").text();
    var leng = $("#SaleordLineItems tr").length;
    if (pickuser !== 'Please select') {

        if ($("#fistorder").length) {
            leng = leng - 1;
        }
        if (leng > 1) {
            $(this).attr("target", "_blank");
            $("#Savedetails").modal('show');
            if ($("#vryopt").val() === "YES") {
                var saleorderno = $("#SaleordLineItems").children('tbody').children('tr:first').find('input.websaleorder').val();
                var valUrl = "../OTPCreation/ResendOTP";
                var message = "Dear your otp is XXXX";

                var idwithname = $("#ddlFristitem option:selected").text();
                var id = idwithname.split('-');

                var data = { empid: parseInt(id[0]) };
                var url = "../Transportation/findphonenumber";
                var mobileno = ajaxcallRedirection(url, data);
                $("#PhoneNo").val(mobileno);
                var valData = { saleorder: saleorderno, mobileno: mobileno, message: message };
                var opt = ajaxcallRedirection(valUrl, valData);
                if (opt !== "Fail") {

                    valUrl = "../OTPCreation/OTP";
                    valData = { salorder: saleorderno };
                    ajaxcallloaddiv(valUrl, valData, 'trnsOTP');
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return true;

            }
        } else { return false }

    }
    else {

        alert('Select pickup user');
        return false;
    }

}

function outletselectindex()
{
    var data = { whName: $("#Outletto").val() };
    var Url ="../OrderDetails/Orederlist";
     $("#divorderlist").empty();
    ajaxcallloaddiv(Url, data, 'divorderlist');

}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


