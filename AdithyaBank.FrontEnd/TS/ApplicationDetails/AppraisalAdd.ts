import * as $ from 'jquery';
import * as moment from 'moment';
import { Autocomplete } from 'Autocomplete';
import * as Popup from 'Popup';
import { AjaxPost, AjaxGet } from 'AjaxRequests';
import * as utilities from 'Utilities';
import Swal from 'sweetalert2';

export class AppraisalAdd {

    InitOnLoad(): void {
        let form = $('#myForm');
        $('#myForm').submit(function (event) {

            if (form.valid() == true) {
                $('#overlay').show(); // Show overlay
                $('#spinner').show(); // Show spinner
            }
           
        });

        $('.datepicker').val(moment().format("DD-MMM-YYYY").toLowerCase());
        $('.selectdate').addClass('input-daterange');
        const optss = {} as DatepickerOptions;
        optss.format = 'dd-M-yyyy';
        optss.autoclose = true;
        optss.orientation = "bottom right";
        optss.todayHighlight = true;

        $('.input-daterange').datepicker(optss);
        $('.selectdate').show();

        //Autocomplete for List generated rows
        let rowCount = $('.maintbodyclass').find('tr').length;
        for (let i = 0; i < rowCount; i++) {
            const elementId = `z${i}__ApplicationCountryName`; // Assuming you have different IDs for each input field
            this.countriesautocompleteforlist(elementId);
        }
        $('tbody').find('tr').each(function () {
            $(this).find('input:not(.productidclass), select, textarea, button').prop('disabled', true);
            $(this).find('a, i').addClass('disabled').css('pointer-events', 'none');
        });
        $('.productidclass').change((event)=> {
            var $checkbox = $(event.target); 
            var isChecked = $checkbox.prop('checked');

            if (isChecked) {
                // Enable the current row
                $checkbox.closest('tr').find('input:not(.productidclass), select, textarea, button').prop('disabled', false);
                $checkbox.closest('tr').find('a, i').removeClass('disabled').css('pointer-events', 'auto');
            } else {
                // Enable all rows
                $checkbox.closest('tr').find('input:not(.productidclass), select, textarea, button').prop('disabled', true);
                $checkbox.closest('tr').find('a, i').addClass('disabled').css('pointer-events', 'none');           
            }
            this.sumofrequestedamount()
        });
 
        $('#ApplicationCountryName').keydown((event) => {
            $('#ApplicationStateId option:not(:first-child)').remove();
            $('#ApplicationDistrictId option:not(:first-child)').remove();
            $('#ApplicationCountryId').val(0);
        })
        this.countriesautocomplete();
        //For add
        $('#ApplicationStateId').change((event) => {
            $('.applicationdistrictname option:not(:first-child)').remove();
            let stateId = parseInt($(event.target).val() as string);
            if (stateId > 0) {
                AjaxGet('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucess(result), (error) => this.onError(error));
            }
        })
        //For List
        $('.applicationstatename').change((event) => {
            $(event.target).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
            let stateId = parseInt($(event.target).val() as string);
            if (stateId > 0) {
                AjaxGet('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucessList(result, event), (error) => this.onError(error));
            }
        })
        $('.fileinput').change(function (event) {
            let file = (event.target as HTMLInputElement).files[0];
            if (file && file.type === 'application/pdf' || file.type==='image/jpeg') {
                var reader = new FileReader();
                reader.onload = function () {
                    var dataUri = reader.result as string;
                    $('#pdfViewer').attr('src', dataUri);
                    const $iframe = $(this).closest('td').find('.previewFrame');
                    $iframe.attr('src', dataUri);
                };
                reader.readAsDataURL(file);
            } else {
                alert('Please select a PDF file.');
            }
        });

        $('#appraisalsave').on('click', () => { this.appraisaldetailssave() })

        //To Open the file upload on clicking of Icon 
        $('.uploadIcon').on('click', (event) => { $(event.target).closest('tr').find('.fileinput').click(); })


        $('.downloadfilebyclientside,.viewfilebyclientside').on('click', (event) => {
            var action = $(event.currentTarget).is('.downloadfilebyclientside') ? 'download' : 'view';
            var id = $(event.currentTarget).data('id');
            var documentId = $(event.currentTarget).data('documentid');
            this.viewordownloadfilebyclient(id, documentId,action);
        });
        $('#addappraisalrow').on('click', (event) => { this.addappraisalrow(event.target) });
        /*$('.maintableclass').on('click', 'tbody tr td addappraisalinlist', function () {
            // Your button click function logic here
            alert();
        });*/
        $('#appraisalselectall').on('change', (event) => this.checAllRows(event.target));
        $("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").on("input", (event)=> {
            let $row = $(event.target).closest('tr');

            $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                let $input = $(this);
                let $message = $input.next(".error-message"); // Assuming the error message is a sibling element

                // Toggle error message and red border for each input field
                let isValidInput = toggleErrorMessage($input, $message);
                // Update overall validation status
           
            });
        });

/*        $('.maintableclass tbody tr').each(function () {
            // Process each row here
            let $row = $(this);
            // Now $row refers to the current row

            $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                let $input = $(this);
                let $message = $input.next(".error-message"); // Assuming the error message is a sibling element

                // Toggle error message and red border for each input field
                let isValidInput = toggleErrorMessage($input, $message);
                alert(isValidInput);
            });
        });*/
        $('.maintableclass').on('click', 'tbody tr td .addappraisalinlist', (event) => { this.appraisalsavefromlist(event) });
        $('.maintableclass').on('click', 'tbody tr td .updateappraisalfromlist', (event) => { this.appraisalsavefromlist(event) });
        $('.maintableclass').on('click', 'tbody tr td .removeappraisalinlist', (event) => this.removeappraisalfromlist(event.target));
        this.sumofrequestedamount();
        $('.maintableclass').on('click', 'tfoot tr td .savelist', (event) => { this.saveallappraisalsinlist(event.target) });
        $('.maintableclass').on('click', 'tbody tr td .deleteappraisal', (event) => { this.deleteappraisalfromlist(event.target) });
    }

    private countriesautocomplete(): void {
        const opts = {} as AutocompleteOptions;
        opts.ElementId = 'ApplicationCountryName';
        opts.ServiceUrl = '/Appraisal/GetCountries';
        opts.OptionalParams = {};
        opts.OnSearchItemSelect = this.onCountryselected;
        opts.OnSearchError = this.onError;
        const autocomplete = new Autocomplete();
        autocomplete.Autocomplete(opts);
    }
   
    private onCountryselected=(data: IdName) =>{
        $('.applicationstatename option:not(:first-child)').remove();
        $('.applicationdistrictname option:not(:first-child)').remove();

        $('.applicationcountryname').val(data.Id);
        let countyId = parseInt(data.Id);
        if (countyId > 0) {
            AjaxGet('/Appraisal/Getstates', { 'countryId': countyId }, (result) => this.onStatesSucess(result), (error) => this.onError(error));
        }
    }
    private onStatesSucess(data) {
       

        $.each(data, (index, item: IdName) => {

            $('.applicationstatename').append($('<option>', {
                value: item.Id,
                text: item.Name
            }));
        });
    }
    private onDistrictsSucess(data) {

        $.each(data, (index, item: IdName) => {

            $('.applicationdistrictname').append($('<option>', {
                value: item.Id,
                text: item.Name
            }));
        });
    }

    private countriesautocompleteforlist(elementId: string): void {
        const opts = {} as AutocompleteOptions;
        opts.ElementId = elementId;
        opts.ServiceUrl = '/Appraisal/GetCountries';
        opts.OptionalParams = {};
        opts.OnSearchItemSelect = (data: IdName) => {
            $(elementId).val(data.Id);
            this.onCountryselectedList(data, $('#' + elementId).closest('tr').index());
        }
        opts.OnSearchError = (err: any) => {
            Swal.fire('Error on getting Countries', 'error');
        };
        const autocomplete = new Autocomplete();
        autocomplete.Autocomplete(opts);
    }
    private checAllRows(result) {
        let checkBoxes = $('.maintableclass').find('input:checkbox.productidclass');
        $(checkBoxes).prop('checked', $(result).is(':checked'));
        $('.productidclass').trigger('change'); //  It will again go to changeclearedproductattributes event
    }
    private appraisaldetailssave() {
        let name = $('#ApplicationName').val() as string;
        let nameexp = new RegExp('^[A-Za-z ]*$');
        if (name == "") {
            Swal.fire("Please Enter name","","error");
            return;
        }
        if (nameexp.test(name) === false) {
            Swal.fire('Name must be in Alphabets with or without Spaces Only',"",'error');
            return;
        }
        let fathername = $('#Applicationfathername').val() as string;
        if (fathername == "") {
            Swal.fire('Please Enter Father Name',"",'error');
            return;
        }
        if (nameexp.test(fathername) === false) {
            Swal.fire('Father Name must be in Alphabets with or without Spaces Only',"",'error');
            return;
        }

        let mothername = $('#Applicationmothername').val() as string;
        if (mothername == "") {
            Swal.fire('Please Enter Mother Name',"",'error');
            return;
        }
        if (nameexp.test(mothername) === false) {
            Swal.fire('Mother Name must be in Alphabets with or without Spaces Only',"",'error');
            return;
        }

        let dob = $('#Applicationdob').val() as string;

        let gender = $('#Applicationgender').val() as string;
        if (gender == "") {
            Swal.fire('Please select Gender', "",'error');
            return;
        }

        let qualification = $('#Applicationqualification').val() as string;
        if (qualification == "") {
            Swal.fire('Please Select Qualification',"", 'error');
            return;
        }

        let applicationMartialStatus = $('#ApplicationMartialStatus').val() as string;
        if (applicationMartialStatus == "") {
            Swal.fire('Please select ApplicationMartialStatus', "",'error');
            return;
        }

        let applicationmobile = $('#Applicationmobile').val() as string;
        let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
        if (applicationmobile == "") {
            Swal.fire('Please Enter Mobile Number',"", 'error');
            return;
        }
        if (mobileexpression.test(applicationmobile) === false) {
            Swal.fire('Mobile Number Must start with 9,8,7 and 10 digits only',"error");
            return;
        }

        let email = $('#Applicationemail').val() as string;
        let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$")
        if (email == "") {
            Swal.fire("Please Enter Email","", "error");
            return;
        }
        if (emailexpression.test(email) === false) {
            Swal.fire("Enter Correct Email", "","error");
            return;
        }
        let documenttypeid = $('#ApplicationDocumentTypeId').val() as string;
        if (documenttypeid == "") {
            Swal.fire("Please Select Application Document Type","", "error");
            return;
        }

        let loanamount = $('#ApplicationRequestedAmount').val() as string;
        let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
        if (loanamount == "") {
            Swal.fire("Please enter Loan amount","", "error");
            return;
        }
        if (loanamountexpreession.test(loanamount) === false) {
            Swal.fire("Loan Amount must be in Number with or without decimals", "", "error");
            return;
        }

        let applicationhobbies = $('#ApplicationHobbies').val() as string[];
        if (applicationhobbies.length <=0) {
            Swal.fire("Please Select application hobbies","", "error");
            return;
        }


        let registerdate = $('#ApplicationRegisterdate').val() as string;
        let address = $('#ApplicationAddress').val() as string;
        if (address == "") {
            Swal.fire("Please Enter Address","", "error");
            return;
        }

        let applicationcountryid = $('#ApplicationCountryId').val() as string;
        if (applicationcountryid == "") {
            Swal.fire("Please Select Country", "","error");
            return;
        }

        let applicationstateid = $('#ApplicationStateId').val() as string;
        if (applicationstateid == "") {
            Swal.fire("Please Select State","", "error");
            return;
        }

        let applicationdistrictid = $('#ApplicationDistrictId').val() as string;
        if (applicationdistrictid == "") {
            Swal.fire("Please Select District","", "error");
            return;
        }

        let documentfile = $('#DocumentFile');
        if (documentfile.val() == "") {
            Swal.fire("Please Select File","","error");
            return;
        }

        let ApplicationIsAcceptedTermsandConditions = $('#ApplicationIsAcceptedTermsandConditions').is(':checked');
        if (ApplicationIsAcceptedTermsandConditions == false) {
            Swal.fire("Please check checkbox","", "error");
            return;
        }


        let formdata = new FormData();
        formdata.append("ApplicationName", name);
        formdata.append("Applicationfathername", fathername);
        formdata.append("Applicationmothername", mothername);
        formdata.append("Applicationdob", dob);
        formdata.append("Applicationgender", gender);
        formdata.append("Applicationqualification", qualification);
        formdata.append("ApplicationMartialStatus", applicationMartialStatus);
        formdata.append("Applicationmobile", applicationmobile);
        formdata.append("Applicationemail", email);
        formdata.append("ApplicationDocumentTypeId", documenttypeid);
        formdata.append("ApplicationRequestedAmount", loanamount);
        applicationhobbies.forEach((val, i) => {
            formdata.append("ApplicationHobbies[" + i + "]", val)
        });
        formdata.append("ApplicationRegisterdate", registerdate);
        formdata.append("ApplicationIsAcceptedTermsandConditions", ApplicationIsAcceptedTermsandConditions.toString());
        formdata.append("ApplicationAddress", address);
        formdata.append("ApplicationDistrictId", applicationdistrictid);
        formdata.append("ApplicationStateId", applicationstateid);
        formdata.append("ApplicationCountryId", applicationcountryid);
        formdata.append("DocumentFile", documentfile[0]['files'][0]);

        const token = $('input[type=hidden][name=__RequestVerificationToken]').val() as string;
       Swal.fire({
            title: "Do you want to save the changes?",
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Save",
            denyButtonText: `Don't save`
        }).then((result) => {

            if(result.isConfirmed) {
                $.ajax({
                    url: '/Appraisal/AddAppraisal',
                    data: formdata,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    headers: {
                        'Accept': 'application/json' // Specify the Accept header to indicate the desired response format
                    },
                    success: (result) => this.appraisalsucess(result),
                    error: (error) => this.onError(error)
                });
            }
            else if (result.isDenied) {
                Swal.fire("Changes are not saved", "", "info");
            }
        });
    }
    private appraisalsucess(result: BaseModel) {
        if (result.IsSuccess == true) {
            Swal.fire(result.Message, "", "success").then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/Appraisal/AddAppraisal';
                }
            });
        }
        else {
            Swal.fire(result.Message, "", "error")
        }
    }
    private onError(error) {
        Swal.fire('Error occured', "", "error");
    }
    public viewordownloadfilebyclient(id, documentid,action) {
        const token = $('input[type=hidden][name=__RequestVerificationToken]').val() as string;
        AjaxPost('/Appraisal/ViewFileByClientSide', { 'id': id, 'documentId': documentid }, token, (result) => action == 'download' ? this.downloadFileFromByteArray(result) : this.viewfilefrombytearray(result), (error) => this.onError(error));
    }
    public downloadFileFromByteArray(result: FileDownloadWithByteArrayResult) {
       
        // Create a temporary link to trigger the download
        var element = document.createElement('a');
        element.href = window.URL.createObjectURL(this.getblobfrombytearraystring(result.FileBytes, result.FileContent));
        element.download = result.FileName; // Set the file name for download
        element.style.display = 'none';

        // Append the link to the document body and trigger the download
        document.body.appendChild(element);
        element.click();

        // Clean up
        document.body.removeChild(element);
        
    }
    public viewfilefrombytearray(result: FileDownloadWithByteArrayResult) {

        var url = window.URL.createObjectURL(this.getblobfrombytearraystring(result.FileBytes, result.FileContent));

        // Open the URL in a new window/tab
        window.open(url, '_blank');

        // Clean up
        window.URL.revokeObjectURL(url);
    }
    private getblobfrombytearraystring(filebytes,filecontent) {
       // Convert the base64-encoded byte string to Uint8Array
       var byteCharacters = atob(filebytes);
       var byteNumbers = new Array(byteCharacters.length);
       for (var i = 0; i < byteCharacters.length; i++) {
           byteNumbers[i] = byteCharacters.charCodeAt(i);
       }
       var byteArray = new Uint8Array(byteNumbers);

       // Create Blob from the Uint8Array
       var blob = new Blob([byteArray], {
           type: filecontent
       });
       return blob;
    }
    public addappraisalrow(event) {
        let rowCount = $('.maintbodyclass').find('tr').length;
        let applicationqualifications = $('#applicationqualifications').html();
        let applicationdocumenttypes = $('#applicationdocumenttypes').html();
        let applicationhobbies = $('#applicationhobbies').html();
        $('.maintbodyclass').append(` <tr>
                                            <td class="d-none"><input class="appraisalid" value="0" hidden="hidden" id="z${rowCount}__Id" name="[${rowCount}].Id"/><span class="error-message" style="display:none;"></span></td>
                                            <td><input type="checkbox" value="false" class="form-check-input productidclass" id="z${rowCount}__ProductId" name="[${rowCount}].ProductId" /><span class="error-message" style="display:none;"></span></td>
                                            <td><input type="text" class="form-control applicationName" id="z${rowCount}__ApplicationName" name="[${rowCount}].ApplicationName" data-validation="name"/><span class="error-message" style="display:none;"></span></td>
                                            <td><input  type="text" class="form-control applicationfathername" id="z${rowCount}__Applicationfathername" name="[${rowCount}].Applicationfathername" data-validation="name"/><span class="error-message" style="display:none;"></span></td>
                                            <td><input  type="text" class="form-control applicationmothername" id="z${rowCount}__Applicationmothername" name="[${rowCount}].Applicationmothername" data-validation="name"/><span class="error-message" style="display:none;"></span></td>
                                            <td>
                                                <div class="selectdate">
                                                    <input type="text" class="form-control datepicker applicationdob" id="z${rowCount}__Applicationdob" name="[${rowCount}].Applicationdob" readonly />
                                                </div>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <div class="radio radio-inline radio-info">
                                                    <input type="radio" id="z${rowCount}__verified" class="isverified applicationgender" name="[${rowCount}].Applicationgender" value="M"/>
                                                    <label for="z${rowCount}__verified">Male</label>
                                                    <input type="radio" id="z${rowCount}__notverified" class="isverified applicationgender" name="[${rowCount}].Applicationgender" value="F"/>
                                                    <label for="z${rowCount}__notverified">Female</label>
                                                </div>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationqualification" id="z${rowCount}__Applicationqualification" name="[${rowCount}].Applicationqualification">
                                                    ${applicationqualifications}
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationMartialStatus" id="z${rowCount}__ApplicationMartialStatus" name="[${rowCount}].ApplicationMartialStatus">
                                                    <option value="">Select</option>
                                                    <option value="Single">Single</option>
                                                    <option value="Married">Married</option>
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <input type="text" class="form-control applicationmobile" id="z${rowCount}__Applicationmobile" name="[${rowCount}].Applicationmobile" data-validation="phone">
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <input type="text" class="form-control applicationemail" id="z${rowCount}__Applicationemail" name="[${rowCount}].Applicationemail" data-validation="email">
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationDocumentTypeId" id="z${rowCount}__ApplicationDocumentTypeId" name="[${rowCount}].ApplicationDocumentTypeId">
                                                   ${applicationdocumenttypes}
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <i class="bi bi-file-arrow-up uploadIcon"></i>
                                                <input class="fileinput" hidden accept=".pdf,.jpg,.jpeg" type="file" id="z${rowCount}__DocumentFile" name="[${rowCount}].DocumentFile">
                                                <span class="error-message" style="display:none;"></span>
                                                <div class="spinner-border text-primary spinner" role="status" style="display:none;">
                                                    <span class="visually-hidden">Loading...</span>
                                                </div>
                                            </td>
                                           <td> <iframe class="previewFrame" style="width: 100%; height: 300px;"></iframe></td>
                                            <td>
                                                <input type="text" class="form-control applicationRequestedAmount" id="z${rowCount}__ApplicationRequestedAmount" name="[${rowCount}].ApplicationRequestedAmount">
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationHobbies" id="z${rowCount}__ApplicationHobbies" name="[${rowCount}].ApplicationHobbies" multiple>
                                                    ${applicationhobbies}
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <div class="selectdate">
                                                    <input type="text" class="form-control datepicker applicationRegisterdate" id="z${rowCount}__ApplicationRegisterdate" name="[${rowCount}].ApplicationRegisterdate" readonly />
                                                </div>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <textarea type="text" class="form-control applicationAddress" id="z${rowCount}__ApplicationAddress" name="[${rowCount}].ApplicationAddress"></textarea>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <div>
                                                    <input type="text" class="form-control autocomplete applicationcountryname" id="z${rowCount}__ApplicationCountryName" name="[${rowCount}].ApplicationCountryName" placeholder="Search by Name">
                                                    <input type="hidden" class="applicationCountryId" id="z${rowCount}__ApplicationCountryId" name="[${rowCount}].ApplicationCountryId" />
                                                </div>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationstatename" id="z${rowCount}__ApplicationStateId" name="[${rowCount}].ApplicationStateId">
                                                    <option value="">Select</option>
                                                    <!-- Options here -->
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <select class="form-select applicationdistrictname" id="z${rowCount}__ApplicationDistrictId" name="[${rowCount}].ApplicationDistrictId">
                                                    <option value="">Select</option>
                                                    <!-- Options here -->
                                                </select>
                                                <span class="error-message" style="display:none;"></span>
                                            </td>
                                            <td>
                                                <input type="checkbox" class="form-check-input applicationIsAcceptedTermsandConditions" id="z${rowCount}__ApplicationIsAcceptedTermsandConditions" name="[${rowCount}].ApplicationIsAcceptedTermsandConditions">
                                                <span class="error-message" style="display:none;"></span>
                                            </td> 
                                            <td>
                                                <button class="btn btn-primary addappraisalinlist" type="button">Add</button>
                                                <button class="btn btn-primary removeappraisalinlist" type="button">Remove</button>
                                            </td>
                                        </tr>
                                        `);
        //For Table live count
        this.gettablelivecount();
        //For Date Picker newly generated rows
        $('.datepicker').val(moment().format("DD-MMM-YYYY").toLowerCase());

        $('.selectdate').addClass('input-daterange');
        const optss = {} as DatepickerOptions;
        optss.format = 'dd-M-yyyy';
        optss.autoclose = true;
        optss.orientation = "bottom right";
        optss.todayHighlight = true;
        $('.input-daterange').datepicker(optss);
        $('.selectdate').show();

        //Autocomplete for newly generated rows
        const opts = {} as AutocompleteOptions;
        opts.ElementId = `z${rowCount}__ApplicationCountryName`;
        opts.ServiceUrl = '/Appraisal/GetCountries';
        opts.OnSearchItemSelect = (data: IdName) => {
            $(`#z${rowCount}__ApplicationCountryId`).val(data.Id);
            this.onCountryselectedList(data, rowCount);
        }
        opts.OnSearchError = (err: any) => {
            Swal.fire('Error on getting Countries', 'error');
        };
        opts.OptionalParams = {};
        const autocomplete = new Autocomplete();
        autocomplete.Autocomplete(opts);


        //Change event for newly generated rows
        $('.applicationstatename').change((event) => {
            $(event.target).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
            let stateId = parseInt($(event.target).val() as string);
            if (stateId > 0) {
                AjaxGet('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucessList(result, event), (error) => this.onError(error));
            }
        })

        //Open upload popup for new generated rows
        $('.uploadIcon').on('click', (event) => { $(event.target).closest('tr').find('.fileinput').click(); })
        $('.fileinput').change(function (event) {
            let file = (event.target as HTMLInputElement).files[0];
            if (file && file.type === 'application/pdf' || file.type === 'image/jpeg') {
                var reader = new FileReader();
                $(event.target).closest('tr').find('.spinner').show();
                reader.onload = function () {
                    var dataUri = reader.result as string;
                    const $iframe = $(event.target).closest('tr').find('.previewFrame');
                    $iframe.attr('src', dataUri);
                    $(event.target).closest('tr').find('.spinner').hide();
                };
                reader.readAsDataURL(file);
            } else {
                alert('Please select a PDF file.');
            }
        });

        // Get all input fields and attach input event listener

        // Event handler for input fields

        $("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").on("input", (event) => {
            let $row = $(event.target).closest('tr');

            $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                let $input = $(this);
                let $message = $input.next(".error-message"); // Assuming the error message is a sibling element

                // Toggle error message and red border for each input field
                let isValidInput = toggleErrorMessage($input, $message);
                // Update overall validation status

            });
        });

    }
    //To get row count 
    private gettablelivecount() {
        let count = $("table.maintableclass > tbody").find('tr');
        $('#count').text('Total Count : ' + count.length + ' ');
    }

    private onCountryselectedList = (data: IdName, rowcount) => {
        $(`#z${rowcount}__ApplicationCountryName`).closest('tr').find('.applicationstatename option:not(:first-child)').remove();
        $(`#z${rowcount}__ApplicationCountryName`).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
        let countyId = parseInt(data.Id);
        if (countyId > 0) {
            AjaxGet('/Appraisal/Getstates', { 'countryId': countyId }, (result) => this.onStatesSucessList(result, rowcount), (error) => this.onError(error));
        }
    }
    private onStatesSucessList(data,rowCount) {
        // Assuming this method is within a class, so you have access to 'this'
        let $select = $(`#z${rowCount}__ApplicationCountryName`).closest('tr').find('.applicationstatename');

        $.each(data, (index, item: IdName) => {
            $select.append($('<option>', {
                value: item.Id,
                text: item.Name
            }));
        });
    }
    private onDistrictsSucessList(data,event) {

        $.each(data, (index, item: IdName) => {

            $(event.target).closest('tr').find('.applicationdistrictname').append($('<option>', {
                value: item.Id,
                text: item.Name
            }));
        });
    }
    private appraisalsavefromlist(event) {
        let isValid = true;
        let $row = $(event.target).closest('tr');

        $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
            let $input = $(this);
            let $message = $input.next(".error-message"); // Assuming the error message is a sibling element

            // Toggle error message and red border for each input field
            let isValidInput = toggleErrorMessage($input, $message);
            // Update overall validation status
            isValid = isValid && isValidInput;
            // Here isValidInput will give validation true or false it will save in isvalid variable
            //By that value save or error will execute
            // if any middle value in fields is not valid then isValid will false then next field is valid
            //then isValid will become true then with out validation our save method will execute
            // to solve this we use  isValid && isValidInput; This condition will check previous field value , present field value 
            // If both true then only isValid will be  true By this each field will validate
        });
        if (isValid) {
            // Save the data here
           
            // Get all values in the row
            let appraisalId = $row.find('.appraisalid').val() as string;
            let productId = $row.find('.productidclass').prop('checked') as boolean;
            let applicationName = $row.find('.applicationName').val() as string;
            let applicationFatherName = $row.find('.applicationfathername').val() as string;
            let applicationMotherName = $row.find('.applicationmothername').val() as string;
            let applicationDob = $row.find('.applicationdob').val() as string;
            let applicationGender = $row.find('.applicationgender:checked').val() as string | undefined;
            let applicationQualification = $row.find('.applicationqualification').val() as string;
            let applicationMartialStatus = $row.find('.applicationMartialStatus').val() as string;
            let applicationMobile = $row.find('.applicationmobile').val() as string;
            let applicationEmail = $row.find('.applicationemail').val() as string;
            let applicationDocumentTypeId = $row.find('.applicationDocumentTypeId').val() as string;
            let applicationRequestedAmount = $row.find('.applicationRequestedAmount').val() as string;
            let applicationHobbies = $row.find('.applicationHobbies').val() as string[];
            let applicationRegisterDate = $row.find('.applicationRegisterdate').val() as string;
            let applicationAddress = $row.find('.applicationAddress').val() as string;
            let applicationCountryName = $row.find('.applicationcountryname').val() as string;
            let applicationCountryId = $row.find('.applicationCountryId').val() as string;
            let applicationStateId = $row.find('.applicationstatename').val() as string;
            let applicationDistrictId = $row.find('.applicationdistrictname').val() as string;
            let applicationIsAcceptedTermsAndConditions = $row.find('.applicationIsAcceptedTermsandConditions').is(':checked') as boolean;
            let documentfile = $row.find('.fileinput');

            let formdata = new FormData();
            formdata.append("ID", appraisalId);
            formdata.append("ApplicationName", applicationName);
            formdata.append("Applicationfathername", applicationFatherName);
            formdata.append("Applicationmothername", applicationMotherName);
            formdata.append("Applicationdob", applicationDob);
            formdata.append("Applicationgender", applicationGender);
            formdata.append("Applicationqualification", applicationQualification);
            formdata.append("ApplicationMartialStatus", applicationMartialStatus);
            formdata.append("Applicationmobile", applicationMobile);
            formdata.append("Applicationemail", applicationEmail);
            formdata.append("ApplicationDocumentTypeId", applicationDocumentTypeId);
            formdata.append("ApplicationRequestedAmount", applicationRequestedAmount);
            applicationHobbies.forEach((val, i) => {
                formdata.append("ApplicationHobbies[" + i + "]", val)
            });
            formdata.append("ApplicationRegisterdate", applicationRegisterDate);
            formdata.append("ApplicationIsAcceptedTermsandConditions", applicationIsAcceptedTermsAndConditions.toString());
            formdata.append("ApplicationAddress", applicationAddress);
            formdata.append("ApplicationDistrictId", applicationDistrictId);
            formdata.append("ApplicationStateId", applicationStateId);
            formdata.append("ApplicationCountryId", applicationCountryId);
            formdata.append("DocumentFile", documentfile[0]['files'][0]);

            const token = $('input[type=hidden][name=__RequestVerificationToken]').val() as string;
            Swal.fire({
                title: "Do you want to save the changes?",
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: "Save",
                denyButtonText: `Don't save`
            }).then((result) => {

                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Appraisal/AddAppraisal',
                        data: formdata,
                        type: 'POST',
                        contentType: false,
                        processData: false,
                        headers: {
                            'Accept': 'application/json' // Specify the Accept header to indicate the desired response format
                        },
                        success: (result) => {
                            if (result.IsSuccess == true) {
                                Swal.fire(result.Message, "", "success").then((result) => {
                                    if (result.isConfirmed) {
                                        window.location.href = '/Appraisal/AppraisalList';
                                    }
                                });
                            }
                            else {
                                Swal.fire(result.Message, "", "error")
                            }
                        },
                        error: (error) => this.onError(error)
                    });
                }
                else if (result.isDenied) {
                    Swal.fire("Changes are not saved", "", "info");
                }
            });
/*
            Swal.fire("All values are correct. Saving data...", `${applicationName
                + applicationFatherName +
                applicationMotherName +
                applicationDob +
                applicationGender
                + applicationQualification +
                applicationMartialStatus +
                applicationMobile +
                applicationEmail +
                applicationDocumentTypeId +
                applicationRequestedAmount +
                applicationHobbies +
                applicationRegisterDate +
                applicationAddress +
                applicationCountryName +
                applicationCountryId +
                applicationStateId +
                applicationDistrictId +
                applicationIsAcceptedTermsAndConditions}`, "success");*/
        } else {

            return false;
        }
      
    }
    public removeappraisalfromlist(deleterow) {
        var a = $(deleterow).closest('tr').remove();
        this.gettablelivecount();
        resetRowsindexes();
    }

    private sumofrequestedamount() {

        // find all number values in table rows
        var totalpaidall = 0 as number;
        var tablerowsall = $('table.maintableclass > tbody > tr').find('.applicationRequestedAmount');

        $(tablerowsall).each(function (i, element) {

            totalpaidall += Number(element['value']);
        });

        $('#totalpaidsum').text('Total Paid : ' + totalpaidall);

        // find onlr checked rows number values 
        var totalPaid = 0 as number;
        var tableRows = $('table.maintableclass > tbody > tr').filter((data, row) => {
            return $(row).find('.productidclass').is(':checked')
        })
        if (tableRows.length > 0) {
            $(tableRows).each(function () {
                var row = $(this);
                let amount = Number(row.find('.applicationRequestedAmount ').val());
                totalPaid += amount;

            });
        }

        $('#totalpaidforcheckrows').text('Total Paid For Selected Rows: ' + totalPaid);


        // find only male rows requested amount total 
        var totalPaidformale = 0 as number;
        var tableRowsformale = $('table.maintableclass > tbody > tr').filter((data, row) => {
            return $(row).find('.applicationgender[value="M"]').is(':checked');
        });
        if (tableRowsformale.length > 0) {
            $(tableRowsformale).each(function () {
                var row = $(this);
                let amount = Number(row.find('.applicationRequestedAmount ').val());
                totalPaidformale += amount;

            });
        }

        $('#totalpaidformalerows').text('Total Paid For Male Rows: ' + totalPaidformale);


        // find only female rows requested amount total 
        var totalPaidforfemale = 0 as number;
        var tableRowsforfemale = $('table.maintableclass > tbody > tr').filter((data, row) => {
            return $(row).find('.applicationgender[value="F"]').is(':checked');
        });
        if (tableRowsforfemale.length > 0) {
            $(tableRowsforfemale).each(function () {
                var row = $(this);
                let amount = Number(row.find('.applicationRequestedAmount ').val());
                totalPaidforfemale += amount;

            });
        }

        $('#totalpaidforfemalerows').text('Total Paid For Female Rows: ' + totalPaidforfemale);

        // find only ssc rows requested amount total 
        var totalPaidforssc = 0 as number;
        var tableRowsForSSC = $('table.maintableclass > tbody > tr').filter((index, row) => {
            var selectedValue = $(row).find('.applicationqualification').val(); // Get the selected value of the dropdown
            return selectedValue === "SSC"; // Check if the selected value is "F" (for example)
        });
        if (tableRowsForSSC.length > 0) {
            $(tableRowsForSSC).each(function () {
                var row = $(this);
                let amount = Number(row.find('.applicationRequestedAmount ').val());
                totalPaidforssc += amount;

            });
        }

        $('#totalpaidforsscrows').text('Total Paid For SSC Rows: ' + totalPaidforssc);
    }

    private saveallappraisalsinlist(event) {
        var tableRows = $('table.maintableclass > tbody > tr').filter((data, row) => {
            return $(row).find('.productidclass').is(':checked');
        });
        if (tableRows.length <= 0) {
            Swal.fire("No apprasials are selected", "", "error");
            return false;
        }
        // For checking file upload based on payment mode
        // define our flag to work validation in each loop
        var validationFailed = false;
        let appraisalsavelist = [];
        tableRows.each((i, row) => {
            let appraisalId = $(row).find('.appraisalid').val() as string;
            let nameexp = new RegExp('^[A-Za-z ]*$');
            let applicationName = $(row).find('.applicationName').val() as string;
            if (applicationName == "" || applicationName == null) {
                Swal.fire("Please Enter Name", "", "error");
                validationFailed = true; // setting flag
                return false;
            }
            if (!nameexp.test(applicationName)) {
                Swal.fire("Please enter correct name", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationFatherName = $(row).find('.applicationfathername').val() as string;
            if (applicationFatherName == "" || applicationFatherName == null) {
                Swal.fire("Please Enter Father Name", "", "error");
                validationFailed = true;
                return false;
            }
            if (!nameexp.test(applicationFatherName)) {
                Swal.fire("Please Enter Correct Father name", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationMotherName = $(row).find('.applicationmothername').val() as string;
            if (applicationMotherName == "" || applicationMotherName == null) {
                Swal.fire("Please Enter Mother Name", "", "error");
                validationFailed = true;
                return false;
            }
            if (!nameexp.test(applicationFatherName)) {
                Swal.fire("Please Enter Correct Other name", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationDob = $(row).find('.applicationdob').val() as string;
            let applicationGender = $(row).find('.applicationgender:checked').val() as string | undefined;
            if (applicationGender == "" || applicationGender == undefined) {
                Swal.fire("Please Select Gender", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationQualification = $(row).find('.applicationqualification').val() as string;
            if (applicationQualification == "" || applicationQualification == null) {
                Swal.fire("Please Select Qualification", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationMartialStatus = $(row).find('.applicationMartialStatus').val() as string;
            if (applicationMartialStatus == "" || applicationMartialStatus == null) {
                Swal.fire("Please select Martail Status", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationMobile = $(row).find('.applicationmobile').val() as string;
            let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
            if (applicationMobile == null || applicationMobile == "") {
                Swal.fire("Please Enter Mobile Number", "", "error");
                validationFailed = true;
                return false;
            }
            if (!mobileexpression.test(applicationMobile)) {
                Swal.fire("Mobile Number Must Be 10 digits", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationEmail = $(row).find('.applicationemail').val() as string;
            if (applicationEmail == null || applicationEmail == "") {
                Swal.fire("Please enter email", "", "error");
                validationFailed = true;
                return false;
            }
            
            let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$")
            if (!emailexpression.test(applicationEmail)) {
                Swal.fire("Enter correct email", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationDocumentTypeId = $(row).find('.applicationDocumentTypeId').val() as string;
            if (applicationDocumentTypeId == "" || applicationDocumentTypeId == "0" || applicationDocumentTypeId == null) {
                Swal.fire("Please Select Document Type", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationRequestedAmount = $(row).find('.applicationRequestedAmount').val() as string;
            let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
            if (applicationRequestedAmount == "0" || applicationRequestedAmount == null) {
                Swal.fire("Please enter request amount", "", "error");
                validationFailed = true;
                return false;;
            }
            if (!loanamountexpreession.test(applicationRequestedAmount)) {
                Swal.fire("Requested amount br in numbers and two decimals only", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationHobbies = $(row).find('.applicationHobbies').val() as string[];
            if (applicationHobbies.length == 0) {
                Swal.fire("Please Select Hobbies", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationRegisterDate = $(row).find('.applicationRegisterdate').val() as string;
            let applicationAddress = $(row).find('.applicationAddress').val() as string;
            if (applicationAddress == "" || applicationAddress == null) {
                Swal.fire("Please Enter addres", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationCountryName = $(row).find('.applicationcountryname').val() as string;
            let applicationCountryId = $(row).find('.applicationCountryId').val() as string;
            if (applicationCountryId == null || applicationCountryId == "0") {
                Swal.fire("Please Search Country Name And Select Proper country", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationStateId = $(row).find('.applicationstatename').val() as string;
            if (applicationStateId == "" || applicationStateId == "0") {
                Swal.fire("Please Select state", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationDistrictId = $(row).find('.applicationdistrictname').val() as string;
            if (applicationDistrictId == "" || applicationDistrictId == "0") {
                Swal.fire("Please Select District", "", "error");
                validationFailed = true;
                return false;
            }
            let applicationIsAcceptedTermsAndConditions = $(row).find('.applicationIsAcceptedTermsandConditions').is(':checked') as boolean;
            let documentfile = $(row).find('.fileinput');
            if (documentfile[0]['files'][0] == null && applicationQualification == "SSC") {
                Swal.fire("Please Upload File for SSC Qualification", "", "error");
                validationFailed = true;
                return false;
            }

            let appraisalsave = {
                Id: appraisalId,
                ApplicationName: applicationName,
                ApplicationFatherName: applicationFatherName,
                ApplicationMotherName: applicationMotherName,
                ApplicationDob: applicationDob,
                ApplicationGender: applicationGender,
                ApplicationQualification: applicationQualification,
                ApplicationMartialStatus: applicationMartialStatus,
                ApplicationMobile: applicationMobile,
                ApplicationEmail: applicationEmail,
                ApplicationDocumentTypeId: applicationDocumentTypeId,
                ApplicationRequestedAmount: applicationRequestedAmount,
                ApplicationHobbies: applicationHobbies,
                ApplicationRegisterDate: applicationRegisterDate,
                ApplicationAddress: applicationAddress,
                ApplicationCountryName: applicationCountryName,
                ApplicationCountryId: applicationCountryId,
                ApplicationStateId: applicationStateId,
                ApplicationDistrictId: applicationDistrictId,
                ApplicationIsAcceptedTermsAndConditions: applicationIsAcceptedTermsAndConditions,
                DocumentFile: documentfile
            };
            appraisalsavelist.push(appraisalsave);
        });
        // Here validation will work
        if (validationFailed) {
            return false;
        }
        var formdata = new FormData();
        for (var i = 0; i < appraisalsavelist.length; i++) {
            var item = appraisalsavelist[i];

            formdata.append("Model[" + i + "].Id", item.Id);
            formdata.append("Model[" + i + "].ProductId", item.ProductId);
            formdata.append("Model[" + i + "].ApplicationName", item.ApplicationName);
            formdata.append("Model[" + i + "].ApplicationFatherName", item.ApplicationFatherName);
            formdata.append("Model[" + i + "].ApplicationMotherName", item.ApplicationMotherName);
            formdata.append("Model[" + i + "].ApplicationDob", item.ApplicationDob);
            formdata.append("Model[" + i + "].ApplicationGender", item.ApplicationGender);
            formdata.append("Model[" + i + "].ApplicationQualification", item.ApplicationQualification);
            formdata.append("Model[" + i + "].ApplicationMartialStatus", item.ApplicationMartialStatus);
            formdata.append("Model[" + i + "].ApplicationMobile", item.ApplicationMobile);
            formdata.append("Model[" + i + "].ApplicationEmail", item.ApplicationEmail);
            formdata.append("Model[" + i + "].ApplicationDocumentTypeId", item.ApplicationDocumentTypeId);
            formdata.append("Model[" + i + "].ApplicationRequestedAmount", item.ApplicationRequestedAmount);
            item.ApplicationHobbies.forEach((val, index) => {
                formdata.append("Model[" + i + "].ApplicationHobbies[" + index + "]", val)
            });
          //  formdata.append("Model[" + i + "].ApplicationHobbies", item.ApplicationHobbies);
            formdata.append("Model[" + i + "].ApplicationRegisterDate", item.ApplicationRegisterDate);
            formdata.append("Model[" + i + "].ApplicationAddress", item.ApplicationAddress);
            formdata.append("Model[" + i + "].ApplicationCountryName", item.ApplicationCountryName);
            formdata.append("Model[" + i + "].ApplicationCountryId", item.ApplicationCountryId);
            formdata.append("Model[" + i + "].ApplicationStateId", item.ApplicationStateId);
            formdata.append("Model[" + i + "].ApplicationDistrictId", item.ApplicationDistrictId);
            formdata.append("Model[" + i + "].ApplicationIsAcceptedTermsAndConditions", item.ApplicationIsAcceptedTermsAndConditions);
            formdata.append("Model[" + i + "].DocumentFile", item.DocumentFile[0]['files'][0] ? item.DocumentFile[0]['files'][0] : null);
        }

        Swal.fire({
            title: "Do you want to save the changes?",
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Save",
            denyButtonText: `Don't save`
        }).then((result) => {

            if (result.isConfirmed) {
                $.ajax({
                    url: '/Appraisal/Saveallappraisals',
                    data: formdata,
                    type: 'POST',
                    contentType: false,
                    processData: false,
                    headers: {
                        'Accept': 'application/json' // Specify the Accept header to indicate the desired response format
                    },
                    success: (result) => {
                        if (result.IsSuccess == true) {
                            Swal.fire(result.Message, "", "success").then((result) => {
                                if (result.isConfirmed) {
                                    window.location.href = '/Appraisal/AppraisalList';
                                }
                            });
                        }
                        else {
                            Swal.fire(result.Message, "", "error")
                        }
                    },
                    error: (error) => this.onError(error)
                });
            }
            else if (result.isDenied) {
                Swal.fire("Changes are not saved", "", "info");
            }
        });
       

       
    }
    private deleteappraisalfromlist(event) {
        var deleterow = $(event).closest('tr').find('.appraisalid').val();
        alert(deleterow);
        const token = $('input[type=hidden][name=__RequestVerificationToken]').val() as string;
        AjaxPost('/Appraisal/DeleteFromappraisalList', { 'id': deleterow }, token, (result: BaseModel) => alert(result.Message), (error) => alert(error));
    }
}
interface FileDownloadWithByteArrayResult {
    FileBytes: string;
    FileContent: string; // Content type
    FileName: string;
    IsSuccess: boolean;
    Message: string;
}
// Function to toggle error message and red border for radio buttons
function toggleErrorMessage($input: JQuery, $message: JQuery) {

    let fieldName = ""; // Define an empty field name variable
    let isValid = true; // Initialize isValid to true
    // Get the column index of the input field
    let colIndex = $input.closest("td").index();

    // Get the field name from the corresponding column in the thead
    let $fieldNameTH = $("thead th").eq(colIndex);
    if ($fieldNameTH.length > 0) {
        fieldName = $fieldNameTH.text().trim();
    }

    let nameexp = new RegExp('^[A-Za-z ]*$');
    let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
    let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$");
    let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
    $input.closest("tr").find(".applicationqualification").on("change", function () {
        let $dropdown = $input.closest("tr").find(".applicationqualification") as JQuery<HTMLSelectElement>;
        let dropdownValue = $dropdown.val() as string;
        let $documenttypedropdown = $input.closest("tr").find(".applicationDocumentTypeId") as JQuery<HTMLSelectElement>;
        let documenttypedropdownValue = $documenttypedropdown.val() as string;
        let dataId = $input.closest("tr").find("i.bi.bi-file-arrow-down.downloadfilebyclientside").data("id");
        if (dataId <= 0 || dataId == "" || dataId == undefined) {
            if ($input.hasClass("applicationDocumentTypeId") || $input.hasClass("fileinput")) {
                if (dropdownValue === "SSC") {
                    let fileInput = $(".fileinput", $input.closest("tr"))[0] as HTMLInputElement;
                    if (fileInput.files.length === 0 && $input.hasClass("fileinput")) {
                        $input.addClass("is-invalid");
                        $message.text(`${$input.hasClass("applicationDocumentTypeId") ? `Please Select ${fieldName} when qualification is SSC.` : `Please select a file for ${fieldName} when qualification is SSC.`}`).addClass("text-danger").show();
                        isValid = false;
                    }
                    else if (documenttypedropdownValue === "" && $input.hasClass("applicationDocumentTypeId")) {
                        $input.addClass("is-invalid");
                        $message.text(`${$input.hasClass("applicationDocumentTypeId") ? `Please Select ${fieldName} when qualification is SSC.` : `Please select a file for ${fieldName} when qualification is SSC.`}`).addClass("text-danger").show();
                        isValid = false;
                    }

                    else {
                        $input.removeClass("is-invalid");
                        $message.hide().text("").removeClass("text-danger");
                    }
                } else {
                    // Remove validation for document type and file upload
                    $input.removeClass("is-invalid");
                    $message.hide().text("").removeClass("text-danger");
                }
            }
        }
        else {
            $input.removeClass("is-invalid");
            $message.hide().text("").removeClass("text-danger");
        }
       
    });
    if ($input.is(":checkbox") && !$input.hasClass("productidclass")) {
        // For checkboxes
        if (!$input.prop("checked")) {
            $input.addClass("is-invalid");
            $message.text(`${fieldName} is required.`).addClass("text-danger").show();
            isValid = false; // Set isValid to false if radio button is not checked
        } else {
            $input.removeClass("is-invalid");
            $message.hide().text("").removeClass("text-danger");
        }
    } else if ($input.is(":file")) {
        let fileInput = $input.get(0) as HTMLInputElement;
        let $dropdown = $input.closest("tr").find(".applicationqualification");
        let dropdownValue = $dropdown.val();

        if (dropdownValue === "SSC" && fileInput.files.length === 0) {
            let $row = $input.closest("tr");
            let dataId = $row.find("i.bi.bi-file-arrow-down.downloadfilebyclientside").data("id");
            if (dataId <= 0 || dataId == "" || dataId == undefined) {
                $input.addClass("is-invalid");
                $message.text(`Please select a file for ${fieldName} when qualification is SSC.`).addClass("text-danger").show();
                isValid = false;
            }
                
        }
        else {
            $input.removeClass("is-invalid");
            $message.hide().text("").removeClass("text-danger");
        }
        
   } else if ($input.is(":radio")) {
       // For radio buttons
       let radioGroupName = $input.attr("name");
       let $radioGroupContainer = $input.closest(".radio-inline");
       let $message = $radioGroupContainer.next(".error-message");

       let $checkedRadio = $("input[name='" + radioGroupName + "']:checked");

       if ($checkedRadio.length === 0) {
           $radioGroupContainer.addClass("is-invalid");
           $message.text(`Please select an option for ${fieldName}.`).addClass("text-danger").show();
           isValid = false; // Set isValid to false if radio button is not checked
       } else {
           $radioGroupContainer.removeClass("is-invalid");
           $message.hide().text("").removeClass("text-danger");
       }
   }
    else if ($input.is("textarea")) {
        // For text areas
        if ($input.val().toString().trim() === "") {
            $input.addClass("is-invalid");
            $message.text(`${fieldName} is required.`).addClass("text-danger").show(); // Show error message with field name in red color
            isValid = false; // Set isValid to false if radio button is not checked
        } else {
            $input.removeClass("is-invalid");
            $message.hide().text("").removeClass("text-danger"); // Hide error message and remove red color
        }
    } 
    else if ($input.is("select")) {
        // For text areas
        if (!$input.hasClass("applicationDocumentTypeId")) {
            if ($input.val().toString().trim() === "") {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} is required.`).addClass("text-danger").show(); // Show error message with field name in red color
                isValid = false; // Set isValid to false if radio button is not checked
            } else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger"); // Hide error message and remove red color
            }
        }
      
        let $dropdown = $input.closest("tr").find(".applicationqualification");
        let dropdownValue = $dropdown.val();
        if ($input.hasClass("applicationDocumentTypeId")) {
            if ($input.val().toString().trim() === "" && dropdownValue == "SSC") {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} is required.`).addClass("text-danger").show(); // Show error message with field name in red color
                isValid = false; // Set isValid to false if radio button is not checked
            } else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger"); // Hide error message and remove red color
            }
        }
       
    } 
    else {
       // For other input types
       // For other input types
        if ($input.val().toString().trim() === "" && !$input.hasClass("productidclass")) {
            if (!$input.is(":file") && !$input.hasClass("applicationDocumentTypeId")) {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} is required.`).addClass("text-danger").show(); // Show error message with field name in red color
                isValid = false; // Set isValid to false if radio button is not checked
            }
           
       } else {
           $input.removeClass("is-invalid");
           $message.hide().text("").removeClass("text-danger"); // Hide error message and remove red color
       }
   }


    if (isValid == false) {
        $input.attr("data-valid", isValid.toString());
        return isValid;
    }
    let value = $input.val().toString().trim();
    switch ($input.attr("data-validation")) {
        case "email":
            if (!emailexpression.test(value)) {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} must be a valid email address.`).addClass("text-danger").show();
                isValid = false;
            } else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
            }
            break;
        case "phone":
            if (!mobileexpression.test(value)) {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} must be a valid 10-digit phone number.`).addClass("text-danger").show();
                isValid = false;
            } else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
            }
            break;
        // Add more cases for other input validations as needed
        default:
            
    }
    return isValid;
}

function resetRowsindexes() {

    // Get all rows in the table body
    const rows = $('.maintbodyclass > tr');

    // Iterate through the rows and update indexes
    rows.each(function (index) {
        const row = $(this);

        // Update index in name attribute
        row.find('input, select, textarea').each(function () {
            const input = $(this);
            let name = input.attr('name');
            if (name) {
                name = name.replace(/\[\d+\]/, '[' + index + ']');
                input.attr('name', name);
            }

            // Update index in id attribute
            let id = input.attr('id');
            if (id) {
                id = id.replace(/z\d+/, 'z' + index);
                input.attr('id', id);
            }
        });

        // Update index in for attribute of labels
        row.find('label').each(function () {
            const label = $(this);
            const forAttr = label.attr('for');
            if (forAttr) {
                const newForAttr = forAttr.replace(/\d+/, index.toString());
                label.attr('for', newForAttr);
            }
        });
    });
}
    // Function to toggle validation based on dropdown change
   

/**/


