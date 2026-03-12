class ImageUploader {
    constructor(options) {

        this.input = document.getElementById(options.inputId);
        this.container = document.getElementById(options.containerId);
        this.existingContainer = document.getElementById(options.existingContainerId);

        this.maxCount = options.maxCount || 5;
        this.maxSize = options.maxSize || (5 * 1024 * 1024);
        this.alertTitle = options.alertTitle || 'Resim Seçimi';

        this.dataTransfer = new DataTransfer();
        this.init();
    }

    init() {
        if (!this.input) return;

        this.input.addEventListener('change', (e) => {
            e.stopImmediatePropagation();

            const newFiles = e.target.files;
            let rejectedFiles = [];

            for (let i = 0; i < newFiles.length; i++) {
                const file = newFiles[i];


                const existingCount = this.existingContainer ?
                    this.existingContainer.querySelectorAll('.position-relative').length : 0;
                const newlyAddedCount = this.dataTransfer.items.length;

                if (!file.type.startsWith('image/')) continue;


                if (existingCount + newlyAddedCount >= this.maxCount) {
                    Alert.fire({
                        title: 'Limit Doldu!',
                        text: `Toplam resim sınırı ${this.maxCount} adettir.`,
                        icon: 'info',
                        confirmButtonText: 'Anladım'
                    });
                    break;
                }


                if (file.size > this.maxSize) {
                    rejectedFiles.push(file.name);
                    continue;
                }

                this.dataTransfer.items.add(file);
            }

            if (rejectedFiles.length > 0) {
                Alert.fire({
                    title: 'Büyük Dosya!',
                    text: `Bazı dosyalar boyutu aştığı için eklenmedi: ${rejectedFiles.join(", ")}`,
                    icon: 'error',
                    confirmButtonText: 'Tamam'
                });
            }

            this.input.files = this.dataTransfer.files;
            this.renderPreviews();
        });
    }

    renderPreviews() {
        this.container.innerHTML = '';
        const files = this.dataTransfer.files;

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            const reader = new FileReader();

            reader.onload = (event) => {
                const wrapperDiv = document.createElement('div');
                wrapperDiv.className = 'col-3 col-md-2 position-relative mt-3';

                wrapperDiv.innerHTML = `
                    <div class="ratio ratio-1x1 border rounded overflow-hidden shadow-sm bg-white">
                        <img src="${event.target.result}" class="img-fluid object-fit-cover">
                    </div>
                    <div class="btn-delete-image" data-index="${i}">
                        <i class="bi bi-x"></i>
                    </div>
                `;

                this.container.appendChild(wrapperDiv);

                wrapperDiv.querySelector('.btn-delete-image').addEventListener('click', () => {
                    this.removeFile(i);
                });
            };
            reader.readAsDataURL(file);
        }
    }

    removeFile(index) {
        this.dataTransfer.items.remove(index);
        this.input.files = this.dataTransfer.files;
        this.renderPreviews();
    }
}