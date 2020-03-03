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

        handleClaimChange: function(claimCheckBox, claimValueInput, defaultValueInput) {
            var defaultValue = "";
            if (defaultValueInput != null) {
                defaultValue = defaultValueInput.value;
            }

            if (claimCheckBox.checked) {
                claimValueInput.readOnly = false;
                claimValueInput.value = defaultValue;
                return;
            }

            claimValueInput.value = defaultValue;
            claimValueInput.readOnly = true;
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
                    $(replaceElement)
                        .replaceWith("<div class=\"alert alert-danger\" role=\"alert\">" + errorThrown + "</div>");
                });
        }
    });

    $(document).ready(function() {
        $("[data-submenu]").submenupicker();
    });
})(jQuery);