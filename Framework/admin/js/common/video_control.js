/**
 * Hung-DV 28/05
 */

var videos = $('.video-control');

$.each(videos,function(key,val){
	var input = $($(val).find('input')[1]);
	input.change(function(){
		var url = $(this).val();
		var id = url.substr(url.indexOf('v=')+2);
		var videoLink = "https://www.youtube.com/embed/"+id+"?autoplay=1";
		$($(val).find('input')[0]).val(id);
		$($(val).find('iframe')[0]).attr('src', videoLink);
		$($(val).find('iframe')[0]).css('display', 'block');
	});
});
