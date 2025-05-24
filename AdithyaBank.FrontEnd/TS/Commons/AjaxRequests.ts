import * as $ from 'jquery';

export const AjaxPost = (url: string, data: any, afToken: string, onSuccess: (resultData: any) => void, onError?: (err: any) => void) => {
    let request = {} as JQuery.UrlAjaxSettings;
    request.url = url;
    request.data = data;
    request.headers = { 'RequestVerificationToken': afToken };
    request.beforeSend = (jqXHR, settings: JQueryAjaxSettings) => {
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
        if (onError!=null) {
            onError(error);
        }
    };

    $.post(request).fail(function () {
        if (!onError) {
            onError("Request failed.");
        }
    });
}

export const AjaxGet = (url: string, data: any, onSuccess: (resultData: any) => void, onError?: (err: any) => void) => {
    let request = {} as JQuery.UrlAjaxSettings;
    request.url = url;
    request.data = data;
    request.beforeSend = (jqXHR, settings: JQueryAjaxSettings) => {
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
}

function excludeUrlFromAjaxLoader(url: string) {
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