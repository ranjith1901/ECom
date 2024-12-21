var baseUrl = window.localStorage.getItem('');

var apiURL = '';
var authToken = '';
var appURL = '';
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
    $("#toastWindow").toast();
    $("#toastWindow").toast('show');
}

$(function () {

    if (window.localStorage.getItem("AuthToken")) {
        authToken = window.localStorage.getItem("AuthToken");
    }
    else {
        window.location.href = "Home/Index";
    }
    if (window.localStorage.getItem("EcomApiURL")) {
        apiURL = window.localStorage.getItem("EcomApiURL");
    }
    else {
        showToast("error", "Ecom", "Api URL is invalid");
    }

    //if (window.localStorage.getItem("ActiveMenu")) {
    //    window.localStorage.setItem("ActiveMenu", selectedMenu);
    //}
    //setActiveMenu('mnuDashboard');
});
function setPageTitle(title) {
    $('#pageTitle').html(title);
}

function setActiveMenu(selectedMenu, parentMenus) {
    console.log(selectedMenu);
    //$('.navigation shadow-sm').children('a').removeClass('active');
  
    //$('a.' + selectedMenu).addClass('active');
    //$.each(parentMenus, function (index, menu) {
    //    $('a.' + menu).addClass('active');
    //});

    $('#mnuTravelType').addClass('active');

}

$('#btnLogout').on("click", function () {
    window.localStorage.removeItem("AuthToken");
    window.location.href = "Home/Index";
});

