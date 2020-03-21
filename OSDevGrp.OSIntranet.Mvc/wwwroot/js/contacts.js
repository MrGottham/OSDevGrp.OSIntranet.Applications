(function($) {
    $.fn.extend({
        getFilter: function() {
            return $("#contactOperationsFilter").val();
        },

        getCountryCode: function() {
            return $("#contactOperationsCountryCode").val();
        },

        newContact: function(countryCode) {
            alert("OS Debug: newContact, countryCode=" + countryCode);
        },

        getContact: function(contactUrl) {
            alert("OS Debug: getContact, contactUrl=" + contactUrl);
        },

        refreshContacts: function(refreshUrl) {
            var loadedContactCollectionElementArray = $("#loadedContactCollection");
            if (loadedContactCollectionElementArray.length === 0) {
                return;
            }

            $.each($("#contactOperationsFilter"), function() {
                $(this).val("");
            });

            $().startLoadingContacts(refreshUrl);
        },

        searchContacts: function(filter, searchUrl) {
            var url = decodeURI(searchUrl);
            if (filter === undefined || filter === null || filter.length === 0) {
                url = url.substr(0, url.indexOf("?"));
            } else {
                url = url.replace("{filter}", filter);
            }

            $().startLoadingContacts(encodeURI(url));
        },

        startLoadingContacts: function(url) {
            $().setReadOnly("#contactOperationsFilter", true);
            $().setDisabled("#contactOperationsSearch", true);

            $.each($("#contactCollection"), function() {
                $().startContactCollectionObserver(this);

                $.each($(this).children("#loadedContactCollection"), function() {
                    $(this).data("url", url);
                    $().replaceWithPartialViewFromUrl(this);
                });
            });
        },

        startContactCollectionObserver: function(element) {
            var contactCollectionObserver = new MutationObserver($().contactCollectionCallback);
            contactCollectionObserver.observe(element, { childList: true });
        },

        contactCollectionCallback: function(mutationsList, observer) {
            if (mutationsList.length === 0) {
                return;
            }

            var loadedContactCollectionElementArray = $(mutationsList[0].target).find("#loadedContactCollection");
            if (loadedContactCollectionElementArray.length === 0) {
                $().handleUrlInstantLoading($(document));
                return;
            }

            observer.disconnect();

            $().setReadOnly("#contactOperationsFilter", false);
            $().setDisabled("#contactOperationsSearch", false);

            $.each(loadedContactCollectionElementArray, function () {
                $.each($(this).find("a"), function() {
                    $(this).on("click", function (e) {
                        e.preventDefault();
                        $().getContact($(this).data("url"));
                    });
                });
            });
        }
    });

    $(document).ready(function() {
        $().startContactCollectionObserver(document.getElementById("contactCollection"));
    });
})(jQuery);