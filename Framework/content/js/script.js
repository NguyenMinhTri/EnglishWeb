$(window).on('load', function () {
    $(".fakeloader").fakeLoader({
        timeToHide: 1200,
        bgColor: "#ffffff",
        spinner: "spinner1",
        zIndex: "999999"
    });
    $(".waiting_loader").fakeLoader({
        timeToHide: 1200,
        bgColor: "rgba(0, 0, 0, 0.59)",
        spinner: "spinner1",
        zIndex: "999999"
    });
    initInput();
});

function initInput() {
    if ($("input.datetimepicker").length != 0) {
        $('input.datetimepicker').daterangepicker(
        {
            locale: {
                format: 'DD/MM/YYYY',
                "daysOfWeek": ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
                "monthNames": ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
                "firstDay": 1
            },
            singleDatePicker: true,
            showDropdowns: true,
            autoUpdateInput: false,
            "autoApply": true
        });
        $('input.datetimepicker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('DD/MM/YYYY'));
            $(this).parent().removeClass("error");
        });
        var regex = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/
        $('input.datetimepicker').keypress(function () {
            if (!regex.test($(this).val())) {
                var parent = $(this).parent();
                var errorLabel = parent.find('span.form-validation');
                if (errorLabel.length == 0) {
                    parent.prepend('<span class="form-validation"></span>');
                }
                parent.find('span.form-validation').text("Đây đâu phải ngày đâu");
                parent.addClass("error");
                $(".daterangepicker.show-calendar").css("display", "none")
            }
        })
    }
    if ($("select.selectpicker").length != 0) {
        $('select.selectpicker').selectpicker();
        $('select.selectpicker').each(function (index) {
            var selected = $(this).attr("data-value");
            if (selected != null) {
                $(this).selectpicker('val', selected.toLowerCase());
            }
        })
    }
}

function scrollto(element) {
    $('html, body').animate({
        scrollTop: $(element).offset().top - 100
    }, 500);
}

function setError(form, errorState) {
    form.find('input').parent().removeClass('error');
    $.each(errorState, function (key, value) {
        if (value.Error.length > 0) {
            var input = form.find('*[name = ' + value.Field + ']');
            var parent = $(input).closest('div')
            parent.addClass('error');
            var errorLabel = parent.find('span.form-validation');
            if (errorLabel.length == 0) {
                $(parent).prepend('<span class="form-validation"></span>');
            }
            parent.find('span.form-validation').text(value.Error[0].ErrorMessage);
        }
    });
}

function removeError(form) {
    form.find('.error').removeClass('error');
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

$(document).ready(function () {
    $(document).on('keydown', 'input', function () { $(this).parent().removeClass('error'); });
});

(function ($) {
    $.fn.serializeFormJSON = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            o[this.name] = this.value || "";
        });
        $(this).find('.push-value').each(function () {
            o[this.name] = $(this).val() || "";
        });
        return o;
    };

    $.fn.initFormData = function (args) {
        if (typeof (args) == "function") {
            $(this).on('init-form-data', args);
            return;
        }
        $(this).trigger('init-form-data', args);
    };

    $.fn.formDataSubmitDone = function (args) {
        if (typeof (args) == "function") {
            $(this).on('form-data-submit-done', args);
            return;
        }
        $(this).trigger('form-data-submit-done', args);
    };

    $.fn.formDataSubmitFail = function (args) {
        if (typeof (args) == "function") {
            $(this).on('formDataSubmitFail', args);
            return;
        }
        $(this).trigger('formDataSubmitFail', args);
    };

    jQuery.fn.insertAt = function (index, element) {
        var lastIndex = this.children().length;
        if (index < 0) {
            index = Math.max(0, lastIndex + 1 + index);
        }
        this.append(element);
        if (index < lastIndex) {
            this.children().eq(index).before(this.children().last());
        }
        return this;
    }

    $('#form-contact-us').formDataSubmitDone(function (response) {
        alert("thank you for submit, We'll contact you soon!");
    });

})(jQuery);

$(document).on("click", ".post-add-icon.upvote", function () {
    var upvote = $(this).find("span");
    var number_upvote = parseInt(upvote.text());
    var downvote = $(this).next().find("span");
    var number_downvote = parseInt(downvote.text());

    if ($(this).hasClass("clicked")) {
        $(this).removeClass("clicked");
        number_upvote--;
        $(this).parent().next().find('.upvote').removeClass("clicked");
    }
    else {
        if ($(this).next().hasClass("clicked")) {
            $(this).next().removeClass("clicked");
            number_downvote--;
        }
        $(this).addClass("clicked");
        number_upvote++;
        $(this).parent().next().find('.upvote').addClass("clicked");
        $(this).parent().next().find('.downvote').removeClass("clicked");
    }

    downvote.text(number_downvote);
    upvote.text(number_upvote);

    if (number_upvote > number_downvote) {
        $(this).addClass("active");
        $(this).next().removeClass("active");
    }
    else {
        $(this).removeClass("active");
        $(this).next().addClass("active");
    }
    scrollto("#" + $(this).parent().closest(".ui-block").attr("id"));
})
$(document).on("click", ".post-add-icon.downvote", function () {
    var downvote = $(this).find("span");
    var number_downvote = parseInt(downvote.text());
    var upvote = $(this).prev().find("span");
    var number_upvote = parseInt(upvote.text());

    if ($(this).hasClass("clicked")) {
        $(this).removeClass("clicked");
        number_downvote--;
        $(this).parent().next().find('.downvote').removeClass("clicked");
    }
    else {
        if ($(this).prev().hasClass("clicked")) {
            $(this).prev().removeClass("clicked");
            number_upvote--;
        }
        $(this).addClass("clicked");
        number_downvote++;
        $(this).parent().next().find('.downvote').addClass("clicked");
        $(this).parent().next().find('.upvote').removeClass("clicked");
    }

    downvote.text(number_downvote);
    upvote.text(number_upvote);

    if (number_downvote > number_upvote) {
        $(this).addClass("active");
        $(this).prev().removeClass("active");
    }
    else {
        $(this).removeClass("active");
        $(this).prev().addClass("active");
    }
    scrollto("#" + $(this).parent().closest(".ui-block").attr("id"));
})

$(document).on("click", ".hentry.post .post-control-button .btn-control.upvote", function () {
    $(this).parent().prev().find('.upvote').click();
})

$(document).on("click", ".hentry.post .post-control-button .btn-control.downvote", function () {
    $(this).parent().prev().find('.downvote').click();
})

$(document).on("click", ".comments-shared .post-add-icon.inline-items", function () {
    $(".comment-section").toggle("slow");
});

$(document).on("click", ".comment-section .more-comments", function () {
    $(".lazy-show.not-showed").slice(0, 2).slideDown(function () {
        $(this).removeClass("not-showed").addClass("showed");
    });
    var total_comment = parseInt($(this).parent().prev().find(".post-additional-info .comments-shared span:last-child").text());
    var current_comment = $(this).prev().find("li.showed").length + 2;
    if (current_comment == total_comment) {
        $(this).slideUp();
    }
});

