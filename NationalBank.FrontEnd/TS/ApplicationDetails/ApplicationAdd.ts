import * as $ from 'jquery';
import { Autocomplete } from 'Autocomplete';
import { AjaxPost } from 'AjaxRequests';
import Swal from 'sweetalert2';
import * as moment from 'moment';

export class ApplicationAdd {
    InitOnLoad(): void {
        $('#searchby_code').hide();
        $('#clientDiv').hide();
        $("#appraisalDiv").hide();
        $("#AppraisalTypeId").change(function () {

            let value = $("#AppraisalTypeId :selected").val();
            if (value == 0) {
                $('#searchby_code').hide();
                $("#clientDiv").hide();
                $("#appraisalDiv").hide();
            }
            else {
                $('#searchby_code').show();
                if (value == 2 || value == 3 || value == 4) {
                    $("#appraisalDiv").show();
                    $("#clientDiv").hide();
                    /*            $('#AppraisalTypeId option:not(:selected)').attr('disabled', true)*/
                    $('.changeLabel span').text('Appraisal Code');
                }
                if (value == 1) {
                    $("#appraisalDiv").hide();
                    /*            $('#AppraisalTypeId option:not(:selected)').attr('disabled', true)*/
                    $("#clientDiv").show();
                    $('.changeLabel span').text('Lead Search');
                }
            }
        });
        this.applicationAutocomplete();
        $('#DateOfBirth').on('input', (event) => this.calculateAgeevent(event));
            
        
    }
    private applicationAutocomplete(): void {
        const opts = {} as AutocompleteOptions;
        opts.ElementId = 'ClientName';
        opts.ServiceUrl = '/Application/Getdata';
        opts.OptionalParams = {};
        opts.OnSearchItemSelect = this.onapplicationSelected;
        opts.OnSearchError = this.onapplicationError;
        const autocomplete = new Autocomplete();
        autocomplete.Autocomplete(opts);
    }
    private onapplicationSelected=(result: IdName) => {
        const token = $('input[type=hidden][name=__RequestVerificationToken]').val() as string;
        AjaxPost('/Application/Getappraisal', { 'id': result.Id }, token, (details) => this.getapplicationdetails(details), (error) => this.onerror(error));
    }
    private onapplicationError(): void {
        alert('error');
    }
    private getapplicationdetails(result: any): void {
        $('#AppraisalDate').val(moment(result.ApplicationRegisterdate).format('DD/MMM/YYYY'));
        $('#FirstName').val(result.ApplicationName);
        $('#Email').val(result.Email);
        $('#FatherName').val(result.FatherName);
        $('#MotherName').val(result.MotherName);
        $('#DateOfBirth').val(moment(result.DateOfBirth).format('DD/MMM/YYYY'));
        $('#Gender').val(result.Gender == 'M' ? '1' : '2');
    }
    private onerror(error) {
        Swal.fire('Error Ocuured', "", "error");
    }

     calculateAge(birthdate) {
        const today = new Date();
        let age = today.getFullYear() - birthdate.getFullYear();
        const monthDifference = today.getMonth() - birthdate.getMonth();

        if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthdate.getDate())) {
            age--;
        }

        return age;
    }

    calculateAgeevent(event) {
        const birthdate = $(event).val() as Date;
        const age = this.calculateAge(new Date(birthdate));
        $('#age').text(age);
    }
}