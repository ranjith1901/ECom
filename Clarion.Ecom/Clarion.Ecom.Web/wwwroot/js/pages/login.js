var apiURL = '';
var authToken = '';
var baseUrl = '';
function getConfig() {
    baseUrl = window.location.href;
    if (baseUrl.endsWith("/")) {
        baseUrl = baseUrl.substring(0, baseUrl.length - 1);
    }
    var configUrl = baseUrl + '/Home/GetAppConfig';
    window.localStorage.setItem("EcomApiURL", baseUrl);
    $.getJSON(configUrl, function (data) {
    }).done(function (data) {
        //console.log(data);
        apiURL = data.apiUrl;
        window.localStorage.setItem("EcomApiURL", apiURL);
    });
}

$(function () {
    clearLocalStorage();
    getConfig();
});

function clearLocalStorage() {
   window.localStorage.clear();
}

$('#btnSignIn').on("click", function () {
    validateLogin();
});

function validateLogin() {

    if ($('#txtEmail').val().trim().length == 0) {
        showToast("error", "Login", "Please enter the Username/Email");
        return;
    }
    if ($('#txtPassword').val().trim().length == 0) {
        showToast("error", "Login", "Please enter the Password");
        return;
    }
    var varUserName = $("#txtEmail").val();
    var varPassword = $('#txtPassword').val().toString();
    var loginModel = {
        "Username": varUserName,
        "Password": varPassword
    };
    $("#btnSignIn").html("<i class='fa fa-spinner fa-pulse'></i> Please wait");
    var baseUrl = window.location.href;
    if (baseUrl.endsWith("/")) {
        baseUrl = baseUrl.substring(0, baseUrl.length - 1);
    }
   
    $.ajax({
        type: "POST",
        url: apiURL + "/Login/Authenticate",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(loginModel),
        async: false,
        success: function (data, statusText, jqXHR) {
            //console.log(data);
            if (data.ResponseCode == 1) {
                window.localStorage.setItem("AuthToken", data.ResponseData[0].Token);
                window.localStorage.setItem("Username", data.ResponseData[0].UserName);
                window.localStorage.setItem("CompanyID", data.ResponseData[0].CompanyID);

                window.location.href = baseUrl + "/Home/DashBoard";
            }
            else {
                showToast("error", "Login", data.Message);
            }
        },
        error: function (xhr, statusText, jqXHR) {
            if (xhr.status === 500 || xhr.status === 412) {
                var jsonResponse = JSON.parse(xhr.responseText);
                showToast("error", "Error", jsonResponse.Message);
            }
            else {
                showToast("error", "Error", xhr.status.toString() + ": Error processing request.")
            }
        }
    });
}

function showToast(type, title, message) {
    $('#toastWindow').removeClass("bg-danger");
    $('#toastWindow').removeClass("bg-success");
    $('.toast-header').removeClass('bg-danger');
    $('.toast-header').removeClass('bg-success');
    let toastTitle;
    switch (type) {
        case "error":
            $('#toastWindow').addClass("bg-danger");
            $('.toast-header').addClass('bg-danger');
            toastTitle = "Alert";
            break;
        case "success":
            $('#toastWindow').addClass("bg-success");
            $('.toast-header').addClass('bg-success');
            toastTitle = "Success";
            break;
    }
    $('#tstTitle').html(toastTitle);
    $('#tstMessage').html(message);
    $("#toastWindow").toast('show');
}