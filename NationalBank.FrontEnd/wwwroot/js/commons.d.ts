/// <reference types="typings" />
declare module "AjaxRequests" {
    export const AjaxPost: (url: string, data: any, afToken: string, onSuccess: (resultData: any) => void, onError?: (err: any) => void) => void;
    export const AjaxGet: (url: string, data: any, onSuccess: (resultData: any) => void, onError?: (err: any) => void) => void;
}
declare module "Autocomplete" {
    export class Autocomplete {
        Autocomplete<T extends IdName>(autoOptions: AutocompleteOptions): void;
        UnbindAutoComplete(elementId: string): void;
    }
}
declare module "file" {
    export class file {
    }
}
