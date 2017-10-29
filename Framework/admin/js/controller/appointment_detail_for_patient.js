


operateFormatter = function (value, row, index) {
    return [
         '<div class="table-icons">',
         '<a rel="tooltip" title="Delete" class="btn btn-simple btn-danger btn-icon table-action remove" href="javascript:void(0)">',
         '<i class="ti-close"></i>', '</a>', '</div>'].join('');
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
        $('#bootstrap-table').bootstrapTable('refresh');
        // $('#bootstrap-table').bootstrapTable('refresh');
        ResetInput();
    }).fail(function (error) {
        Message("Fail", "Fail!", "danger");
    });
}