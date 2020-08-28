(function($) {
    $.fn.extend({
        askForDeletion: function(header, deleteUrl, deleteData, removeElement = null, presentResult = false) {
            if (confirm(header + "\r\n\r\nEr du sikker?")) {
                $.post(deleteUrl, deleteData, null, "html")
                    .done(function(html) {
                        if (removeElement !== undefined && removeElement !== null) {
                            removeElement.remove();
                        }

                        if (presentResult !== undefined && presentResult !== null && presentResult) {
                            var newDocument = document.open("text/html", "replace");
                            newDocument.write(html);
                            newDocument.close();
                        }
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
            $.each(elementArray, function() {
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
        },

        toggleDisplay: function(elementId) {
            var elementArray = $(document).find(elementId);
            if (elementArray.length === 0) {
                return;
            }

            $.each(elementArray, function() {
                if ($(this).hasClass("d-none")) {
                    $(this).addClass("d-block").removeClass("d-none").show();
                    return;
                }

                if ($(this).hasClass("d-block")) {
                    $(this).addClass("d-none").removeClass("d-block").hide();
                }
            });
        },

        isDisplayed: function(elementId) {
            return $(elementId).hasClass("d-block");
        },

        getRadioValue: function(elementId) {
            if ($(document).find(elementId) === 0) {
                return null;
            }

            var selectedValue = $(document).find(elementId + ":checked").val();
            if (selectedValue === undefined) {
                return null;
            }

            return selectedValue;
        },

        enableFormValidation: function(formId) {
            var formArray = $(document).find(formId);
            if (formArray.length === 0) {
                return;
            }

            $.each(formArray, function() {
                $(this).removeData("validator");
                $(this).removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse($(this));
            });
        },

        applyValue: function(elementId, value) {
            $(elementId).val(value);
        }
    });

    $(document).ready(function() {
        $("[data-submenu]").submenupicker();

        $().handleUrlInstantLoading($(document));
        $().handleUrlCollapseLoading($(document));
    });
})(jQuery);