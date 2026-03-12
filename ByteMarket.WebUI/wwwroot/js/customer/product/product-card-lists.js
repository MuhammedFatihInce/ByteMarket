$(document).ready(function () {

	$(".image-box").on('mousemove', function (e) {
		
		const $imgDiv = $(this);
		const $imgElement = $imgDiv.find('img');

		const images = $imgDiv.data('images').split(',');
		const totalImages = images.length;

		const offset = $imgDiv.offset();
		const relativeX = e.pageX - offset.left;
		const width = $imgDiv.outerWidth();

		let index = Math.floor((relativeX / width) * totalImages);

		if (index < 0) index = 0;
		if (index >= totalImages) index = totalImages - 1;

		$imgElement.attr('src', images[index]);

		const $dots = $imgDiv.closest('.product-card').find('.dot');
		$dots.removeClass('active');
		$dots.eq(index).addClass('active');

	});

	$('.image-box').on('mouseleave', function () {
		const $imgDiv = $(this);
		const $imgElement = $imgDiv.find('img');
		const images = $imgDiv.data('images').split(',');

		$imgElement.attr('src', images[0]);

		const $dots = $imgDiv.closest('.product-card').find('.dot');
		$dots.removeClass('active');
		$dots.eq(0).addClass('active');
	});

});