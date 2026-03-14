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

document.addEventListener("DOMContentLoaded", function () {
    const existingContainer = document.getElementById('existing-images-container');
    const orderInput = document.getElementById('ordered-image-ids');

    function updateOrderInput() {
        const ids = [];
        existingContainer.querySelectorAll('[data-id]').forEach(el => {
            const id = el.getAttribute('data-id');
            if (id) ids.push(id);
        });
        orderInput.value = ids.join(',');
    }

    if (existingContainer) {
        new Sortable(existingContainer, {
            animation: 150,
            ghostClass: 'bg-light',
            draggable: '.col-3',
            onEnd: function () {
                updateOrderInput();
            }
        });
        updateOrderInput();
    }
});