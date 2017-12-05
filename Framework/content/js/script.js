﻿$(window).on('load', function () {
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
    $(this).parent().parent().parent().parent().find(".comment-section").toggle('fast');
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

$(document).on('keyup', 'textarea#comment', function (e) {
    if ($(this).val().replace(/\r?\n/g, "").length != 0) {
        $(this).parent().find(".options-message#submit").addClass("active");
    }
    else {
        $(this).parent().find(".options-message#submit").removeClass("active");
    }
    if (e.which == 13 || e.keyCode == 13) {
        $(this).parent().find(".options-message#submit.active").click();
    }
});

$(document).on("click", "#submit.options-message.active", function () {
    var parent = $(this).parent().parent().parent();
    var comment_section = parent.parent().find(".comments-list");
    var textarea = $(this).parent().parent().find("textarea#comment");
    var total = parent.parent().parent().find(".comments-shared span:last-child");
    var comment = textarea.val();
    var button = $(this);
    var new_comment;
    if (comment.length != 0) {
        var data = {
            Comment: comment,

        };
        if (!$(this).hasClass("child-comment")) {
            $.post("/Home/Comment", data).done(function (html) {
                $(comment_section).append(html);
                textarea.val("");
                button.removeClass("active");
                total.text($(comment_section).find("li").length);
                new_comment = $(comment_section).find("li.new-comment");
                new_comment.find(".author-date time").attr("data-time", Date.now);
                $(comment_section).find("li.new-comment").show('slow').addClass("showed");
            }).fail(function (response) {
                $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
                $("#notify-button").click();
            })
        }
        else {
            var index = $(this).attr("class").match(/(?:\s|^)child-comment-(\d+)/)[1];
            var parentcomment_section = $(comment_section).find("li.parent")[index];
            var childcomment_section = $(parentcomment_section).find("ul.children");
            $.post("/Home/ChildComment", data).done(function (html) {
                $(childcomment_section).append(html);
                textarea.val("");
                new_comment = $(comment_section).find("li.new-comment");
                new_comment.find(".author-date time").attr("data-time", Date.now);
                button.removeClass("active").removeClass(function (index, className) {
                    return (className.match(/(^|\s)child-comment-\S+/g) || []).join(' ');
                }).removeClass("child-comment");
                total.text($(comment_section).find("li").length);
                $(childcomment_section).find("li.new-comment").show('slow').addClass("showed");
                scrollto(parentcomment_section);
            }).fail(function (response) {
                $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
                $("#notify-button").click();
            })
        }
    }
});

$(document).on("click", ".comments-list a.reply", function () {
    var parent = $(this).parent().parent().parent();
    var textarea = parent.find("textarea#comment");
    var arr = Array.prototype.slice.call(parent.find("li.parent"));
    var index = arr.indexOf($(this).parent()[0]);
    var button = parent.find(".options-message#submit").removeClass (function (index, className) {
        return (className.match(/(^|\s)child-comment-\S+/g) || []).join(' ');
    }).addClass("child-comment child-comment-" + index);
    scrollto(textarea);
    $(this).parent().addClass("has-children");
    textarea.focus();
});

function ms2Time(ms) {
    var secs = ms / 1000;
    ms = Math.floor(ms % 1000);
    var minutes = secs / 60;
    secs = Math.floor(secs % 60);
    var hours = minutes / 60;
    minutes = Math.floor(minutes % 60);
    hours = Math.floor(hours % 24);
    var string = "";
    if (hours == 0 && minutes == 0) {
        string += secs + " giây "
    }
    else {
        if (hours == 0 && minutes != 0) {
            if (minutes != 0) {
                string += minutes + " phút "
            }
            if (secs != 0) {
                string += secs + " giây "
            }
        }
        else {
            if (hours != 0) {
                string += hours + " giờ "
            }
            if (minutes != 0) {
                string += minutes + " phút "
            }
        }
    }
    return string + " trước";
}

function TimeStamp() {
    var end = Date.now();
    var start;
    $(".author-date time").each(function () {
        var time = parseInt($(this).attr("data-time"));
        start = new Date(time);
        $(this).text(ms2Time(end - start));
    });
}

$(document).ready(function () {
    TimeStamp();
    window.setInterval(function () {
        TimeStamp();
    }, 1000);
});

$(document).on("click", ".news-feed-form .form-group .selection .togglebutton label input[type=checkbox]", function () {
    if ($(this).is(':checked')) {
        $(".news-feed-form .form-group .selection p").text("Hỏi thầy cô cho chắc").css("color", "#fe5e3a");
        $(this).parent().parent().attr("title", "Tắt để hỏi mọi người nha!").attr("data-original-title", "Tắt để hỏi mọi người nha!");
        $("input[name='Type']").val("1");
    }
    else {
        $(".news-feed-form .form-group .selection p").text("Hỏi tất cả mọi người").css("color", "#888da8");
        $(this).parent().parent().attr("title", "Bật để hỏi Thầy cô nha!").attr("data-original-title", "Bật để hỏi Thầy cô nha!");
        $("input[name='Type']").val("0");
    }
});
