async function removeExistingImage(imageId) {

	await Alert.fire({
		title: 'Görseli Sil',
		text: 'Bu görseli kalıcı olarak silmek istediğinize emin misiniz?',
		successMessage: 'Görsel başarıyla sistemden kaldırıldı.',
		onConfirm: async () => {
			var response = await CustomAjax.delete(`Admin/Product/DeleteImage/${imageId}`);

			if (response && response.success) {
				const element = document.getElementById(`img-wrapper-${imageId}`);
				if (element) {
					element.classList.add('fade-out');
					setTimeout(() => {
						element.remove();
					}, 300);
				}
			}
		}
	});
}