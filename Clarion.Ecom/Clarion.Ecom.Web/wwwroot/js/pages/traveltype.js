var apiTravelTypeID = 0;  // Used to send in API Request
var tableTravelType;
var apiURL = window.localStorage.getItem("EcomApiURL");
var baseUrl = window.location.href;

var currentURL = window.location.href;
var splitURL = currentURL.split("/");
var travelTypeID = 0;

$(function () {
    // setPageTitle("Travel Type");

    travelTypeID = splitURL[splitURL.length - 1];
    if (travelTypeID > 0) {
        getTravelTypebyID(travelTypeID);
    } else {
        travelTypeID = 0;
    }

    $('#mnuTravelType').addClass('active');

    //setActiveMenu('mnuTravelType', ['menuAdmins']);

    if ($("#hdnTravelTypeID").val() == undefined) {
        InitTable();
    };
   
});

function InitTable() {
    tableTravelType = new DataTable("#tblTravelType", {
        scrollX: true,
        responsive: true,
        layout: {
            top2End: 'buttons'
        },
        buttons: [
            {
                extend: 'excel', className: 'fa-regular fa-file-excel', titleAttr: 'Excel',
                exportOptions: {
                    columns: ':not(:last-child)' // Excludes the last column
                }
            },
            {
                extend: 'pdf', className: 'fa-regular fa-file-pdf', titleAttr: 'Pdf',
                exportOptions: {
                    columns: ':not(:last-child)' // Excludes the last column
                }
            },
            {
                extend: 'print', className: 'fas fa-print', titleAttr: 'Print',
                exportOptions: {
                    columns: ':not(:last-child)' // Excludes the last column
                }
            },
        ],
        columnDefs: [
            { orderable: false, targets: -1 }, // Disables sorting on the last column
        ],
        initComplete: function () {
            $('.dt-buttons span').remove();
        }
    });
}

var TravelTypeID = null;
var TravelTypeName = null;
var TravelTypeStatus = null;

function resetSearchFields() {
    $('#txtSearchTravelType').val('');
    $('#ddlSearchTravelStatus').val('-1');
    $("#showTravelTypeGrid").hide();
}

//To get Search Travel Types
function SearchTravelTypes() {

    TravelTypeName = null;
    TravelTypeStatus = null;

    if ($('#txtSearchTravelType').val().trim().length > 0) {
        TravelTypeName = $('#txtSearchTravelType').val().trim();
    }
    if ($('#ddlSearchTravelStatus').val() > -1) {
        TravelTypeStatus = $('#ddlSearchTravelStatus').val();
    }

    var searchData = { "TravelTypeName": TravelTypeName, "TravelTypeStatus": TravelTypeStatus};
 
    $.ajax({
        type: "POST",
        url: apiURL + '/TravelType/GetTravelTypeBySearch',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchData),
        headers: { Authorization: "Bearer " + authToken },
        async: false,
        success: function (response) {
            tableTravelType.destroy();
            $('#tblBodyTravelType').empty();
            var apiData = response.ResponseData;
            //console.log(response)
            let viewIcon = "";
            let editIcon = "";
            let StatusIcon = "";

            for (var i = 0; i < apiData.length; i++) {
                let tempData = apiData[i];
                let sNo = i + 1;
     
                if (tempData.TravelTypeStatus == 0) {
                     viewIcon = '<a class="data-view btn btn-sm btn-outline-info" style="cursor: pointer;" data-toggle="tooltip" data-placement="bottom" title="View" onclick="viewTravelType(' + tempData.TravelTypeID + ')"><i class="fa fa-eye"></i></a>';

                     editIcon = '<a class="data-view btn btn-sm btn-outline-primary" style="cursor: pointer;" data-toggle="tooltip" data-placement="bottom" title="Edit" onclick="editTravelType(' + tempData.TravelTypeID + ')"><i class="fa-solid fa-pen"></i></a>&nbsp;&nbsp;';

                    StatusIcon = '<a class="data-delete btn btn-sm btn-outline-danger" style="cursor:pointer;" data-toggle="tooltip" data-placement="bottom" title="Deactivate" onclick="updateTravelType(' + tempData.TravelTypeID + ',1)" > <i class="fa-solid fa fa-times"></i></a>';
                } else {
                    editIcon = "";

                    StatusIcon = '<a class="data-delete btn btn-sm btn-outline-danger" style="cursor:pointer;" data-toggle="tooltip" data-placement="bottom" title="Activate" onclick="updateTravelType(' + tempData.TravelTypeID + ',0)" > <i class="fa-solid fa fa-check"></i></a>';
                }

                let typeRow = '<tr data-value=' + tempData.TravelTypeID + '>' +
                    '<td style="text-align:center">' + sNo + '</td>' +
                    '<td>' + tempData.TravelTypeName + ' </td>' +
                    '<td style="text-align:center" class="action-icon">' +
                    '<div class="icon-container">' + viewIcon + '&nbsp;&nbsp;' + editIcon + '' + StatusIcon + '</div>' +
                    '</td>' +
                    '</tr>';

                $('#tblBodyTravelType').append(typeRow);
            }
            $("#showTravelTypeGrid").show();
            // Re-initialize DataTable after repopulating the table
            InitTable();
        },
        error: function (xhr, textStatus, errorThrown) {
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

$("#btnSave").on('click', function () {
    AddOrUpdateTravelType();
});

$("#btnClear").on('click', function () {
    clearTravelType();
});

function backToSearch(returnPage) {
    window.location.href = returnPage;
}

//To get Travel Type by ID
function getTravelTypebyID(tTypeID) {

    $('#btnActive').hide();
    $('#btnDeactive').hide();

    $.ajax({
        type: "GET",
        url: apiURL + '/TravelType/GetTravelType/' + tTypeID,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { Authorization: "Bearer " + authToken },
        async: false,
        success: function (response) {
            if (response.ResponseCode == 1) {
                let objTravelType = response.ResponseData[0];
                $('#txtTravelTypeName').val(objTravelType.TravelTypeName);
                $('#ddlTravelTypeStatus').val(objTravelType.TravelTypeStatus);
                $("#ddlTravelTypeStatus").prop("disabled", true);
                //$("#btnSave").contents().last()[0].textContent = "Update";

                $('#lblTravelTypeName').text(objTravelType.TravelTypeName);
                $('#lblTravelTypeStatus').text(objTravelType.TravelTypeStatus);

                $('#hdnTravelTypeID').val(objTravelType.TravelTypeID); 

                if (objTravelType.TravelTypeStatus == 0) {
                    $('#lblTravelTypeStatus').html('<span style="line-height: 2.0;font-size:10px;" class="badge badge-success">Active</span>');
                    $('#btnActive').show();
                }
                else {
                    $('#lblTravelTypeStatus').html('<span style="line-height:2.0;font-size:10px;" class="badge badge-danger">Inactive</span>');
                    $('#btnDeactive').show();
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
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

//To clear all 
function clearTravelType() {
    $('#txtTravelTypeName').val("");
    TravelTypeID = 0;
}
//Api call for AddorUpdate TravelType
function AddOrUpdateTravelType() {
    var travelTypeName = $('#txtTravelTypeName').val().trim();
    if (travelTypeName.length == 0) {
        showToast("error", "Error", "Please provide Travel Type Name");
        return;
    }
    TravelTypeID = travelTypeID;
    var apiData = { "TravelTypeID": TravelTypeID, "TravelTypeName": travelTypeName };
    $.ajax({
        type: "POST",
        url: apiURL + '/TravelType/AddorupdateTravelType',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(apiData),
        headers: { Authorization: "Bearer " + authToken },
        async: false,
        success: function (xhr,response) {
            if (TravelTypeID == 0) {
                showToast("success", "Success", 'Travel Type "<b>' + travelTypeName + '</b>" has been saved successfully.');
                //setTimeout(function () {
                //    showToast("success", "Success", response.Message);
                //}, 5000);
                clearTravelType();
                apiData = {};
                //TravelTypeID = 0;
            }
            else {
                showToast("success", "Success", 'Travel Type "<b>' + travelTypeName + '</b>" has been updated successfully.');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
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

//To edit TravelType
function editTravelType(tTypeID) {
    if (tTypeID == 0) {
        tTypeID = travelTypeID;
    }
    window.location.href = "/TravelType/Edit/" + tTypeID;
}

//To view TravelType
function viewTravelType(tTypeID) {
    window.location.href = baseUrl + "/Details/" + tTypeID;
}

function updateStatus(status) {
    travelTypeID = $('#hdnTravelTypeID').val();
    updateTravelType(travelTypeID, status);
}

//To delete TravelType
function updateTravelType(tTypeID,status) {

    var promptMsg = "Are you sure want to deactivate Travel Type?";
    if (status == 0) {
        promptMsg = "Are you sure want to activate Travel Type?";
    }

    var isDeleteTravelType = confirm(promptMsg);
    if (isDeleteTravelType) {    
        saveTravelTypeStatus(tTypeID, status);
    }
}

function saveTravelTypeStatus(tTypeID, status) {

    var apiData = { "TravelTypeID": tTypeID, "TravelTypeStatus": status };

    $.ajax({
        type: "POST",
        url: apiURL + '/TravelType/UpdateTravelTypeStatus',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        headers: { Authorization: "Bearer " + authToken },
        async: false,
        data: JSON.stringify(apiData),
        success: function (response) {
            showToast("success", "Success", response.Message);
            if (tTypeID > 0 ) {
                location.reload();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
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