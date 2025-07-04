define("AjaxRequests", ["require", "exports", "jquery"], function (require, exports, $) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.AjaxGet = exports.AjaxPost = void 0;
    const AjaxPost = (url, data, afToken, onSuccess, onError) => {
        let request = {};
        request.url = url;
        request.data = data;
        request.headers = { 'RequestVerificationToken': afToken };
        request.beforeSend = (jqXHR, settings) => {
            if (excludeUrlFromAjaxLoader(settings.url)) {
                return;
            }
            $(".preloader").css('background', 'transparent');
            $(".preloader").fadeIn('slow');
        };
        request.complete = () => {
            if ($('.preloader').is(":visible")) {
                $(".preloader").fadeOut();
            }
        };
        request.success = (result, status, xhr) => { onSuccess(result); };
        request.error = (xhr, status, error) => {
            if (onError != null) {
                onError(error);
            }
        };
        $.post(request).fail(function () {
            if (!onError) {
                onError("Request failed.");
            }
        });
    };
    exports.AjaxPost = AjaxPost;
    const AjaxGet = (url, data, onSuccess, onError) => {
        let request = {};
        request.url = url;
        request.data = data;
        request.beforeSend = (jqXHR, settings) => {
            if (excludeUrlFromAjaxLoader(settings.url)) {
                return;
            }
            $(".preloader").css('background', 'transparent');
            $(".preloader").fadeIn('slow');
        };
        request.complete = () => {
            if ($('.preloader').is(":visible")) {
                $(".preloader").fadeOut();
            }
        };
        request.success = (result, status, xhr) => { onSuccess(result); };
        request.error = (xhr, status, error) => {
            if (!onError) {
                onError(error);
            }
        };
        $.get(request).fail(function () {
            if (!onError) {
                onError("Request failed.");
            }
        });
    };
    exports.AjaxGet = AjaxGet;
    function excludeUrlFromAjaxLoader(url) {
        var result = false;
        switch (url) {
            case '/Reports/GetReports':
                result = true;
                break;
            case '/LevelMember/GetVillageIdNames':
                result = true;
                break;
        }
        return result;
    }
});
define("Autocomplete", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Autocomplete = void 0;
    class Autocomplete {
        Autocomplete(autoOptions) {
            const autoInput = document.getElementById(autoOptions.ElementId);
            const options = {};
            options.minChars = 3;
            options.type = 'GET';
            options.dataType = 'json';
            options.deferRequestBy = 1000;
            options.serviceUrl = autoOptions.ServiceUrl;
            options.paramName = 'term';
            const ajaxSettings = {};
            ajaxSettings.timeout = 80000;
            options.ajaxSettings = ajaxSettings;
            if (autoOptions.OptionalParams !== null) {
                options.params = autoOptions.OptionalParams;
            }
            options.triggerSelectOnValidInput = false;
            options.autoSelectFirst = true;
            options.showNoSuggestionNotice = true;
            options.transformResult = (response) => {
                try {
                    return {
                        suggestions: $.map(response, function (dataItem) {
                            return { value: dataItem.Name, data: dataItem };
                        })
                    };
                }
                catch (e) {
                    $(autoInput).val('');
                    autoOptions.OnSearchError(e);
                }
            };
            options.onSelect = (data) => {
                const _data = data.data;
                autoOptions.OnSearchItemSelect(_data);
            };
            options.onSearchError = (query, jqXHR, textStatus, errorThrown) => {
                $(autoInput).val('');
                autoOptions.OnSearchError(errorThrown);
            };
            $(autoInput).off('focus.' + autoOptions.ElementId).autocomplete(options);
        }
        UnbindAutoComplete(elementId) {
            $("#" + elementId).autocomplete({});
        }
    }
    exports.Autocomplete = Autocomplete;
});
define("file", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.file = void 0;
    class file {
    }
    exports.file = file;
});
//# sourceMappingURL=commons.js.map