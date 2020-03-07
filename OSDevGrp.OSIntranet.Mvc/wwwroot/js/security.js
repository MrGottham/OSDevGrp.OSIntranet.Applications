(function($) {
    $.fn.extend({
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
    });
})(jQuery);