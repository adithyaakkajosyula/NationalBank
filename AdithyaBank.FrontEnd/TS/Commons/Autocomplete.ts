export class Autocomplete {
    Autocomplete<T extends IdName>(autoOptions: AutocompleteOptions): void {
        const autoInput = document.getElementById(autoOptions.ElementId);
        const options = {} as JQueryAutocompleteOptions;
        options.minChars = 3;
        options.type = 'GET';
        options.dataType = 'json';
        options.deferRequestBy = 1000;
        options.serviceUrl = autoOptions.ServiceUrl;
        options.paramName = 'term';


        const ajaxSettings = {} as JQueryAjaxSettings;
        ajaxSettings.timeout = 80000;
        options.ajaxSettings = ajaxSettings;

        if (autoOptions.OptionalParams !== null) {
            options.params = autoOptions.OptionalParams;
        }
        options.triggerSelectOnValidInput = false
        options.autoSelectFirst = true;
        options.showNoSuggestionNotice = true;
        options.transformResult = (response) => {
            try {
                return {
                    suggestions: $.map(response, function (dataItem: T) {
                    //suggestions: $.map(response.slice(0,30), function (dataItem: T) {
                      
                        return { value: dataItem.Name, data: dataItem };
                    })
                };
            }
            catch (e) {
                //need this to prevent multiple server requests on error.
                $(autoInput).val('');
                autoOptions.OnSearchError(e);
            }
        };
        options.onSelect = (data) => {
            const _data = data.data as T;
            autoOptions.OnSearchItemSelect(_data);
        }
        options.onSearchError = (query, jqXHR, textStatus, errorThrown) => {
            //need this to prevent multiple server requests on error.
            $(autoInput).val('');
            autoOptions.OnSearchError(errorThrown);
        };

        $(autoInput).off('focus.' + autoOptions.ElementId).autocomplete(options);
    }

    UnbindAutoComplete(elementId: string) {
        $("#" + elementId).autocomplete({} as JQueryAutocompleteOptions);
    }
}
