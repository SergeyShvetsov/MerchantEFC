$(function () {
    var ModalDialogElement = $('#ModalDialog');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var decodedUrl = decodeURIComponent(url);
        $.get(decodedUrl).done(function (data) {
            ModalDialogElement.html(data);
            ModalDialogElement.find('.modal').modal('show');
        })
    })

    ModalDialogElement.on('click', '[data-save="modal"]', function (event) {
        //event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            ModalDialogElement.find('.modal').modal('hide');
            window.location.reload();
        })
    })
})