!function ($) {

    "use strict"; // jshint ;_;    

    var SprintGrid = function (key, grid, options) {

        this.gridKey = key;

        this.options = this.getOptions(options);

        this.$grid = $(grid);

        //#region Group Selctors

        this.groupHeaderSelector = '.js-' + key + '-grouping-header';

        this.groupIndicatorSelector = '.js-' + key + '-group-indicator';

        this.dropGroupSelector = this.groupIndicatorSelector + ' a.group-drop-button';

        this.sortGroupSelector = this.groupIndicatorSelector + ' a.group-sort';

        this.sortGroupGridSelector = '.js-' + key + '-sort-group-grid';

        this.groupColumnSelector = '.js-' + key + '-group-column';

        //#endregion


        this.gridOptionsSelector = '.js-' + key + '-options';

        this.paginateSelector = '.js-' + key + '-page-link';

        this.groupPaginateSelector = '.js-' + key + '-group-page-link';

        this.sortSelector = '.js-' + key + '-sort-link';

        this.expandGroupSelector = '.js-' + key + '-expand-group';

        this.progressSelector = '.' + key + '-status';

        this.groupOptionsSelector = '.js-' + key + '-group-options';

        this.ajaxLoaderSelector = '.js-' + key + '-ajax-loader';

        this.hierarchySelector = '.js-' + key + '-hierarchy';

        this.init();

    };

    SprintGrid.prototype = {
        constructor: SprintGrid,

        version: '1.0.1.1',

        options: {},

        init: function () {
            this.multisort = this.$grid.attr('data-multisort') === 'True';

            this.prefix = this.$grid.attr('data-prefix');

            this.gridOptions = jQuery.parseJSON($(this.gridOptionsSelector).val());

            if (!this.gridOptions.ColOpt)
                this.gridOptions.ColOpt = {};

            if (!this.gridOptions.PageOpt)
                this.gridOptions.PageOpt = {};

            this.$grid.on('click', this.paginateSelector, $.proxy(this.paginateHandler, this));

            this.$grid.on('click', this.groupPaginateSelector, $.proxy(this.groupPaginateHandler, this));

            this.$grid.on('click', this.sortSelector, $.proxy(this.sortHandler, this));

            this.$grid.on('click', this.sortGroupGridSelector, $.proxy(this.sortGroupGridHandler, this));

            this.$grid.on('click', this.dropGroupSelector, $.proxy(this.dropGroupHandler, this));

            this.$grid.on('click', this.sortGroupSelector, $.proxy(this.sortGroupHandler, this));

            this.$grid.on('click', this.expandGroupSelector, $.proxy(this.expandGroupHandler, this));

            this.$grid.on('click', this.hierarchySelector, $.proxy(this.hierarchyHandler, this));

            this.initGrouping();

        },

        paginateHandler: function (event) {
            var $self = $(event.currentTarget);
            var page = parseInt($self.attr('href'));
            if (page) {
                this.gridOptions.PageOpt.p = page;
            }

            this.refresh();
            return false;
        },

        groupPaginateHandler: function (event) {
            var $self = $(event.currentTarget);
            var page = parseInt($self.attr('href'));
            var $groupWrap = $self.parents('.grid-group-wrap:first');
            var $progress = $groupWrap.find('.' + this.gridKey + '-group-status');
            var groupOptions = jQuery.parseJSON($groupWrap.find(this.groupOptionsSelector).val());

            if (groupOptions) {
                if (page)
                    groupOptions.GPage = page;

                $progress.removeClass('refresh').addClass('loading');

                groupOptions = $.extend(groupOptions, this.gridOptions);

                this.refresh(groupOptions, function (data) {
                    $groupWrap = $groupWrap.replaceWith(data);
                }, function () {
                    $progress.removeClass('loading').addClass('refresh');
                });
            }
            return false;
        },

        sortHandler: function (event) {
            var $self = $(event.currentTarget);
            var key = $self.attr('data-key');
            var direction = parseInt($self.attr('data-direction'));
            var order = parseInt($self.attr('data-sortorder'));

            if (!this.gridOptions.ColOpt[key])
                this.gridOptions.ColOpt[key] = {};


            if (this.multisort) {
                this.gridOptions.ColOpt[key].sd = direction;
                this.gridOptions.ColOpt[key].so = order;

                this.reInitColumnOrder('so', this.gridOptions.ColOpt);

            } else {

                for (var propertyName in this.gridOptions.ColOpt) {
                    if (this.gridOptions.ColOpt[propertyName].sd !== undefined)
                        this.gridOptions.ColOpt[propertyName].sd = null;
                }

                this.gridOptions.ColOpt[key].sd = direction;
            }

            this.refresh();
        },

        sortGroupGridHandler: function (event) {
            var $self = $(event.currentTarget);
            var key = $self.attr('data-key');
            var direction = parseInt($self.attr('data-direction'));
            var order = parseInt($self.attr('data-sortorder'));
            var $groupWrap = $self.parents('.grid-group-wrap:first');
            var $progress = $groupWrap.find('.' + this.gridKey + '-group-status');
            var groupOptions = jQuery.parseJSON($groupWrap.find(this.groupOptionsSelector).val());

            if (groupOptions) {
                if (!groupOptions.GColOpt[key])
                    groupOptions.GColOpt[key] = {};

                if (this.multisort) {

                    groupOptions.GColOpt[key].sd = direction;
                    groupOptions.GColOpt[key].so = order;

                    this.reInitColumnOrder('so', groupOptions.GColOpt);

                } else {

                    for (var propertyName in groupOptions.GColOpt) {
                        if (groupOptions.GColOpt[propertyName].sd !== undefined)
                            groupOptions.GColOpt[propertyName].sd = null;
                    }

                    groupOptions.GColOpt[key].sd = direction;
                }

                $progress.removeClass('refresh').addClass('loading');
                groupOptions = $.extend(groupOptions, this.gridOptions);
                this.refresh(groupOptions, function (data) {
                    $groupWrap = $groupWrap.replaceWith(data);
                }, function () {
                    $progress.removeClass('loading').addClass('refresh');
                });
            }
        },

        sortGroupHandler: function (event) {
            var $self = $(event.currentTarget);
            var $groupIndicator = $self.parents(this.groupIndicatorSelector).first();
            var key = $groupIndicator.attr('data-key');
            var direction = parseInt($groupIndicator.attr('data-direction'));
            if (key) {
                if (!this.gridOptions.ColOpt[key])
                    this.gridOptions.ColOpt[key] = {};

                this.gridOptions.ColOpt[key].gd = direction;
            }

            this.refresh();
        },

        dropGroupHandler: function (event) {
            var $self = $(event.currentTarget);
            var $groupIndicator = $self.parents(this.groupIndicatorSelector).first();
            var key = $groupIndicator.attr('data-key');
            if (key) {
                if (!this.gridOptions.ColOpt[key])
                    this.gridOptions.ColOpt[key] = {};

                this.gridOptions.ColOpt[key].go = null;
                this.gridOptions.ColOpt[key].gd = 0;
                this.reInitColumnOrder('go', this.gridOptions.ColOpt);
                this.refresh();
            }
        },

        expandGroupHandler: function (event) {
            var $self = $(event.currentTarget);
            var $tr = $self.parents('tr:first').next();
            if ($self.hasClass('plus') && !$self.data('initialized')) {
                var $rawOptions = $tr.find(this.groupOptionsSelector);
                var groupOptions = jQuery.parseJSON($rawOptions.val());

                if (groupOptions) {

                    var $td = $tr.find('td.grid-group-row');
                    groupOptions = $.extend(groupOptions, this.gridOptions);
                    this.refresh(groupOptions, function (data) {
                        $td.append(data);
                        $self.data('initialized', true);
                        $tr.fadeIn(150);
                        $self.removeClass('plus').addClass('minus');
                    });
                }

            } else {
                if ($self.hasClass('plus')) {
                    $tr.fadeIn(150);
                    $self.removeClass('plus').addClass('minus');
                } else {
                    if ($self.hasClass('minus')) {
                        $self.removeClass('minus').addClass('plus');
                        $tr.fadeOut(150);
                    }
                }
            }
        },

        hierarchyHandler: function (event) {
            event.preventDefault();
            var $self = $(event.currentTarget);
            var $tr = $self.parents('tr:first').next();
            if ($self.hasClass('plus') && !$self.data('initialized')) {
                var url = $self.attr('href');
                var context = this;
                if (url) {

                    var $progress = $(this.progressSelector).removeClass('refresh').addClass('loading');
                    var $td = $tr.find('td.grid-hierarchy-row');
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'html',
                        complete: function () {
                            $progress.removeClass('loading').addClass('refresh');
                        },
                        success: function (data) {
                            $td.append(data);
                            $self.data('initialized', true);
                            $tr.fadeIn(150);
                            $self.removeClass('plus').addClass('minus');
                            if (context.options.expandHierarchySuccess) {
                                context.options.expandHierarchySuccess($td);
                            }
                        }
                    });
                }
            } else {
                if ($self.hasClass('plus')) {
                    $tr.fadeIn(150);
                    $self.removeClass('plus').addClass('minus');
                } else {
                    if ($self.hasClass('minus')) {
                        $self.removeClass('minus').addClass('plus');
                        $tr.fadeOut(150);
                    }
                }
            }

            return false;
        },

        initGrouping: function () {
            var $groupcolumns = $(this.groupColumnSelector);
            var $groupHeader = $(this.groupHeaderSelector);
            var context = this;
            var containerSelector = '#' + context.gridKey;

            $groupHeader.sortable({
                items: context.groupIndicatorSelector,
                appendTo: containerSelector,
                placeholder: "group-placeholder",
                distance: 5,
                tolerance: "pointer",
                forcePlaceholderSize: true,
                update: function (event, ui) {
                    var $self = $(this);
                    var groupColumnClass = 'js-' + context.gridKey + '-group-column';
                    if (ui.item.hasClass(groupColumnClass)) {
                        var groupIndicator = '<div data-key="' + ui.item.attr('data-key') + '" class="group-indicator"><a class="group-sort"><span class="grid-icon asc-group-sort "></span>' + ui.item.attr('data-title') + '</a><a class="group-drop-button"><span class="grid-icon"></span></a></div>';
                        ui.item.replaceWith(groupIndicator);
                    }

                    $self.children().each(function () {
                        var $item = $(this);
                        var index = $item.index();
                        var key = $item.find('[data-key]:first').attr('data-key') || $item.attr('data-key');
                        if (key) {
                            if (!context.gridOptions.ColOpt[key])
                                context.gridOptions.ColOpt[key] = {};

                            context.gridOptions.ColOpt[key].go = index;
                        }

                    });
                    context.reInitColumnOrder('go', context.gridOptions.ColOpt);

                    context.refresh();
                },
                helper: function (event, item) {
                    var $item = $(item);
                    var name = $item.find('a.group-sort').text();
                    return '<div class="group-draggable-header"><i></i>' + name + '</div>';
                },
                stop: function () {
                    var $self = $(this);
                    var $emptyText = $self.find('.grouping-header-empty');
                    if ($emptyText && $self.find('.default-group-indicator'))
                        $emptyText.hide();
                },
                over: function (event, ui) {
                    var $self = $(this);
                    var $emptyText = $self.find('.grouping-header-empty');
                    if ($emptyText)
                        $emptyText.hide();

                    if (ui.helper) {
                        ui.helper.css({ width: 'auto', height: 'auto' });
                        ui.helper.addClass('add');
                    }
                },
                out: function (event, ui) {
                    var $self = $(this);
                    var $emptyText = $self.find('.grouping-header-empty');
                    if ($emptyText)
                        $emptyText.show();
                    if (ui.helper) {
                        ui.helper.removeClass('add');
                    }
                }
            }).disableSelection();

            $groupcolumns.draggable({
                appendTo: containerSelector,
                connectToSortable: $groupHeader,
                helper: function () {
                    var $item = $(this);
                    var name = $item.attr('data-title');
                    return '<div class="group-draggable-header"><i></i>' + name + '</div>';
                }
            }).disableSelection();
        },

        refresh: function (options, success, complete) {
            var url = this.$grid.attr('data-action');
            options = this.addPrefix(options || this.gridOptions);

            var context = this;
            success = success || function (data) {
                var id = context.$grid.attr('id');
                context.$grid.replaceWith(data);
                context.$grid = $('#' + id);
                context.init();
            };
            var $progress = $(this.progressSelector).removeClass('refresh').addClass('loading');

            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(options),
                dataType: 'html',
                complete: function () {
                    $progress.removeClass('loading').addClass('refresh');
                    if (complete) {
                        complete();
                    }

                    context.$grid.trigger('sprint-grid-complete');
                },
                success: success
            });

        },

        addPrefix: function (options, prefix) {
            prefix = prefix || this.prefix;
            var prefixOptions = {};

            if (prefix) {
                for (var propertyName in options) {
                    var key = prefix + '.' + propertyName;
                    prefixOptions[key] = options[propertyName];
                }
                return prefixOptions;
            }
            return options;
        },

        getOptions: function (options) {
            var ops = $.extend({}, $.extend(true, {}, $.fn.sprintgrid.defaults), options, $('body').data(this.gridKey));

            return ops;
        },

        reInitColumnOrder: function (columnProperty, dictionary) {
            var columns = dictionary || this.gridOptions.Columns;
            var index = 0;

            var tuples = [];

            for (var key in columns)
                tuples.push([key, columns[key]]);

            tuples = tuples.sort(function (a, b) {
                var aValue = parseInt(a[1][columnProperty]);
                var bValue = parseInt(b[1][columnProperty]);
                if (!(aValue === 0 || aValue))
                    aValue = -1;

                if (!(bValue === 0 || bValue))
                    bValue = -1;

                if (aValue === bValue)
                    return 0;

                if (aValue > bValue)
                    return 1;
                else
                    return -1;
            });
            for (var i = 0; i < tuples.length; i++) {
                if (tuples[i][1][columnProperty] === 0 || tuples[i][1][columnProperty]) {
                    tuples[i][1][columnProperty] = index++;
                }
            }
        }

    };

    $.fn.sprintgrid = function (option) {
        if (typeof JSON !== 'object') {
            JSON = {};
        }

        JSON.stringify = JSON.stringify || function (obj) {
            var t = typeof (obj);
            if (t != "object" || obj === null) {
                if (t == "string") obj = '"' + obj + '"';
                return String(obj);
            }
            else {

                var n, v, json = [], arr = (obj && obj.constructor == Array);
                for (n in obj) {
                    v = obj[n]; t = typeof (v);
                    if (t == "string") v = '"' + v + '"';
                    else if (t == "object" && v !== null) v = JSON.stringify(v);
                    json.push((arr ? "" : '"' + n + '":') + String(v));
                }
                return (arr ? "[" : "{") + String(json) + (arr ? "]" : "}");
            }
        };

        return this.each(function () {

            var $this, data, options, $body;

            $body = $('body');
            $this = $(this);

            var gridKey = $this.attr("name");

            data = $body.data(gridKey);
            options = typeof option === 'object' && option;

            if (!data) {
                $body.data(gridKey, new SprintGrid(gridKey, this, options));
            }
        });
    };

    $.fn.sprintgrid.defaults = {
        expandHierarchySuccess: $.noop
    };

    $.fn.sprintgrid.Constructor = SprintGrid;

}(window.jQuery);