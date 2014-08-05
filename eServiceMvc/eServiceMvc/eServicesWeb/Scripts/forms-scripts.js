var fileUploadFormsCollection = [];

//Used for JS is loaded and for dynamic control creation
function attachCustomEvents(object) {
    // Adds functionality to form element label help "buttons"
    object.find('.help-tooltip').tooltip({
        "container": "body",
        "trigger": "hover click",
        "placement": "auto top"
    });

    // Enables selectpickers
    object.find('.selectpicker').selectpicker();

    // Adds Symbol counter for fields with MaxLength attribute - will show up when less than 10 symbols left
    object.find('input[maxlength]').maxlength();

    // Enables Datepickers and makes them appear only on button press (should be OPT-ioned in future https://github.com/eternicode/bootstrap-datepicker/issues/377)
    object.find('.input-group.date').datepicker()
        .on('changeDate', function (ev) {
            $(this).children('input').valid();
        });
    object.find(".input-group.date input").focus(function (ctr) {
        $(".input-group.date").datepicker('hide');
        this.select();
    });

    /* Script to auto-position onened form sections when either header is clicked or "save" button - auto-clicks same header */
    object.find('.btn-sm.formblocksubmit').click(function () {
        $('.accordion .expander-header').eq($(this).data("targetpanel")).click();
    });
    object.find('.expander-block').on('shown.bs.collapse', function () {
        var block = $(this)[0];
        var offsetGap = block.offsetTop - 190;
        $('html, body').animate({
            scrollTop: offsetGap
        }, 0);
    });

    

    //Used for file upload control functionality initiation if form div with umaAjaxFileUpload class
    //TODO: Fix dropZone for IE
    object.find("form").each(function (elem) {
        var form = $($("form")[elem]);
        var attribs = form.find('.umaAjaxFileUpload');

        if (attribs != null && attribs.length != 0) {
            var uloadUrl = attribs.attr('uploadUrl');
            var maxnumberoffiles = parseInt(attribs.attr('maxnumberoffiles'));
            var minumberoffiles = parseInt(attribs.attr('minnumberoffiles'));
            var fileCount = 0;

            fileUploadFormsCollection.push({
                form: form,
                uloadUrl: uloadUrl,
                data: []
            });

            if (isNaN(minumberoffiles)) {
                minumberoffiles = -1;
            }
            enableDisableControls();

            form.find('input').each(function (fElem) {
                var input = $(form.find('input')[fElem]);

                if (input.attr('type') != null && input.attr('type') == 'file') {
                    input.attr('accept', attribs.attr('mimes'));

                    if (isNaN(maxnumberoffiles) || maxnumberoffiles > 1) {
                        input.attr('multiple', 'multiple');
                    }

                    return;
                }
            });

            $(form.find('.template-upload')).livequery(function (cnt, control) {
                currentMatch = '';
                modelArrayLengthCounter = 0;
                modelArrayLength($(control).parent());

                formatControlForRazor($(control).parent(), $(control));
                showHideEmptyTextAndFormatRazor();
            });

            $(form.find('.template-download')).livequery(function (cnt, control) {
                currentMatch = '';
                modelArrayLengthCounter = 0;
                modelArrayLength($(control).parent());

                formatControlForRazor($(control).parent(), $(control));
                
                showHideEmptyTextAndFormatRazor();
                subscribeChildInputFieldsToAuotoSave($(control));
            });

            form.attr('enctype', 'multipart/form-data');
            var addedFiles = [];

            form.fileupload({
                dropZone: form,
                url: uloadUrl,
            }).bind('fileuploadadd', function (e, data) {
                // uncomment this if necessary do upload after form all submit button pressed
                // $.each(fileUploadFormsCollection, function(i, frmElem) {
                //    if (frmElem.form == form) {
                //        frmElem.data.push(data);
                //    }
                // });

                addedFiles.push(data.files);
                enableDisablUpploadButton();
            }).bind('fileuploadfail', function (e, data) {
                addedFiles.remove(data.files);
                enableDisablUpploadButton();
            }).bind('fileuploadfailed', function (e, data) {
                showHideEmptyTextAndFormatRazor();
            });

            form.fileupload('option', {
                maxFileSize: attribs.attr('maxFileSize'),
                acceptFileTypes: eval(attribs.attr('acceptfiletypes')),
                maxNumberOfFiles: isNaN(maxnumberoffiles) ? '@' : maxnumberoffiles,
                singleFileUploads: isNaN(maxnumberoffiles) ? true : false,
                autoUpload: true,
                completed: function (e, data) {
                    fileCount++;
                    addedFiles = [];
                    
                    $.each(fileUploadFormsCollection, function (i, frmElem) {
                        if (frmElem.form == form) {
                            frmElem.data = [];
                        }
                    });

                    enableDisablUpploadButton();
                    enableDisableControls();
                    showHideEmptyTextAndFormatRazor();
                },
                destroyed: function (e, data) {
                    fileCount--;
                    enableDisableControls();
                    showHideEmptyTextAndFormatRazor();
                },
                processfail: function (e, data) {
                    addedFiles.remove(data.files);
                    enableDisablUpploadButton();
                }
            });

            var existingFilesVariableNotEmpty = true;

            try {
                var tmp = eval(attribs.attr('existingFilesVariable'));
            } catch(e) {
                existingFilesVariableNotEmpty = false;
            }

            if (existingFilesVariableNotEmpty) {
                form.fileupload('option', 'done').call(form, $.Event('done'), {
                    result: {
                        files: eval(attribs.attr('existingFilesVariable'))
                    }
                });
            }

            function enableDisablUpploadButton() {
                if (addedFiles.length > 0) {
                    form.find('.btn.start').removeAttr('disabled');
                } else {
                    form.find('.btn.start').attr('disabled', 'disbaled');
                }
            }

            function showHideEmptyTextAndFormatRazor() {
                if (form.find('.files').children().length > 1) {
                    form.find('.empty-string').hide();
                } else {
                    form.find('.empty-string').show();
                }
                //TODO: Reafctor this - remove changeModelArrayCounter and currentMatch
                changeModelArrayCounter = -1;
                currentMatch = '';
                changeModelArray(form.find('.files'));
            }

            function enableDisableControls() {
                var ids = attribs.attr('enablingids').split(',');

                ids.forEach(function (entry) {
                    if (fileCount < minumberoffiles) {
                        $('#' + entry).attr('disabled', 'disbaled');
                    }
                    else {
                        $('#' + entry).removeAttr('disabled');
                    }
                });
            }
        }
    });

    subscribeChildInputFieldsToAuotoSave(object);
}

//Form auto saving logic
$('form').each(function (elem) {
    var form = $($("form")[elem]);
    subscribeChildInputFieldsToAuotoSave(form);
});

function subscribeChildInputFieldsToAuotoSave(parent) {
    var form = $(parent.closest('form'));
   
    if (form != null && form.attr('ajaxSavingUrl') != null) {
        var ajaxSavingUrl = form.attr('ajaxSavingUrl');
        parent.find('.formblocksubmit').click(function() {
            ajaxFormPost(ajaxSavingUrl, form);
        });
        parent.find('input[type=text]').focusout(function () {
            ajaxFormPost(ajaxSavingUrl, form);
        });
        parent.find('input[type=radio]').click(function () {
            ajaxFormPost(ajaxSavingUrl, form);
        });

        parent.find('.input-group.date').datepicker().on('changeDate', function (ev) {
            ajaxFormPost(ajaxSavingUrl, form);
        });

        parent.find('select').change(function () {
            ajaxFormPost(ajaxSavingUrl, form);
        });

        parent.find('input[type=checkbox]').click(function () {
            ajaxFormPost(ajaxSavingUrl, form);
        });

        parent.find('textarea').focusout(function () {
            ajaxFormPost(ajaxSavingUrl, form);
        });
    }
}

var ajaxRequestCount = 0;

function ajaxFormPost(ajaxSavingUrl, form) {
    ajaxRequestCount++;
    setTimeout(function() { doAjaxFormPost(ajaxSavingUrl, form); }, 2000);
}

function doAjaxFormPost(ajaxSavingUrl, form) {
    ajaxRequestCount--;
    if (ajaxRequestCount != 0) {
        return;
    }

    var securityToken = form.find('[name=__RequestVerificationToken]').val();
    form.__RequestVerificationToken = securityToken;

    $.ajax({
        url: ajaxSavingUrl,
        type: 'POST',
        cache: false,
        data: form.serializeObject(),
        success: function (data) {
        }
    });
}

/* Page 6 = payment page height calculations */
function checkHeight(array) {
    var maxHeight = 0;
    array.each(function () { maxHeight = Math.max(maxHeight, $(this).height()); })
                       .height(maxHeight);
}

$(function () {
    checkHeight($(".height-h3"));
    checkHeight($(".height-1"));
    checkHeight($(".height-2"));
    checkHeight($(".height-3"));
});

// jQuery Validator for date based on supplied datetime format
jQuery(function ($) {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        if (moment(value, element.attributes.placeholder.value.toUpperCase()).isValid()) {
            return true;
        }
        return false;
    });
});

function submitAllFileUploadForms() {
    $.each(fileUploadFormsCollection, function(i, formData) {
        if (formData.data.length == 0) {
            formData.success = function () {
                submitNextFileUploadForm();
            }
        } else {
            formData.form.bind('fileuploadcompleted', function (e, data) {
                submitNextFileUploadForm();
            });
        }
    });

    submitNextFileUploadForm();

    return false;
}

function submitNextFileUploadForm() {
    if (fileUploadFormsCollection.length == 0) {
        $("form").last().submit();
        return;
    }

    var fForm = fileUploadFormsCollection[0];
    if (fForm.data.length <= 1) {
        fileUploadFormsCollection.remove(fForm);
    }

    if (fForm.data.length == 0) {
        $.ajax({
            url: fForm.uloadUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                model: fForm.form.serializeObject()
            }),
            success: function(data) {
                fForm.success();
            }
        });
    } else {
        var dt = fForm.data[0];
        fForm.data.remove(dt);

        dt.submit();
    }
}

Array.prototype.remove = function (v) { this.splice(this.indexOf(v) == -1 ? this.length : this.indexOf(v), 1); }



attachCustomEvents($(document));