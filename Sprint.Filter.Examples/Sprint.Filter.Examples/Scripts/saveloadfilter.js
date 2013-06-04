//#region Save filter form
$('body').on('mouseenter focus', 'a[data-save-filter]', function () {
    var $link = $(this);
    if ($link.data('initialized')) return;
    
    var key = $link.attr('data-save-filter');
    
    var options = {
        show: function ($content) {
            
            var $form = $('form', $content);
            
            var currentmodalbox = $link.data('modalbox');

            if ($form.length) {
                //Enable form validation
                $.validator.unobtrusive.parseDynamicContent($form[0]);

                var ajaxFormOptions = {
                    beforeSerialize: function () {
                        //add carent filter state.
                        var $filterRawOptions = $form.find('#FilterRawOptions');
                        $filterRawOptions.val($('#' + key).val());                                               
                    },

                    beforeSubmit: function () {
                        if (!$form.valid())
                            return false;
                        
                        var $buttons = $form.find(':submit');
                        $buttons.attr('disabled', 'disabled');
                    },

                    error: function (xhr) {
                        var $buttons = $form.find(':submit');
                        $buttons.removeAttr('disabled');

                        if (xhr.status === 400) {
                            var errors = $.parseJSON(xhr.responseText);
                            $form.data('validator').showErrors(errors);
                        }
                    },

                    success: function () {
                        currentmodalbox.client_hide('hide');
                        var $filterlist = $link.closest('div[data-filter-panel]').find('div[data-filter-list]');
                        
                        //refresh filter list
                        $.get($filterlist.attr('data-filter-list'), function (data) {
                            $filterlist.html(data);
                        });                                               
                    }
                };                

                $form.ajaxForm(ajaxFormOptions);
            }
        }
    };

    $link.modalbox(options);

    $link.data('initialized', true);
});
//#endregion

//#region Load saved filter

$('body').on('click', 'a.load-filter', function () {
    var $self = $(this);

    $.ajax({
        dataType: "json",
        url: $self.attr('href'),
        success: function (options) {
            var $filterPanel = $self.closest('div[data-filter-panel]');
            var $filterForm = $filterPanel.find('form:first');
            var gridurl = $filterForm.attr('action');
            var target = $filterForm.attr('data-ajax-update');
                       
            if ($filterPanel.length > 0) {
                
                //#region update filter panel
                
                var rawOptions = JSON.stringify(options);
                
                $.ajax({
                    url:  $filterPanel.attr('data-filter-panel'),
                    type: 'POST',
                    contentType: 'application/json',
                    data: rawOptions,
                    dataType: 'html',
                    success: function (filterPanelHtml) {
                        $filterPanel.replaceWith(filterPanelHtml);
                        
                        //#region update table
                        $.ajax({
                            url: gridurl,
                            type: 'POST',
                            contentType: 'application/json',
                            data: rawOptions,
                            dataType: 'html',
                            success: function (gridHtml) {
                                $(target).replaceWith(gridHtml);
                            }
                        });
                        //#endregion
                    }
                });
                
                //#endregion                               
            }
        }
    });
    
    return false;
});

//#endregion


//#region Delete saved filter

$('body').on('click', '[data-filter-delete]', function () {
    var $self = $(this);

    if (confirm("Are you sure you want to remove the selected entry?")) {
        $.post($self.attr('data-filter-delete'), function () {
            var $ul = $self.closest('ul');
            $self.closest('li').remove();
            
            if (!$ul.find('li').length) {
                $ul.append('<li class="empty">Empty.</li>');
            }
        });
    }

});

//#endregion

(function ($) {
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    };
})($);