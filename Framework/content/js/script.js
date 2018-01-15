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
    $(document).on('keydown', ".w-search input[name='keyword'], .w-search-1 input[name='keyword']", function () {
        if ($(this).val() === "") {
            $(this).addClass("error");
            $(this).next().css("cursor", "not-allowed");
        }
        else {
            $(this).removeClass("error");
            $(this).attr("placeholder", "Tìm kiếm bạn...");
            $(this).next().css("cursor", "pointer");
        }
    });
    $(".w-search, .w-search-1").submit(function () {
        var input = $(this).find("input[name='keyword']");
        if ($(input).val() === "") {
            $(input).attr("placeholder", "Hãy nhập từ khóa...");
            return false;
        }
    });
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

$(document).on("click", ".comments-shared .post-add-icon.inline-items", function () {
    var comment_section = $(this).parent().parent().parent().parent().find(".comment-section");
    comment_section.toggle('fast', function () {
        if ($(comment_section).css("display") == "none") {
            $(comment_section).prev().css("border-radius", "5px");
        }
        else {
            $(comment_section).prev().css("border-radius", "5px 5px 0 0");
        }
    });
});

$(document).on("click", ".comment-section .more-comments", function () {
    $(this).parent().find(".lazy-show.not-showed").slice(0, 2).slideDown(function () {
        $(this).removeClass("not-showed").addClass("showed");
    });
    var total_comment = parseInt($(this).parent().prev().find(".post-additional-info .comments-shared span:last-child").text());
    var current_comment = $(this).prev().find("li.showed").length + 2;
    if (current_comment >= total_comment) {
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

$(document).on("click", ".comments-list a.reply", function () {
    var parent = $(this).parent().parent().parent();
    var textarea = parent.find("textarea#comment");
    var arr = Array.prototype.slice.call(parent.find("li.parent"));
    var index = arr.indexOf($(this).parent()[0]);
    var button = parent.find(".options-message#submit").removeClass(function (index, className) {
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
    if (num_notirequest != 0) {
        $(".notirequest").text(num_notirequest);
        $(".notirequest").css("display", "table-cell");
        $(".empty-notirequest").css("display", "none");
    }
    else {
        $(".notirequest").css("display", "none");
        $(".empty-notirequest").css("display", "block");
    }

    if (num_notification != 0) {
        $(".notification").text(num_notification);
        $(".notification").css("display", "table-cell");
        $(".empty-notify").css("display", "none");
    }
    else {
        $(".notification").css("display", "none");
        $(".empty-notify").css("display", "block");
    }
});

$(document).on("click", ".news-feed-form .form-group .selection .togglebutton label input[type=checkbox]", function () {
    if ($(this).is(':checked')) {
        $(".news-feed-form .form-group .selection p").text("Hỏi chuyên gia cho chắc").css("color", "#fe5e3a");
        $(this).parent().parent().attr("title", "Tắt để hỏi mọi người nha!").attr("data-original-title", "Tắt để hỏi mọi người nha!");
        $("input[name='Option']").val("1");
        $(".news-feed-form #home-1 .post-control-button").fadeIn();
    }
    else {
        $(".news-feed-form .form-group .selection p").text("Hỏi tất cả mọi người").css("color", "#888da8");
        $(this).parent().parent().attr("title", "Bật để hỏi chuyên gia nha!").attr("data-original-title", "Bật để hỏi chuyên gia nha!");
        $("input[name='Option']").val("0");
        $(".news-feed-form #home-1 .post-control-button").fadeOut();
    }
});

$(document).on("click", "#question-form input[type='submit']", function (e) {
    e.preventDefault();

    var comment = $("#question-form textarea").val();
    if (comment.length != 0) {
        $(".waiting_loader").css("display", "block");

        var form = $(this).closest("form");
        $(form).find("input[name='DatePost']").val(Date.now);
        var data = form.serialize();

        $.post('/Home/Post', data).done(function (html) {
            $("#partial-question").prepend(html);
            $("#question-form textarea").val("");
        }).fail(function (response) {
            $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!")
            $("#notify-button").click();
        }).always(function () {
            $(".waiting_loader").css("display", "none");
        });
    }
});

$(document).on('keyup', "#question-form textarea", function (e) {
    if ($(this).val().replace(/\r?\n/g, "").length != 0) {
        $("#question-form input[type='submit']").addClass("active");
    }
    else {
        $("#question-form input[type='submit']").removeClass("active");
    }
});

$("input[name='keyword']").keydown(function () {
    if ($("input[name='keyword']").val() === "") {
        $(this).addClass("error");
        $(".w-search button[type='submit']").css("cursor", "not-allowed");
    }
    else {
        $(this).removeClass("error");
        $(this).attr("placeholder", "Tìm kiếm bạn...");
        $(".w-search button[type='submit']").css("cursor", "pointer");
    }
});
$(".w-search").submit(function () {
    if ($("input[name='keyword']").val() === "") {
        $("input[name='keyword']").attr("placeholder", "Hãy nhập từ khóa...");
        return false;
    }
});

$(document).on("click", ".add_friend", function () {
    $("button#hidden").click($._data($(".search-friend a.js-sidebar-open").get(0), "events").click["0"].handler);
    var reject = false;
    var user = $("#RequestUserName").val();
    var user_ele = $($("ul.notification-list.friend-requests li[data-email='" + user + "']")[0]);

    if (user_ele.find(".notification-icon .accept-request.friend").length==2) {
        reject = true;
    }
    var data = {
        Id_User: $("#UserId").val(),
        Id_Friend: $(this).attr("data-id"),
        CodeRelationshipId: $(this).attr("data-code"),
        Name: $("#RequestUserName").val(),
        Reject: reject
    }
    $.post('/Friend/Friend_action', data).done(function (response) {
        if (response.result == "success") {
            dataSentNotiRequest = data;
            $("#sentNotiRequest").click();
            var parent = $(".request_" + response.id);
            var email = parent.attr("data-email");
            var name = parent.attr("data-name");
            if (data.CodeRelationshipId == 0) {
                if (reject == false) {
                    $("#delete-ok-button").click();
                }
                $(".profile-section .friend").removeClass("bg-primary").addClass("bg-blue");
                $(".profile-section .friend.delete, .profile-section .friend.accept").css("display", "none");
                $(".profile-section .friend.add").css("display", "inline-block");
                $(parent).fadeOut();
            }
            else if (data.CodeRelationshipId == 1) {
                $("#accept-ok-button").click();
                $(".profile-section .friend").removeClass("bg-yellow").addClass("bg-primary");
                $(".profile-section .friend.delete").css("display", "inline-block");
                $(".profile-section .friend.accept").css("display", "none");
                parent.addClass("accepted");
                $(parent).find(".notification-event").html("<p>Bạn và <a class='h6 notification-friend'>" + name + "</a> vừa trở thành bạn của nhau. Cùng xem tường nhà <a href='Profile?username=" + email + "&option=newsfeed' class='notification-link'>" + name + "</a>.</p>");
                $(parent).find(".notification-icon").html("<svg class='olymp-happy-face-icon'><use xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='content/icons/icons.svg#olymp-happy-face-icon'></use></svg>");
            } else if (data.CodeRelationshipId == -1) {
                $("#add-ok-button").click();
                $(".profile-section .friend").removeClass("bg-blue").addClass("bg-primary");
                $(".profile-section .friend.delete").css("display", "inline-block");
                $(".profile-section .friend.add").css("display", "none");
            }
            data = {
                Id_User: data.Id_Friend
            }
            $.post("/Profile/FriendSection", data).done(function (html) {
                $("#friend-section").html(html);
            }).fail(function (response) {
                $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
                $("#notify-button").click();
            })
            $.post("/Layout/FriendChatSection").done(function (html) {
                $("#friend-chat-section").html(html);
                $(".search-friend a.js-sidebar-open").click($._data($("button#hidden").get(0), "events").click["0"].handler);
                $("body").tooltip({ selector: '[data-toggle=tooltip]' });
            }).fail(function (response) {
                $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
                $("#notify-button").click();
            });
            num_notirequest--;
            if (num_notirequest != 0) {
                $(".notirequest").text(num_notirequest);
            }
            else {
                $(".notirequest").css("display", "none");
                if ($(".header-content-wrapper .notification-list.friend-requests li.accepted").length == 0) {
                    $(".empty-notirequest").css("display", "block");
                }
            }
        }
        else {
            $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
            $("#notify-button").click();
        }
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!")
        $("#notify-button").click();
    })
});

var chat = $.connection.chatHub;

// Create a function that the hub can call to broadcast messages.
chat.client.receiveNotiRequest = function (who, sender) {
    data = {
        username: who
    }
    $.post("/Layout/NotiRequestSection", data).done(function (html) {
        $(".notiRequestPartial").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
    $.post("/YourAccount/NotiRequestSectionAccount", data).done(function (html) {
        $(".addNewRequest").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
    if (getQueryVariable("username") == sender) {
        $(".profile-section .friend.delete, .profile-section .friend.accept").css("display", "inline-block");
        $(".profile-section .friend.add").css("display", "none");
    }
};

chat.client.receiveNotiRequestReject = function (who, reject) {
    data = {
        username: who
    }
    $.post("/Layout/NotiRequestSection", data).done(function (html) {
        $(".notiRequestPartial").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
    $.post("/YourAccount/NotiRequestSectionAccount", data).done(function (html) {
        $(".addNewRequest").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
    if (reject == true) {
        $("#reject-button").click();
    }
    $(".profile-section .friend").removeClass("bg-primary").addClass("bg-blue");
    $(".profile-section .friend.delete, .profile-section .friend.accept").css("display", "none");
    $(".profile-section .friend.add").css("display", "inline-block");
};

chat.client.receiveNotification = function (who, sender) {
    data = {
        username: who
    }
    $.post("/Layout/NotificationSection", data).done(function (html) {
        $(".notificationPartial").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
    $.post("/YourAccount/NotificationSectionAccount", data).done(function (html) {
        $(".addNewNotification").html(html);
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
        $("#notify-button").click();
    });
};

chat.client.receiveComment = function (id_post, id_comment) {
    data = {
        id_comment: id_comment
    }
    if (window.location.href.search("Post?Id") == -1 && id_post == getQueryVariable("Id")) {
        var total_comment = $(".comments-shared span");
        var total_text = parseInt(total_comment.text());
        $.post("/Post/CommentSection", data).done(function (html) {
            $(".comments-list").append(html);
            $(".comments-list").find("li.new-comment").show('slow').addClass("showed");
            total_text++;
            $(total_comment).text(total_text);
        }).fail(function (response) {
            $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
            $("#notify-button").click();
            total_comment--;
        })
    }
};

var dataSentNotiRequest;
var dataSentNotification;
var dataSentComment;

$.connection.hub.start().done(function () {
    $('#sentNotiRequest').click(function () {
        chat.server.sentNotiRequest(dataSentNotiRequest.Name, dataSentNotiRequest.CodeRelationshipId, dataSentNotiRequest.Reject);
    });
    $('#sentNotification').click(function () {
        chat.server.sentNotification(dataSentNotification);
    });
    $('#sentComment').click(function () {
        chat.server.sentComment(dataSentComment.name, dataSentComment.id_post, dataSentComment.id_comment)
    });
});

$(document).on("click", ".un-read", function () {
    var parent = $(this);
    num_notification--;
    if (num_notification != 0) {
        $(".notification").text(num_notification);
    }
    else {
        $(".notification").css("display", "none");
    }
    var data = {
        Id: $(parent).attr("class").match(/notification_(\d+)/)[1]
    }
    $(".notification_" + data.Id).removeClass("un-read");
    $.post('/Notification/Notification_action', data).done(function (response) {
        if (response.result == "success") {
        }
        else {
            $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!");
            $("#notify-button").click();
        }
    }).fail(function (response) {
        $("#notify .ui-block-content p").html("Thành thật xin lỗi. <br/>Hình như có lỗi gì đó rồi, thử lại sau nhé!!!")
        $("#notify-button").click();
    })
});

