define("ApplicationAdd", ["require", "exports", "jquery", "Autocomplete", "AjaxRequests", "sweetalert2", "moment"], function (require, exports, $, Autocomplete_1, AjaxRequests_1, sweetalert2_1, moment) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ApplicationAdd = void 0;
    class ApplicationAdd {
        constructor() {
            this.onapplicationSelected = (result) => {
                const token = $('input[type=hidden][name=__RequestVerificationToken]').val();
                (0, AjaxRequests_1.AjaxPost)('/Application/Getappraisal', { 'id': result.Id }, token, (details) => this.getapplicationdetails(details), (error) => this.onerror(error));
            };
        }
        InitOnLoad() {
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
                        $('.changeLabel span').text('Appraisal Code');
                    }
                    if (value == 1) {
                        $("#appraisalDiv").hide();
                        $("#clientDiv").show();
                        $('.changeLabel span').text('Lead Search');
                    }
                }
            });
            this.applicationAutocomplete();
            $('#DateOfBirth').on('input', (event) => this.calculateAgeevent(event));
        }
        applicationAutocomplete() {
            const opts = {};
            opts.ElementId = 'ClientName';
            opts.ServiceUrl = '/Application/Getdata';
            opts.OptionalParams = {};
            opts.OnSearchItemSelect = this.onapplicationSelected;
            opts.OnSearchError = this.onapplicationError;
            const autocomplete = new Autocomplete_1.Autocomplete();
            autocomplete.Autocomplete(opts);
        }
        onapplicationError() {
            alert('error');
        }
        getapplicationdetails(result) {
            $('#AppraisalDate').val(moment(result.ApplicationRegisterdate).format('DD/MMM/YYYY'));
            $('#FirstName').val(result.ApplicationName);
            $('#Email').val(result.Email);
            $('#FatherName').val(result.FatherName);
            $('#MotherName').val(result.MotherName);
            $('#DateOfBirth').val(moment(result.DateOfBirth).format('DD/MMM/YYYY'));
            $('#Gender').val(result.Gender == 'M' ? '1' : '2');
        }
        onerror(error) {
            sweetalert2_1.default.fire('Error Ocuured', "", "error");
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
            const birthdate = $(event).val();
            const age = this.calculateAge(new Date(birthdate));
            $('#age').text(age);
        }
    }
    exports.ApplicationAdd = ApplicationAdd;
});
define("AppraisalAdd", ["require", "exports", "jquery", "moment", "Autocomplete", "AjaxRequests", "sweetalert2"], function (require, exports, $, moment, Autocomplete_2, AjaxRequests_2, sweetalert2_2) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.AppraisalAdd = void 0;
    class AppraisalAdd {
        constructor() {
            this.onCountryselected = (data) => {
                $('.applicationstatename option:not(:first-child)').remove();
                $('.applicationdistrictname option:not(:first-child)').remove();
                $('.applicationcountryname').val(data.Id);
                let countyId = parseInt(data.Id);
                if (countyId > 0) {
                    (0, AjaxRequests_2.AjaxGet)('/Appraisal/Getstates', { 'countryId': countyId }, (result) => this.onStatesSucess(result), (error) => this.onError(error));
                }
            };
            this.onCountryselectedList = (data, rowcount) => {
                $(`#z${rowcount}__ApplicationCountryName`).closest('tr').find('.applicationstatename option:not(:first-child)').remove();
                $(`#z${rowcount}__ApplicationCountryName`).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
                let countyId = parseInt(data.Id);
                if (countyId > 0) {
                    (0, AjaxRequests_2.AjaxGet)('/Appraisal/Getstates', { 'countryId': countyId }, (result) => this.onStatesSucessList(result, rowcount), (error) => this.onError(error));
                }
            };
        }
        InitOnLoad() {
            let form = $('#myForm');
            $('#myForm').submit(function (event) {
                if (form.valid() == true) {
                    $('#overlay').show();
                    $('#spinner').show();
                }
            });
            $('.datepicker').val(moment().format("DD-MMM-YYYY").toLowerCase());
            $('.selectdate').addClass('input-daterange');
            const optss = {};
            optss.format = 'dd-M-yyyy';
            optss.autoclose = true;
            optss.orientation = "bottom right";
            optss.todayHighlight = true;
            $('.input-daterange').datepicker(optss);
            $('.selectdate').show();
            let rowCount = $('.maintbodyclass').find('tr').length;
            for (let i = 0; i < rowCount; i++) {
                const elementId = `z${i}__ApplicationCountryName`;
                this.countriesautocompleteforlist(elementId);
            }
            $('tbody').find('tr').each(function () {
                $(this).find('input:not(.productidclass), select, textarea, button').prop('disabled', true);
                $(this).find('a, i').addClass('disabled').css('pointer-events', 'none');
            });
            $('.productidclass').change((event) => {
                var $checkbox = $(event.target);
                var isChecked = $checkbox.prop('checked');
                if (isChecked) {
                    $checkbox.closest('tr').find('input:not(.productidclass), select, textarea, button').prop('disabled', false);
                    $checkbox.closest('tr').find('a, i').removeClass('disabled').css('pointer-events', 'auto');
                }
                else {
                    $checkbox.closest('tr').find('input:not(.productidclass), select, textarea, button').prop('disabled', true);
                    $checkbox.closest('tr').find('a, i').addClass('disabled').css('pointer-events', 'none');
                }
                this.sumofrequestedamount();
            });
            $('#ApplicationCountryName').keydown((event) => {
                $('#ApplicationStateId option:not(:first-child)').remove();
                $('#ApplicationDistrictId option:not(:first-child)').remove();
                $('#ApplicationCountryId').val(0);
            });
            this.countriesautocomplete();
            $('#ApplicationStateId').change((event) => {
                $('.applicationdistrictname option:not(:first-child)').remove();
                let stateId = parseInt($(event.target).val());
                if (stateId > 0) {
                    (0, AjaxRequests_2.AjaxGet)('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucess(result), (error) => this.onError(error));
                }
            });
            $('.applicationstatename').change((event) => {
                $(event.target).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
                let stateId = parseInt($(event.target).val());
                if (stateId > 0) {
                    (0, AjaxRequests_2.AjaxGet)('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucessList(result, event), (error) => this.onError(error));
                }
            });
            $('.fileinput').change(function (event) {
                let file = event.target.files[0];
                if (file && file.type === 'application/pdf' || file.type === 'image/jpeg') {
                    var reader = new FileReader();
                    reader.onload = function () {
                        var dataUri = reader.result;
                        $('#pdfViewer').attr('src', dataUri);
                        const $iframe = $(this).closest('td').find('.previewFrame');
                        $iframe.attr('src', dataUri);
                    };
                    reader.readAsDataURL(file);
                }
                else {
                    alert('Please select a PDF file.');
                }
            });
            $('#appraisalsave').on('click', () => { this.appraisaldetailssave(); });
            $('.uploadIcon').on('click', (event) => { $(event.target).closest('tr').find('.fileinput').click(); });
            $('.downloadfilebyclientside,.viewfilebyclientside').on('click', (event) => {
                var action = $(event.currentTarget).is('.downloadfilebyclientside') ? 'download' : 'view';
                var id = $(event.currentTarget).data('id');
                var documentId = $(event.currentTarget).data('documentid');
                this.viewordownloadfilebyclient(id, documentId, action);
            });
            $('#addappraisalrow').on('click', (event) => { this.addappraisalrow(event.target); });
            $('#appraisalselectall').on('change', (event) => this.checAllRows(event.target));
            $("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").on("input", (event) => {
                let $row = $(event.target).closest('tr');
                $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                    let $input = $(this);
                    let $message = $input.next(".error-message");
                    let isValidInput = toggleErrorMessage($input, $message);
                });
            });
            $('.maintableclass').on('click', 'tbody tr td .addappraisalinlist', (event) => { this.appraisalsavefromlist(event); });
            $('.maintableclass').on('click', 'tbody tr td .updateappraisalfromlist', (event) => { this.appraisalsavefromlist(event); });
            $('.maintableclass').on('click', 'tbody tr td .removeappraisalinlist', (event) => this.removeappraisalfromlist(event.target));
            this.sumofrequestedamount();
            $('.maintableclass').on('click', 'tfoot tr td .savelist', (event) => { this.saveallappraisalsinlist(event.target); });
            $('.maintableclass').on('click', 'tbody tr td .deleteappraisal', (event) => { this.deleteappraisalfromlist(event.target); });
        }
        countriesautocomplete() {
            const opts = {};
            opts.ElementId = 'ApplicationCountryName';
            opts.ServiceUrl = '/Appraisal/GetCountries';
            opts.OptionalParams = {};
            opts.OnSearchItemSelect = this.onCountryselected;
            opts.OnSearchError = this.onError;
            const autocomplete = new Autocomplete_2.Autocomplete();
            autocomplete.Autocomplete(opts);
        }
        onStatesSucess(data) {
            $.each(data, (index, item) => {
                $('.applicationstatename').append($('<option>', {
                    value: item.Id,
                    text: item.Name
                }));
            });
        }
        onDistrictsSucess(data) {
            $.each(data, (index, item) => {
                $('.applicationdistrictname').append($('<option>', {
                    value: item.Id,
                    text: item.Name
                }));
            });
        }
        countriesautocompleteforlist(elementId) {
            const opts = {};
            opts.ElementId = elementId;
            opts.ServiceUrl = '/Appraisal/GetCountries';
            opts.OptionalParams = {};
            opts.OnSearchItemSelect = (data) => {
                $(elementId).val(data.Id);
                this.onCountryselectedList(data, $('#' + elementId).closest('tr').index());
            };
            opts.OnSearchError = (err) => {
                sweetalert2_2.default.fire('Error on getting Countries', 'error');
            };
            const autocomplete = new Autocomplete_2.Autocomplete();
            autocomplete.Autocomplete(opts);
        }
        checAllRows(result) {
            let checkBoxes = $('.maintableclass').find('input:checkbox.productidclass');
            $(checkBoxes).prop('checked', $(result).is(':checked'));
            $('.productidclass').trigger('change');
        }
        appraisaldetailssave() {
            let name = $('#ApplicationName').val();
            let nameexp = new RegExp('^[A-Za-z ]*$');
            if (name == "") {
                sweetalert2_2.default.fire("Please Enter name", "", "error");
                return;
            }
            if (nameexp.test(name) === false) {
                sweetalert2_2.default.fire('Name must be in Alphabets with or without Spaces Only', "", 'error');
                return;
            }
            let fathername = $('#Applicationfathername').val();
            if (fathername == "") {
                sweetalert2_2.default.fire('Please Enter Father Name', "", 'error');
                return;
            }
            if (nameexp.test(fathername) === false) {
                sweetalert2_2.default.fire('Father Name must be in Alphabets with or without Spaces Only', "", 'error');
                return;
            }
            let mothername = $('#Applicationmothername').val();
            if (mothername == "") {
                sweetalert2_2.default.fire('Please Enter Mother Name', "", 'error');
                return;
            }
            if (nameexp.test(mothername) === false) {
                sweetalert2_2.default.fire('Mother Name must be in Alphabets with or without Spaces Only', "", 'error');
                return;
            }
            let dob = $('#Applicationdob').val();
            let gender = $('#Applicationgender').val();
            if (gender == "") {
                sweetalert2_2.default.fire('Please select Gender', "", 'error');
                return;
            }
            let qualification = $('#Applicationqualification').val();
            if (qualification == "") {
                sweetalert2_2.default.fire('Please Select Qualification', "", 'error');
                return;
            }
            let applicationMartialStatus = $('#ApplicationMartialStatus').val();
            if (applicationMartialStatus == "") {
                sweetalert2_2.default.fire('Please select ApplicationMartialStatus', "", 'error');
                return;
            }
            let applicationmobile = $('#Applicationmobile').val();
            let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
            if (applicationmobile == "") {
                sweetalert2_2.default.fire('Please Enter Mobile Number', "", 'error');
                return;
            }
            if (mobileexpression.test(applicationmobile) === false) {
                sweetalert2_2.default.fire('Mobile Number Must start with 9,8,7 and 10 digits only', "error");
                return;
            }
            let email = $('#Applicationemail').val();
            let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$");
            if (email == "") {
                sweetalert2_2.default.fire("Please Enter Email", "", "error");
                return;
            }
            if (emailexpression.test(email) === false) {
                sweetalert2_2.default.fire("Enter Correct Email", "", "error");
                return;
            }
            let documenttypeid = $('#ApplicationDocumentTypeId').val();
            if (documenttypeid == "") {
                sweetalert2_2.default.fire("Please Select Application Document Type", "", "error");
                return;
            }
            let loanamount = $('#ApplicationRequestedAmount').val();
            let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
            if (loanamount == "") {
                sweetalert2_2.default.fire("Please enter Loan amount", "", "error");
                return;
            }
            if (loanamountexpreession.test(loanamount) === false) {
                sweetalert2_2.default.fire("Loan Amount must be in Number with or without decimals", "", "error");
                return;
            }
            let applicationhobbies = $('#ApplicationHobbies').val();
            if (applicationhobbies.length <= 0) {
                sweetalert2_2.default.fire("Please Select application hobbies", "", "error");
                return;
            }
            let registerdate = $('#ApplicationRegisterdate').val();
            let address = $('#ApplicationAddress').val();
            if (address == "") {
                sweetalert2_2.default.fire("Please Enter Address", "", "error");
                return;
            }
            let applicationcountryid = $('#ApplicationCountryId').val();
            if (applicationcountryid == "") {
                sweetalert2_2.default.fire("Please Select Country", "", "error");
                return;
            }
            let applicationstateid = $('#ApplicationStateId').val();
            if (applicationstateid == "") {
                sweetalert2_2.default.fire("Please Select State", "", "error");
                return;
            }
            let applicationdistrictid = $('#ApplicationDistrictId').val();
            if (applicationdistrictid == "") {
                sweetalert2_2.default.fire("Please Select District", "", "error");
                return;
            }
            let documentfile = $('#DocumentFile');
            if (documentfile.val() == "") {
                sweetalert2_2.default.fire("Please Select File", "", "error");
                return;
            }
            let ApplicationIsAcceptedTermsandConditions = $('#ApplicationIsAcceptedTermsandConditions').is(':checked');
            if (ApplicationIsAcceptedTermsandConditions == false) {
                sweetalert2_2.default.fire("Please check checkbox", "", "error");
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
                formdata.append("ApplicationHobbies[" + i + "]", val);
            });
            formdata.append("ApplicationRegisterdate", registerdate);
            formdata.append("ApplicationIsAcceptedTermsandConditions", ApplicationIsAcceptedTermsandConditions.toString());
            formdata.append("ApplicationAddress", address);
            formdata.append("ApplicationDistrictId", applicationdistrictid);
            formdata.append("ApplicationStateId", applicationstateid);
            formdata.append("ApplicationCountryId", applicationcountryid);
            formdata.append("DocumentFile", documentfile[0]['files'][0]);
            const token = $('input[type=hidden][name=__RequestVerificationToken]').val();
            sweetalert2_2.default.fire({
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
                            'Accept': 'application/json'
                        },
                        success: (result) => this.appraisalsucess(result),
                        error: (error) => this.onError(error)
                    });
                }
                else if (result.isDenied) {
                    sweetalert2_2.default.fire("Changes are not saved", "", "info");
                }
            });
        }
        appraisalsucess(result) {
            if (result.IsSuccess == true) {
                sweetalert2_2.default.fire(result.Message, "", "success").then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/Appraisal/AddAppraisal';
                    }
                });
            }
            else {
                sweetalert2_2.default.fire(result.Message, "", "error");
            }
        }
        onError(error) {
            sweetalert2_2.default.fire('Error occured', "", "error");
        }
        viewordownloadfilebyclient(id, documentid, action) {
            const token = $('input[type=hidden][name=__RequestVerificationToken]').val();
            (0, AjaxRequests_2.AjaxPost)('/Appraisal/ViewFileByClientSide', { 'id': id, 'documentId': documentid }, token, (result) => action == 'download' ? this.downloadFileFromByteArray(result) : this.viewfilefrombytearray(result), (error) => this.onError(error));
        }
        downloadFileFromByteArray(result) {
            var element = document.createElement('a');
            element.href = window.URL.createObjectURL(this.getblobfrombytearraystring(result.FileBytes, result.FileContent));
            element.download = result.FileName;
            element.style.display = 'none';
            document.body.appendChild(element);
            element.click();
            document.body.removeChild(element);
        }
        viewfilefrombytearray(result) {
            var url = window.URL.createObjectURL(this.getblobfrombytearraystring(result.FileBytes, result.FileContent));
            window.open(url, '_blank');
            window.URL.revokeObjectURL(url);
        }
        getblobfrombytearraystring(filebytes, filecontent) {
            var byteCharacters = atob(filebytes);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var blob = new Blob([byteArray], {
                type: filecontent
            });
            return blob;
        }
        addappraisalrow(event) {
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
            this.gettablelivecount();
            $('.datepicker').val(moment().format("DD-MMM-YYYY").toLowerCase());
            $('.selectdate').addClass('input-daterange');
            const optss = {};
            optss.format = 'dd-M-yyyy';
            optss.autoclose = true;
            optss.orientation = "bottom right";
            optss.todayHighlight = true;
            $('.input-daterange').datepicker(optss);
            $('.selectdate').show();
            const opts = {};
            opts.ElementId = `z${rowCount}__ApplicationCountryName`;
            opts.ServiceUrl = '/Appraisal/GetCountries';
            opts.OnSearchItemSelect = (data) => {
                $(`#z${rowCount}__ApplicationCountryId`).val(data.Id);
                this.onCountryselectedList(data, rowCount);
            };
            opts.OnSearchError = (err) => {
                sweetalert2_2.default.fire('Error on getting Countries', 'error');
            };
            opts.OptionalParams = {};
            const autocomplete = new Autocomplete_2.Autocomplete();
            autocomplete.Autocomplete(opts);
            $('.applicationstatename').change((event) => {
                $(event.target).closest('tr').find('.applicationdistrictname option:not(:first-child)').remove();
                let stateId = parseInt($(event.target).val());
                if (stateId > 0) {
                    (0, AjaxRequests_2.AjaxGet)('/Appraisal/GetDistricts', { 'stateId': stateId }, (result) => this.onDistrictsSucessList(result, event), (error) => this.onError(error));
                }
            });
            $('.uploadIcon').on('click', (event) => { $(event.target).closest('tr').find('.fileinput').click(); });
            $('.fileinput').change(function (event) {
                let file = event.target.files[0];
                if (file && file.type === 'application/pdf' || file.type === 'image/jpeg') {
                    var reader = new FileReader();
                    $(event.target).closest('tr').find('.spinner').show();
                    reader.onload = function () {
                        var dataUri = reader.result;
                        const $iframe = $(event.target).closest('tr').find('.previewFrame');
                        $iframe.attr('src', dataUri);
                        $(event.target).closest('tr').find('.spinner').hide();
                    };
                    reader.readAsDataURL(file);
                }
                else {
                    alert('Please select a PDF file.');
                }
            });
            $("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").on("input", (event) => {
                let $row = $(event.target).closest('tr');
                $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                    let $input = $(this);
                    let $message = $input.next(".error-message");
                    let isValidInput = toggleErrorMessage($input, $message);
                });
            });
        }
        gettablelivecount() {
            let count = $("table.maintableclass > tbody").find('tr');
            $('#count').text('Total Count : ' + count.length + ' ');
        }
        onStatesSucessList(data, rowCount) {
            let $select = $(`#z${rowCount}__ApplicationCountryName`).closest('tr').find('.applicationstatename');
            $.each(data, (index, item) => {
                $select.append($('<option>', {
                    value: item.Id,
                    text: item.Name
                }));
            });
        }
        onDistrictsSucessList(data, event) {
            $.each(data, (index, item) => {
                $(event.target).closest('tr').find('.applicationdistrictname').append($('<option>', {
                    value: item.Id,
                    text: item.Name
                }));
            });
        }
        appraisalsavefromlist(event) {
            let isValid = true;
            let $row = $(event.target).closest('tr');
            $row.find("input[type='text'], input[type='checkbox'], input[type='file'], input[type='radio'], select,textarea").each(function () {
                let $input = $(this);
                let $message = $input.next(".error-message");
                let isValidInput = toggleErrorMessage($input, $message);
                isValid = isValid && isValidInput;
            });
            if (isValid) {
                let appraisalId = $row.find('.appraisalid').val();
                let productId = $row.find('.productidclass').prop('checked');
                let applicationName = $row.find('.applicationName').val();
                let applicationFatherName = $row.find('.applicationfathername').val();
                let applicationMotherName = $row.find('.applicationmothername').val();
                let applicationDob = $row.find('.applicationdob').val();
                let applicationGender = $row.find('.applicationgender:checked').val();
                let applicationQualification = $row.find('.applicationqualification').val();
                let applicationMartialStatus = $row.find('.applicationMartialStatus').val();
                let applicationMobile = $row.find('.applicationmobile').val();
                let applicationEmail = $row.find('.applicationemail').val();
                let applicationDocumentTypeId = $row.find('.applicationDocumentTypeId').val();
                let applicationRequestedAmount = $row.find('.applicationRequestedAmount').val();
                let applicationHobbies = $row.find('.applicationHobbies').val();
                let applicationRegisterDate = $row.find('.applicationRegisterdate').val();
                let applicationAddress = $row.find('.applicationAddress').val();
                let applicationCountryName = $row.find('.applicationcountryname').val();
                let applicationCountryId = $row.find('.applicationCountryId').val();
                let applicationStateId = $row.find('.applicationstatename').val();
                let applicationDistrictId = $row.find('.applicationdistrictname').val();
                let applicationIsAcceptedTermsAndConditions = $row.find('.applicationIsAcceptedTermsandConditions').is(':checked');
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
                    formdata.append("ApplicationHobbies[" + i + "]", val);
                });
                formdata.append("ApplicationRegisterdate", applicationRegisterDate);
                formdata.append("ApplicationIsAcceptedTermsandConditions", applicationIsAcceptedTermsAndConditions.toString());
                formdata.append("ApplicationAddress", applicationAddress);
                formdata.append("ApplicationDistrictId", applicationDistrictId);
                formdata.append("ApplicationStateId", applicationStateId);
                formdata.append("ApplicationCountryId", applicationCountryId);
                formdata.append("DocumentFile", documentfile[0]['files'][0]);
                const token = $('input[type=hidden][name=__RequestVerificationToken]').val();
                sweetalert2_2.default.fire({
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
                                'Accept': 'application/json'
                            },
                            success: (result) => {
                                if (result.IsSuccess == true) {
                                    sweetalert2_2.default.fire(result.Message, "", "success").then((result) => {
                                        if (result.isConfirmed) {
                                            window.location.href = '/Appraisal/AppraisalList';
                                        }
                                    });
                                }
                                else {
                                    sweetalert2_2.default.fire(result.Message, "", "error");
                                }
                            },
                            error: (error) => this.onError(error)
                        });
                    }
                    else if (result.isDenied) {
                        sweetalert2_2.default.fire("Changes are not saved", "", "info");
                    }
                });
            }
            else {
                return false;
            }
        }
        removeappraisalfromlist(deleterow) {
            var a = $(deleterow).closest('tr').remove();
            this.gettablelivecount();
            resetRowsindexes();
        }
        sumofrequestedamount() {
            var totalpaidall = 0;
            var tablerowsall = $('table.maintableclass > tbody > tr').find('.applicationRequestedAmount');
            $(tablerowsall).each(function (i, element) {
                totalpaidall += Number(element['value']);
            });
            $('#totalpaidsum').text('Total Paid : ' + totalpaidall);
            var totalPaid = 0;
            var tableRows = $('table.maintableclass > tbody > tr').filter((data, row) => {
                return $(row).find('.productidclass').is(':checked');
            });
            if (tableRows.length > 0) {
                $(tableRows).each(function () {
                    var row = $(this);
                    let amount = Number(row.find('.applicationRequestedAmount ').val());
                    totalPaid += amount;
                });
            }
            $('#totalpaidforcheckrows').text('Total Paid For Selected Rows: ' + totalPaid);
            var totalPaidformale = 0;
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
            var totalPaidforfemale = 0;
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
            var totalPaidforssc = 0;
            var tableRowsForSSC = $('table.maintableclass > tbody > tr').filter((index, row) => {
                var selectedValue = $(row).find('.applicationqualification').val();
                return selectedValue === "SSC";
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
        saveallappraisalsinlist(event) {
            var tableRows = $('table.maintableclass > tbody > tr').filter((data, row) => {
                return $(row).find('.productidclass').is(':checked');
            });
            if (tableRows.length <= 0) {
                sweetalert2_2.default.fire("No apprasials are selected", "", "error");
                return false;
            }
            var validationFailed = false;
            let appraisalsavelist = [];
            tableRows.each((i, row) => {
                let appraisalId = $(row).find('.appraisalid').val();
                let nameexp = new RegExp('^[A-Za-z ]*$');
                let applicationName = $(row).find('.applicationName').val();
                if (applicationName == "" || applicationName == null) {
                    sweetalert2_2.default.fire("Please Enter Name", "", "error");
                    validationFailed = true;
                    return false;
                }
                if (!nameexp.test(applicationName)) {
                    sweetalert2_2.default.fire("Please enter correct name", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationFatherName = $(row).find('.applicationfathername').val();
                if (applicationFatherName == "" || applicationFatherName == null) {
                    sweetalert2_2.default.fire("Please Enter Father Name", "", "error");
                    validationFailed = true;
                    return false;
                }
                if (!nameexp.test(applicationFatherName)) {
                    sweetalert2_2.default.fire("Please Enter Correct Father name", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationMotherName = $(row).find('.applicationmothername').val();
                if (applicationMotherName == "" || applicationMotherName == null) {
                    sweetalert2_2.default.fire("Please Enter Mother Name", "", "error");
                    validationFailed = true;
                    return false;
                }
                if (!nameexp.test(applicationFatherName)) {
                    sweetalert2_2.default.fire("Please Enter Correct Other name", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationDob = $(row).find('.applicationdob').val();
                let applicationGender = $(row).find('.applicationgender:checked').val();
                if (applicationGender == "" || applicationGender == undefined) {
                    sweetalert2_2.default.fire("Please Select Gender", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationQualification = $(row).find('.applicationqualification').val();
                if (applicationQualification == "" || applicationQualification == null) {
                    sweetalert2_2.default.fire("Please Select Qualification", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationMartialStatus = $(row).find('.applicationMartialStatus').val();
                if (applicationMartialStatus == "" || applicationMartialStatus == null) {
                    sweetalert2_2.default.fire("Please select Martail Status", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationMobile = $(row).find('.applicationmobile').val();
                let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
                if (applicationMobile == null || applicationMobile == "") {
                    sweetalert2_2.default.fire("Please Enter Mobile Number", "", "error");
                    validationFailed = true;
                    return false;
                }
                if (!mobileexpression.test(applicationMobile)) {
                    sweetalert2_2.default.fire("Mobile Number Must Be 10 digits", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationEmail = $(row).find('.applicationemail').val();
                if (applicationEmail == null || applicationEmail == "") {
                    sweetalert2_2.default.fire("Please enter email", "", "error");
                    validationFailed = true;
                    return false;
                }
                let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$");
                if (!emailexpression.test(applicationEmail)) {
                    sweetalert2_2.default.fire("Enter correct email", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationDocumentTypeId = $(row).find('.applicationDocumentTypeId').val();
                if (applicationDocumentTypeId == "" || applicationDocumentTypeId == "0" || applicationDocumentTypeId == null) {
                    sweetalert2_2.default.fire("Please Select Document Type", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationRequestedAmount = $(row).find('.applicationRequestedAmount').val();
                let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
                if (applicationRequestedAmount == "0" || applicationRequestedAmount == null) {
                    sweetalert2_2.default.fire("Please enter request amount", "", "error");
                    validationFailed = true;
                    return false;
                    ;
                }
                if (!loanamountexpreession.test(applicationRequestedAmount)) {
                    sweetalert2_2.default.fire("Requested amount br in numbers and two decimals only", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationHobbies = $(row).find('.applicationHobbies').val();
                if (applicationHobbies.length == 0) {
                    sweetalert2_2.default.fire("Please Select Hobbies", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationRegisterDate = $(row).find('.applicationRegisterdate').val();
                let applicationAddress = $(row).find('.applicationAddress').val();
                if (applicationAddress == "" || applicationAddress == null) {
                    sweetalert2_2.default.fire("Please Enter addres", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationCountryName = $(row).find('.applicationcountryname').val();
                let applicationCountryId = $(row).find('.applicationCountryId').val();
                if (applicationCountryId == null || applicationCountryId == "0") {
                    sweetalert2_2.default.fire("Please Search Country Name And Select Proper country", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationStateId = $(row).find('.applicationstatename').val();
                if (applicationStateId == "" || applicationStateId == "0") {
                    sweetalert2_2.default.fire("Please Select state", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationDistrictId = $(row).find('.applicationdistrictname').val();
                if (applicationDistrictId == "" || applicationDistrictId == "0") {
                    sweetalert2_2.default.fire("Please Select District", "", "error");
                    validationFailed = true;
                    return false;
                }
                let applicationIsAcceptedTermsAndConditions = $(row).find('.applicationIsAcceptedTermsandConditions').is(':checked');
                let documentfile = $(row).find('.fileinput');
                if (documentfile[0]['files'][0] == null && applicationQualification == "SSC") {
                    sweetalert2_2.default.fire("Please Upload File for SSC Qualification", "", "error");
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
                    formdata.append("Model[" + i + "].ApplicationHobbies[" + index + "]", val);
                });
                formdata.append("Model[" + i + "].ApplicationRegisterDate", item.ApplicationRegisterDate);
                formdata.append("Model[" + i + "].ApplicationAddress", item.ApplicationAddress);
                formdata.append("Model[" + i + "].ApplicationCountryName", item.ApplicationCountryName);
                formdata.append("Model[" + i + "].ApplicationCountryId", item.ApplicationCountryId);
                formdata.append("Model[" + i + "].ApplicationStateId", item.ApplicationStateId);
                formdata.append("Model[" + i + "].ApplicationDistrictId", item.ApplicationDistrictId);
                formdata.append("Model[" + i + "].ApplicationIsAcceptedTermsAndConditions", item.ApplicationIsAcceptedTermsAndConditions);
                formdata.append("Model[" + i + "].DocumentFile", item.DocumentFile[0]['files'][0] ? item.DocumentFile[0]['files'][0] : null);
            }
            sweetalert2_2.default.fire({
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
                            'Accept': 'application/json'
                        },
                        success: (result) => {
                            if (result.IsSuccess == true) {
                                sweetalert2_2.default.fire(result.Message, "", "success").then((result) => {
                                    if (result.isConfirmed) {
                                        window.location.href = '/Appraisal/AppraisalList';
                                    }
                                });
                            }
                            else {
                                sweetalert2_2.default.fire(result.Message, "", "error");
                            }
                        },
                        error: (error) => this.onError(error)
                    });
                }
                else if (result.isDenied) {
                    sweetalert2_2.default.fire("Changes are not saved", "", "info");
                }
            });
        }
        deleteappraisalfromlist(event) {
            var deleterow = $(event).closest('tr').find('.appraisalid').val();
            alert(deleterow);
            const token = $('input[type=hidden][name=__RequestVerificationToken]').val();
            (0, AjaxRequests_2.AjaxPost)('/Appraisal/DeleteFromappraisalList', { 'id': deleterow }, token, (result) => alert(result.Message), (error) => alert(error));
        }
    }
    exports.AppraisalAdd = AppraisalAdd;
    function toggleErrorMessage($input, $message) {
        let fieldName = "";
        let isValid = true;
        let colIndex = $input.closest("td").index();
        let $fieldNameTH = $("thead th").eq(colIndex);
        if ($fieldNameTH.length > 0) {
            fieldName = $fieldNameTH.text().trim();
        }
        let nameexp = new RegExp('^[A-Za-z ]*$');
        let mobileexpression = new RegExp('^[9,8,7]{1}[0-9]{9}$');
        let emailexpression = new RegExp("^[a-zA-z0-9]*@[a-z]+[/.][a-z]{2,3}$");
        let loanamountexpreession = new RegExp("^[0-9]+([.][0-9]{2})?$");
        $input.closest("tr").find(".applicationqualification").on("change", function () {
            let $dropdown = $input.closest("tr").find(".applicationqualification");
            let dropdownValue = $dropdown.val();
            let $documenttypedropdown = $input.closest("tr").find(".applicationDocumentTypeId");
            let documenttypedropdownValue = $documenttypedropdown.val();
            let dataId = $input.closest("tr").find("i.bi.bi-file-arrow-down.downloadfilebyclientside").data("id");
            if (dataId <= 0 || dataId == "" || dataId == undefined) {
                if ($input.hasClass("applicationDocumentTypeId") || $input.hasClass("fileinput")) {
                    if (dropdownValue === "SSC") {
                        let fileInput = $(".fileinput", $input.closest("tr"))[0];
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
                    }
                    else {
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
            if (!$input.prop("checked")) {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} is required.`).addClass("text-danger").show();
                isValid = false;
            }
            else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
            }
        }
        else if ($input.is(":file")) {
            let fileInput = $input.get(0);
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
        }
        else if ($input.is(":radio")) {
            let radioGroupName = $input.attr("name");
            let $radioGroupContainer = $input.closest(".radio-inline");
            let $message = $radioGroupContainer.next(".error-message");
            let $checkedRadio = $("input[name='" + radioGroupName + "']:checked");
            if ($checkedRadio.length === 0) {
                $radioGroupContainer.addClass("is-invalid");
                $message.text(`Please select an option for ${fieldName}.`).addClass("text-danger").show();
                isValid = false;
            }
            else {
                $radioGroupContainer.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
            }
        }
        else if ($input.is("textarea")) {
            if ($input.val().toString().trim() === "") {
                $input.addClass("is-invalid");
                $message.text(`${fieldName} is required.`).addClass("text-danger").show();
                isValid = false;
            }
            else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
            }
        }
        else if ($input.is("select")) {
            if (!$input.hasClass("applicationDocumentTypeId")) {
                if ($input.val().toString().trim() === "") {
                    $input.addClass("is-invalid");
                    $message.text(`${fieldName} is required.`).addClass("text-danger").show();
                    isValid = false;
                }
                else {
                    $input.removeClass("is-invalid");
                    $message.hide().text("").removeClass("text-danger");
                }
            }
            let $dropdown = $input.closest("tr").find(".applicationqualification");
            let dropdownValue = $dropdown.val();
            if ($input.hasClass("applicationDocumentTypeId")) {
                if ($input.val().toString().trim() === "" && dropdownValue == "SSC") {
                    $input.addClass("is-invalid");
                    $message.text(`${fieldName} is required.`).addClass("text-danger").show();
                    isValid = false;
                }
                else {
                    $input.removeClass("is-invalid");
                    $message.hide().text("").removeClass("text-danger");
                }
            }
        }
        else {
            if ($input.val().toString().trim() === "" && !$input.hasClass("productidclass")) {
                if (!$input.is(":file") && !$input.hasClass("applicationDocumentTypeId")) {
                    $input.addClass("is-invalid");
                    $message.text(`${fieldName} is required.`).addClass("text-danger").show();
                    isValid = false;
                }
            }
            else {
                $input.removeClass("is-invalid");
                $message.hide().text("").removeClass("text-danger");
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
                }
                else {
                    $input.removeClass("is-invalid");
                    $message.hide().text("").removeClass("text-danger");
                }
                break;
            case "phone":
                if (!mobileexpression.test(value)) {
                    $input.addClass("is-invalid");
                    $message.text(`${fieldName} must be a valid 10-digit phone number.`).addClass("text-danger").show();
                    isValid = false;
                }
                else {
                    $input.removeClass("is-invalid");
                    $message.hide().text("").removeClass("text-danger");
                }
                break;
            default:
        }
        return isValid;
    }
    function resetRowsindexes() {
        const rows = $('.maintbodyclass > tr');
        rows.each(function (index) {
            const row = $(this);
            row.find('input, select, textarea').each(function () {
                const input = $(this);
                let name = input.attr('name');
                if (name) {
                    name = name.replace(/\[\d+\]/, '[' + index + ']');
                    input.attr('name', name);
                }
                let id = input.attr('id');
                if (id) {
                    id = id.replace(/z\d+/, 'z' + index);
                    input.attr('id', id);
                }
            });
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
});
define("GetApplications", ["require", "exports", "jquery", "Autocomplete", "AjaxRequests", "moment", "sweetalert2"], function (require, exports, $, Autocomplete_3, AjaxRequests_3, moment, sweetalert2_3) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.GetApplications = void 0;
    class GetApplications {
        constructor() {
            this.onapplicationSelected = (data) => {
                $('#Id').val(data.Id);
                (0, AjaxRequests_3.AjaxGet)('/GetApplications/Getapplicantdetails', { 'applicantid': data.Id }, success => this.onApplicantdetailssucess(success), error => this.onApplicantdetailserror(error));
            };
            this.onapplicationError = () => {
                sweetalert2_3.default.fire('Error occured');
            };
        }
        InitOnLoad() {
            $('#divcontainer').hide();
            this.applicationAutocomplete();
            $('.selectdate').addClass('input-daterange');
            const optss = {};
            optss.format = 'dd-M-yyyy';
            optss.autoclose = true;
            optss.orientation = "bottom right";
            optss.todayHighlight = true;
            $('.input-daterange').datepicker(optss);
            $('.selectdate').show();
        }
        applicationAutocomplete() {
            const opts = {};
            opts.ElementId = 'ApplicationName';
            opts.ServiceUrl = '/GetApplications/Getdata';
            opts.OptionalParams = {};
            opts.OnSearchItemSelect = this.onapplicationSelected;
            opts.OnSearchError = this.onapplicationError;
            const autocomplete = new Autocomplete_3.Autocomplete();
            autocomplete.Autocomplete(opts);
        }
        onApplicantdetailssucess(sucess) {
            $('#divcontainer').slideDown('slow');
            $('#getapplicationname').text(sucess.ApplicationName);
            $('#getApplicationRegisterdate').text(moment(sucess.ApplicationRegisterdate).format('DD/MM/yyyy'));
            $('#getApplicationmobile').text(sucess.Applicationmobile);
            $('#getApplicationRequestedAmount').text(sucess.ApplicationRequestedAmount);
        }
        onApplicantdetailserror(error) {
        }
    }
    exports.GetApplications = GetApplications;
});
define("file", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
//# sourceMappingURL=applicationdetails.js.map