(function($) {
    $.fn.extend({
        newContact: function(defaultCountryCode) {
            alert('OS Debug: newContact, defaultCountryCode=' + defaultCountryCode)
        },

        searchContacts: function(filter, defaultCountryCode) {
            alert('OS Debug: searchContacts, filter=' + filter + ', defaultCountryCode=' + defaultCountryCode);
        }
    });
})(jQuery);