function doAdd(form) {
    var data = form.serializeObject();
    data.Id = 0;
    var curUrl = window.location.href;
    data.UserId = curUrl.substring(curUrl.indexOf("=") + 1);
    $.ajax({
        url: postUrl,
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (data) {
            if (data.result == "failed") {
                Message("Fail", data.message, "danger");
                return;
            }
            Message("Success", "Add sucess!", "success");
            $('#bootstrap-table').bootstrapTable('refresh');
            ResetInput();
            refreshSelectPicker($('#RoleId'));
        },
        error: function () {
            Message("Fail", "Fail!", "danger");
        }
    });
}

function doUpdateSuccess(data) {
    Message("Success", "Update sucess!", "success");
    var indexUpdate = $('#index-update').val();
    $('#bootstrap-table').bootstrapTable('refresh');
    ResetInput();
    $("#expand").click();
    refreshSelectPicker($('#RoleId'));
}

function doDelete(data, index) {
    var deleteViewData = { Id: data.Id };
    $.post(deleteUrl, deleteViewData).done(function (data) {
        if (data.result == "failed") {
            Message("Fail", data.message, "danger");
            return;
        }
        Message("Success", "Delete sucess!", "success");
        data.Status = false;
        $('#bootstrap-table').bootstrapTable('remove', { field: 'Id', values: [data.Id] });
        refreshSelectPicker($('#RoleId'));
        ResetInput();
    }).fail(function (error) {
        Message("Fail", "Fail!", "danger");
    });
}