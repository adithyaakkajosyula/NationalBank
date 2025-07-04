
(function (global) {
    SystemJS.config({
        paths: {
            "jquery": "/lib/jquery/jquery.min.js",
            "jquery-validation": "/lib/jquery-validation/dist/jquery.validate.min.js",
            "jqueryunob": "/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
            "jquerazaxunobtrusive":"/lib/jquery.unobtrusive-ajax.js",
            "bootstrap": "https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js",
            "typeahead-jquery": "/lib/Typeahead/typeahead.jquery.js",
            "typeahead-bundle": "/lib/Typeahead/typeahead.bundle.js",
            "bloodhound": "/lib/Typeahead/bloodhound.js",
            "site": "/js/site.min.js",
            "sweetalert2": "/lib/sweetalert2/sweetalert2.min.js",
            "jqueryauto": "/lib/jquery.autocomplete.min.js",
            "test": "/js/test.js",
            "applicationdetails": "/js/applicationdetails.js",
            "commons": "/js/commons.js",
            "bootstrap-datepicker": "https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.10.0/js/bootstrap-datepicker.min.js",
            "moment":"/lib/moment/moment.min.js"
        },
        packages: {
            'jquery': {
                defaultExtension: 'js'
            },
            'jquery-validation': {
                defaultExtension: 'js'
            },
            'jqueryunob': {
                defaultExtension: 'js'
            },
            'jquerazaxunobtrusive': {
                defaultExtension: 'js'
            },
            'typeahead-jquery': {
                defaultExtension: 'js'
            },
            'bootstrap': {
                defaultExtension: 'js'
            },
            'typeahead-bundle': {
                defaultExtension: 'js'
            },
            'bloodhound': {
                defaultExtension: 'js'
            },
            'sweetalert2': {
                defaultExtension: 'js'
            },
            'jqueryauto': {
                defaultExtension: 'js'
            },
            'bootstrap-datepicker': {
                defaultExtension: 'js'
            },
            'site': {
                defaultExtension: 'js'
            },
            'moment': {
                defaultExtension: 'js'
            },
            'commons': {
                format: 'amd',
                defaultExtension: 'js',
                meta: {
                    '*': {
                        deps: [
                            'jquery',
                            'commons'
                        ]
                    }
                }
            },
            'test': {
                format: 'amd',
                defaultExtension: 'js',
                meta: {
                    '*': {
                        deps: [
                            'jquery',
                            'test'
                        ]
                    }
                }
            },
            'applicationdetails': {
                format: 'amd',
                defaultExtension: 'js',
                meta: {
                    '*': {
                        deps: [
                            'jquery',
                            'applicationdetails'
                        ]
                    }
                }
            },
        },
        meta: {
            'jquery-validation': {
                deps: ['jquery']
            },
            'jqueryunob': {
                deps: ['jquery']
            },
            'typeahead-jquery': {
                deps: ['jquery']
            },
            'typeahead-bundle': {
                deps: ['jquery']
            },
            'bloodhound': {
                deps: ['jquery']
            },
            'sweetalert2': {
                deps: ['jquery']
            },
            'site': {
                deps: ['jquery', 'bootstrap']
            },
            'moment': {
                deps: ['jquery']
            }
        },

        bundles: {
            '/js/commons.js': ['Autocomplete','AjaxRequests'],
            '/js/test.js': ['Sweetalert'],
            '/js/applicationdetails.js': ['ApplicationAdd', 'GetApplications','AppraisalAdd']
        }
    });
})(this);