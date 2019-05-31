(function($) {
    $.fn.extend({
        askForDeletion: function(header, deletionForm) {
            if (confirm(header + '\r\n\r\nEr du sikker?'))
            {
                deletionForm.submit();
            }
        },

        handleClaimChange: function(claimCheckBox, claimValueInput, defaultValueInput) {
            defaultValue = '';
            if (defaultValueInput != null)
            {
                defaultValue = defaultValueInput.value;
            }

            if (claimCheckBox.checked)
            {
                claimValueInput.readOnly = false;
                claimValueInput.value = defaultValue;
                return;
            }

            claimValueInput.value = defaultValue;
            claimValueInput.readOnly = true;
        }
    });
})(jQuery);