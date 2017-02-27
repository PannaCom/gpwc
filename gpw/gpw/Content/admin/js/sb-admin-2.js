/*!
 * Start Bootstrap - SB Admin 2 v3.3.7+1 (http://startbootstrap.com/template-overviews/sb-admin-2)
 * Copyright 2013-2016 Start Bootstrap
 * Licensed under MIT (https://github.com/BlackrockDigital/startbootstrap/blob/gh-pages/LICENSE)
 */
$(function () {
    $('#side-menu').metisMenu();
});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function () {
    $(window).bind("load resize", function () {
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    // var element = $('ul.nav a').filter(function() {
    //     return this.href == url;
    // }).addClass('active').parent().parent().addClass('in').parent();
    var element = $('ul.nav a').filter(function () {
        return this.href == url;
    }).addClass('active').parent();

    while (true) {
        if (element.is('li')) {
            element = element.parent().addClass('in').parent();
        } else {
            break;
        }
    }
});

function goBack() {
    window.history.back();
}

function getExtension(path) {
    var basename = path.split(/[\\/]/).pop(),  // extract file name from full path ...
                                               // (supports `\\` and `/` separators)
        pos = basename.lastIndexOf(".");       // get last position of `.`

    if (basename === "" || pos < 1)            // if file name is empty or ...
        return "";                             //  `.` not found (-1) or comes first (0)

    return basename.slice(pos + 1);            // extract extension ignoring `.`
}

//function formatDate(date) {
//    var hours = date.getHours();
//    var minutes = date.getMinutes();
//    var ampm = hours >= 12 ? 'pm' : 'am';
//    hours = hours % 12;
//    hours = hours ? hours : 12; // the hour '0' should be '12'
//    minutes = minutes < 10 ? '0' + minutes : minutes;
//    var strTime = hours + ':' + minutes + ' ' + ampm;
//    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
//}

function formatDateDD(date) {
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
}

function toDate(s) {
    if (s !== undefined) {
        var from = s.split(' ');
        var from2 = from[0].split('/');
        var from3 = from[1].split(':');
        return new Date(from2[2], from2[0] - 1, from2[1], from3[0]);
    }
    return false;
}

function popitup(url, windowName) {
    newwindow = window.open(url, windowName, 'height=600,width=500,toolbar=0,menubar=0,location=0');
    if (window.focus) { newwindow.focus(); }
    return false;
}

function gotoUrl(url) {
    location.href = url;
}

function confirmDelete(event) {
    if (confirm("Bạn không thể khôi phục lại nếu bạn đã xóa. Bạn chắc chắn tiếp tục xóa.")) {
        return true;
    } else {
        event.preventDefault();
        return false;
    }
}

jQuery.fn.confirmSubmit = function (message) {
    $(this).submit(function (event) {
        if (confirm(message)) {
            $(this).find('button[type="submit"]').prop('disabled', true);
        } else {
            event.preventDefault();
        }
    });
};

function UploadImage(urllink, d1, d2, d3, i1) { /*Hàm upanh, phần tử up ảnh, phần tử hiện thị, input nhận, dung lượng ảnh cho phép*/
    var myUpAnh = new Dropzone(d1, {
        url: urllink,
        addRemoveLinks: true,
        maxFiles: 1,
        maxFilesize: i1,
        uploadMultiple: true,
        acceptedFiles: "image/*",
        clickable: d2 + '>i',
        dictFallbackMessage: "Trình duyệt của bạn không hỗ trợ kéo thả tệp để tải lên.",
        dictFallbackText: "Please use the fallback form below to upload your files like in the olden days.",
        dictFileTooBig: "Tệp có dung lượng quá lớn ({{filesize}}MiB). Dung lượng cho phép: {{maxFilesize}}MiB.",
        dictInvalidFileType: "Tệp bạn chọn không được phép tải lên.",
        dictResponseError: "Server responded with {{statusCode}} code.",
        success: function (file, response) {
            var imgPath = response.Message;
            if (imgPath !== "") {
                $(d3).val(imgPath);
                $(d2).css('background-image', 'url("' + imgPath + '")');
            }
            this.removeFile(file);
        },
        error: function (file, response) {
            alert(response);
            this.removeFile(file);
        },
        init: function () {
            this.on("thumbnail", function (file) {
                $(d2).append('<div class="loaded">'
                + '<i class="fa fa-spinner fa-pulse fa-3x fa-fw"></i>'
                + '</div>');
            })
        },
        complete: function () {
            $(d2).children('.loaded').remove();
        },
        HiddenFilesPath: 'body'
    });
}

// Define appendVal by extending JQuery
$.fn.appendVal = function (TextToAppend) {
    var x = $(this).val($(this).val() + TextToAppend + ",").slice(0, -1);
    return x;
};

$.fn.RemoveVal = function (TextToAppend) {
    return $(this).val($(this).val().replace(TextToAppend, "").slice(0, -1)
    );
};

