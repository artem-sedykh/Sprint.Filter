var notificationLifeTime = 5000;

$.ajaxSetup({ cache: false });

//#region ajax error handler

$(document).ajaxError(function (event, xhr, ajaxSettings, thrownError) {
    //если xhr.status равен 0, то произошла отмена запроса
    if (xhr.status > 0) {
        try {
            var globalError = $.parseJSON(xhr.responseText);
            var showError = function () {
                if (globalError.ErrorMessage) {
                    $.fn.notification({ lifeTime: notificationLifeTime, text: globalError.ErrorMessage, type: 'failure' });
                }
                if (globalError.InnerException) {
                    globalError = globalError.InnerException;
                    showError();
                }
            };
            showError();
            return;
        } catch (e) {
            $.fn.notification({ lifeTime: notificationLifeTime, text: thrownError, type: 'failure' });
        }
    }
});

//#endregion

$(document).ready(function () {
    $('.sprint-grid').sprintgrid();
});

window.initAjaxControl = function (content) {
    var $content = $(content);

    $('ul.columnorder', $content).sortable({
        helper: "clone",
        cursor: 'move',
        axis: 'y',
        start: function (event, ui) {
            ui.helper.addClass('dragging');
        }
    }).disableSelection();

};

//#region Search

//Processing keystrokes in the search box
$('body').on('keypress', 'input.search', function (e) {
    var code = e.keyCode || e.which;
    if (code === 13) {
        var $input = $(this);
        var gridkey = $input.attr('data-update-grid');
        if (gridkey) {
            var grid = $('body').data(gridkey);
            if (grid) {
                grid.gridOptions.SearchString = $input.val();
                grid.refresh();
            }
        }
    }
});

//#endregion

//#region Save carrent state

$('body').on('mouseenter focus', 'a[data-grid-save]', function () {
    var $link = $(this);

    if ($link.data('initialized')) return;

    var gridKey = $link.attr('data-grid-save');

    var options = {
        show: function ($content) {
            var currentmodalbox = $link.data('modalbox');
            var $form = $('form', $content);

            //#region Initialize objects inside a modal window

            if (window.initAjaxControl)
                window.initAjaxControl($form);

            //#endregion

            if ($form.length) {
                //Include validation form
                $.validator.unobtrusive.parseDynamicContent($form[0]);

                var ajaxFormOptions = {
                    beforeSerialize: function () {
                        //record the current state of the field in the form  
                        var $actionGridRawOptions = $form.find('#ActionGridRawOptions');
                        var grid = $('body').data(gridKey);
                        $actionGridRawOptions.val(JSON.stringify(grid.gridOptions));
                    },

                    beforeSubmit: function () {
                        if (!$form.valid())
                            return false;
                        
                        //#region Locking the buttons when the form is submitted to the server

                        var $buttons = $form.find(':submit');
                        $buttons.attr('disabled', 'disabled');

                        //#endregion
                    },

                    error: function (xhr) {

                        var $buttons = $form.find(':submit');
                        $buttons.removeAttr('disabled');

                        if (xhr.status === 400) {
                            var errors = $.parseJSON(xhr.responseText);
                            $form.data('validator').showErrors(errors);
                        }
                    },

                    success: function (stateId) {
                        //#region update saved filter list
                        
                        var $filterList = $('div[data-filter-list="' + gridKey + '"]');
                        
                        $.get($filterList.attr('data-action'), {stateId:stateId}, function(data) {
                            $filterList.html(data);
                            //close modal window
                            currentmodalbox.client_hide('hide');
                        });
                        
                        //#endregion
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

$('body').on('change', '.grid-state-list', function () {
    var $self = $(this);
    var $name = $self.parents('form:first').find('#Name');
    if ($self.val()) {
        $name.val($self.find(":selected").text());
    } else {
        $name.val('');
    }
});

//#region load state

$('body').on('click', 'a[data-load-grid-state]', function () {
    var $link = $(this);

    $.ajax({
        dataType: "json",
        url: $link.attr('href'),
        success: function (gridOptions) {
            //refresh grid
            var gridkey = $link.attr('data-load-grid-state');
            var $search = $('input.search[data-update-grid="' + gridkey + '"]');
            $search.val(gridOptions.SearchString);
            if (gridkey) {
                var grid = $('body').data(gridkey);
                if (grid) {
                    grid.refresh(gridOptions);
                    $link.closest('ul.saved-filters').find('a.selected').removeClass('selected');
                    $link.addClass('selected');                    
                }
            }
        }
    });
    return false;
});

//#endregion

//#region clear state

$('body').on('click', 'a[data-clear-state]', function () {
    
    var $link = $(this);
    
    $.ajax({
        dataType: "json",
        url: $link.attr('href'),
        success: function (gridOptions) {
            //refresh grid
            var gridkey = $link.attr('data-clear-state');
            var $search = $('input.search[data-update-grid="' + gridkey + '"]');
            $search.val(gridOptions.SearchString);
            if (gridkey) {
                var grid = $('body').data(gridkey);
                if (grid) {
                    grid.refresh(gridOptions);                    
                    $('div[data-filter-list="' + gridkey + '"]').find('a.selected').removeClass('selected');                                        
                }
            }
        }
    });
    return false;
});

//#endregion

//#region Grid settings

$('body').on('mouseenter focus', 'a[data-grid-setting]', function () {
    var $link = $(this);

    if ($link.data('initialized')) return;

    var gridKey = $link.attr('data-grid-setting');

    var modalboxOptions = {
        show: function ($content) {

            var currentmodalbox = $link.data('modalbox');

            var $form = $('form', $content);

            //#region Initialize objects inside a modal window

            if (window.initAjaxControl)
                window.initAjaxControl($form);

            //#endregion

            if ($form.length) {
                
                $.validator.unobtrusive.parseDynamicContent($form[0]);

                $form.submit(function () {
                    if (!$form.valid())
                        return false;

                    var grid = $('body').data(gridKey);

                    var gridOptions = grid.gridOptions;
                    gridOptions.PageOpt = gridOptions.PageOpt || {};
                    gridOptions.PageOpt.ps = $('#PageSize').val();
                    $('ul.columnorder li').each(function (index, li) {
                        var $column = $(li).find(':checkbox');
                        if ($column.length) {
                            var key = $column.attr('id');
                            var visible = $column.is(':checked');
                            gridOptions.ColOpt = gridOptions.ColOpt || {};
                            gridOptions.ColOpt[key] = gridOptions.ColOpt[key] || {};
                            gridOptions.ColOpt[key].co = index;
                            gridOptions.ColOpt[key].vc = visible;
                        }
                    });

                    grid.refresh(null, null, function () {
                        currentmodalbox.client_hide('hide');
                    });

                    return false;
                });
            }
        },
        ajaxSettings: {
            type: 'POST',
            contentType: 'application/json',
            data: function () {
                var grid = $('body').data(gridKey);
                return JSON.stringify(grid.gridOptions);
            },
            dataType: 'html'
        }
    };

    $link.modalbox(modalboxOptions);
});

//#endregion

//#region Filter

$('body').on('mouseenter focus', 'a[data-filter]', function () {
    
    var $link = $(this);

    if ($link.data('initialized')) return;

    var gridKey = $link.attr('data-filter');
    
    var modalboxOptions = {
        show: function ($content) {

            var currentmodalbox = $link.data('modalbox');

            var $form = $('form', $content);

            //#region Initialize objects inside a modal window

            if (window.initAjaxControl)
                window.initAjaxControl($form);

            //#endregion

            if ($form.length) {

                $.validator.unobtrusive.parseDynamicContent($form[0]);

                $form.submit(function () {
                    if (!$form.valid())
                        return false;

                    if (gridKey) {
                        var sprintGrid = $('body').data(gridKey);
                        var gridOptions = sprintGrid.gridOptions;
                        delete gridOptions["Filters"];
                        var arr = $form.serializeArray();
                        $.each(arr, function () {
                            if (gridOptions[this.name] !== undefined) {
                                if (!gridOptions[this.name].push) {
                                    gridOptions[this.name] = [gridOptions[this.name]];
                                }
                                gridOptions[this.name].push(this.value || '');
                            } else {
                                gridOptions[this.name] = this.value || '';
                            }
                        });
                        sprintGrid.refresh(null, null, function () {
                            currentmodalbox.client_hide('hide');
                        });
                    }
                    return false;
                });
            }
        },
        ajaxSettings: {
            type: 'POST',
            contentType: 'application/json',
            data: function () {
                var grid = $('body').data(gridKey);
                return JSON.stringify(grid.gridOptions);
            },
            dataType: 'html'
        }
    };

    $link.modalbox(modalboxOptions);

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

//#region init form validation

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

//#endregion