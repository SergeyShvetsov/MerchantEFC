function onImageSelected(event) {
    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("productimage");
    var noimgtag = document.getElementById("noproductimage");

    if (selectedFile != null) {

        $('#productimage').show();
        $('#noproductimage').hide();
        $('#ProductImageChanged').val('True');
        imgtag.title = selectedFile.name;

        reader.onload = function (event) {
            imgtag.src = event.target.result;
        };

        reader.readAsDataURL(selectedFile);
    }
    else {
        $('#productimage').hide();
        $('#noproductimage').show();
        imgtag.title = 'no selected file';
        imgtag.src = window.location.host + "/Images/no_image.png";
    }

}
