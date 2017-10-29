
var oldImages = [];
var oldImagesIndex = 0;

$("#add").prop("name", "PUT");
$("#add span span").text("UPDATE");
$(".panel-title span").text("Update a record ... ")

POST = function () {
    
}

doUpdate = function () {
    var data = $("#form").serializeObject();
    $.post(updateUrl, data).done(function (response) {
        Message("Success", "Update sucess!", "success");


        //$('#bootstrap-table').bootstrapTable('refresh');
        var indexUpdate = $('#index-update').val();
        $('#bootstrap-table').bootstrapTable('updateRow', { index: indexUpdate, row: response.data });

        ResetInput();
        $("#expand").click();
    }).fail(function (error) {
        Message("Fail", "Fail!", "danger");
    });
}

PUT = function () {

    oldImagesIndex = 0;
    var images = $('#form').find('.image-input-group');
    if (images.length > 0) {
        $.each(images, function (key, val) {

            var valueImageElem = $(val).parent().find('.value-image');


            var error = 0;
            if (key == images.length - 1) {
                if (oldImages[oldImagesIndex][1]) {
                    error = uploadImage($(val), error, function () {
                        if (error == 0)
                            $.post('Resource/DeleteFile', { FileName: oldImages[oldImagesIndex++][0] });
                        doUpdate();
                        
                        return;
                    });
                }
                else {
                    doUpdate();
                   
                    return;
                }
            }
            else {
                if (oldImages[oldImagesIndex][1]) {
                    error = uploadImage($(val), error);
                    if (error == 0)
                        $.post('Resource/DeleteFile', { FileName: oldImages[oldImagesIndex++][0] });
                }
            }

            if (error > 0) {
                $(val).parent().addClass('error');
                return;
            }
        });
        return;
    }


    doUpdate();
    return;


}

DELETE = function (data) {

}



function ResetInput() {
    $('#form').each(function () {
        this.reset();
    });
    $("#add").prop("name", "PUT");
    $("#add span span").text("Update");
    $(".panel-title span").text("Update a record ... ")
    $("[id$='-error']").remove();
    $('#form').find(".error").removeClass("error");

    $('.video-control iframe').css('display', 'none')
    $('.video-control iframe').attr('src', '')
    $('#form .image-show').css('display', 'none');

    if ($(this).find('span').text().indexOf("Add") != -1) {
        $('#hidden-when-add').css('display', 'none');
    }
    else {
        $('#hidden-when-add').css('display', 'block');
    }
}

var imageElems = $('#form .image-input-group');

function Update(data, index) {

    if (!$("#collapse").hasClass("in")) {
        $("#expand").click();
    }
    $('#hidden-when-add').css('display', 'block');
    $("#add").prop("name", "PUT");
    $("#add span span").text("UPDATE");
    $(".panel-title span").text("Update a record ... ")
    $(".main-panel").animate({ scrollTop: 0 }, 500);
    $(".main-panel").perfectScrollbar('update');

    $(".form-input-group").each(function () {
        name = $(this).attr('name');
        //value = data[name];
        $(this).jsonValue(data[name]);
    });

    $('#index-update').val(index);
    images = $('#form').find('.value-image');

    oldImagesIndex = 0;

    $.each(images, function (key, val) {
        oldImages[oldImagesIndex++] = [$(val).val(), false];
    });
}



operateFormatter = function (value, row, index) {
    return [
			'<div class="table-icons">',
			'<a rel="tooltip" title="Update" class="btn btn-simple btn-warning btn-icon table-action edit" href="javascript:void(0)">',
			'<i class="ti-pencil-alt"></i>',
			'</a>'].join('');
}