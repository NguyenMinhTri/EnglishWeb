
function operateFormatter(value, row, index) {
    return [
            '<div class="table-icons">',
            '<a rel="tooltip" title="Reset Password" class="btn btn-simple btn-warning btn-icon table-action changepass" href="javascript:void(0)">',
            '<i class="ti-unlock"></i>',

            '<a rel="tooltip" title="Change Role" class="btn btn-simple btn-warning btn-icon table-action role" href="javascript:void(0)">',
            '<i class="ti-user"></i>',
            '</a>',

            '<a rel="tooltip" title="Update" class="btn btn-simple btn-warning btn-icon table-action edit" href="javascript:void(0)">',
            '<i class="ti-pencil-alt"></i>',
            '</a>',

            '<a rel="tooltip" title="Expand" href="#" class="btn btn-simple btn-warning btn-icon table-action expand" href="javascript:void(0)">',
            '<i class="ti-angle-down"></i>',
            '</a>',

            '<a rel="tooltip" title="Delete" class="btn btn-simple btn-danger btn-icon table-action remove" href="javascript:void(0)">',
            '<i class="ti-close"></i>', '</a>', '</div>', '</a>'


    ].join('');
}

function ChangeRole(row) {
    window.location.href = "/AdminApplicationUserRole?userId=" + row.Id;
}

function CreateDoctor(row) {
    if (!row.IsDoctor) {
        window.location.href = "/AdminDoctor?userId=" + row.Id;
    }
    else {
        window.location.href = "/AdminDoctor?userId=" + row.Id + "&updateDoctor="+true;
    }
}


window.operateEvents = {
    'click .role': function (e, value, row, index) {
        ChangeRole(row);
    },
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