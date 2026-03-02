$(document).on('click', '.btn-delete', function () {
    const productId = $(this).data('id');
    const productName = $(this).data('name');

    Alert.fire({
        title: 'Ürünü Sil',
        text: `"${productName}" isimli ürün ve bağlı tüm görseller kalıcı olarak silinecek!`,
        confirmButtonText: 'Evet, Sil!',
        onConfirm: async () => {
            const response = await CustomAjax.delete(`Admin/Product/Delete/${productId}`);


            if (response && response.success) {
                const row = document.getElementById(`row-${productId}`);

                if (row) {
                    row.classList.add('fade-out');
                    setTimeout(() => row.remove(), 400);
                }
            }
        },
        successMessage: 'Ürün başarıyla silindi.'
    });

});