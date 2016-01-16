/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.unobtrusive-ajax.js" />
/// <reference path="jquery-1.8.2.intellisense.js" />
/// <reference path="jquery-ui-1.8.24.js" />

$(document).ready(function() {

    $(function() {
        $("#menu>li ul").hide();
        $('#menu li').hover(function() {
            $(this).children('ul').stop().slideToggle(400); //Hides if shown, shows if hidden
        }).mouseover(function() { $(this).addClass("a_hand"); }); //Hand!
    });

    // disable stage menu nav items
    $("a.disabled").click(function (e) {
        e.preventDefault();
    });
});

//Jquery UI setup.
$(document).ready(function () {

    if ($.datepicker != undefined) {
        $.datepicker.formatDate("dd/mm/yy", new Date(2001, 1 - 1, 1));
        setupDateInputs();
        setupAccordionControls();
        setupInputs();
    }   
});

function setupDateInputs() {
    $(".date").datepicker({
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true
    });
}

function setupAccordionControls() {
    
    $(".accordionControl").accordion({ heightStyle: "content" });
    $(".accordionControlSingle").accordion({ heightStyle: "content", collapsible: true, active: false });
}

function setupInputs() {
    $("input[type=submit].button, a.button, button.button").button();

    $("input[type=submit].addButton, a.addButton, button.addButton").button({
        icons: { primary: "ui-icon-circle-plus" },
        text: false
    });

    $("input[type=submit].delButton, a.delButton, button.delButton").button({
        icons: { primary: "ui-icon-circle-close" },
        text: false
    });
}

function navigateTo(url) {
    if (msieversion() > 0) {
        window.location.href = url;
    } else {
        document.location.href = url;
    }

    return false;
}

function refreshPage() {
    window.navigateTo(document.URL);
}

function jsonDateToDate(jsonDate) {
    try {
        if (typeof jsonDate === 'string') {
            return getMinimumDate(new Date(parseInt(jsonDate.substr(6))));
        } else if (jsonDate instanceof Date) {
            return getMinimumDate(jsonDate);
        } else if (jsonDate == null) {
            return null;
        }
        return new Date();
    } catch(err) {
        console.error(err);
        return new Date();
    }
}

function getMinimumDate(date) {
    //sql server can't store less then 1/1/1753
    if (date.getFullYear() < 1800) {
        date.setFullYear(1800);
    }
    return date;
}

function toNumber(numberString) {
    var result = Number(numberString);
    if (isNaN(result))
        return 0;
    else
        return result;
}

function reloadPage() {
    location.reload();
}

//formatters -start
String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

String.prototype.toDate = function () {
    var str = this;
    var splitDate = str.replaceAll('/', '-').split('-');
    //return splitDate[2] + '-' + splitDate[1] + '-' + splitDate[0];
    return new Date(splitDate[2], splitDate[1]-1, splitDate[0]);
};

Date.prototype.toOldJSON = Date.prototype.toJSON;

Date.prototype.toJSON = function() {
    return this.getFullYear() == 1800 ? null : this.toOldJSON();
}

$(document).ready(function () {
    if ($.validator == undefined)
        return;
    $.validator.methods.date = function(value, element) {
        return this.optional(element) || (/^\d{1,2}[\/-]\d{1,2}[\/-]\d{4}(\s\d{2}:\d{2}(:\d{2})?)?$/.test(value)
            && !/Invalid|NaN/.test(value.toDate()));
    };

    $.validator.unobtrusive.adapters.add("mustbetrue", ['maxint'], function (options) {
        options.rules["mustbetrue"] = options.params;
        options.messages["mustbetrue"] = options.message;
    });

    $.validator.addMethod("mustbetrue", function (value, element, params) {
        return $(element).is(':checked');
    });

});

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    };

    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1,
            (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                    ("00" + o[k]).substr(("" + o[k]).length));
    return format;
};

if (!String.format) {
    String.format = function(format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function(match, number) {
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    };
}

Number.prototype.toCurrency = function (places, symbol, thousand, decimal) {
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "£";
    thousand = thousand || ",";
    decimal = decimal || ".";
    var number = this,
        negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
};

//30 = 30.00%
Number.prototype.toPercentage = function (decimal) {
    decimal = decimal || 2;
    return this.toFixed(decimal) + '%';
};

//0.30= 30.00% 
Number.prototype.decimalToPercentage = function (decimal) {
    decimal = decimal || 2;
    return (this.toFixed(decimal) * 100) + '%';
};

String.prototype.toNumber = function() {
    var result = Number(this);
    if (isNaN(result))
        return 0;
    else
        return result;
};

//formatter -end


//Ajax call scripts
$(document).ajaxComplete(onAjaxCallComplete);
$(document).ajaxStart(onAjaxCallBegin);

var isRunning = false;
var dialogIndex = dialogIndex || 100000;
function showWaitDiv() {    
    $("#waitDiv").css("z-index", dialogIndex + 1);
    $("#waitDiv").css("display", "block");
}
function hideWaitDiv() {
    $("#waitDiv").css("display", "none");
}

function onAjaxCallBegin() {
    isRunning = true;
    showWaitDiv();
}
function onAjaxCallComplete(xhr, status, error) {
    isRunning = false;
    hideWaitDiv();
}

function onAjaxCallError(xhr, status, error) {
    isRunning = false;
    hideWaitDiv();
    if (xhr.status == 403) {
        alert("Unauthorized access. try to log back on system.");
        return;
    } else if (xhr.status == 500) {
        pushDialog({ htmlData: xhr.responseText });
    } else {
        var err = eval("(" + xhr.responseText + ")");
        alert("Error :" + err.Message);
    }
}

function postJsonData(url, data, success, allowMultipleRequests) {

    allowMultipleRequests = typeof allowMultipleRequests !== 'undefined' ? allowMultipleRequests : false;
    
    if (isRunning === true && allowMultipleRequests === false)
        return false;        
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: 'application/json',
        data: data,
        success: success,
        error: onAjaxCallError
    });
    return true;
}

function getJsonData(url, success, allowMultipleRequests) {

    allowMultipleRequests = typeof allowMultipleRequests !== 'undefined' ? allowMultipleRequests : false;

    if (isRunning === true && allowMultipleRequests === false)
        return false;

    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        contentType: 'application/json',
        success: success,
        error: onAjaxCallError
    });
    return true;
}

function getHtmlData(url, success) {
    if (isRunning === true)
        return false;

    $.ajax({
        type: "GET",
        url: url,
        success: success,
        error: onAjaxCallError
    });
    return true;
}

function submitForm(formElement, success) {
    var form = $(formElement);

    if (isRunning === true)
        return false;

    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(),
        success: success,
        error: onAjaxCallError
    });

    return true;
}

function subscribeSubmitForm(formElement, success) {
    var form = $(formElement);
    form.submit(function(e) {
        e.preventDefault();
        return submitForm(formElement, success);
    });
}

function closeDialog(data, successMessage) {
    if (data.Success === true) {
        if (window.parent != null) {
            window.parent.popDialog({ data: data.Data });
            window.parent.showSuccessMessage(successMessage);
        } else {
            alert(successMessage);
        }
    } else {
        showErrorMessage("Check validation errors", data.Errors);
    }
}

///Ajax call scripts end


//grid ajax link scripts start

function updateField(anchor, param ,idToUpdate) {
    var element = $(idToUpdate);
    //alert(anchor.href);

    var url = anchor.href;
    var params = url.split('?')[1].split('&');
    for (var i = 0; i < params.length; i++) {
        //alert(params[i].indexOf(param));
        if (params[i].indexOf(param) >= 0) {
            var value = params[i].split('=')[1];
            element.val(value);
            break;
        }
    }
}

//grid ajax link scripts end

//guid generator start
function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
               .toString(16)
               .substring(1);
};

function guid() {
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
           s4() + '-' + s4() + s4() + s4();
}
//var uuid = guid();

function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });
    return uuid;
};

//guid generator end

//forms validation

function validateAllForms(getErrorCallback) {
    var validated = true;
    var errors = [];
    $('form').each(function() {
        //validated = validated && $(this).valid();
        var validator = $(this).validate();
        var isValid = validator.form();
        if (!isValid) {
            for (var i = 0; i < validator.errorList.length; i++) {
                errors.push(validator.errorList[i]);
            }
        }
        validated = validated && isValid;
    });

    if (getErrorCallback !== undefined) {
        getErrorCallback(errors);
    }
    
    return validated;
}

function validate() {

    var errors;

    var isValid= validateAllForms(function(err) {
        errors = err;
    });

    if (isValid == false) {
        window.showErrorMessage("Check validation errors.", errors);
    }

    return isValid;
}

function validateForm(form) {
    var errors = [];
    var validator = $(form).validate();
    var isValid = validator.form();
    if (!isValid) {
        for (var i = 0; i < validator.errorList.length; i++) {
            errors.push(validator.errorList[i]);
        }
        window.showErrorMessage("Check validation errors.", errors);
    }
    
    return isValid;
}
//

//JS linQ start
Array.prototype.forEach = function (action) {
    for (var i = 0; i < this.length; i++) {
        action(this[i], i);
    }
};

Array.prototype.where = function (predicate) { 
    var result = new Array();
    this.forEach(function(x) {
        if (predicate(x)) {
            result.push(x);
        }
    });
    return result;
};

Array.prototype.any = function (predicate) {
    if (predicate !== null) {
        for (var i = 0; i < this.length; i++) {
            if (predicate(this[i])) {
                return true;
            }
        }
        return false;
    }
    return this.length > 0;
};

Array.prototype.firstOrDefault = function (predicate) {
    if (predicate !== null) {
        for (var i = 0; i < this.length; i++) {
            if (predicate(this[i])) {
                return this[i];
            }
        }
        return null;
    }
    return this.length > 0 ? this[0] : null;
};

Array.prototype.first = function(predicate) {
    var result = this.firstOrDefault(predicate);
    if (result == null) {
        throw 'not found';
    } else {
        return result;
    }
};

Array.prototype.select = function(selector) {
    var result = new Array();
    this.forEach(function(x) { result.push(selector(x)); });
    return result;
};

Array.prototype.sum = function (source) {
    var result = 0;
    this.forEach(function(x) { result += source(x); });
    return result;
};

Array.prototype.aggregate = function(action) {
    var current = this[0];
    for (var i = 1; i < this.length; i++) {
        current = action(current, this[i]);
    }
    //this.forEach(function (x) { current = action(current, x); });
    return current;
};

//js linq end

var ie = (function() {

    var undef, v = 3, div = document.createElement('div'), all = div.getElementsByTagName('i');

    while (div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->', all[0]) ;

    return v > 4 ? v : undef;

}());

function msieversion() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0) // If Internet Explorer, return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)));
    else // If another browser, return 0
        return 0;
}

function confirmDelete() {
    return confirm('Are you sure want to delete?');
}

