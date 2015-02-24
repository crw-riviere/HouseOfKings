(function (riviere, $, undefined) {
    $.fn.loadingButton = function (options) {
        var settings = $.extend({
            text: 'Loading...',
            reset: false
        }, options);

        if (!settings.reset) {
            this.button('reset');

            this.attr('data-loading-text', '&emsp;' + settings.text);
            this.button('loading');
        }
        else {
            this.button('reset');
        }

        return this;
    };

    riviere.modal = {
        show: function (options) {
            var settings = $.extend({
                title: '',
                icon: '',
                content: '',
                action: null,
                showCancel:true,
                confirmText: 'Yes',
                cancelText: 'Cancel'
            }, options);

            var $modal = $('<div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="confirmationModal" aria-hidden="true"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="lblModalTitle">' + settings.title + '</h4></div><div class="modal-body"><p class="text-center"><span class="' + settings.icon + ' fa-3x"></span><br /><br />' + settings.content + '</div><div class="modal-footer"></div></div></div></div>');

            if (settings.action) {
                $modal.find('.modal-footer').append($('<button type="button" id="action-confirm" class="btn btn-primary" data-dismiss="modal">' + settings.confirmText + '</button>'));
                $(document).on('click', '#action-confirm', function () {
                    settings.action();
                    $(document).off('click', '#action-confirm');
                }).on('click', '#action-cancel', function () {
                    $(document).off('click', '#action-confirm');
                });
            }

            if (settings.showCancel) {
                $modal.find('.modal-footer').append($('<button type="button" id="action-cancel" class="btn btn-default" data-dismiss="modal">' + settings.cancelText + '</button>'));

            }

            $modal.modal({ backdrop: 'static', keyboard:false });
        }
    };
}(window.riviere = window.riviere || {}, jQuery));