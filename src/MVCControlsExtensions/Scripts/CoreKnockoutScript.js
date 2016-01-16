/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.unobtrusive-ajax.js" />
/// <reference path="knockout-3.0.0.debug.js" />
/// <reference path="jquery-1.8.2.intellisense.js" />
/// <reference path="jquery-ui-1.8.24.js" />
/// <reference path="CoreScript.js" />


ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $el = $(element);

        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $el.datepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($el.datepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $el.datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            $el = $(element),
            current = $el.datepicker("getDate");

        if (value - current !== 0) {
            $el.datepicker("setDate", value);
        }
    }
};

//show/hide start
ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).fadeIn() : $(element).fadeOut();
    }
};
//Show/Hide end

var uniqId = 0;

ko.bindingHandlers.dateString = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();

        var uid = $(element).attr('id') + (uniqId++);
        $(element).attr('id', uid);
        
        var allBindings = allBindingsAccessor();

        if (allBindings.showDatePicker === true) {
            $(element).datepicker({ dateFormat: "dd/mm/yy" });
        }

        var valueUnwrapped = unwrap(value);

        var newValue = updateDateValue(valueUnwrapped);
        
        if (element.type === "text") {
            $(element).val(newValue);
            ko.utils.registerEventHandler(element, "change", function() {
                var updateValue = valueAccessor();
                try {
                    //var tmpDate = $(element).datepicker("getDate");
                    var date = element.value.toDate();
                    updateValue(date);
                } catch(e) {

                }
            });

            ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
                $(element).datepicker("destroy");
            });
            
        } else {
            $(element).text(newValue);
        }
    },

    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = unwrap(value);
        var newValue = updateDateValue(valueUnwrapped);
        if (element.type === "text") {
            if (valueUnwrapped != 'Invalid Date') {
                $(element).val(newValue);
            }
        } else {
            $(element).text(newValue);
        }
    },
};

ko.bindingHandlers.datetimeString = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        datetimeBinder(element, valueAccessor, allBindings, viewModel, bindingContext);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        datetimeBinder(element, valueAccessor, allBindings, viewModel, bindingContext);
    }
};

function datetimeBinder(element, valueAccessor, allBindings, viewModel, bindingContext) {
    var value = valueAccessor();
    var valueUnwrapped = unwrap(value);
    var newValue = updateDateTimeValue(valueUnwrapped);
    if (element.type === "text") {
        $(element).val(newValue);
    } else {
        $(element).text(newValue);
    }
}

//readonly bindingHandler
ko.bindingHandlers.percentageString = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var newValue = valueAccessorToNumber(valueAccessor);
        var convertedValue = newValue == null ? '' : newValue.toPercentage(); // if null then return empty string to display;
        $(element).text(convertedValue);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var newValue = valueAccessorToNumber(valueAccessor);
        var convertedValue = newValue == null ? '' : newValue.toPercentage(); // if null then return empty string to display;
        $(element).text(convertedValue);
    }
};

function valueAccessorToNumber(valueAccessor) {
    var value = valueAccessor();
    var valueUnwrapped = unwrap(value);
    return valueUnwrapped != null ? getNumber(valueUnwrapped) : null; // if null then return null; 
}

//readonly bindingHandler
ko.bindingHandlers.currencyString = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var newValue = valueAccessorToNumber(valueAccessor);
        var convertedValue = newValue == null ? '' : newValue.toCurrency(); // if null then return empty string to display;
        $(element).text(convertedValue);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var newValue = valueAccessorToNumber(valueAccessor);
        var convertedValue = newValue == null ? '' : newValue.toCurrency(); // if null then return empty string to display;
        $(element).text(convertedValue);
    }
};

ko.bindingHandlers.anchorParams = {
    init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        anchorParamsBinder(element, valueAccessor, allBindings, viewModel, bindingContext);
    },
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        anchorParamsBinder(element, valueAccessor, allBindings, viewModel, bindingContext);
    }
};

function anchorParamsBinder(element, valueAccessor, allBindings, viewModel, bindingContext) {

    if (element.href == undefined)
        return; //return if this is not an anchor tag
    var hrefAttrVal = element.getAttribute('data-url');
    var hasDataUrl = hrefAttrVal != null;
   
    //DON'T USE element.href to get and set value.
    var hrefArr = (hasDataUrl ? hrefAttrVal : element.getAttribute('href')).split('?');

    var hrefPage = hrefArr[0];
    var hrefParamsArr = hrefArr.length > 1 ? hrefArr[1].split('&') : [];

    //find Id in (/0) url. e.g http://localhost/SODA/Opportunity/Edit/0
    var urlWithIdParam = hrefPage.split('/');
    var idIdResult = Number(urlWithIdParam[urlWithIdParam.length - 1]);
    //remove id number and add into para
    if (!isNaN(idIdResult)) {

        urlWithIdParam[urlWithIdParam.length - 1] = '';
        hrefPage = urlWithIdParam.aggregate(function (c, n) { return n == '' ? c : c + '/' + n; });
        //urlWithIdParam[urlWithIdParam.length - 1] = viewModel['Id']();
        //hrefPage = urlWithIdParam.aggregate(function(c, n) { return c + '/' + n; });
    }

    var hrefParamNames = hrefParamsArr.select(function (x) { return x.split('=')[0]; });
    
    var paramString = valueAccessor();

    paramString.split(',').forEach(function(x, i) {

        var queryParamArr = x.split('=');
        var paramName = queryParamArr[0];
        var value = unwrap(viewModel[queryParamArr[1]]);
        var queryParamValue = paramName + '=' + value;

        var indexOfhrefParam = hrefParamNames.indexOf(paramName);

        if (indexOfhrefParam > -1) {
            hrefParamsArr[indexOfhrefParam] = queryParamValue;
        } else {
            hrefParamsArr.push(queryParamValue);
        }
    });

    var queryParams = '?' + hrefParamsArr.aggregate(function (c, n) { return c + '&' + n; });

    //DON'T USE element.href to get and set value.
    //element.attributes['href'].value = hrefPage + queryParams;
    element.setAttribute((hasDataUrl ? 'data-url' : 'href'), hrefPage + queryParams);
}

ko.bindingHandlers.disableClick = {
    init: function(element, valueAccessor) {
        $(element).click(function(evt) {
            if (valueAccessor())
                evt.preventDefault();
        });

    },

    update: function(element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        ko.bindingHandlers.css.update(element, function() {
            return { disabled_anchor: value };
        });
    }
};

function unwrap(value) {
    if (typeof value === 'string') {
        return value;
    } else if (value instanceof Date) {
        return value;
    } else if (value instanceof Number) {
        return value;
    }
    return value();
}

function updateDateValue(valueUnwrapped) {

    var value;
    if (typeof valueUnwrapped === 'string') {
        value = getKOMinimumDate(new Date(parseInt(valueUnwrapped.substr(6))));
        return value.format("dd/MM/yyyy");
    } else if (valueUnwrapped instanceof Date) {
        value = getKOMinimumDate(valueUnwrapped);
        return value.format("dd/MM/yyyy");
    }
    return valueUnwrapped;
}

function updateDateTimeValue(valueUnwrapped) {

    var value;
    if (typeof valueUnwrapped === 'string') {
        value = getKOMinimumDate(new Date(parseInt(valueUnwrapped.substr(6))));
        return value.format("dd/MM/yyyy hh:mm");
    } else if (valueUnwrapped instanceof Date) {
        value = getKOMinimumDate(valueUnwrapped);
        return value.format("dd/MM/yyyy  hh:mm");
    }
    return valueUnwrapped;
}


function getKOMinimumDate(date) {
    //sql server can't store less then 1/1/1753
    if (date.getFullYear() < 1800) {
        date.setFullYear(1800);
    }
    return date;
}

function getNumber(valueUnwrapped) {
    if (valueUnwrapped == null) {
        return 0;
    }
    if (typeof valueUnwrapped === 'string') {
        return valueUnwrapped.toNumber();
    } else if (valueUnwrapped instanceof Date) {
        throw "not a number";
    }
    return valueUnwrapped;
}

ko.extenders.notifyChange = function (target, option) {
    target.subscribe(function (newValue) {
        console.log(option + ": " + newValue);
    });
    return target;
};