function addEvents(object) {
    object.find('.dynamic-controls-trigger').on('click', function(e)
    {
        e.preventDefault();
        var wrapper = $(this).closest('.dynamic-controls');
        var control = wrapper.find('.dynamic-controls-template').clone();

        control = formatControlForRazorUsingHtml(wrapper, control);

        control.addClass('dynamic-controls-item');
        control.show();
        control.find('.bootstrap-select').remove();
        wrapper.find('.dynamic-controls-container').append(control);
        attachCustomEvents(control);
    });

    object.find('.dynamic-controls-container').on('click', '.dynamic-controls-remove-button img', function (e) {
        e.preventDefault();
        var item = $(this).closest('.dynamic-controls-item');
        var wrapper = $(this).closest('.dynamic-controls');

        item.remove();

        changeModelArrayCounter = -1;
        currentMatch = '';
        changeModelArray(wrapper);
    });
}

function formatControlForRazor(wrapper, control) {
    $.each(control[0].attributes, function (i, attrib) {
        attrib.value = attrib.value.format(modelArrayLengthCounter);
        if (attrib.name == 'dynamic-value') {
            $(control).val(attrib.value);
        }
    });

    control.children().each(function () {
        formatControlForRazor(wrapper, $(this));
    });
}

function formatControlForRazorUsingHtml(wrapper, control) {
    currentMatch = '';
    modelArrayLengthCounter = 0;
    modelArrayLength(wrapper);

    var html = control.html();
    html = html.format(modelArrayLengthCounter);
    return $($.parseHTML(html));
}

var currentMatch = '';
var changeModelArrayCounter = -1;

function changeModelArray(elem) {
    $.each(elem[0].attributes, function (i, attrib) {
        var reg = new RegExp("\\[[0-9]*\]", "gm");
        attrib.value = attrib.value.replace(reg, function (str) {
                if (currentMatch != str) {
                    currentMatch = str;
                    changeModelArrayCounter++;
                }
                return "[" + changeModelArrayCounter + "]";
            });
    });

    elem.children().each(function() {
        changeModelArray($(this));
    });
}

var modelArrayLengthCounter = 0;

function modelArrayLength(elem) {
    $.each(elem[0].attributes, function (i, attrib) {
        var reg = new RegExp("\\[[0-9]*\]", "gm");
        attrib.value.replace(reg, function (str) {
            if (currentMatch != str) {
                currentMatch = str;
                modelArrayLengthCounter++;
            }
            return;
        });
    });

    elem.children().each(function () {
        modelArrayLength($(this));
    });
}

$(document).ready(function () {
    String.prototype.format = function () {
        var str = this;
        for (var i = 0; i < arguments.length; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            str = str.replace(reg, arguments[i]);
        }
        return str;
    }

    addEvents($(this));
});

