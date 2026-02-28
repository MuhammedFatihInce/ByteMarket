$(document).on('click', '.btn-delete', function () {
    const id = $(this).data('id');
    const name = $(this).data('name');

    Alert.fire({
        title: 'Kategoriyi Sil?',
        text: `"${name}" kategorisi silinecek. Bu işlem geri alınamaz!`,
        confirmButtonText: 'Evet, Sil!',
        onConfirm: async () => {
            const response = await CustomAjax.delete(`/Category/Delete/${id}`)

            if (response && response.success) {
                const row = document.getElementById(`row-${id}`);
                if (row) {
                    row.classList.add('fade-out');
                    setTimeout(() => row.remove(), 300);
                }
            }

            
        },
        successMessage: 'Kategori başarıyla silindi.'
    });
});