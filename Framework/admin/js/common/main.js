

var oldImages = [];
var oldImagesIndex = 0;

function uploadImage(imageElem, error, success) {
    var imageId = $(imageElem).attr('id');
    var formData = new FormData();
    var totalFiles = document.getElementById(imageId).files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById(imageId).files[i];
        formData.append(imageId, file);
    }
    var addImage = $(this).parent().find('.add-image');

    $.ajax({
        type: "POST",
        url: 'Resource/Upload',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.result === "success") {
                $(imageElem).parent().find('.value-image').val(response.name);
                if (success != null)
                    success();
            }
            else {
                error++;
                Message("Fail", "image is not valid", "danger");
                $(imageElem).addClass('error');
            }
        },
        error: function (error) {
            Message("Fail", "image is not valid", "danger");
            $(imageElem).addClass('error');
            error++;
        }
    });
    return error;
}


function POST() {
    var form = $('#form');
    var images = form.find('.image-input-group');
    if (images.length > 0) {
        $.each(images, function (key, val) {
            var error = 0;
            if (key == images.length - 1) {
                error = uploadImage($(val), error, function () {
                    doAdd(form);
                });
            }
            else {
                error = uploadImage($(val), error);
            }

            if (error > 0) {
                $(val).parent().addClass('error');
                return;
            }
        });
        return;
    }

    doAdd(form);
   
}
function doAddSuccess(data) {
    Message("Success", "Add sucess!", "success");
    $('#bootstrap-table').bootstrapTable('append',data);
    //$('#bootstrap-table').bootstrapTable('refresh');
    ResetInput();
}

function doAdd(form) {
    var data = form.serializeObject();
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
            doAddSuccess(data);
        },
        error: function () {
            Message("Fail", "Fail!", "danger");
        }
    });
}

function doUpdateSuccess(data) {
    Message("Success", "Update sucess!", "success");
    var indexUpdate = $('#index-update').val();
    $('#bootstrap-table').bootstrapTable('updateRow', { index: indexUpdate, row: data.data });
    ResetInput();
    $("#expand").click();
}

function doUpdate() {
    var data = $("#form").serializeObject();
    $.post(updateUrl, data).done(function (reponse) {
        // $('#bootstrap-table').bootstrapTable('refresh');
        if (reponse.result == "failed") {
            Message("Fail", reponse.message, "danger");
            return;
        }
        doUpdateSuccess(reponse);
    }).fail(function (error) {
        Message("Fail", "Fail!", "danger");
    });
}

function doDelete(data,index) {

    var deleteViewData = { Id: data.Id };

    $.post(deleteUrl, deleteViewData).done(function (data) {
    if (data.result == "failed") {
            Message("Fail", data.message, "danger");
            return;
        }
        Message("Success", "Delete sucess!", "success");
        data.Status = false;
        $('#bootstrap-table').bootstrapTable('updateRow', { index: index, row: data });
        // $('#bootstrap-table').bootstrapTable('refresh');
        ResetInput();
    }).fail(function (error) {
        Message("Fail", "Fail!", "danger");
    });
}

function PUT() {

    oldImagesIndex = 0;
    var images = $('#form').find('.image-input-group');
    if (images.length > 0) {
        $.each(images, function (key, val) {

            var valueImageElem = $(val).parent().find('.value-image');


            var error = 0;
            if (key == images.length - 1) {
                if (oldImages[oldImagesIndex][1]) {
                    error = uploadImage($(val), error, function () {
                        doUpdate();
                        if(error==0)
                            $.post('Resource/DeleteFile', { FileName: oldImages[oldImagesIndex++][0] });
                    });
                }
                else {
                    doUpdate();
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



}


function DELETE(data, index) {
    if (data.Status == false) {
        Message("Fail", "Row already deleted!", "danger");
        return;
    }
    doDelete(data, index);

}


$().ready(function () {

    $("#expand").click(function () {
        if ($(this).find('span').text().indexOf("Add") != -1) {
            $('#hidden-when-add').css('display', 'none');
        }
        else {
            $('#hidden-when-add').css('display', 'block');
        }
    });

    InitControl();
    setNavigation();
    $(".navbar-brand").text(document.title);
    $('#form').validate();
});



function InitControl() {
    $("#add").click(function (e) {
        $('.hidden-when-update').css('display', 'block');
        e.preventDefault();
        if ($('#form').valid()) {
            if ($("#add").prop("name") == "POST") {
                POST();
            } else if ($("#add").prop("name") == "PUT") {
                PUT();
            }
        }
    });
    $("#clear").click(function (e) {
        e.preventDefault();
        ResetInput();
    });
}

(function ($) {
    $.fn.serializeFormJSON = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = "'" + [o[this.name]] + "'";
                }
                o[this.name].push(this.value || "'");
            } else {
                o[this.name] = this.value || "'";
            }
        });
        $(this).find("input[type = file]").each(function (i, obj) {
            let
			file = obj.files[0];
            if (file != null) {
                o[this.name] = file.name;
            } else {
                o[this.name] = "avatar1.jpg";
            }
        });
        return o;
    };
})(jQuery);

function ResetInput() {
    $('#form').each(function () {
        this.reset();
    });
    $("#add").prop("name", "POST");
    $("#add span span").text("ADD");
    $(".panel-title span").text("Add a new ... ")
    $("[id$='-error']").remove();
    $('#form').find(".error").removeClass("error");

    $('.video-control iframe').css('display', 'none')
    $('.video-control iframe').attr('src', '')

    $('#form .image-show').css('display', 'none');

    $('#hidden-when-add').css('display', 'none');
    //if ($(this).find('span').text().indexOf("Add") != -1) {
        
    //}
    //else {
    //    $('#hidden-when-add').css('display', 'block');
    //}
}

var imageElems = $('#form .image-input-group');

$.each (imageElems,function (key,val) {
    var imageElem = val;
    $(val).change(function () {
        oldImages[key][1] = true;
    });
})

function Update(data,index) {

    if (!$("#collapse").hasClass("in")) {
        $("#expand").click();
    }
    $('#hidden-when-add').css('display', 'block');
    $('.hidden-when-update').css('display', 'none');
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

function Message(title, message, type) {
    $.notify({
        animate: {
            enter: 'animated bounceInDown',
            exit: 'animated bounceOutUp'
        },
        title: '<strong>' + title + '!</strong>',
        message: message
    }, {
        type: type,
        timer: 1000,
        placement: {
            from: 'top',
            align: 'center'
        }
    });
}

function setNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path).substring(path.length, path.lastIndexOf("/") + 1);

    $(".nav a").each(function () {
        var href = $(this).attr('href');
        if (path.substring(0, href.length) === href) {
            $(this).closest('li').addClass('active');
            $(this).closest('li').parent().closest('li').addClass('active');
            $(this).closest('li').parent().closest('li').find('a').click();
        }
    });
}