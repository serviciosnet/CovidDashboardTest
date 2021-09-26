function getProvidenceList() {
    var mySelect = $("#cmbProvidence")
    $.getJSON('api/CovidReport/getProvidence')
        .done(function (result) {
            console.log("Operations.js");
            console.log(result);
            $.each(result, function () {
                mySelect.append('<option value = "' + this.iso + '" > ' + this.name + '</option > ');
            });
            mySelect.val = "000";

            // load data
            getPatientData()
        })
        .fail(function () {
            
        });
}

function getPatientData() {
   //var startDate = $("#startDate");
    try {
        var cmbProvidence = $("#cmbProvidence");
        var valor = cmbProvidence.val();
       
        var startDate = $("#startDate");
        var valorFecha = startDate.val();

        var tableTitle = $("#tableTitle");

        if (valor == "000")
            tableTitle.text("Region");
        else
            tableTitle.text("Providence");

        var baseUrl = "api/CovidReport/getreport/"+valorFecha+"/"+valor
        $.getJSON(baseUrl)
            .done(function (result) {
                console.log("Operations.js");
                console.log(result);
                var row = "";
                var totalRows = rowCount(result);

                if (totalRows > 0)

                $.each(result, function (index, item) {
                    row += "<tr><td>" + item.city + "</td><td>" + new Intl.NumberFormat("en-US").format(item.confirmedCases) + "</td><td>" + new Intl.NumberFormat("en-US").format(item.activeCases) + "</td><td>" + new Intl.NumberFormat("en-US").format(item.deceasedCases) + "</td><td>" + new Intl.NumberFormat("en-US").format(item.recoveredCases) + "</td></tr > ";
                    console.log(row);
                });
                else
                    row = "<tr><td colspan = '5'> No Data Found.Please Select another date or another Region</td ></tr >";
                $("#reportDetails").html(row);
            })
            .fail(function () {

            });
    } catch (e) {
        console.error(e);
    }
    
}
function rowCount(data) {
    var count = 0;
    $.each(data, function () {
        count++;
    });
    return count;
}
function ExportCSV() {
    try {
        var cmbProvidence = $("#cmbProvidence");
        var valor = cmbProvidence.val();

        var baseUrl = "api/CovidReport/getreport/" + valorFecha + "/" + valor

        var startDate = $("#startDate");
        var valorFecha = startDate.val();
        $.getJSON
        $.post("api/ExportData/exportdatatofile",
            {
                startDate: valorFecha,
                filterId: valor,
                exportType :"csv"
            }, function (data, status) {
                alert("Status: " + status);
            });
    } catch (e) {
        console.error(e);
    }
}

