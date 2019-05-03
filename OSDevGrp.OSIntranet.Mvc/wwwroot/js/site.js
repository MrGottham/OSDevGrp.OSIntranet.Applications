(function($) {
    $.fn.extend({
        askForDeletion: function(header, deletionForm) {
            if (confirm(header + '\r\n\r\nEr du sikker?'))
            {
                deletionForm.submit();
            }
        }
    });
})(jQuery);