function jsonObjectFromUrl(url, sucessCallBack) {
    $.ajax({
        url: url,
        type: 'get',
        cache: false,
        success: sucessCallBack
    });
};

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(input).parent().find('.image-show')
                .attr('src', e.target.result)
                .width(150);
            $(input).parent().find('.image-show').css('display', 'block');
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function getTop(element) {
    return element.offset().top;
}
function getBottom(element) {
    return getTop(element) + element.height();
}
function getMarginBottom(element) {
    var strMarginBottom = element.css('margin-bottom');
    var len = strMarginBottom.length;
    return parseInt(strMarginBottom.substring(0, len - 2));
}

function getPaddingBottom(element) {
    var strPaddingBottom = element.css('padding-bottom');
    var len = strPaddingBottom.length;
    return parseInt(strPaddingBottom.substring(0, len - 2));
}

function getMarginTop(element) {
    var strMarginBottom = element.css('margin-bottom');
    var len = strMarginBottom.length;
    return parseInt(strMarginBottom.substring(0, len - 2));
}

function getContainingHeight(element) {
    return window.innerHeight - getBottom(element) - getMarginBottom(element) - getPaddingBottom(element);
}

function between(min, p, max) {
    var result = false;
    if (min < max) {
        if (p > min && p < max) {
            result = true;
        }
    }
    if (min > max) {
        if (p > max && p < min) {
            result = true;
        }
    }
    if (p === min || p === max) {
        result = true;
    }
    return result;
}

function point_in_rectagnle(x, y, left, top, right, bottom) {
    var result = false;
    if (between(left, x, right) && between(top, y, bottom)) {
        result = true;
    }
    return result;
}

$.fn.serializeObject = function () {
    var data = {};
    var inputs = $(this).find('.form-input-group');
    $.each(inputs, function (key, val) {

        if (!($(val).hasClass('disable-create')))
            data[$(val).attr('name')] = $(val).jsonValue();
    });
    return data;
};

function jsonTextField(val) {
    if (val == null) {
        return $(this).find('input').val();
    }
    $(this).find('input').val(val);
}

function jsonDateField(val) {
    if (val == null) {
        return $(this).find('input').val();
    }
    $(this).find('input').val(moment(val).format('MM/DD/YYYY'));
}

function jsonVideoField(val) {
    if (val == null) {
        return $(this).find('input').val();
    }

    $($(this).find('input')[0]).val(val);

    $($(this).find('input')[1]).val("https://www.youtube.com/watch?v=" + val);

    var videoLink = "https://www.youtube.com/embed/" + val + "?autoplay=1";
    var videoElem = $($(this).find('iframe')[0]);
    videoElem.attr('src', videoLink);
    videoElem.css('display', 'block');

}




function jsonSwitch(val) {
    if (val == null) {
        return $(this).find('span').offset().left - $(this).find('span').parent().offset().left > -10;
    }
    var curValue = $(this).jsonValue();
    if (curValue != val) {
        $(this).find('.onoffswitch-label').click();
        var switchData = $(this).find('.switch-data');

        if (val) {
            switchData.addClass('switch-on');
        }
        else {
            switchData.removeClass('switch-on');
        }
    }



    $(this).find('input').val(val);
}

function jsonRadioButton(val) {
    if (val == null) {
        return parseInt($(this).find('.checked').attr('value'));
    }

    var options = $(this).find('.my-radiobutton');

    $.each(options, function (key, obj) {
        if ($(obj).attr('value') == val) {
            $(obj).click();
            return;
        }
    });

}

function jsonTextArea(val) {
    if (val == null) {
        return tinymce.get($(this).attr('name') + 'txt').getContent();
    }
    tinymce.get($(this).attr('name') + 'txt').setContent(val);

}

function jsonNumberField(val) {
    if (val == null) {
        return parseFloat($(this).find('input').val());
    }
    $(this).find('input').val(val);
}

function jsonCombobox(val) {
    if (val == null) {
        var selectElement = $(this).find('select')[0];
        var optionElements = $(selectElement).find('option');
        var index = parseInt($($(this).find('.selected')[0]).attr('data-original-index'));
        return $(optionElements[index]).attr('value');
    }

    $(this).find('.selectpicker').val(val);
    $('.selectpicker').selectpicker('refresh');

}

function jsonImageFile(val) {
    if (val == null) {
        return $(this).find('.value-image').val();
        //return srcImageArray($(this).find('.img-show-demo')).join(';');
        //return $(this).find('.img-show-demo').attr('src');
    }

    $(this).find('.value-image').val(val);
    $(this).find('.image-show').attr('src', val);
    $(this).find('.image-show').css('display', 'block');

    // $(this).find('.file-show').text(val);

    //var images = val.split(";");

    //var container = $(this);

    //$.each(images, function (key, image) {
    //    var addImage = container.find('.add-image');

    //    addImage.text(image.substring(image.lastIndexOf('/') + 1));
    //    addImage.removeClass('add-image');
    //    addImage.addClass('active');
    //    addImage.parent().append('<div class="file-show add-image">Thêm hình ảnh</div>');
    //    $('.add-image').click(function (e) {
    //        container.parent().parent().find('.imageInput').click();
    //    });
    //    $(addImage).unbind("click");
    //    $(addImage).click(function (e) {
    //        var images = addImage.parent().parent().parent().parent().find('.img-content-group').find('.img-show-demo');
    //        $(images[$('.file-show.active').index(addImage[0])]).remove();
    //        $(addImage).remove();
    //    });
    //    var imgStr = '<img class="img-show-demo" src="' + image + '" alt="' + image.substring(image.lastIndexOf('/') + 1) + '" title="' + image.substring(image.lastIndexOf('/') + 1) + '"  />';
    //    container.find('.img-content-group').append(imgStr);

    //});

    //container.find('.add-image').click(function (e) {
    //    var images = addImage.parent().parent().parent().parent().find('.img-content-group').find('.img-show-demo');
    //    $(images[$('.file-show.active').index(addImage[0])]).remove();
    //    $(addImage).remove();
    //});
}

$.fn.jsonValue = function (val) {
    if (val == null) {
        if ($(this).attr('jsonvalue') != null)
            return window[$(this).attr('jsonvalue')].call(this);
        return '';
    }
    else {
        window[$(this).attr('jsonvalue')].call(this, val);
    }
}

$.fn.element = function (id, val) {
    if (val == null) {
        return $(this).find('#' + id);
    }
    $(this).find('#' + id).jsonValue(val);
}



$(function () {

    var formSubmitSuccess = [];

    $.each($('.my-form'), function (key, val) {
        $(val).find('.submit-form').click(function (e) {
            e.preventDefault();
            var url = $(val).attr('url-post');
            $.post(url, $(val).serializeObject(), function (response) {
                if (response.result === "success") {
                    if (formSubmitSuccess[$(val).attr('id')] != null)
                        formSubmitSuccess[$(val).attr('id')](response);
                }
            });
        });

    });

    $('.add-image').click(function (e) {
        $(this).parent().parent().find('.imageInput').click();
    });

});


function srcImageArray(className) {
    return $(className).map(function () {
        return $.trim($(this).attr('src'));
    }).get();
}