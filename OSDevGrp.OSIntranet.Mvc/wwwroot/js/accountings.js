(function($) {
    var onPostingDateChangeTimer = undefined;
    var onAccountNumberChangeTimer = undefined;
    var onBudgetAccountNumberChangeTimer = undefined;
    var onContactAccountNumberChangeTimer = undefined;

    $.fn.extend({
        getDefaultAccountingNumber: function() {
            var defaultAccountingNumber = $("#accountingOperations").find("#DefaultAccountingNumber").val();
            if (defaultAccountingNumber === undefined ||
                defaultAccountingNumber === null ||
                defaultAccountingNumber.length === 0) {
                return null;
            }

            return parseInt(defaultAccountingNumber);
        },

        getUrlForDefaultAccountingNumber: function() {
            var urlForDefaultAccountingNumber =
                $("#accountingOperations").find("#DefaultAccountingNumber").parent().data("url");
            if (urlForDefaultAccountingNumber === undefined ||
                urlForDefaultAccountingNumber === null ||
                urlForDefaultAccountingNumber.length === 0) {
                return null;
            }

            return urlForDefaultAccountingNumber;
        },

        getActiveAccountingNumber: function() {
            var accountingNumberElementArray = $("#loadedAccountingCollection").children("[data-accounting-number]");
            if (accountingNumberElementArray.length === 0) {
                return null;
            }

            var activeAccountingNumber = null;
            $.each(accountingNumberElementArray,
                function() {
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

        newAccounting: function(createAccountingUrl) {
            var presentAccountingElement = $().getPresentAccountingElement();
            if (presentAccountingElement === null) {
                return;
            }

            $(presentAccountingElement).data("url", encodeURI(createAccountingUrl));

            $.each($(presentAccountingElement).parent(),
                function() {
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

            $.each($(presentAccountingElement).parent(),
                function() {
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

        cancelEditAccounting: function() {
            if ($("#editAccounting").length === 0) {
                $("#editAccountingForm").remove();

                return;
            }

            if ($().isDisplayed("#editAccounting") === false) {
                return;
            }

            $().toggleDisplay("#editAccounting");
            $().toggleDisplay("#presentAccounting");
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

        newAccount: function(createAccountUrl) {
            $().startAccountPresentation(createAccountUrl);
        },

        editAccount: function(editAccountUrl) {
            $().startAccountPresentation(editAccountUrl);
        },

        startLoadingAccountings: function(url) {
            $.each($("#accountingCollection"),
                function() {
                    $().startAccountingCollectionObserver(this);

                    $.each($(this).children("#loadedAccountingCollection"),
                        function() {
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

            var rightContentIsHidden = $().isRightContentHidden();

            $.each(loadedAccountingCollectionElementArray,
                function() {
                    $.each($(this).find("a"),
                        function() {
                            if (rightContentIsHidden === true && $(this).hasClass("active")) {
                                $(this).removeClass("active");
                            }

                            $(this).on("click",
                                function(e) {
                                    e.preventDefault();
                                    $(this).tab("show");
                                });

                            $(this).on("shown.bs.tab",
                                function(e) {
                                    $().getAccounting($(e.target).data("url"));
                                });
                        });
                });
        },

        isRightContentHidden: function() {
            return $("#rightContent").is(":hidden");
        },

        getPresentAccountingElement: function() {
            var element = $("#rightContent");
            if (element.is(":hidden")) {
                element = $("#leftContent");
            }

            var presentAccountingElement = null;

            var elementNo = 0;
            $.each($(element).children("div"),
                function() {
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

                $(".input-group.date").datepicker({
                    format: "dd-mm-yyyy",
                    clearBtn: false,
                    todayBtn: true,
                    language: "da"
                });

                $().enableFormValidation("#editAccountingForm");
                $().enableFormValidation("#addPostingLineToPostingJournalForm");

                return;
            }

            $.each(loadAccountingElementArray,
                function() {
                    $().replaceWithPartialViewFromUrl(this);
                });
        },

        startAccountPresentation: function(accountPresentationUrl) {
            var presentAccountElement = $().getPresentAccountingElement();
            if (presentAccountElement === null) {
                return;
            }

            $(presentAccountElement).data("url", encodeURI(accountPresentationUrl));

            $.each($(presentAccountElement).parent(),
                function() {
                    $().startAccountPresentationObserver(this);
                });

            $().replaceWithPartialViewFromUrl(presentAccountElement);
        },

        startAccountPresentationObserver: function(element) {
            var accountPresentationObserver = new MutationObserver($().accountPresentationCallback);
            accountPresentationObserver.observe(element, { childList: true });
        },

        accountPresentationCallback: function(mutationsList, observer) {
            if (mutationsList.length === 0) {
                return;
            }

            var loadAccountElementArray = $(mutationsList[0].target).find("[data-url]");
            if (loadAccountElementArray.length === 0) {
                observer.disconnect();

                $().enableFormValidation("#editAccountForm");

                return;
            }

            $.each(loadAccountElementArray,
                function() {
                    $().replaceWithPartialViewFromUrl(this);
                });
        },

        startAddingToPostingJournal: function(accountingNumber, postingJournalKey) {
            $("#addPostingLineToPostingJournalModal").on("shown.bs.modal",
                function() {
                    $("#addPostingLineToPostingJournalForm").attr("data-accounting-number", accountingNumber);
                    $("#addPostingLineToPostingJournalForm").attr("data-posting-journal-key", postingJournalKey);
                });

            $("#addPostingLineToPostingJournalModal").modal("show");
        },

        addToPostingJournal: function(postingJournalHeaderElementId, identifierElementId, postingDateElementId, referenceElementId, accountNumberElementId, detailsElementId, budgetAccountNumberElementId, debitElementId, creditElementId, contactAccountNumberElementId, sortOrderElementId, addPostingLineToPostingJournalUrl, addPostingLineToPostingJournalData) {
            var disablePostingJournalElements = function(disable) {
                $().setReadOnly(postingDateElementId, disable);
                $().setDisabled(postingDateElementId, disable);
                $().setReadOnly(referenceElementId, disable);
                $().setDisabled(referenceElementId, disable);
                $().setReadOnly(accountNumberElementId, disable);
                $().setDisabled(accountNumberElementId, disable);
                $().setReadOnly(detailsElementId, disable);
                $().setDisabled(detailsElementId, disable);
                $().setReadOnly(budgetAccountNumberElementId, disable);
                $().setDisabled(budgetAccountNumberElementId, disable);
                $().setReadOnly(debitElementId, disable);
                $().setDisabled(debitElementId, disable);
                $().setReadOnly(creditElementId, disable);
                $().setDisabled(creditElementId, disable);
                $().setReadOnly(contactAccountNumberElementId, disable);
                $().setDisabled(contactAccountNumberElementId, disable);
                $().setDisabled("#addToPostingJournalButton", disable);
            }

            disablePostingJournalElements(true);

            var validator = $("#addPostingLineToPostingJournalForm").validate();

            if (validator.checkForm() === false) {
                validator.showErrors();

                disablePostingJournalElements(false);

                return;
            }

            var accountingNumber = $("#addPostingLineToPostingJournalForm").data("accounting-number");
            var postingJournalKey = $("#addPostingLineToPostingJournalForm").data("posting-journal-key");

            addPostingLineToPostingJournalUrl = encodeURI(decodeURI(addPostingLineToPostingJournalUrl)
                .replace("{accountingNumber}", accountingNumber));

            addPostingLineToPostingJournalData.postingJournalKey = postingJournalKey;
            addPostingLineToPostingJournalData.postingLine = JSON.stringify({
                Identifier: $().resolveElementValue(identifierElementId),
                PostingDate: $().resolvePostingDateAsISOString(postingDateElementId),
                Reference: $().resolveElementValue(referenceElementId),
                AccountNumber: $().resolveAccountNumber(accountNumberElementId),
                Details: $().resolveElementValue(detailsElementId),
                BudgetAccountNumber: $().resolveAccountNumber(budgetAccountNumberElementId),
                Debit: $().resolvePostingValue(debitElementId),
                Credit: $().resolvePostingValue(creditElementId),
                ContactAccountNumber: $().resolveAccountNumber(contactAccountNumberElementId),
                SortOrder: $().resolveSortOrder(sortOrderElementId)
            });
            addPostingLineToPostingJournalData.postingJournalHeader = $().resolveElementText(postingJournalHeaderElementId);

            $.post(addPostingLineToPostingJournalUrl, addPostingLineToPostingJournalData, null, "html")
                .done(function(html) {
                    disablePostingJournalElements(false);

                    $("#addPostingLineToPostingJournalModal").on("hidden.bs.modal", function() {
                        $("#postingJournal").replaceWith(html);
                    });

                    $("#addPostingLineToPostingJournalModal").modal("hide");
                })
                .fail(function(jqXhr, textStatus, errorThrown) {
                    disablePostingJournalElements(false);

                    if (errorThrown === undefined || errorThrown === null || errorThrown.length === 0) {
                        return;
                    }

                    alert(errorThrown);
                });
        },

        removeFromPostingJournal: function(accountingNumber, postingJournalKey, postingLineIdentifier, postingJournalHeaderElementId) {
            alert("OS Debug: removeFromPostingJournal, accountingNumber=" + accountingNumber + ", postingJournalKey=" + postingJournalKey + ", postingLineIdentifier=" + postingLineIdentifier + ", postingJournalHeaderElementId=" + postingJournalHeaderElementId);
        },

        onPostingDataChange: function(postingDateElementId, accountNumberElementId, accountNameElementId, accountCreditElementId, accountAvailableElementId, resolveAccountUrl, budgetAccountNumberElementId, budgetAccountNameElementId, budgetAccountPostedElementId, budgetAccountAvailableElementId, resolveBudgetAccountUrl, contactAccountNumberElementId, contactAccountNameElementId, contactAccountBalanceElementId, resolveContactAccountUrl) {
            if (onPostingDateChangeTimer !== undefined) {
                clearTimeout(onPostingDateChangeTimer);
            }

            onPostingDateChangeTimer = setTimeout(function() {
                    var accountingNumber = $("#addPostingLineToPostingJournalForm").data("accounting-number");
                    var postingDate = $().resolvePostingDateAsISOString(postingDateElementId);
                    var accountNumber = $().resolveAccountNumber(accountNumberElementId);
                    var budgetAccountNumber = $().resolveAccountNumber(budgetAccountNumberElementId);
                    var contactAccountNumber = $().resolveAccountNumber(contactAccountNumberElementId);

                    $().resolveAccount(accountingNumber, postingDate, accountNumber, accountNameElementId, accountCreditElementId, accountAvailableElementId, resolveAccountUrl);
                    $().resolveBudgetAccount(accountingNumber, postingDate, budgetAccountNumber, budgetAccountNameElementId, budgetAccountPostedElementId, budgetAccountAvailableElementId, resolveBudgetAccountUrl);
                    $().resolveContactAccount(accountingNumber, postingDate, contactAccountNumber, contactAccountNameElementId, contactAccountBalanceElementId, resolveContactAccountUrl);
                },
                500);
        },

        onAccountNumberChange: function(postingDateElementId, accountNumberElementId, accountNameElementId, accountCreditElementId, accountAvailableElementId, resolveAccountUrl) {
            if (onAccountNumberChangeTimer !== undefined) {
                clearTimeout(onAccountNumberChangeTimer);
            }

            onAccountNumberChangeTimer = setTimeout(function() {
                    var accountingNumber = $("#addPostingLineToPostingJournalForm").data("accounting-number");
                    var postingDate = $().resolvePostingDateAsISOString(postingDateElementId);
                    var accountNumber = $().resolveAccountNumber(accountNumberElementId);

                    $().resolveAccount(accountingNumber, postingDate, accountNumber, accountNameElementId, accountCreditElementId, accountAvailableElementId, resolveAccountUrl);
                },
                500);
        },

        resolveAccount: function(accountingNumber, postingDate, accountNumber, accountNameElementId, accountCreditElementId, accountAvailableElementId, resolveAccountUrl) {
            $(accountNameElementId).val("");
            $(accountCreditElementId).val("");
            $(accountAvailableElementId).val("");

            if (accountingNumber === undefined || accountingNumber === null) {
                return;
            }

            if (postingDate === undefined || postingDate === null || postingDate.length === 0) {
                return;
            }

            if (accountNumber === undefined || accountNumber === null || accountNumber.length === 0) {
                return;
            }

            resolveAccountUrl = encodeURI(decodeURI(resolveAccountUrl)
                .replace("{accountingNumber}", accountingNumber)
                .replace("{accountNumber}", accountNumber)
                .replace("{statusDate}", postingDate));

            $.get(resolveAccountUrl, null, null, "json")
                .done(function(jsonData) {
                    $(accountNameElementId).val(jsonData.accountName);
                    $(accountCreditElementId).val($().currencyFormat(jsonData.valuesAtStatusDate.credit));
                    $(accountAvailableElementId).val($().currencyFormat(jsonData.valuesAtStatusDate.available));
                });
        },

        onBudgetAccountNumberChange: function(postingDateElementId, budgetAccountNumberElementId, budgetAccountNameElementId, budgetAccountPostedElementId, budgetAccountAvailableElementId, resolveBudgetAccountUrl) {
            if (onBudgetAccountNumberChangeTimer !== undefined) {
                clearTimeout(onBudgetAccountNumberChangeTimer);
            }

            onBudgetAccountNumberChangeTimer = setTimeout(function() {
                    var accountingNumber = $("#addPostingLineToPostingJournalForm").data("accounting-number");
                    var postingDate = $().resolvePostingDateAsISOString(postingDateElementId);
                    var budgetAccountNumber = $().resolveAccountNumber(budgetAccountNumberElementId);

                    $().resolveBudgetAccount(accountingNumber, postingDate, budgetAccountNumber, budgetAccountNameElementId, budgetAccountPostedElementId, budgetAccountAvailableElementId, resolveBudgetAccountUrl);
                },
                500);
        },

        resolveBudgetAccount: function(accountingNumber, postingDate, budgetAccountNumber, budgetAccountNameElementId, budgetAccountPostedElementId, budgetAccountAvailableElementId, resolveBudgetAccountUrl) {
            $(budgetAccountNameElementId).val("");
            $(budgetAccountPostedElementId).val("");
            $(budgetAccountAvailableElementId).val("");

            if (accountingNumber === undefined || accountingNumber === null) {
                return;
            }

            if (postingDate === undefined || postingDate === null || postingDate.length === 0) {
                return;
            }

            if (budgetAccountNumber === undefined || budgetAccountNumber === null || budgetAccountNumber.length === 0) {
                return;
            }

            resolveBudgetAccountUrl = encodeURI(decodeURI(resolveBudgetAccountUrl)
                .replace("{accountingNumber}", accountingNumber)
                .replace("{accountNumber}", budgetAccountNumber)
                .replace("{statusDate}", postingDate));

            $.get(resolveBudgetAccountUrl, null, null, "json")
                .done(function(jsonData) {
                    $(budgetAccountNameElementId).val(jsonData.accountName);
                    $(budgetAccountPostedElementId).val($().currencyFormat(jsonData.valuesForMonthOfStatusDate.posted));
                    $(budgetAccountAvailableElementId).val($().currencyFormat(jsonData.valuesForMonthOfStatusDate.available));
                });
        },

        onContactAccountNumberChange: function(postingDateElementId, contactAccountNumberElementId, contactAccountNameElementId, contactAccountBalanceElementId, resolveContactAccountUrl) {
            if (onContactAccountNumberChangeTimer !== undefined) {
                clearTimeout(onContactAccountNumberChangeTimer);
            }

            onContactAccountNumberChangeTimer = setTimeout(function() {
                    var accountingNumber = $("#addPostingLineToPostingJournalForm").data("accounting-number");
                    var postingDate = $().resolvePostingDateAsISOString(postingDateElementId);
                    var contactAccountNumber = $().resolveAccountNumber(contactAccountNumberElementId);

                    $().resolveContactAccount(accountingNumber, postingDate, contactAccountNumber, contactAccountNameElementId, contactAccountBalanceElementId, resolveContactAccountUrl);
                },
                500);
        },

        resolveContactAccount: function(accountingNumber, postingDate, contactAccountNumber, contactAccountNameElementId, contactAccountBalanceElementId, resolveContactAccountUrl) {
            $(contactAccountNameElementId).val("");
            $(contactAccountBalanceElementId).val("");

            if (accountingNumber === undefined || accountingNumber === null) {
                return;
            }

            if (postingDate === undefined || postingDate === null || postingDate.length === 0) {
                return;
            }

            if (contactAccountNumber === undefined || contactAccountNumber === null || contactAccountNumber.length === 0) {
                return;
            }

            resolveContactAccountUrl = encodeURI(decodeURI(resolveContactAccountUrl)
                .replace("{accountingNumber}", accountingNumber)
                .replace("{accountNumber}", contactAccountNumber)
                .replace("{statusDate}", postingDate));

            $.get(resolveContactAccountUrl, null, null, "json")
                .done(function(jsonData) {
                    $(contactAccountNameElementId).val(jsonData.accountName);
                    $(contactAccountBalanceElementId).val($().currencyFormat(jsonData.valuesAtStatusDate.balance));
                });
        },

        resolvePostingDateAsISOString: function(postingDateElementId) {
            var postingDate = $().resolvePostingDate(postingDateElementId);
            if (postingDate === undefined || postingDate === null) {
                return null;
            }

            return postingDate.toISOString();
        },

        resolvePostingDate: function(postingDateElementId) {
            var postingDate = $().resolveElementValue(postingDateElementId);
            if (postingDate === undefined || postingDate === null || postingDate.length === 0) {
                return null;
            }

            var postingDateRegExp = new RegExp("^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[012])-((19|20)[0-9]{2})$");
            if (postingDateRegExp.test(postingDate)) {
                return new Date(postingDate.substr(6, 4), postingDate.substr(3, 2) - 1, postingDate.substr(0, 2));
            }

            return null;
        },

        resolveAccountNumber: function(accountNumberElementId) {
            var accountNumber = $().resolveElementValue(accountNumberElementId);
            if (accountNumber === undefined || accountNumber === null || accountNumber.length === 0) {
                return null;
            }

            return accountNumber.toUpperCase();
        },

        resolvePostingValue: function(postingValueElementId) {
            var postingValue = $().resolveElementValue(postingValueElementId);
            if (postingValue === undefined || postingValue === null || postingValue.length === 0) {
                return null;
            }

            postingValue = postingValue.trim();
            var result = postingValue.replace(/[^0-9]/g, "");
            if (/[,\.]\d{2}$/.test(postingValue)) {
                result = result.replace(/(\d{2})$/, ".$1");
            }

            return parseFloat(parseFloat(result).toFixed(2));
        },

        resolveSortOrder: function(sortOrderElementId) {
            var sortOrder = $().resolveElementValue(sortOrderElementId);
            if (sortOrder === undefined || sortOrder === null || sortOrder.length === 0) {
                return null;
            }

            return parseInt(sortOrder);
        },

        resolveElementValue: function(elementId) {
            var value = $(elementId).val();
            if (value === undefined || value === null || value.length === 0) {
                return null;
            }

            return value;
        },

        resolveElementText: function(elementId) {
            var text = $(elementId).text();
            if (text === undefined || text === null || text.length === 0) {
                return null;
            }

            return text;
        },

        currencyFormat: function(value) {
            return "kr. " + (value.toFixed(2).replace(".", ",").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1."));
        }
    });

    $(document).ready(function() {
        $().startAccountingCollectionObserver(document.getElementById("accountingCollection"));

        var urlForDefaultAccountingNumber = $().getUrlForDefaultAccountingNumber();
        if (urlForDefaultAccountingNumber === null) {
            return;
        }

        if ($().isRightContentHidden()) {
            return;
        }

        $().getAccounting(urlForDefaultAccountingNumber);
    });
})(jQuery);