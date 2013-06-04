; (function ($) {
    "use strict";
    var Notification = function (type, element, options) {
        this.init(type, element, options);
    };

    Notification.prototype = {

        constructor: Notification,

        init: function (element, options) {
            this.options = this.getOptions(options);

            if (element != undefined && this.options.removeTarget) {
                $(element).remove();
            }

            this.$notification = $(this.options.template).addClass(this.options.type);
            this.$content = this.$notification.find('.notification-content');
            if (element != undefined) {
                this.$content.html($(element).html());
            }
            else {
                this.$content.html(this.options.text);
            }
            this.$notification.find('a.message-close').click($.proxy(function () { this.$notification.fadeOut(300, function () { $(this).remove(); }); }, this));
            this.$stackContainer = $('#notifier-box');

            if (!this.$stackContainer.length) {
                this.$stackContainer = $('<div id="notifier-box"/>').prependTo(document.body);
            }
        },

        show: function () {
            this.$notification.appendTo(this.$stackContainer).fadeIn();
            if (this.options.lifeTime > 0) {
                setTimeout($.proxy(function () { this.$notification.fadeOut(300, function () { $(this).remove(); }); }, this), this.options.lifeTime);
            }
        },

        getOptions: function (options) {
            return $.extend({}, $.extend(true, {}, $.fn.notification.defaults), options);
        }

    };

    $.fn.notification = function (option) {
        if (this.length > 0) {
            return this.each(function () {
                var notification = new Notification(this, option);
                notification.show();
            });
        }
        else {
            var notification = new Notification(undefined, option);
            notification.show();
        }
    };

    $.fn.notification.Constructor = Notification;

    $.fn.notification.defaults = {
        removeTarget: true,
        type: 'information',
        lifeTime: 0,
        text: '',
        template: '<div class="notification"><div class="notification-bg" id="notification-bg-n"></div><div class="notification-bg" id="notification-bg-ne"></div><div class="notification-bg" id="notification-bg-e"></div><div class="notification-bg" id="notification-bg-se"></div><div class="notification-bg" id="notification-bg-s"></div><div class="notification-bg" id="notification-bg-sw"></div><div class="notification-bg" id="notification-bg-w"></div><div class="notification-bg" id="notification-bg-nw"></div><a class="message-close close">&times;</a><p class="notification-content"></p></div>'
    };

})(jQuery);