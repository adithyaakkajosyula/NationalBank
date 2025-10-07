$(window).on('load', function () {
    window.scrollTo(0, 0);
});
$(document).ready(function () {
    let pageNumber = 1;
    const pageSize = 20;
    let isLoading = false;
    let hasMore = true;
    $(window).on('scroll', function () {
        if (isLoading || !hasMore) return;
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadMoreComplaints();
        }
    });

    function loadMoreComplaints() {
        isLoading = true;
        pageNumber++;
        $('#loading').show();

        $.ajax({
            url: '/Complaints/List',
            data: { pageNumber: pageNumber, pageSize: pageSize },
            type: 'GET',
            success: function (response) {
                if (response.trim().length === 0) hasMore = false;
                else $('#complaintsContainer').append(response);
            },
            complete: function () {
                isLoading = false;
                $('#loading').hide();
            }
        });
    }
});