(function($) {
    var onPostalCodeChangeTimer = undefined;

    $.fn.extend({
        getFilter: function() {
            return $("#contactOperationsFilter").val();
        },

        getCountryCode: function() {
            return $("#contactOperationsCountryCode").val();
        },

        newContact: function(createContactUrl) {
            var presentContactElement = $().getPresentContactElement();
            if (presentContactElement === null) {
                return;
            }

            createContactUrl = decodeURI(createContactUrl).replace("{countryCode}", $().getCountryCode());
            $(presentContactElement).data("url", encodeURI(createContactUrl));

            $.each($(presentContactElement).parent(),
                function() {
                    $().startPresentingContactObserver(this);
                });

            $().replaceWithPartialViewFromUrl(presentContactElement);
        },

        getContact: function(contactUrl) {
            var presentContactElement = $().getPresentContactElement();
            if (presentContactElement === null) {
                return;
            }

            contactUrl = decodeURI(contactUrl).replace("{countryCode}", $().getCountryCode());
            $(presentContactElement).data("url", encodeURI(contactUrl));

            $.each($(presentContactElement).parent(),
                function() {
                    $().startPresentingContactObserver(this);
                });

            $().replaceWithPartialViewFromUrl(presentContactElement);
        },

        editContact: function() {
            if ($().isDisplayed("#editContact")) {
                return;
            }

            $().toggleDisplay("#presentContact");
            $().toggleDisplay("#editContact");
        },

        refreshContacts: function(refreshUrl) {
            var loadedContactCollectionElementArray = $("#loadedContactCollection");
            if (loadedContactCollectionElementArray.length === 0) {
                return;
            }

            $.each($("#contactOperationsFilter"),
                function() {
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

            $.each($("#contactCollection"),
                function() {
                    $().startContactCollectionObserver(this);

                    $.each($(this).children("#loadedContactCollection"),
                        function() {
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

            $.each(loadedContactCollectionElementArray,
                function() {
                    $.each($(this).find("a"),
                        function() {
                            $(this).on("click",
                                function(e) {
                                    e.preventDefault();
                                    $(this).tab("show");
                                });

                            $(this).on("shown.bs.tab",
                                function(e) {
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
            $.each($(element).children("div"),
                function() {
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

                if ($().isPerson()) {
                    $().toggleDisplay("#editContactPersonName");
                    $().toggleDisplay("#editContactPersonPhoneNumbers");
                } else if ($().isCompany()) {
                    $().toggleDisplay("#editContactCompanyName");
                    $().toggleDisplay("#editContactCompanyPhoneNumbers");
                }

                $().enableFormValidation("#editContactForm");

                return;
            }

            $.each(loadContactElementArray,
                function() {
                    $().replaceWithPartialViewFromUrl(this);
                });
        },

        isPerson: function() {
            return $().getRadioValue("#ContactType") === "Person";
        },

        applyPersonContent: function() {
            if ($().isDisplayed("#editContactPersonName")) {
                return;
            }

            if ($().isDisplayed("#editContactCompanyName")) {
                var companyName = $("#CompanyName").val();
                if (companyName !== undefined && companyName !== null && companyName.length > 0) {
                    $("#Surname").val(companyName);
                };
                $("#CompanyName").val("");

                $().toggleDisplay("#editContactCompanyName");
            }

            if ($().isDisplayed("#editContactCompanyPhoneNumbers")) {
                $().toggleDisplay("#editContactCompanyPhoneNumbers");
            }

            $().toggleDisplay("#editContactPersonName");

            if ($().isDisplayed("#editContactPersonPhoneNumbers") === false) {
                $().toggleDisplay("#editContactPersonPhoneNumbers");
            }
        },

        isCompany: function() {
            return $().getRadioValue("#ContactType") === "Company";
        },

        applyCompanyContent: function() {
            if ($().isDisplayed("#editContactCompanyName")) {
                return;
            }

            if ($().isDisplayed("#editContactPersonName")) {
                var companyName = "";

                var givenName = $("#GivenName").val();
                if (givenName !== undefined && givenName !== null && givenName.length > 0) {
                    companyName = givenName;
                };
                $("#GivenName").val("");

                var middleName = $("#MiddleName").val();
                if (middleName !== undefined && middleName !== null && middleName.length > 0) {
                    if (companyName.length > 0) {
                        companyName = companyName + " " + middleName;
                    } else {
                        companyName = middleName;
                    }
                }
                $("#MiddleName").val("");

                var surname = $("#Surname").val();
                if (surname !== undefined && surname !== null && surname.length > 0) {
                    if (companyName.length > 0) {
                        companyName = companyName + " " + surname;
                    } else {
                        companyName = surname;
                    }
                }
                $("#Surname").val("");

                $("#CompanyName").val(companyName);

                $().toggleDisplay("#editContactPersonName");
            }

            if ($().isDisplayed("#editContactPersonPhoneNumbers")) {
                $().toggleDisplay("#editContactPersonPhoneNumbers");
            }

            $().toggleDisplay("#editContactCompanyName");

            if ($().isDisplayed("#editContactCompanyPhoneNumbers") === false) {
                $().toggleDisplay("#editContactCompanyPhoneNumbers");
            }
        },

        onPostalCodeChange: function(postalCodeElementId, cityElementId, stateElementId, countryElementId, resolveUrl) {
            if (onPostalCodeChangeTimer !== undefined) {
                clearTimeout(onPostalCodeChangeTimer);
            }

            onPostalCodeChangeTimer = setTimeout(function() {
                    var countryCode = $().getCountryCode();
                    if (countryCode === undefined || countryCode === null || countryCode.length === 0) {
                        return;
                    }

                    var postalCode = $(postalCodeElementId).val();
                    if (postalCode === undefined || postalCode === null || postalCode.length === 0) {
                        return;
                    }

                    resolveUrl = encodeURI(decodeURI(resolveUrl)
                        .replace("{countryCode}", countryCode)
                        .replace("{postalCode}", postalCode));

                    $.get(resolveUrl, null, null, "json")
                        .done(function(jsonData) {
                            $(cityElementId).val(jsonData.city);
                            $(stateElementId).val(jsonData.state);
                            if (jsonData.country.defaultForPrincipal) {
                                $(countryElementId).val("");
                            } else {
                                $(countryElementId).val(jsonData.country.universalName);
                            }
                        });
                },
                500);
        }
    });

    $(document).ready(function() {
        $().startContactCollectionObserver(document.getElementById("contactCollection"));
    });
})(jQuery);