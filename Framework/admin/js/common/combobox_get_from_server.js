/**
 * Hung-DV 28/05
 */

function refreshSelectPickerSuccess(par) {

}

var comboboxDatas = [];

var selects = $('.selectpicker');

var indexSelect = 0;

function createDataCombobox(fieldName, elem) {
    var options = $('#' + fieldName + ' .selectpicker option');
    comboboxDatas[fieldName] = [];
    options.each(function (e) { comboboxDatas[fieldName][$(this).attr('value')] = $(this).text(); });
    indexSelect++;
    if (indexSelect==selects.length)
        $('#bootstrap-table').bootstrapTable('refresh', { silent: true });
}

function refreshSelectPicker(par) {
    var selectpickers = par.find('.selectpicker');
    $.each(selectpickers, function (key, val) {
        var selectpicker = $(val);
        var url = selectpicker.attr('data-ajax');
        var fieldName = selectpicker.closest('.form-input-group').attr('id');
        if (url == null) {
            $(val).selectpicker('refresh');
            refreshSelectPickerSuccess(par);
            createDataCombobox(fieldName, $(val));
            return;
        }
        var displayMember = selectpicker.attr('display-member');
        var valueMember = selectpicker.attr('value-member');
        var comboboxContainer = $(val);
        var datas = [];
        var indexdata = 0;
        $.ajax({
            url: url,
            method: "GET",
            contentType: "application/json",
            success: function (data) {
                comboboxContainer.empty();
                $.each(data, function (key, values) {
                    datas[indexdata++] = ['<option value="', values[valueMember],
                            '">', values[displayMember], '</option>', ].join('');
                });
                comboboxContainer.append(datas.join(''));
                $(val).selectpicker('refresh');

                var parent = $(val).parent().parent().parent();

                $.each($(parent).find('li'), function (k, v) {
                    $(v).click(
                            function (e) {
                                $(parent.find('input')[0]).val($($(val).find('option')[k + 1]).val());

                            });
                });

                par = $(val).closest('.form-input-group');
                createDataCombobox(fieldName, $(val));
                refreshSelectPickerSuccess(par);

            },
            error: function () {
                // Message("Thất bại", "Đã có lỗi xảy ra!", "danger");
            }
        });
    });
}

refreshSelectPicker($('body'));