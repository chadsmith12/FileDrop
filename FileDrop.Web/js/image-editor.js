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

$(document).ready(function () {
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
});