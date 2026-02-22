const Alert = {
    /**
     * @param {Object} options - Özelleştirme seçenekleri
     */
    fire: async function (options) {
        // 1. VARSAYILAN AYARLAR (Şablon)
        const defaults = {
            title: 'Emin misiniz?',
            text: 'Bu işlem geri alınamaz!',
            icon: 'warning',
            confirmButtonText: 'Evet, Devam Et',
            cancelButtonText: 'Vazgeç',
            confirmButtonColor: '#ff6000',
            cancelButtonColor: '#6c757d',
            showCancelButton: true,
            reverseButtons: true,
            successTitle: 'Başarılı!',
            successMessage: 'İşlem başarıyla tamamlandı.',
            onConfirm: async () => { }, 
            showSuccessAlert: true
        };

        const settings = { ...defaults, ...options };

        const result = await Swal.fire({
            title: settings.title,
            text: settings.text,
            icon: settings.icon,
            showCancelButton: settings.showCancelButton,
            confirmButtonColor: settings.confirmButtonColor,
            cancelButtonColor: settings.cancelButtonColor,
            confirmButtonText: settings.confirmButtonText,
            cancelButtonText: settings.cancelButtonText,
            reverseButtons: settings.reverseButtons
        });

        if (result.isConfirmed) {
            try {
                await settings.onConfirm();

                if (settings.showSuccessAlert) {
                    Swal.fire(settings.successTitle, settings.successMessage, 'success');
                }

                return true;
            } catch (error) {
                console.error("ByteAlert Hatası:", error);
                Swal.fire('Hata!', 'İşlem gerçekleştirilirken bir sorun oluştu.', 'error');
                return false;
            }
        }
        return false;
    }
};