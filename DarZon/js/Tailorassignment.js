function GetSelectedTextValue(a) {

    var selectedText = a.options[a.selectedIndex].innerHTML;
    if (selectedText === 'Please select') {
        $(a).closest("tr").find('input.Status').val('');
    }
    else {
        $(a).closest("tr").find('input.Status').val('Assigned');
    }


}


function CheckValidation() {

    if (required('fromdate') && required('todate')) {
        return true;
    }
    else {

        return false;
    }
}
function GetSelectedmaster(a) {

    var prname = a.options[a.selectedIndex].innerHTML;

    var master = $(a).closest("tr").find('select.master');
    //alert(master.val());
    if (prname === 'Please select') {
        alert("Select Production designer");
    }
    else {


        prname = a.options[a.selectedIndex].innerHTML;
        var valUrl = '../TailorassignmentSave/masterdata/';

        var data = { prname: prname };
        var Redirectstatus = ajaxcallRedirection(valUrl, data);
        $(master).empty();
        $(Redirectstatus).each(function () {
            var $option = $("<option></option>", {
                "text": this.Value,
                "value": this.Text
            });
            //this refers to the current item being iterated over
            master.append($option);
          
           
            
        });
        //[master.selectedIndex].values = Redirectstatus;

    }


}
function getAcclocateddata() {
    if (CheckValidation() === true) {
    
        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = { fromdate: fromdate, todate: todate };
        var url = "../TailorassignmentSave/tailorAddItem";

        var Redirectstatus = ajaxcallRedirection(url, data);

        if (Redirectstatus === 'Success') {
            window.location.href = '../TailorassignmentSave/Tailorassignment';
        }

    }
}
function selectrow(e) {

    var Docnum = $(e).closest("tr").find('span.custcode').text();

    var fromdate = $("#fromdate").val();
    var todate = $("#todate").val();
    var data = { Docnum: Docnum, fromdate: fromdate, todate: todate };
    var url = "../TailorassignmentSave/ReallAddItem";
    ajaxcallloaddiv(url, data, 'divtailor');

}

function select(e) {
    var Docnum = $(e).closest("tr").find('span.custcode').text();

    var fromdate = $("#fromdate").val();
    var todate = $("#todate").val();
    var data = { Docnum: Docnum, fromdate: fromdate, todate: todate };
    var url = "../TailorassignmentSave/tailorAddItem";


    ajaxcallloaddiv(url, data, 'divtailor');

}

function reallocateddata() {
   

        var fromdate = $("#fromdate").val();
        var todate = $("#todate").val();
        var data = { fromdate: fromdate, todate: todate };
        var url = "../TailorassignmentSave/ReallAddItem";

        var Redirectstatus = ajaxcallRedirection(url, data);

    if (Redirectstatus === 'Success') {
        alert("Page is under construction!");
        window.location.href = '../TailorassignmentSave/Tailorassignment';
    }
    else {
        alert("Page is under construction!");
    }
   
}

function refreshpage() {
    window.location.href = '../Home/Home';
}