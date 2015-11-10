$(document).ready(function() {
    // get the image ready, turn it into a canvas element so it can be edited
    Caman("#editImage", function () { });


    $("#brightnessSlider").on("change", function() {
        var slider = $(this);
        var currentValue = parseInt(slider.val());
        var brightnessLabel = $("#brightnessLabel");

        Caman("#editImage", function() {
            this.brightness(currentValue);
            this.revert(false);

            brightnessLabel.text("Brightness: " + currentValue);
            this.render();
        });
    });

    $("#contrastSlider").on("change", function () {
        var slider = $(this);
        var currentValue = parseInt(slider.val());
        var contrastLabel = $("#contrastLabel");

        Caman("#editImage", function () {
            this.contrast(currentValue);
            this.revert(false);

            contrastLabel.text("Contrast: " + currentValue);
            this.render();
        });
    });

});