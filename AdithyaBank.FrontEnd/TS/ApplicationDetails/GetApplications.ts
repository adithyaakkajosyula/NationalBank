import * as $ from 'jquery';
import { Autocomplete } from 'Autocomplete';
import { AjaxPost, AjaxGet } from 'AjaxRequests';
import * as moment from 'moment';
import Swal from 'sweetalert2';
export class GetApplications {
    InitOnLoad(): void {
        $('#divcontainer').hide();
        this.applicationAutocomplete();
        $('.selectdate').addClass('input-daterange');
        const optss = {} as DatepickerOptions;
        optss.format = 'dd-M-yyyy';
        optss.autoclose = true;
        optss.orientation = "bottom right";
        optss.todayHighlight = true;

        $('.input-daterange').datepicker(optss);
        $('.selectdate').show();
    }
    private applicationAutocomplete(): void {
        const opts = {} as AutocompleteOptions;
        opts.ElementId = 'ApplicationName';
        opts.ServiceUrl = '/GetApplications/Getdata';
        opts.OptionalParams = {};
        opts.OnSearchItemSelect = this.onapplicationSelected;
        opts.OnSearchError = this.onapplicationError;
        const autocomplete = new Autocomplete();
        autocomplete.Autocomplete(opts);
    }
    private onapplicationSelected=(data: IdName)=>{
        $('#Id').val(data.Id);
        AjaxGet('/GetApplications/Getapplicantdetails', { 'applicantid': data.Id }, success => this.onApplicantdetailssucess(success), error => this.onApplicantdetailserror(error));
    }
    private onapplicationError = () => {
        Swal.fire('Error occured');
/*        Swal.fire({
            title: "Do you want to save the changes?",
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Save",
            denyButtonText: `Don't save`
        }).then((result) => {

            if(result.isConfirmed) {
                Swal.fire("Saved!", "", "success");
            }
            else if (result.isDenied) {
                Swal.fire("Changes are not saved", "", "info");
            }
        });*/
    }
    onApplicantdetailssucess(sucess): void {
        $('#divcontainer').slideDown('slow');
        $('#getapplicationname').text(sucess.ApplicationName);
        $('#getApplicationRegisterdate').text(moment(sucess.ApplicationRegisterdate).format('DD/MM/yyyy'));
        $('#getApplicationmobile').text(sucess.Applicationmobile);
        $('#getApplicationRequestedAmount').text(sucess.ApplicationRequestedAmount);
    }
    private onApplicantdetailserror(error) {

    }
}