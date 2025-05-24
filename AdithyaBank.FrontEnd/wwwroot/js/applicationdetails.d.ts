declare module "ApplicationAdd" {
    export class ApplicationAdd {
        InitOnLoad(): void;
        private applicationAutocomplete;
        private onapplicationSelected;
        private onapplicationError;
        private getapplicationdetails;
        private onerror;
        calculateAge(birthdate: any): number;
        calculateAgeevent(event: any): void;
    }
}
declare module "AppraisalAdd" {
    export class AppraisalAdd {
        InitOnLoad(): void;
        private countriesautocomplete;
        private onCountryselected;
        private onStatesSucess;
        private onDistrictsSucess;
        private countriesautocompleteforlist;
        private checAllRows;
        private appraisaldetailssave;
        private appraisalsucess;
        private onError;
        viewordownloadfilebyclient(id: any, documentid: any, action: any): void;
        downloadFileFromByteArray(result: FileDownloadWithByteArrayResult): void;
        viewfilefrombytearray(result: FileDownloadWithByteArrayResult): void;
        private getblobfrombytearraystring;
        addappraisalrow(event: any): void;
        private gettablelivecount;
        private onCountryselectedList;
        private onStatesSucessList;
        private onDistrictsSucessList;
        private appraisalsavefromlist;
        removeappraisalfromlist(deleterow: any): void;
        private sumofrequestedamount;
        private saveallappraisalsinlist;
        private deleteappraisalfromlist;
    }
    interface FileDownloadWithByteArrayResult {
        FileBytes: string;
        FileContent: string;
        FileName: string;
        IsSuccess: boolean;
        Message: string;
    }
}
declare module "GetApplications" {
    export class GetApplications {
        InitOnLoad(): void;
        private applicationAutocomplete;
        private onapplicationSelected;
        private onapplicationError;
        onApplicantdetailssucess(sucess: any): void;
        private onApplicantdetailserror;
    }
}
declare module "file" { }
