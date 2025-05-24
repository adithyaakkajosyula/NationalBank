// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(() => {

    $('#sidebarbutton').on('click', () => {
        $('.sidebar').toggleClass('open');
        $('.main').toggleClass('open');
        $('footer').toggleClass('open');
    });

    $('.menu-link').on('click', function () {
        $(this).next('ul.nav-content').collapse('toggle') // By this element will --> collapse --> Collapsing --> Show will addd then the element will shown
        $(this).toggleClass('collapsed'); // own collapsed class will remove

    });
    (function ($) {
        $.validator.unobtrusive.adapters.addBool("checkboxrequired", "required");
    }(jQuery));

})
