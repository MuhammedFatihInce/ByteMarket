$(document).on('click', '.btn-delete', function () {
    const productId = $(this).data('id');
    const productName = $(this).data('name');

    Alert.fire({
        title: 'Ürünü Sil',
        text: `"${productName}" isimli ürün ve bağlı tüm görseller kalıcı olarak silinecek!`,
        confirmButtonText: 'Evet, Sil!',
        onConfirm: async () => {
            await HttpClient.delete(`Products/Delete/${productId}`);

            const row = document.getElementById(`row-${productId}`);

            if (row) {
                row.style.transition = "all 0.4s ease";
                row.style.opacity = "0";
                row.style.transform = "translateX(20px)";
                setTimeout(() => row.remove(), 400);
            }
        }
    });

});