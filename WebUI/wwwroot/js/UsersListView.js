$(function () {
    /* Select users by role */
    $("#SelectRole").on("change", function () {
        var url = $(this).val();

        if (url) {
            window.location = "/admin/users/List?roleId=" + url;
        }
        return false;
    });
});