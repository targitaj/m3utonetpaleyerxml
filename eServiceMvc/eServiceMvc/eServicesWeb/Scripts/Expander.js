$('.expanding-element').on('hidden.bs.collapse', function () {
    expanded(this, true);
});

$('.expanding-element').on('shown.bs.collapse', function () {
    expanded(this, false);
});

function expanded(elem, isExpanding) {
    var img = $(elem).prev().find('img');
    if (isExpanding) {
        img.attr('src', img.attr('src').replace('expander_up_icon.png', 'expander_down_icon.png'));
    } else {
        img.attr('src', img.attr('src').replace('expander_down_icon.png', 'expander_up_icon.png'));
    }
}

$(document).ready(function() {
	    $(".expander-block input").each(function(index, value) {
	        var input = $(value);
	        var requiredAttrVal = input.attr('required');
	        var type = input.attr('type');

	        if (requiredAttrVal == 'required' && type == 'text') {
	            input.blur(function () {
	                checkAllInputs(getParentExpander($(this)));
	            });
	        }

	        
	    });

	    $(".expander-block").each(function (index, value) {
            var expander = $(value);
            var fillPercentage = expander.attr('fillPercentage');

            if (fillPercentage != null) {
                expander.find('.expander-green-line').css('width', fillPercentage + "%");
                expander.find('.expander-pink-line').css("background-color", "#fc6e50");

            }
	    });
	});

	function getParentExpander(elem) {
	    if (elem.hasClass('expander-block') || elem == null) {
	        return elem;
	    }

	    return getParentExpander(elem.parent());
	}

	function checkAllInputs(expander) {
	    //if (expander == null) {
	    //    return;
	    //}

	    //var requiredCnt = 0;
	    //var filledInputCnt = 0;

	    //expander.find('input').each(function (index, value) {
	    //    var input = $(value);
	    //    var requiredAttrVal = input.attr('required');
	    //    var type = input.attr('type');

	    //    if (requiredAttrVal == 'required' &&  type == 'text') {
	    //        requiredCnt++;
	    //        if (input.val().trim() != '') {
	    //            filledInputCnt++;
	    //        }
	    //    }
	    //});

	    //var greenWidth = (filledInputCnt / requiredCnt) * 100;
	    //expander.find('.expander-green-line').css('width', greenWidth + "%");
	}
