function backtoprvpage()
{
    parent.history.back();
    return false;

}
    function readURL(input) {
        var imgul = input;
        $("#imageurl").val(imgul);
        if ($("#FileUpload1").length) {
            $("#FileUpload1").trigger("click");
        }
}

function readURLfromid(input)
{
  
    var url = $("#" + input).val();
    $("#measure").attr("src", url);

}
 function additemfromlist(Itemcode, Itemname, Unitpeice,Nofdays) {
        $("#ItemCode").val(Itemcode);
    $("#Itemdiscription").val(Itemname);
    $("#UnitPrice").val(Math.round(Unitpeice,0));
        $("#noofdays").val(Nofdays);
        $("#divmeasurDetls").show();
    $("#divAddonDetls").show();


        totalAmount();
        ValuesMainList();



    }
    function checkboxselect(a) {
        //   alert($(a).is(":checked"));
    }
    function calucation(a) {
            var quantity = $(a).closest("tr").find('input.Quantity').val();
  //  alert($(a).closest("tr").find('input.checkbox').is(":checked"));
    var meterialcost = $(a).closest("tr").find('input.MeterialCost').val();
    var ServiceCost = $(a).closest("tr").find('input.ServiceCost').val();
        var maxQty = $(a).closest("tr").find('input.maxQty').val();
        var minQty = $(a).closest("tr").find('input.minQty').val();
    var total = 0;
        debugger;
        var intqty = parseInt(quantity);
        var intmaxQty = parseInt(maxQty);
        var inminQty = parseInt(minQty);

        if (!(intqty <= intmaxQty && intqty >= inminQty))
        {

            alert('Please Enter With In The Defined Quantity Rage');
            quantity = minQty;
            $(a).closest("tr").find('input.Quantity').val(minQty);
        }
        total = parseFloat(ServiceCost) + (parseFloat(meterialcost) * parseFloat(quantity));
       $(a).closest("tr").find('input.AddonTotalAmount').val(total);
       totalAmount();
             //  alert(quantity);
}
    function totalAmount() {
            var calculated_total_sum = 0;
            $("#myAddons .AddonTotalAmount").each(function () {
                var get_textbox_value = $(this).val();
                if ($.isNumeric(get_textbox_value)) {
        calculated_total_sum += parseFloat(get_textbox_value);
    }
});
$("#AddonAmount").val(calculated_total_sum);
var total = parseFloat($("#AddonAmount").val()) + parseFloat($("#UnitPrice").val());
$("#ItemPrice").val(total);

}
    function ValuesList()
{

    var valUrl = "../Measurmentdetails/subgrouplist";
    var valData = { id: $("#dlladdonroup").val()};

    $.ajax({
        type: "POST",
        url: valUrl,
        cache: false,
        data: valData,
        async: false,
        success: function (data) {
            if (data.length !== 1) {
                var markup = "<option value='0'>Select Sub Addon</option>";
            }
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $("#dllsubgroup").html(markup).show();
        }
    });
}
function AddAddon() {
    debugger;
    //var Itemcode = $(this).closest("tr").find('span.Itemcode').text();
    //var Itemname = $(this).closest("tr").find('td.Itemname').text();
    //var Unitpeice = parseFloat($(this).closest("tr").find('td.Unitprice').text());
    var rowCount = $('#myAddons tr').length;
    var alter = $("#Alteration").val();
    var mainaddon = $("#dlladdonroup option:selected").text();
    var subaddonid = $("#dllsubgroup").val();
   
    var itemname = $("#dllsubgroup option:selected").text();
    var itemcode = $("#dllsubgroup option:selected").val();
    var mainaddonid = $("#dlladdonroup").val();
    var mainitemcode = $("#dlladdonroup").val();
    var data;
    var url;
    
  var noofoptions=  $("#dllsubgroup > option").length;
    if ((rowCount <= 2 && alter === "Yes") || alter !== "Yes") {
        if ($.trim(mainaddon) === "Select Addon") {
            alert('Please select grop');
        }
        else {
            if (noofoptions > 1) {

                if ($.trim(itemname) !== "Select Sub Addon") {
                    if ($('#myAddons tr:contains(' + itemcode + ')').length>=1)
                        {
                        alert('item is already selceted '); 
                    }
                    else {
                        data = { mainaddon: mainaddon, subaddonid: subaddonid, itemname: itemname };
                        url = "../Measurmentdetails/AddItem";
                        // alert(data);
                        ajaxcallloadtable(url, data, 'myAddons');
                        totalAmount();
                        
                    }
                }
                else {
                    alert('please select  sub group');
                }

            }
            else {

                if ($.trim(itemname) === "Select Sub Addon") {


                    subaddonid = mainaddonid;
                    itemname = mainaddon;


                }

                if ($('#myAddons tr:contains(' + mainaddonid + ')').length>=1) {
                    alert('item is already selceted ');
                } else {
                    
                    data = { mainaddon: mainaddon, subaddonid: subaddonid, itemname: itemname };
                    url = "../Measurmentdetails/AddItem";
                    // alert(data);
                    ajaxcallloadtable(url, data, 'myAddons');
                    totalAmount();
                }

            }

        
        }

    }
    else {
        alert('only one addon is allowed');
       

    }
    }
    function deleterow(a) {
    $(a).closest('tr').remove();
    totalAmount();
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function Mescheckvalidation(control)
{
    var rowCount = $('#myAddons tr').length;
    var alter = $("#Alteration").val();
    var result = required(control);
    var catgeory = $('#CategoryDetails').val();
    var Nofab =parseInt($("#NoFabraic").val());

    if (result === true) {
        if ((catgeory === 'ALTERATIONS' && Nofab === 0) || (catgeory !== 'ALTERATIONS' && Nofab > 0) ) {

            if ((rowCount > 2 || alter !== "Yes")) {
                return true;
            }
            else {
                alert('Please select atleast one addon');
                return false;

            }



        }
        else {
            alert('please check NO of Fabrics');
            return false;
        }
    }
    else {

      
        return false;

    }

}

