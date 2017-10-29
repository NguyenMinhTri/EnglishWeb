
function clickAtPosition(x, y) {
    var ev = new MouseEvent('click', {
        'view': window,
        'bubbles': true,
        'cancelable': true,
        'screenX': x,
        'screenY': y
    });

    var el = document.elementFromPoint(x, y);

    el.dispatchEvent(ev);
}


function operateFormatter(value, row, index) {
    return [
            '<div class="table-icons">',
            '<a rel="tooltip" title="Update" class="btn btn-simple btn-warning btn-icon table-action edit" href="javascript:void(0)">',
            '<i class="ti-pencil-alt"></i>',
            '</a>',
            '<a rel="tooltip" title="Delete" class="btn btn-simple btn-danger btn-icon table-action remove" href="javascript:void(0)">',
            '<i class="ti-close"></i>', '</a>', '</div>'].join('');
}

function parseJsonDate(jsonDateString) {
    if (jsonDateString !== null && jsonDateString !== '') {
        var dateStr = "" + new Date(parseInt(jsonDateString.replace('/Date(', '').replace(')/', '')));
        dateStr = dateStr.substring(0, dateStr.indexOf('GMT'));
        return dateStr;
    }
    else
        return '';
}



function imageFormatter(value, row, index) {
    if (value == null)
        return '';
    return '<div><img src="' + value + '" alt="' + value + '"/></div>';
}

function comboboxFormatter(value, row, index, field) {
    if (value == null)
        return '';
    //while (comboboxDatas[field] == null)
    //{
    //}

    if (comboboxDatas[field] == null)
        return '';

    return comboboxDatas[field][value];
}

function videoFormatter(value, row, index) {
    if (value == null)
        return '';
    var videoLink = "https://www.youtube.com/embed/" + value + "?autoplay=0";
    return "<iframe width=\"100%\" height=\"100%\" src=\"" + videoLink + "\"></iframe>";
}

function defaultFormatter(value, row, index) {
    if (value == null)
        return '';
    return '<div>' + value + '</div>';
}

function switchFormatter(value, row, index, field) {
    if (field != 'Status')
        return value;
    if (value == null)
        return '';
    if (value == false) {
        return '<p class="falseStatus">false</p>';
    }
    return value;
}

var table = $("#bootstrap-table");

$('[rel="tooltip"]').tooltip();

function filterStatus(tag) {
    if (tag == 1) {
        $("#bootstrap-table").bootstrapTable("filterBy", {});
        return;
    }
    if (tag == 2) {
        $("#bootstrap-table").bootstrapTable("filterBy", { Status: true });
        return;
    }
    if (tag == 3) {
        $("#bootstrap-table").bootstrapTable("filterBy", { Status: false });
        return;
    }
}

var widthTableFull = -1;

table.on('load-success.bs.table', function (e, data) {
    //$('#bootstrap-table tbody tr:has(p)').addClass('falseStatus');
    $('.table-resize').css('display', 'flex');
    if (widthTableFull < 0)
        widthTableFull = $('#bootstrap-table').outerWidth();
    if ($('#table .pull-left.search #change-size').length == 0) {
        $('#table .pull-left.search').append('<input type="number" id="change-size" class="form-control" value="100" min="100" placeholder="Table width" />');
    }

    if ($('#table .columns.columns-right.pull-right .show-status').length == 0) {
        $('#table .columns.columns-right.pull-right').prepend(
['<div class="flex-reponsive show-status">',
  '<div class="flex-item keep-open" title="Trash"> ',
  '<label class="flex-item" id="status-show-label">Show all rows</label> ',
  '<button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="true">',
  '<i class="glyphicon fa fa-trash-o"></i> <span class="caret"></span>',
  '</button> ',
  '<ul class="dropdown-menu" role="menu"> ',
  '<li> ',
  '<label> ',
  '<input type="radio" tag="1" name="show-option" data-field="actions" value="0" checked="checked">',
  '<span>Show all rows</span> ',
  '</label> ',
  '</li> ',
  '<li> ',
  '<label> ',
  '<input type="radio" tag="2" name="show-option" data-field="Id" value="1">',
  '<span>Show rows where status is true</span> ',
  '</label> ',
  '</li> ',
  '<li> ',
  '<label> ',
  '<input type="radio" tag="3" name="show-option" data-field="Link" value="2">',
  '<span>Show rows where status is false</span> ',
  '</label> ',
  '</li> ',
  '</ul> ',
  '</div>',
  '</div>'].join(''));
    }



    $('#change-size').focusout(function () {
        changeSizeActivity(this);
    });

    $(document).keyup(function (e) {
        if ($("#change-size").is(":focus") && (e.keyCode == 13)) {
            changeSizeActivity($("#change-size"));
        }
    });






    $('.show-status ul span').click(function (e) {
        e.preventDefault();
        $($('.show-status .keep-open label')[0]).text($(this).text());
        $('.show-status ul input').prop('checked', false);
        var input = $(this).parent().find('input');
        input.prop('checked', true);

        var tag = input.attr('tag');
        filterStatus(tag);
    })


    $('.show-status ul label input').change(function (e) {
        e.preventDefault();
        if ($(this).prop('checked'))
            $($('.show-status .keep-open label')[0]).text($(this).parent().text());
        //$('.show-status ul input').prop('checked', false);
        //$(this).prop('checked', true);
    })

    $('#status-show-label').click(function (e) {
        e.preventDefault();

        $('.show-status .keep-open button').click();

    });

});


table.on('reset-view.bs.table', function (e, data) {
    $('#bootstrap-table tbody tr:has(p.falseStatus)').addClass('falseStatus');

    var inputs = $('.show-status ul span').parent().find('input');

    var input = $(inputs[0]);
    inputs.each(function () {
        if ($(this).prop('checked')) {
            input = $(this);
        }
    });

    //var tag = input.attr('tag');
    //filterStatus(tag);
});

table.bootstrapTable({
    toolbar: ".toolbar",
    clickToSelect: true,
    showRefresh: true,
    search: true,
    showToggle: true,
    showColumns: true,
    pagination: true,
    searchAlign: 'left',
    pageSize: 8,
    clickToSelect: false,
    pageList: [8, 10, 25, 50, 100],

    formatShowingRows: function (pageFrom, pageTo, totalRows) {
        // do nothing here, we don't want to show the text "showing x of y
        // from..."
    },
    formatRecordsPerPage: function (pageNumber) {



        return pageNumber + " rows visible";
    },
    icons: {
        refresh: 'fa fa-refresh',
        toggle: 'fa fa-th-list',
        columns: 'fa fa-columns',
        detailOpen: 'fa fa-plus-circle',
        detailClose: 'ti-close'
    }

});

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        Update(row, index);
    },
    'click .remove': function (e, value, row, index) {
        var r = confirm("Are you sure?");
        if (r == true) {
            DELETE(row, index);
        }
    },

    'click .expand': function (e, value, row, index) {
        var elem = $(e.target);
        if (elem.find('i').length > 0) {
            elem = elem.find('i');
        }
        if (elem.hasClass('ti-angle-down')) {
            elem.closest('tr').find('td>div').css('position', 'relative');
            elem.removeClass('ti-angle-down');
            elem.addClass('ti-angle-up');
        }
        else {
            elem.closest('tr').find('td>div').css('position', 'absolute');
            elem.removeClass('ti-angle-up');
            elem.addClass('ti-angle-down');
        }
    }
};

function dateFormat(value, row, index) {
    return moment(value).format('MM/DD/YYYY');
}

function dateFormatter(value, row, index) {
    return moment(value).format('MM/DD/YYYY');
}



$(window).resize(function () {
    table.bootstrapTable('resetView');
});


function resizeInit() {

    table.colResizable({
        draggingClass: 'JCLRgripDrag',
        gripInnerHtml: '',
        minWidth: 15,
        hoverCursor: "col-resize",
        dragCursor: "col-resize",
        postbackSafe: false,
        flush: false,
        marginLeft: null,
        marginRight: null,
        disable: false,
        partialRefresh: false,
        disabledColumns: [],
        removePadding: true
    });
}

resizeInit();




(function ($) {
    'use strict';

    var initResizable = function (that) {
        //Deletes the plugin to re-create it
        that.$el.colResizable({ disable: true });

        //Creates the plugin
        that.$el.colResizable({
            headerOnly: that.options.headerOnly,
            minWidth: that.options.minWidth,
            hoverCursor: that.options.hoverCursor,
            dragCursor: that.options.dragCursor,
            onResize: that.onResize,
            onDrag: that.options.onResizableDrag
        });
    };

    $.extend($.fn.bootstrapTable.defaults, {
        resizable: false,
        liveDrag: false,
        fixed: true,
        headerOnly: false,
        minWidth: 15,
        hoverCursor: 'e-resize',
        dragCursor: 'e-resize',
        onResizableResize: function (e) {
            return false;
        },
        onResizableDrag: function (e) {
            return false;
        }
    });

    var BootstrapTable = $.fn.bootstrapTable.Constructor,
        _toggleView = BootstrapTable.prototype.toggleView,
        _resetView = BootstrapTable.prototype.resetView;

    BootstrapTable.prototype.toggleView = function () {
        _toggleView.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.resizable && this.options.cardView) {
            //Deletes the plugin
            $(this.$el).colResizable({ disable: true });
        }
    };

    BootstrapTable.prototype.resetView = function () {
        var that = this;

        _resetView.apply(this, Array.prototype.slice.apply(arguments));

        if (this.options.resizable) {
            // because in fitHeader function, we use setTimeout(func, 100);
            setTimeout(function () {
                initResizable(that);
            }, 100);
        }
    };

    BootstrapTable.prototype.onResize = function (e) {
        var that = $(e.currentTarget);
        that.bootstrapTable('resetView');
        resizeInit();
        // that.data('bootstrap.table').options.onResizableResize.apply(e);
    }
})(jQuery);

function changeSizeActivity(elem) {
    var valStr = $(elem).val();
    val = parseInt(valStr);
    if (val < 100) {
        $(elem).val(100);
        changeSize(100);
        return;
    }
    changeSize(val);
}



function changeSize(size) {

    table.css('width', widthTableFull * size / 100 + 'px');

    $(window).resize();
    //table.bootstrapTable('resetView');


}


