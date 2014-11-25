;(function ( $, window, document, undefined ) {
    'use strict';

    var pluginName = "selectboxsearch",
        defaults   = {
            delay : 100,
            bind : 'keyup',
        };

    function Plugin(element, target, options)
    {
        this.element = element;
        this.$element = $(element);
        this.target     = target;
        this.options    = $.extend({}, defaults, options);
        this._defaults  = defaults;
        this._name      = pluginName;

        this.vars = {
            optionRows: $(this.target).children().map(function () { return this;})
        };

        this.init();
    }

    Plugin.prototype = {
        init: function() {
            var self = this,
                delay = this.options.delay;

            this.$element.on(this.options.bind, function () {
                var timeout = window.setTimeout(function () {
                    self.go();
                }, delay);
                
            });
        },

        go: function() {
            var array = this.vars.optionRows,
                val = this.$element.val();

            // いったん削除
            $(this.target).children().remove();
            for (var i = 0, len = array.length; i < len; i++) {
                if(array[i]) {
                    var pos = array[i].innerHTML.indexOf(val,0);
                    // キーワードが空、もしくはヒットした場合要素追加
                    if((val.replace(' ', '').length === 0) || pos >= 0) {
                        $(this.target).append(array[i]);
                    }
                }
            }
        },

        additem: function(items) {
            var self = this,
                array = this.vars.optionRows,
                len = this.vars.optionRows.length;

            $.each(items, function(index, item) {
                var add = true;
                for (var i = 0, len; i < len; i++) {
                    if(item.value == array[i].value) {
                        add = false;
                    }
                }
                if(add == true) {
                    array.push(item);
                }
            });

            this.vars.optionRows = array;
            self.go();
        },

        delitem: function(items) {
            var self = this,
                array = [];

            $.each(this.vars.optionRows, function(index, item) {
                var del = false;
                for (var i = 0, len = items.length; i < len; i++) {
                    if(item.value == items[i].value) {
                        del = true;
                    }
                }
                if(del == false) {
                    array.push(item);
                }
            });

            this.vars.optionRows = array;
            self.go();
        }
    };

    $.fn[pluginName] = function(target, options)
    {
        return this.each(function()
        {
            if(!$.data(this, "plugin_" + pluginName))
            {
                $.data(this, "plugin_" + pluginName, new Plugin($(this), target, options));
            }
        });
    };

})( jQuery, window, document );

