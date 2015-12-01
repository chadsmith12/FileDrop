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
    });

    // reset the data uri we would be using for the download
    var canvas = document.getElementById("editImage");
    var $downloadObject = $("#download");
    var imageData = canvas.toDataURL();
    var date = new Date();
    var hour = date.getHours();
    var minutes = date.getMinutes();
    var fileName = hour.toString() + "-" + minutes.toString() + "-edit.png";

    $downloadObject.attr("href", imageData);
    $downloadObject.attr("download", fileName);
}

// function to save the file
// Url - the url to save the file too
// file - the raw FormData object to send to the server
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
    var $image = $("#editImage");
    var $imageWidth = $("#imageWidth");
    var $imageHeight = $("#imageHeight");
    var image = new Image();
    image.src = $image.attr("src");
    var originalImage = image.src;

    // set our image details on load
    $imageWidth.append(image.width + " px");
    $imageHeight.append(image.height +  " px");

    // get the image ready, turn it into a canvas element so it can be edited
    Caman("#editImage", function () { });

    // listen to caman events so we can set the image editor to busy or not
    Caman.Event.listen("renderFinished", function(job) {
        abp.ui.clearBusy("#imageEditor");
    });

    // apply our sliders when we change them
    $("input[type=range]").on("change", applySliders);

    // applying a filter
    $("#applyFilter").on("click", function (e) {
        abp.ui.setBusy("#imageEditor");
        var selectedValue = $("#filtersList").val();

        switch(selectedValue) {
            case "clarity":
                Caman("#editImage", function() {
                    this.clarity().render();
                });
                break;
            case "crossProcess":
                Caman("#editImage", function () {
                    this.crossProcess().render();
                });
                break;
            case "hdr":
                Caman("#editImage", function() {
                    this.contrast(10);
                    this.contrast(10);
                    this.jarques();
                    this.render();
                });
                break;
            case "blur":
                Caman("#editImage", function () {
                    this.radialBlur().render();
                });
                break;

            case "greyscale":
                Caman("#editImage", function () {
                    this.greyscale().render();
                });
                break;

            case "vintage":
                Caman("#editImage", function() {
                    this.vintage().render();
                });
                break;
        }

        // reset the data uri we would be using for the download
        var canvas = document.getElementById("editImage");
        var $downloadObject = $("#download");
        var imageData = canvas.toDataURL();
        var date = new Date();
        var hour = date.getHours();
        var minutes = date.getMinutes();
        var fileName = hour.toString() + "-" + minutes.toString() + "-edit.png";

        $downloadObject.attr("href", imageData);
        $downloadObject.attr("download", fileName);
    });

    // reset the image back to the original
    $("#reset").on("click", function() {
        $("input[type=range]").val(0);

        Caman("#editImage", function() {
            this.revert(false);
            this.render();
        });
    });

    //$("#download").on("click", function () {
    //    var $downloadObject = $(this);
    //    var canvas = document.getElementById("editImage");
    //    var imageData = canvas.toDataURL();
    //    var date = new Date();
    //    var hour = date.getHours();
    //    var minutes = date.getMinutes();
    //    var fileName = hour.toString() + "-" + minutes.toString() + "-edit.png";

    //    $downloadObject.attr("href", imageData);
    //    $downloadObject.attr("download", fileName);
    //});

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
            closeOnConfirm: true
        }, function (isConfirm) {
            // overwriting file
            if (isConfirm) {
                // TODO: save file here by overwriting file
                file.append("ImageEditorViewModel.FileId", fileId);

                abp.ui.setBusy($("body"), saveFile(resourceUrl, file).done(function (data) {
                    swal({
                        title: "Saved!",
                        text: "Your file has successfully been saved!",
                        type: "success",
                        confirmButtonColor: "#007fff"
                    });
                }));

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
                    abp.ui.setBusy($("body"), saveFile(resourceUrl, file).done(function(data) {
                        console.log(data);
                        swal({
                            title: "Saved!",
                            text: data.result.fileName + " has successfully been saved!",
                            type: "success",
                            confirmButtonColor: "#007fff"
                        });
                    }));
                });
            }
        });
    });

    // want to compare the images
    $("#beforeAfterModal").on("show.bs.modal", function () {
        $("#after").hide();
        $("#before").attr("src", originalImage);
    });

    // before after click
    $("#beforeBtn").on("click", function (e) {
        $("#beforeAfterLabel").text("Before");
        $("#after").hide();
        $("#before").show();
        $("#before").attr("src", originalImage);
    });
    $("#afterBtn").on("click", function (e) {
        var canvas = document.getElementById("editImage");
        $("#beforeAfterLabel").text("After");
        var imageData = canvas.toDataURL("image/jpeg", 1);
        $("#after").show();
        $("#before").hide();
        $("#after").attr("src", imageData);
    });
});