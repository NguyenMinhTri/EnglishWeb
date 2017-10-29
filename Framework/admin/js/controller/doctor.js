$(function () {
    if (userId != null && userId != "") {
        if (updateDoctor=="False")
            $('#expand').click();
    }
});

var firstLoad = true;

var rowUpdate = -1;

table.on('load-success.bs.table', function (e, data) {
    if (!firstLoad)
        return;
    firstLoad = false;
    if (updateDoctor == "True") {
        var data = table.bootstrapTable('getData')
        $.each(data, function (key, val) {
            if (val.UserId == userId) {
                rowUpdate = key;
            }
        });
        if (rowUpdate >= 0) {
            var tr = $(table.find('tr')[rowUpdate+1]);
            tr.find('.edit').click();
        }
    }
});

function dateFormatter(value, row, index) {
    return parseJsonDate(value);
}
