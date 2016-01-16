/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.unobtrusive-ajax.js" />
/// <reference path="CoreScript.js" />
/// <reference path="knockout-3.0.0.js" />
/// <reference path="jquery-2.0.3.js" />
/// <reference path="jquery-ui-1.10.3.js" />

//messages start

function showSuccessMessage(messaage) {
    var element = $(".sodaSuccessMessage");
    element.find(".msgTitle").text(messaage);
    element.css("z-index", dialogIndex + 1);
    element.fadeIn();//.css("display", "block");
}

function closeSuccessMessage() {
    var element = $(".sodaSuccessMessage");
    element.fadeOut(); //.css("display", "none");
    element.find(".msgTitle").text("");
}

function showErrorMessage(messaage, errors) {
    var element = $(".sodaErrorMessage");
    element.find(".msgTitle").text(messaage);
    if (errors != undefined && errors.length > 0) {
        var errorsHtml = '';
        for (var i = 0; i < errors.length; i++) {
            errorsHtml += '<div>' +
                (errors[i].message != null ? errors[i].message : errors[i])
                + '</div>';
        }
        element.find(".msgErrors").html(errorsHtml);
    }
    element.css("z-index", dialogIndex+1);
    element.fadeIn();//.css("display", "block");
}

function closeErrorMessage() {
    var element = $(".sodaErrorMessage");
    element.fadeOut(); //.css("display", "none");
    element.find(".msgTitle").text("");
    element.find(".msgErrors").html("");
}

var dialogIndex = 100000;

function pushDialog(htmlData, dialogClass, titleText, ondialogCancel, ondialogClose) {
    dialogClass = dialogClass || 'center500_300';
    titleText = titleText || '';

    var divId = 'MainOverlayDIV' + (++dialogIndex);
    //var closeButton = "<button style ='float:right' onclick='popDialog()'>X</button>";
    var title = "<span class='dialogTitle'>" + titleText + "</span>";
    var closeButton = "<a style ='float:right' href='#' id = 'closeButton' name='closeButton' class = 'dialogCloseButton' onclick='popDialog(this)'>X</a>";
    //var titleAndcloseButton = "<table style='width:100%'><tr><td style='width:33%'></td><td>" + title + "</td><td style='width:33%'>" + closeButton + "</td></tr></table>";
    var titleAndcloseButton = "<table style='width:100%'><caption>" + title + "</caption></table>" + closeButton;

    var contentDiv = " <div class='" + dialogClass + "'>" + titleAndcloseButton + htmlData + " </div>";

    $("body").append("<div id='" + divId + "' class='MainOverlayDIV'>" + contentDiv + "</div>");

    var element = $("#" + divId);
    
    if (ondialogCancel !== undefined && ondialogCancel != null) {
        element.attr("data-ondialogCancel", ondialogCancel);
    }

    if (ondialogClose !== undefined && ondialogClose != null) {
        element.attr("data-ondialogClose", ondialogClose);
    }
    
    var closeButtons = element.find('#closeButton');

    if (closeButtons.length > 0) {
        var closeButtonElement = closeButtons[0];
        closeButtonElement["data-linkDivId"] = divId;
    }
    
    element.fadeIn();
}

function popDialog(element, removeElement) {
    var divId = 'MainOverlayDIV' + dialogIndex;
    //if there is a linked div id,then use that id.
    if (element != undefined && element.getAttribute("data-linkDivId") != undefined) {
        divId = element.getAttribute("data-linkDivId");
    }
    var dialogElement = $("#" + divId);
    
    var ondialogCancel = dialogElement.attr("data-ondialogCancel");
    var ondialogClose = dialogElement.attr("data-ondialogClose");

    dialogElement.fadeOut();
    
    if (removeElement == undefined || removeElement == true) {
        dialogElement.remove();
    }
    
    dialogIndex--;

    if (element != null && element.id == 'closeButton') {
        if (ondialogCancel != null) {
            var cancelFunction = new Function(ondialogCancel + "();");
            cancelFunction();
        }
    } else {
        if (ondialogClose != null) {
            var closeFunction = new Function(ondialogClose + "();");
            closeFunction();
        }
    }
}

function showDialog(htmlData) {
    pushDialog(htmlData, "center500_300 border");
}

function showDialogWithIframe(element) {
    var url = element.getAttribute("data-url");
    var dialogClass = element.getAttribute("data-dialogClass");
    var dialogTitle = element.getAttribute("data-dialogTitle");
    var ondialogCancel = element.getAttribute("data-ondialogCancel");
    var ondialogClose = element.getAttribute("data-ondialogClose");

    var htmlData = "<iframe style='height:90%;width:99%;' src='" + url + "'></iframe>";
    pushDialog(htmlData, dialogClass, dialogTitle, ondialogCancel, ondialogClose);
}

function showDialogWithUrl(element) {
    var url = element.getAttribute("data-url");
    var dialogClass = element.getAttribute("data-dialogClass");
    var dialogTitle = element.getAttribute("data-dialogTitle");

    getHtmlData(url, function(data) {
        pushDialog(data, dialogClass, dialogTitle);
    });
}

function pushElementAsDialog(elementId) {
    ++dialogIndex;
    var element = $(elementId);
    element.fadeIn();
}

function popElementAsDialog(elementId) {
    --dialogIndex;
    var element = $(elementId);
    element.fadeOut();
}

//messages end