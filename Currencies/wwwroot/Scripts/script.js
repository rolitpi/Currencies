;
$(function () {
    var initDate = new Date($("#Date").data("kendoDatePicker").value()).toISOString();
    Request(initDate);
});
function change() {
    var newDate = this.value().toISOString();
    Request(newDate);
}

function Request(newDate) {

    var data = new FormData();
    data.append("dateString", newDate);
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", '/Home/GetCurrenciesForDate', true);
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200) {
                var currencies = JSON.parse(xhttp.response);
                var table = $("#currency-table");
                $("#currency-table tbody").html("");
                table.append("<tr><td>Название</td><td>Номинал</td><td>Символьный код</td><td>Номерной код</td><td>Значение</td></tr>");
                for (index = 0; index < currencies.length; ++index) {
                    var cur = currencies[index];
                    var info = cur["CurrencyInfo"];
                    table.append("<tr> <td>" + info["Name"] + "</td> <td>" + info["Nominal"] + "</td> <td>" + info["CharCode"] + "</td> <td>" + info["NumCode"] + "</td> <td>" + cur["Value"] + "</td> </tr>");
                }
            }
            else {
                alert(xhttp.response);
            }
        }
    };
    xhttp.send(data);
}