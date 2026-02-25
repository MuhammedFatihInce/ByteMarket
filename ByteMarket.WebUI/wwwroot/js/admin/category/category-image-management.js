const categoryUploader = new ImageUploader({
    inputId: 'image-input',
    containerId: 'image-preview-container',
    existingContainerId: 'existing-images-container',
    maxCount: 1,
    maxSize: 5 * 1024 * 1024
});