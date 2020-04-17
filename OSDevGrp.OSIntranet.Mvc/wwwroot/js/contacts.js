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
            var presentContactElement = $().getPresentContactElement();
            if (presentContactElement === null) {
                return;
            }

            contactUrl = decodeURI(contactUrl).replace("{countryCode}", $().getCountryCode());
            $(presentContactElement).data("url", encodeURI(contactUrl));

            $.each($(presentContactElement).parent(), function() {
                $().startPresentingContactObserver(this);
            });

            $().replaceWithPartialViewFromUrl(presentContactElement);
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

            $.each(loadedContactCollectionElementArray, function() {
                $.each($(this).find("a"), function() {
                    $(this).on("click", function(e) {
                        e.preventDefault();
                        $(this).tab("show");
                    });

                    $(this).on("shown.bs.tab", function(e) {
                        $().getContact($(e.target).data("url"));
                    });
                });
            });
        },

        getPresentContactElement: function() {
            var element = $("#rightContent");
            if (element.is(":hidden")) {
                element = $("#leftContent");
            }

            var presentContactElement = null;

            var elementNo = 0;
            $.each($(element).children("div"), function() {
                if (elementNo !== 0) {
                    $(this).remove();
                    return;
                }

                presentContactElement = $(this);
                elementNo++;
            });

            return presentContactElement;
        },

        startPresentingContactObserver: function(element) {
            var presentingContactObserver = new MutationObserver($().presentingContactCallback);
            presentingContactObserver.observe(element, { childList: true });
        },

        presentingContactCallback: function(mutationsList, observer) {
            if (mutationsList.length === 0) {
                return;
            }

            var loadContactElementArray = $(mutationsList[0].target).find("[data-url]");
            if (loadContactElementArray.length === 0) {
                observer.disconnect();
                return;
            }

            $.each(loadContactElementArray, function () {
                $().replaceWithPartialViewFromUrl(this);
            });
        }
    });

    $(document).ready(function() {
        $().startContactCollectionObserver(document.getElementById("contactCollection"));
    });
})(jQuery);