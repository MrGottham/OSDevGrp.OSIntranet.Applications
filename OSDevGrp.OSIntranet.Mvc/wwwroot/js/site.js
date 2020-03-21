(function($) {
    $.fn.extend({
        askForDeletion: function(header, deleteUrl, deleteData, removeElement) {
            if (confirm(header + "\r\n\r\nEr du sikker?")) {
                $.post(deleteUrl, deleteData, null, "html")
                    .done(function() {
                        removeElement.remove();
                    })
                    .fail(function(jqXhr, textStatus, errorThrown) {
                        alert(errorThrown);
                    });
            }
        },

        handleUrlInstantLoading: function(document) {
            var elementArray = $.makeArray(document.find("[data-url-instant-loading]"));
            $.each(elementArray, function() {
                var replaceElementArray = $(this).find("[data-url]");
                if (replaceElementArray.length === 0) {
                    return;
                }

                $.each(replaceElementArray, function() {
                    $().replaceWithPartialViewFromUrl($(this));
                });
            });
        },

        handleUrlCollapseLoading: function(document) {
            var elementArray = $.makeArray(document.find("[data-url-collapse-loading]"));
            $.each(elementArray, function () {
                var replaceElementArray = $(this).find("[data-url]");
                if (replaceElementArray.length === 0) {
                    return;
                }

                $(this).on("shown.bs.collapse", function() {
                    $.each(replaceElementArray, function() {
                        $().replaceWithPartialViewFromUrl($(this));
                    });
                });
            });
        },

        replaceWithPartialViewFromUrl: function(replaceElement) {
            var url = $(replaceElement).data("url");
            if (url == null) {
                return;
            }

            $.get(url, null, null, "html")
                .done(function(data) {
                    $(replaceElement).replaceWith(data);
                })
                .fail(function(jqXhr, textStatus, errorThrown) {
                    $(replaceElement).replaceWith("<div class=\"alert alert-danger\" role=\"alert\">" + errorThrown + "</div>");
                });
        },

        setReadOnly: function(elementId, readOnly) {
            var elementArray = $(document).find(elementId);
            if (elementArray.length === 0) {
                return;
            }

            $.each(elementArray, function() {
                $(this).prop("readonly", readOnly);
            });
        },

        setDisabled: function(elementId, disabled) {
            var elementArray = $(document).find(elementId);
            if (elementArray.length === 0) {
                return;
            }

            $.each(elementArray, function() {
                $(this).prop("disabled", disabled);
            });
        }
    });

    $(document).ready(function() {
        $("[data-submenu]").submenupicker();

        $().handleUrlInstantLoading($(document));
        $().handleUrlCollapseLoading($(document));
    });
})(jQuery);