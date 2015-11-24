// automatically apply all sliders
function applySliders() {
    var brightness = parseInt($('#brightnessSlider').val());
    var hue = parseInt($('#hueSlider').val());
    var contrast = parseInt($('#contrastSlider').val());
    var vibrance = parseInt($('#vibranceSlider').val());
    var sepia = parseInt($('#sepiaSlider').val());
    var gamma = parseInt($("#gammaSlider").val());
    var exposure = parseInt($("#exposureSlider").val());

    abp.ui.setBusy("#imageEditor");
    Caman("#editImage", function () {
        this.revert(false);
        this.brightness(brightness).hue(hue).contrast(contrast).vibrance(vibrance).sepia(sepia).gamma(gamma).exposure(exposure).render();

        abp.ui.clearBusy("#imageEditor");
    });
}

// function to save the file
function saveFile(url, file) {
    return $.ajax({
        type: "POST",
        url: url,
        processData: false,
        contentType: false,
        data: file
    });
}

$(document).ready(function () {
    $("select").select2({
        theme: "bootstrap"
    });

    // get the image ready, turn it into a canvas element so it can be edited
    Caman("#editImage", function () { });

    $("input[type=range]").on("change", applySliders);

    $("#reset").on("click", function() {
        $("input[type=range]").val(0);

        Caman("#editImage", function() {
            this.revert(false);
            this.render();
        });
    });

    $("#download").on("click", function () {
        var $downloadObject = $(this);
        var canvas = document.getElementById("editImage");
        var imageData = canvas.toDataURL("image/jpeg", 1);
        var date = new Date();
        var hour = date.getHours();
        var minutes = date.getMinutes();
        var fileName = hour.toString() + "-" + minutes.toString() + "-edit.jpeg";

        $downloadObject.attr("href", imageData);
        $downloadObject.attr("download", fileName);
    });

    // saving the image they edited
    $("#save").on("click", function () {
        var fileId = $(this).data("fileid");
        var canvas = document.getElementById("editImage");
        var imageData = canvas.toDataURL("image/png");
        var resourceUrl = $(this).data("resourceurl");


        var file = new FormData();
        file.append("ImageEditorViewModel.DataUrl", imageData);

        // ask the user how they want to save the file (overwrite or new file)
        swal({
            title: "Are you sure?",
            text: "Do you want to overwrite the original image?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#007fff",
            confirmButtonText: "Yes, overwrite it!",
            cancelButtonText: "No, save as a new file!",
            closeOnCancel: false,
            closeOnConfirm: false
        }, function (isConfirm) {
            // overwriting file
            if (isConfirm) {
                // TODO: save file here by overwriting file
                file.append("ImageEditorViewModel.FileId", fileId);
                saveFile(resourceUrl, file).done(function(data) {
                    swal({
                        title: "Saved!",
                        text: "Your file has successfully been saved!",
                        type: "success",
                        confirmButtonColor: "#007fff"
                    });
                });
            }
            // as a new file, get the filename
            else {
                swal({
                    title: "Save As...",
                    text: "Filename: ",
                    type: "input",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    confirmButtonColor: "#007fff",
                    inputPlaceholder: "Enter filename..."
                }, function (inputValue) {
                    if (inputValue === false) return false;
                    if (inputValue === "") {
                        swal.showInputError("Please enter a valid filename...");
                        return false;
                    }

                    // TODO: Add saving as new file here
                    file.append("ImageEditorViewModel.FileId", 0);
                    file.append("ImageEditorViewModel.FileName", inputValue);
                    saveFile(resourceUrl, file).done(function (data) {
                        console.log(data);
                        debugger;
                        swal({
                            title: "Saved!",
                            text: data.result.fileName + " has successfully been saved!",
                            type: "success",
                            confirmButtonColor: "#007fff"
                        });
                    });
                });
            }
        });
    });
});