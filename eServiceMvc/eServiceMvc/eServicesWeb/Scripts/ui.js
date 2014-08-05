function findParentHref(currTag, cnt)
{
    if (cnt == null) {
        cnt = 0;
    }

    var ret = currTag.parent();

    if (ret != null && (ret.length == 0 || ret[0].tagName != 'A') && cnt <= 10) {
        cnt++;
        ret = findParentHref(ret, cnt);
    }

    if (cnt > 10) {
        ret = null;
    }

    return ret;
}

function findParentClickable(currTag, cnt) {
    if (cnt == null) {
        cnt = 0;
    }

    var ret = currTag.parent();

    if (ret.hasClass('actionblock')) {
        var t = 0;
        t++;
    }

    if (!isClickable(ret) && cnt <= 10 && !ret.hasClass('actionblock')) {
        cnt++;
        ret = findParentClickable(ret, cnt);
    }

    if (cnt > 10) {
        ret = null;
    }

    return ret;
}

function isClickable(tag) {
    return tag.attr('onclick') != null;
}

var translationGoToTheTranslationPageEventOccurs = false;

$(document).ready(function () {
    $("span.EditorLink").each(function (elem) {
        var span = $($("span.EditorLink")[elem]);

        if (span.attr('href') != null) {
            span.click(function () {
                translationGoToTheTranslationPageEventOccurs = true;
                window.location.href = $(this).attr('href');
            });

            var clickable = findParentClickable(span);
            if (clickable != undefined && clickable.attr('onclick').indexOf('continueClickable') < 0) {
                clickable.attr('onclick', 'if (continueClickable()) {' + clickable.attr('onclick') + '}');
            }

            var href = findParentHref(span);
            if (href != null) {
                $(href).click(function () {
                    return !translationGoToTheTranslationPageEventOccurs;
                });
            }
        }
    });
});

function continueClickable() {
    return !translationGoToTheTranslationPageEventOccurs;
}


