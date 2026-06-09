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

	$('#productReviewForm').on('submit', async function (e) {
		e.preventDefault();

		const productId = $('#productId').val();
		const rating = $("input[name='rating']:checked").val();
		const comment = $('#comment').val();

		const data = {
			Comment: comment,
			Rating: rating,
			ProductId: productId
		}

		if (!rating) {
			$('#responseMessage').html('<div class="alert alert-danger">Lütfen bir puan seçiniz!</div>')
			return;
		}
		$('#responseMessage').addClass("d-none");

		try {
			const response = await CustomAjax.post('Order/AddReview', data);

			if (response && response.success) {
				Alert.toast({ title: response.message, icon: 'success' });
				setTimeout(() => location.reload(), 1500);
			}
		} catch (error) {
			console.error("Yorum hatası:", error);
		}
	});


});