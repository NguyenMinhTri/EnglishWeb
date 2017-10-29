var oldPost = POST;

POST = function () {
    var dayFrom = $('#DayFrom').jsonValue();
    var dayTo = $('#DayTo').jsonValue();
    var hourFrom = $('#HourFrom').jsonValue();
    var hourTo = $('#HourTo').jsonValue();

    if(dayFrom>dayTo)
    {
        var control = $('#DayFrom').parent().parent();
        control.append('<label id="-error" class="error" for="">From Day cannot greater than To Day.</label>');
        $('#DayFrom').find('button').parent().addClass('error');
        $('#DayTo').find('button').parent().addClass('error');
        return;
    }

    if (hourFrom > hourTo) {
        var control = $('#HourFrom').parent().parent();
        control.append('<label id="-error" class="error" for="">From Hour cannot greater than To Hour.</label>');
        $('#HourFrom').find('button').parent().addClass('error');
        $('#HourTo').find('button').parent().addClass('error');
        return;
    }
    oldPost();
}

function doAddSuccess(data) {
    Message("Success", "Add sucess!", "success");
    $('#bootstrap-table').bootstrapTable('refresh');
    //$('#bootstrap-table').bootstrapTable('refresh');
    ResetInput();
}


function doUpdateSuccess(data) {
    Message("Success", "Update sucess!", "success");
    var indexUpdate = $('#index-update').val();
    $('#bootstrap-table').bootstrapTable('refresh');
    ResetInput();
    $("#expand").click();
}