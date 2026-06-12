$(document).ready(function () {

	$('#productReviewModal').on('show.bs.modal', function (event) {

		const button = $(event.relatedTarget);

		const productId = button.data('bs-product-id');
		const orderId = button.data('bs-order-id');
		const productName = button.data('bs-product-name');
		const producImageUrl = button.data('bs-product-image');

		$(this).find('#productId').val(productId);
		$(this).find('#orderId').val(orderId);
		$(this).find('#productName').text(`${productName}`);
		
		if (producImageUrl) {
			$(this).find('#productImage').attr('src', producImageUrl).attr('alt', productName).show();
		}
	});

	$('#productReviewForm').on('submit', async function (e) {
		e.preventDefault();

		const productId = $('#productId').val();
		const orderId = $('#orderId').val();
		const rating = $("input[name='rating']:checked").val();
		const comment = $('#comment').val();

		console.log(orderId);

		const data = {
			Comment: comment,
			Rating: rating,
			ProductId: productId,
			OrderId: orderId
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

	$('#editReviewModal').on('show.bs.modal', function (event) {

		const button = $(event.relatedTarget);

		const productName = button.data('bs-product-name');
		const producImageUrl = button.data('bs-product-image');
		const reviewId = button.data('bs-product-review-id');
		const comment = button.data('bs-product-comment');
		const rating = button.data('bs-product-rating');

		$(this).find('#editReviewId').val(reviewId);
		$(this).find(`input[name="editRating"][value="${rating}"]`).prop("checked", true);
		$(this).find('#editComment').val(comment);
		$(this).find('#editProductName').text(`${productName}`);

		if (producImageUrl) {
			$(this).find('#editProductImage').attr('src', producImageUrl).attr('alt', productName).show();
		}
	});

	$('#editReviewForm').on('submit', async function (e) {
		e.preventDefault();

		const reviewId = $('#editReviewId').val();
		const rating = $("input[name='editRating']:checked").val();
		const comment = $('#editComment').val();

		if (!rating) {
			$('#responseMessage').html('<div class="alert alert-danger">Lütfen bir puan seçiniz!</div>')
			return;
		}
		$('#responseMessage').addClass("d-none");

		const data = {
			Id: reviewId,
			Comment: comment,
			Rating: rating
		}

		try {
			const response = await CustomAjax.put('Order/UpdateReview', data);

			if (response && response.success) {
				Alert.toast({ title: response.message, icon: 'success' });
				setTimeout(() => location.reload(), 1500);
			}
		} catch (error) {
			console.error("Yorum hatası:", error);
		}
	});

	$('#btnDeleteReview').on('click', async function () {

		const reviewId = $(this).data('review-id');

		Alert.fire({
			title: 'Yorum Silinecek!',
			text: `Bu yorumu silmek istediğinize emin misiniz?`,
			icon: 'warning',
			confirmButtonText: 'Evet, Sil',
			onConfirm: async () => {
				try {
					const response = await CustomAjax.delete(`/Order/DeleteReview/${reviewId}`);

					if (response && response.success) {
						Alert.toast({ title: response.message, icon: 'success' });
						setTimeout(() => location.reload(), 1500);

					} else {
						Alert.error({ title: "Hata", text: response.message });
					}
				} catch (error) {
					Alert.error({ title: "Hata", text: "Bağlantı kurulamadı!" });
				}
			},
			successMessage: 'Yorum başarıyla silindi!'
		});
	});

});