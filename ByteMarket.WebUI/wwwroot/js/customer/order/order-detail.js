$(document).ready(function () {

	$('#productReviewModal').on('show.bs.modal', function (event) {

		const button = $(event.relatedTarget);

		const productId = button.data('bs-product-id');
		const productName = button.data('bs-product-name');
		const producImageUrl = button.data('bs-product-image');

		$(this).find('#productId').val(productId);
		$(this).find('#productName').text(`${productName}`);
		
		if (producImageUrl) {
			$(this).find('#productImage').attr('src', producImageUrl).attr('alt', productName).show();
		}


	});


});