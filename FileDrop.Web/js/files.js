function addPercent(pct) {
    $("#progressSection").show();
    if (pct > 30) {
        $("#progressMessage").find("h4").text("Encrypting File, Please Wait");
    }
    if (pct > 60) {
        $("#progressMessage").find("h4").text("Saving File, Please Wait");
    }

    if (pct >= 99) {
        $("#progressBar").css("border-radius", "25px");
        $("#progressBar").css("width", "99%");
        $("#progressPercent").text("99%");
    } else {
        $("#progressBar").css("width", pct + "%");
        $("#progressPercent").text(pct + "%");
        $("#progressMessage").find("h4").text("Uploading File, Please Wait");
    }
}

// saveFile - sends an ajax request to the resource url and saves the file
// fileId - the id of the file we are saving
// fileName - the file name of the file we are updating
function saveFile(fileId, fileName, resourceUrl) {
    var file = {
        id: fileId,
        fileName: fileName
    };

    $.post(resourceUrl, file);
}

// reloadFilesTable - reloads the files table from the server
// resourceUrl - the url to get the data
// selector - the part of the dom to update
// options(optional) - object that is send to the server when reloading the table
function reloadFileTable(resourceUrl, selector, options) {
    var parameters = options || {};
    selector.load(resourceUrl, parameters, function(data) {
        abp.notify.success("Files successfully uploaded");
    });
}

$(document).ready(function () {
    $("[data-toggle=\"tooltip\"]").tooltip();

    // dropzone
    var myDropZone = new Dropzone("#dropZoneDiv", {
        url: "/File/UploadFile",
        uploadprogress: function (data, progress) {
            addPercent(parseInt(progress));
        },
        previewTemplate: "<div class=\"dz-preview dz-file-preview\">" +
                    "<div class=\"dz-details\">" +
                    "<div class=\"dz-filename\"><span data-dz-name></span></div>" +
                    "<div class=\"dz-progress\"><span class=\"dz-upload\" data-dz-uploadprogress></span></div>" +
                    "</div>",
        success: function (data) {
            var resourceUrl = $("#filesTable").data("resourceurl");
            reloadFileTable(resourceUrl, $("#filesTable"));
            this.removeAllFiles();
            $("#progressSection").hide();
            $("#uploadModal").modal("hide");
        }
    });

    // make a delegate for when we hover over a table column
    $("#filesTable").delegate("td", "mouseover mouseleave", function(e) {
        var cellObject = $(this);
        var editBtn = $(this).find(".editFile");
        if (e.type == "mouseover") {
            if (cellObject.data("filename") === true) {
                $(editBtn).show();
                cellObject.parent().addClass("active");
            }
        } else {
            $(editBtn).hide();
            cellObject.parent().removeClass("active");
        }
    });

    // table search
    $("#fileSearch").on("change", function(e) {
        var searchTerm = $(this).val();
        var resourceUrl = $("#filesTable").data("resourceurl");

        if (searchTerm.length > 3) {
            reloadFileTable(resourceUrl, $("#filesTable"), { searchTerm: searchTerm });
        }
    });

    // click on the edit button to edit a filename
    $(".editFile").click(function (e) {
        // buttn we clicked on
        var editBtn = $(this);
        var resourceUrl = editBtn.data("url");
        // the cell we are in
        var cellObject = $(this).parent("td");
        var id = parseInt($(cellObject).data("fileid"));
        // text of the cell
        var cellText = cellObject.find("span");
        // the input box inside that cell
        var cellInput = cellObject.find("input");
        // edit mode or not?
        var isEditMode = editBtn.data("editmode");

        // allow them to edit
        if (isEditMode === false) {
            editBtn.data("editmode", true);
            editBtn.removeClass("btn-danger");
            editBtn.addClass("btn-primary");
            editBtn.text("Save");
            $(cellText).hide();
            $(cellInput).show();
        }
        // already were in edit mode, save cell now
        else {
            var inputValue = $(cellInput).val();

            saveFile(id, inputValue, resourceUrl);
            editBtn.data("editmode", false);
            editBtn.removeClass("btn-primary");
            editBtn.addClass("btn-danger");
            editBtn.text("Edit");
            $(cellInput).hide();
            $(cellText).text(inputValue);
            $(cellText).show();
        }
    });

    // downloading the file
    $(".downloadFile").click(function (e) {
        var downloadBtn = $(this);
        var fileUrl = downloadBtn.data("resourceurl");

        window.open(fileUrl);
    });

    $("#uploadFile").click(function (e) {
        e.preventDefault();

        $("#uploadModal").modal("show");
    });

    $(".editImage").click(function (e) {
        var editBtn = $(this);
        var imageUrl = editBtn.data("resourceurl");

        window.open(imageUrl);
    });
});