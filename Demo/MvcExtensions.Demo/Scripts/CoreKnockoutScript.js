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
            $(element).val(newValue);
        } else {
            $(element).text(newValue);
        }
    },
};

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