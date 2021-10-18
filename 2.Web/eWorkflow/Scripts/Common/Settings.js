$(document).ready(function(){   
    $.fn.tabs(".tab");
});

$.fn.tabs = function (selectorName) {

    fn_SetAreaToggle();
    $(selectorName + ' li a').each(function () {
        $(this).click(function () {
            $(selectorName + ' li').each(function () {
                $(this).removeClass('on');
            });
            $(this).parent().addClass('on');
            var id = $(this).attr('href')

            fn_SetAreaToggle(id);

            return false;
        });
    });

    function fn_SetAreaToggle(id) {
        if (id == undefined) {
            id = $($(selectorName +' li a')[0]).attr("href").substr(1);
        }

        $('div[id^=tab]').each(function () {
            if ($(selectorName + ' li a[href=#' + $(this).attr("id") + ']').parent().hasClass('on'))
                $(this).css({ display: '' });
            else
                $(this).css({ display: 'none' });
        })
    }

}
  