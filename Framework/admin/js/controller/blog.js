

function operateFormatter(value, row, index) {
    return [
            '<div class="table-icons">',

            '<a rel="tooltip" title="Update" class="btn btn-simple btn-warning btn-icon table-action edit" href="javascript:void(0)">',
            '<i class="ti-pencil-alt"></i>',
            '</a>',

            '<a rel="tooltip" title="Delete" class="btn btn-simple btn-danger btn-icon table-action remove" href="javascript:void(0)">',
            '<i class="ti-close"></i>',
            '</a>',

            '<a rel="tooltip" title="Images" href="#" class="btn btn-simple btn-warning btn-icon table-action expand" href="javascript:void(0)">',
            '<i class="ti-angle-down"></i>',
            '</a>',

            , '</div>'].join('');
}


$('.dropdown-menu li').click(function () {
    var text = $(this).find('span').text();
    if (text == "Video Blog") {
        $('#VideoId').parent().css('display', 'block');
    }

    if (text == "Slide Image Blog") {
        $('#VideoId').parent().css('display', 'none');
    }
});
