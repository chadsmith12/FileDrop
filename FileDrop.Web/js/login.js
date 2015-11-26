
(function() {
    $("#loginBtn").click(function(e) {
        e.preventDefault();
        var resourceUrl = $(this).data("resourceurl");

        var loginModel = {
            emailAddress: $("#user").val(),
            password: $("#password").val(),
            rememberMe: false
        }

        abp.ui.setBusy($("#loginbox"),
            abp.ajax({
                url: resourceUrl,
                type: "POST",
                data: JSON.stringify(loginModel)
            })
        );
    });
})();