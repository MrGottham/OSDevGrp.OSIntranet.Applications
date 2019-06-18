(function($) {
    $.fn.extend({
        askForDeletion: function(header, deleteUrl, deleteData, removeElement) {
            if (confirm(header + '\r\n\r\nEr du sikker?')) {
                $.post(deleteUrl, deleteData, null, 'html')
                    .done(function() {
                        removeElement.remove();
                    })
                    .fail(function(jqXHR, textStatus, errorThrown) {
                        alert(errorThrown);
                    });
            }
        },

        handleClaimChange: function(claimCheckBox, claimValueInput, defaultValueInput) {
            var defaultValue = '';
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
        }
    });
})(jQuery);