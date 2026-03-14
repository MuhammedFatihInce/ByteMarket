document.addEventListener("DOMContentLoaded", function () {
   
    const productUploader = new ImageUploader({
        inputId: 'image-input',                
        containerId: 'image-preview-container', 
        existingContainerId: 'existing-images-container',
        maxCount: 5,                           
        maxSize: 5 * 1024 * 1024,             
        alertTitle: 'Ürün Fotoğrafı'
    });
});