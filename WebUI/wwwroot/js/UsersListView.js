$(function () {
    /* Select product from specified category */
    $("#SelectRole").on("change", function () {
        var url = $(this).val();

        if (url) {
            window.location = "/admin/users/List?roleId=" + url;
        }
        return false;
    });
    /* Confirm page deletion */
    $("a.delete").click(function () {
        if (!confirm("Confirm user deletion")) return false;
    });
});