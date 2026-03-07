
    $(document).ready(function () {



    });
    function additemfromlist(Docnum, Qty, DocDueDate) {

        //var Itemcode = $(this).closest("tr").find('span.Itemcode').text();
        //var Itemname = $(this).closest("tr").find('td.Itemname').text();
        //var Unitpeice = parseFloat($(this).closest("tr").find('td.Unitprice').text());
        //  var category = $("#ddlFristitem option:selected").text();
     
        var data = { Orderno: Docnum, Qty: Qty, Schedulefordelivery: DocDueDate };
  
    var url = "../Transportation/AddItem";


    ajaxcallloadtable(url, data, 'SaleordLineItems');



}
    $('.delete').click(function () {
   
        $(this).closest('tr').remove();

    });
    function deleterow(e) {
     
        $(e).closest('tr').remove();
    }



        $(document).ready(function () {

            $(".jquery-datepicker").datepicker({
                dateFormat: 'dd/mm/yy',
                numberOfMonths: 1,
                minDate: new Date,

            });
        });

