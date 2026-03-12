const Alert = {

    _toastMixin: Swal.mixin({
        toast: true,
        position: 'top-end', 
        showConfirmButton: false,
        timer: 3000, 
        timerProgressBar: true,
        showCloseButton: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
            toast.parentElement.style.paddingTop = '55px';
            toast.parentElement.style.paddingRight = '25px';
        }
    }),

    /**
     * @param {Object} options - Özelleştirme seçenekleri
     */

    toast: function (options) {
        const defaults = {
            icon: 'success', // success, error, warning, info
            title: 'İşlem başarılı!',
            iconColor: '#ff6000' // Senin turuncun
        };
        const settings = { ...defaults, ...options };
        return this._toastMixin.fire(settings);
    },

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
            showSuccessAlert: true,
            successAsToast: true,
            didClose: () => { },
            willClose: () => { },
            onClose: () => { }
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
            reverseButtons: settings.reverseButtons,
            didClose: settings.didClose,
            willClose: settings.willClose
        });

        if (result.isConfirmed) {
            try {
                await settings.onConfirm();

                if (settings.showSuccessAlert) {

                    if (settings.successAsToast) {
                        this.toast({ title: settings.successMessage, icon: 'success' });
                    } else {
                        Swal.fire(settings.successTitle, settings.successMessage, 'success');
                    }
                    
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