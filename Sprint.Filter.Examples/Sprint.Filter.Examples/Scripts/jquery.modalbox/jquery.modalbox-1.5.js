!function ($) {

    "use strict";

    var W = $(window);

    var ModalBox = function (type, link, options) {

        this.resizeId = ((((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1) + (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1));

        this.$element = $(link);

        this.type = type;

        this.options = this.getOptions(options);

        this.$element.on('click.modalbox', $.proxy(this.client_show, this));

        this.$appendtotarget = $(this.options.appendtotarget);        
    };

    ModalBox.prototype = {

        version: '1.5.0',

        isShown: false,

        show: function (data, isAjax) {
            this.isShown = true;

            if (data !== undefined) {
                if (isAjax) {
                    this.$tempContainer = $('<div class="modalbox-tmp"/>').appendTo(document.body);

                    this.$tempContainer.html(data);

                    this.$modalContent.append(this.$tempContainer.children());

                } else {
                    this.$modalContent.append(data);
                }

                this.$modalWrap.appendTo(this.$appendtotarget);

                this.$modalWrap.on('click.dismiss.modalbox', '[data-dismiss="modalbox"]', $.proxy(this.client_hide, this));
            }

            this.resize();

            this.removeProgress();

            this.$modalWrap.show();

            this.options.show(this.$modalWrap, this.$element);

            this.$modalWrap.trigger('modalbox-show');

            $(window).on('resize.modalbox.' + this.resizeId, $.proxy(this.resize, this));

            this.escape();

            if (this.$tempContainer) {
                this.$tempContainer.remove();
            }
        },

        client_show: function (event) {            
            if (event)
                event.preventDefault();

            if (!this.isShown) {

                this.progress();

                this.backdrop();

                if (this.$modalWrap === undefined || this.$modalWrap === null) {
                    this.$modalWrap = $(this.options.template);

                    this.$modalContent = this.$modalWrap.find('.modalbox-content');

                    if (this.options.target) {
                        this.show($(this.options.target).html(), false);
                    } else {
                        this.getAjaxContent();
                    }

                } else {
                    this.show();
                }
            }
            return false;
        },

        client_hide: function () {

            this.options.close(this.$modalWrap);

            this.$modalWrap.trigger('modalbox-close');

            if (this.isShown) {
                this.removeBackdrop();
                if (this.options.closetype == 'hide') {
                    this.$modalWrap.hide();
                } else {
                    this.$modalWrap.remove();

                    this.$modalWrap = null;
                }

                $(window).off('resize.modalbox.' + this.resizeId);

                this.isShown = false;

                this.escape();
            }
        },

        client_destroy: function () {
            if (this.isShown) {
                this.client_hide();
            } else {
                if (this.$modalWrap !== undefined && this.$modalWrap !== null) {
                    this.$modalWrap.remove();
                }
            }

            this.$element.off('click.modalbox');

            this.$element.removeData(this.type);
        },

        getViewport: function () {
            return {
                x: W.scrollLeft(),
                y: W.scrollTop(),
                w: W.width(),
                h: W.height()
            };
        },

        resize: function () {
            if (this.isShown) {
                var width = this.$modalWrap.width();

                var height = this.$modalWrap.height();

                var viewport = this.getViewport();

                var top = parseInt(Math.max(viewport.y - 20, viewport.y + ((viewport.h - height - 40) * 0.5)), 10) - parseInt(this.$appendtotarget.offset().top);

                var left = parseInt(Math.max(viewport.x - 20, viewport.x + ((viewport.w - width - 40) * 0.5)), 10) - parseInt(this.$appendtotarget.offset().left);

                this.$modalWrap.css({ 'left': left, 'top': top });
            }
        },

        backdrop: function () {
            if (this.options.backdrop) {
                this.$backdrop = $('<div class="modalbox-backdrop fade in" />').appendTo(document.body);
            }
        },

        removeBackdrop: function () {
            if (this.$backdrop !== undefined && this.$backdrop !== null) {
                this.$backdrop.remove();
            }
            this.$backdrop = null;
        },

        progress: function () {
            this.$progress = $('<div class="modalbox-loading"/>').appendTo('body').appendTo(document.body);
        },

        removeProgress: function () {
            if (this.$progress !== undefined && this.$progress !== null) {
                this.$progress.remove();
            }

            this.$progress = null;
        },

        escape: function () {
            var self = this;
            if (self.isShown && self.options.keyboard) {
                $(document).on('keyup.dismiss.modalbox' + this.resizeId, function (e) { e.which === 27 && self.client_hide(); });
            } else if (!this.isShown) {
                $(document).off('keyup.dismiss.modalbox' + this.resizeId);
            }
        },

        getOptions: function (options) {
            var ops = $.extend({}, $.extend(true, {}, $.fn.modalbox.defaults), options, this.$element.data());

            return ops;
        },

        getAjaxContent: function () {
            if (this.$element.attr('href')) {
                var data = undefined;
                
                if (typeof this.options.ajaxSettings.data === 'function') {
                    data = this.options.ajaxSettings.data();
                } else {
                    data = this.options.ajaxSettings.data;
                }
                
                $.ajax({
                    type: this.options.ajaxSettings.type,
                    contentType: this.options.ajaxSettings.contentType,
                    dataType: this.options.ajaxSettings.dataType,
                    data: data,
                    cache: false,                    
                    url: this.$element.attr('href'),
                    context: this,
                    error: function () {
                        this.removeProgress();
                        this.removeBackdrop();
                        this.isShown = false;
                        this.$modalWrap = null;
                    },
                    success: function (data) {
                        this.show(data, true);
                    }
                });
            } else {
                throw "href not found";
            }
        }
    };

    $.fn.modalbox = function (option) {
        return this.each(function () {

            var $this, data, options, type;

            type = 'modalbox';

            $this = $(this);

            data = $(this).data(type);

            options = typeof option === 'object' && option;

            if (!data) {
                $this.data(type, (data = new ModalBox(type, this, options)));
            }

            if (typeof option === 'string') {
                data['client_' + option]();
            }
        });
    };

    $.fn.modalbox.defaults = {
        ajaxSettings: {
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: 'html',
            data:undefined
        },
        
        appendtotarget: 'body',

        closetype: 'clear',

        backdrop: true,

        show: $.noop,

        close: $.noop,

        target: undefined,

        keyboard: true,

        template: '<div class="modalbox-wrap"><div class="modalbox-outer"><div class="modalbox-bg modalbox-bg-n"></div><div class="modalbox-bg modalbox-bg-ne"></div><div class="modalbox-bg modalbox-bg-e"></div><div class="modalbox-bg modalbox-bg-se"></div><div class="modalbox-bg modalbox-bg-s"></div><div class="modalbox-bg modalbox-bg-sw"></div><div class="modalbox-bg modalbox-bg-w"></div><div class="modalbox-bg modalbox-bg-nw"></div><div class="modalbox-content"></div></div></div>'
    };

    $.fn.modalbox.Constructor = ModalBox;

} (window.jQuery);
