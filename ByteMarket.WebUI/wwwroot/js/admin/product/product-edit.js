async function removeExistingImage(imageId) {

	await Alert.fire({
		title: 'Görseli Sil',
		text: 'Bu görseli kalıcı olarak silmek istediğinize emin misiniz?',
		successMessage: 'Görsel başarıyla sistemden kaldırıldı.',
		onConfirm: async () => {
			await HttpClient.delete(`ProductImages/Delete/${imageId}`);

			const element = document.getElementById(`img-wrapper-${imageId}`);
			if (element) {
				element.classList.add('fade-out');
				setTimeout(() => {
					element.remove();
				}, 300);
			}
		}
	});
}