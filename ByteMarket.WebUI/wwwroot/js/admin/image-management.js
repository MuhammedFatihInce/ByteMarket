class ImageUploader {
    constructor(options) {

        this.input = document.getElementById(options.inputId);
        this.container = document.getElementById(options.containerId);
        this.existingContainer = document.getElementById(options.existingContainerId);

        this.maxCount = options.maxCount || 5;
        this.maxSize = options.maxSize || (5 * 1024 * 1024);
        this.alertTitle = options.alertTitle || 'Resim Seçimi';


        this.filesArray = [];
        this.init();
        this.initSortable();
    }

    init() {
        if (!this.input) return;

        this.input.addEventListener('change', (e) => {
            const newFiles = Array.from(e.target.files);

            for (const file of newFiles) {
                let existingCount = 0;

                if (this.existingContainer) {
                    existingCount = this.existingContainer.querySelectorAll('.position-relative').length;
                }

                const newlyAddedCount = this.filesArray.length;


                if (existingCount + newlyAddedCount >= this.maxCount) {
                    Alert.fire({
                        title: 'Limit Doldu!',
                        text: `Toplam resim sınırı ${this.maxCount} adettir.`,
                        icon: 'info',
                        confirmButtonText: 'Anladım'
                    });
                    break;
                }
                if (file.size <= this.maxSize && file.type.startsWith('image/')) {
                    this.filesArray.push(file);
                }
            }
            this.syncAndRender();
            
        });
    }

    initSortable() {
        if (!this.container) return;
        new Sortable(this.container, {
            animation: 150,
            ghostClass: 'bg-light',
            swapThreshold: 0.65, 
            invertSwap: true,     
            draggable: '.cursor-move',
            onEnd: () => {
                this.reorderFilesArray();
            }
        });
    }

    reorderFilesArray() {
        const newOrderFiles = [];
        const currentElements = this.container.querySelectorAll('.btn-delete-image');

        currentElements.forEach(btn => {
            const oldIndex = btn.getAttribute('data-id');
            newOrderFiles.push(this.filesArray[oldIndex]);
        });

        this.filesArray = newOrderFiles;

        this.syncHiddenInput();
        this.updateDomIndices();
    }

    syncHiddenInput() {
        const dataTransfer = new DataTransfer();
        this.filesArray.forEach(file => dataTransfer.items.add(file));
        this.input.files = dataTransfer.files;
    }

    updateDomIndices() {
        const deleteButtons = this.container.querySelectorAll('.btn-delete-image');
        deleteButtons.forEach((btn, i) => {
            btn.setAttribute('data-id', i);
        });
    }

    syncAndRender() {
        this.syncHiddenInput();
        this.renderPreviews();
    }


    renderPreviews() {
        this.container.innerHTML = '';
        this.filesArray.forEach((file, i) => {
            const reader = new FileReader();
            reader.onload = (event) => {
                const wrapperDiv = document.createElement('div');
                wrapperDiv.className = 'col-3 col-md-2 position-relative mt-3 cursor-move'; 
                wrapperDiv.innerHTML = `
                    <div class="ratio ratio-1x1 border rounded overflow-hidden shadow-sm bg-white">
                        <img src="${event.target.result}" class="img-fluid object-fit-cover">
                    </div>
                    <div class="btn-delete-image" data-id="${i}">
                        <i class="bi bi-x"></i>
                    </div>
                `;
                this.container.appendChild(wrapperDiv);

                wrapperDiv.querySelector('.btn-delete-image').addEventListener('click', () => {
                    const currentIndex = Array.from(this.container.children).indexOf(wrapperDiv);
                    this.removeFile(currentIndex);
                });
            };
            reader.readAsDataURL(file);
        });
    }

    removeFile(index) {
        this.filesArray.splice(index, 1);
        this.syncAndRender();
    }

}