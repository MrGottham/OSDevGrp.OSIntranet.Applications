(function($) {
    $.fn.extend({
        getDefaultAccountingNumber: function() {
            var defaultAccountingNumber = $("#accountingOptionsAccountingNumber").val();
            if (defaultAccountingNumber.length === 0) {
                return null;
            }

            return parseInt(defaultAccountingNumber);
        },

        getActiveAccountingNumber: function() {
            var accountingNumberElementArray = $("#loadedAccountingCollection").children("[data-accounting-number]");
            if (accountingNumberElementArray.length === 0) {
                return null;
            }

            var activeAccountingNumber = null;
            $.each(accountingNumberElementArray, function() {
                if ($(this).hasClass("active") && activeAccountingNumber === null) {
                    activeAccountingNumber = parseInt($(this).data("accounting-number"));
                }
            });

            return activeAccountingNumber;
        },

        getActiveOrDefaultAccountingNumber: function() {
            var accountingNumber = $().getActiveAccountingNumber();
            if (accountingNumber !== undefined && accountingNumber !== null) {
                return accountingNumber;
            }

            return $().getDefaultAccountingNumber();
        },

        getActiveAccountingUrl: function() {
            var accountingNumberElementArray = $("#loadedAccountingCollection").children("[data-url]");
            if (accountingNumberElementArray.length === 0) {
                return null;
            }

            var activeAccountingUrl = null;
            $.each(accountingNumberElementArray, function() {
                if ($(this).hasClass("active") && activeAccountingUrl === null) {
                    activeAccountingUrl = $(this).data("url");
                }
            });

            return activeAccountingUrl;
        },

        newAccounting: function(createAccountingUrl) {
            var presentAccountingElement = $().getPresentAccountingElement();
            if (presentAccountingElement === null) {
                return;
            }

            $(presentAccountingElement).data("url", encodeURI(createAccountingUrl));

            $.each($(presentAccountingElement).parent(), function() {
                $().startPresentingAccountingObserver(this);
            });

            $().replaceWithPartialViewFromUrl(presentAccountingElement);
        },

        getAccounting: function(accountingUrl) {
            var presentAccountingElement = $().getPresentAccountingElement();
            if (presentAccountingElement === null) {
                return;
            }

            $(presentAccountingElement).data("url", encodeURI(accountingUrl));

            $.each($(presentAccountingElement).parent(), function() {
                $().startPresentingAccountingObserver(this);
            });

            $().replaceWithPartialViewFromUrl(presentAccountingElement);
        },

        editAccounting: function() {
            if ($().isDisplayed("#editAccounting")) {
                return;
            }

            $().toggleDisplay("#presentAccounting");
            $().toggleDisplay("#editAccounting");
        },

        refreshAccountings: function(refreshUrl) {
            var loadedAccountingCollectionElementArray = $("#loadedAccountingCollection");
            if (loadedAccountingCollectionElementArray.length === 0) {
                return;
            }

            var accountingNumber = $().getActiveOrDefaultAccountingNumber();

            var url = decodeURI(refreshUrl);
            if (accountingNumber === undefined || accountingNumber === null) {
                url = url.substr(0, url.indexOf("?"));
            } else {
                url = url.replace("{accountingNumber}", accountingNumber);
            }

            $().startLoadingAccountings(encodeURI(url));
        },

        startLoadingAccountings: function(url) {
            $.each($("#accountingCollection"), function() {
                $().startAccountingCollectionObserver(this);

                $.each($(this).children("#loadedAccountingCollection"), function() {
                    $(this).data("url", url);
                    $().replaceWithPartialViewFromUrl(this);
                });
            });
        },

        startAccountingCollectionObserver: function(element) {
            var accountingCollectionObserver = new MutationObserver($().accountingCollectionCallback);
            accountingCollectionObserver.observe(element, { childList: true });
        },

        accountingCollectionCallback: function(mutationsList, observer) {
            if (mutationsList.length === 0) {
                return;
            }

            var loadedAccountingCollectionElementArray = $(mutationsList[0].target).find("#loadedAccountingCollection");
            if (loadedAccountingCollectionElementArray.length === 0) {
                $().handleUrlInstantLoading($(document));
                return;
            }

            observer.disconnect();

            var rightContentIsHidden = $("#rightContent").is(":hidden");

            $.each(loadedAccountingCollectionElementArray, function() {
                $.each($(this).find("a"), function() {
                    if (rightContentIsHidden === true && $(this).hasClass("active")) {
                        $(this).removeClass("active");
                    }

                    $(this).on("click", function(e) {
                        e.preventDefault();
                        $(this).tab("show");
                    });

                    $(this).on("shown.bs.tab", function(e) {
                        $().getAccounting($(e.target).data("url"));
                    });
                });
            });

            if (rightContentIsHidden === true) {
                return;
            }

            var activeAccountingUrl = $().getActiveAccountingUrl();
            if (activeAccountingUrl === undefined || activeAccountingUrl === null) {
                return;
            }

            $().getAccounting(activeAccountingUrl);
        },

        getPresentAccountingElement: function() {
            var element = $("#rightContent");
            if (element.is(":hidden")) {
                element = $("#leftContent");
            }

            var presentAccountingElement = null;

            var elementNo = 0;
            $.each($(element).children("div"), function() {
                if (elementNo !== 0) {
                    $(this).remove();
                    return;
                }

                presentAccountingElement = $(this);
                elementNo++;
            });

            return presentAccountingElement;
        },

        startPresentingAccountingObserver: function(element) {
            var presentingAccountingObserver = new MutationObserver($().presentingAccountingCallback);
            presentingAccountingObserver.observe(element, { childList: true });
        },

        presentingAccountingCallback: function(mutationsList, observer) {
            if (mutationsList.length === 0) {
                return;
            }

            var loadAccountingElementArray = $(mutationsList[0].target).find("[data-url]");
            if (loadAccountingElementArray.length === 0) {
                observer.disconnect();

                $().enableFormValidation("#editAccountingForm");

                return;
            }

            $.each(loadAccountingElementArray, function() {
                $().replaceWithPartialViewFromUrl(this);
            });
        }
    });

    $(document).ready(function() {
        $().startAccountingCollectionObserver(document.getElementById("accountingCollection"));
    });
})(jQuery);