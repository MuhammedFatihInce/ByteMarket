function deleteRole(id, roleName) {
    Alert.fire({
        title: `${roleName} rolünü silmek istiyor musunuz?`,
        text: "Bu işlem geri alınamaz ve bu role sahip kullanıcıların yetkileri etkilenebilir!",
        icon: 'warning',
        confirmButtonText: 'Evet, Sil',
        onConfirm: async () => {

            const response = await CustomAjax.delete(`/Admin/Role/Delete/${id}`);
            if (response && response.success) {
                location.reload();
            }
        },
        successMessage: 'Rol başarıyla silindi!'
    });
}

async function openEditModal(roleId, roleName) {
    $('#editRoleTitle').html(`<i class="bi bi-shield-check me-2"></i>${roleName} Yetkileri`);
    $('#editRoleId').val(roleId);
    $('#editPermissionsContainer').html('<div class="text-center text-muted spinner-border-sm" role="status">Yükleniyor...</div>');
    $('#editRoleModal').modal('show');

    try {
        // fetch ve .json() yerine doğrudan CustomAjax.get() kullanıyoruz
        const result = await CustomAjax.get(`/Admin/Role/GetPermissions?roleId=${roleId}`);

        if (result && result.success && result.data && result.data.permissions) {
            let html = '';
            result.data.permissions.forEach((p, index) => {
                let checkedStr = p.isExist ? 'checked' : '';
                html += `
                <div class="form-check mb-2">
                    <input class="form-check-input edit-perm-checkbox" type="checkbox" value="${p.permissionValue}" id="edit_${index}" ${checkedStr}>
                    <label class="form-check-label" for="edit_${index}">${p.permissionValue}</label>
                </div>`;
            });
            $('#editPermissionsContainer').html(html);
        } else {
            $('#editPermissionsContainer').html('<div class="text-danger">Yetkiler yüklenirken bir hata oluştu.</div>');
        }
    } catch (error) {
        // CustomAjax kendi içinde Alert.toast gösterse de, modal içindeki yükleniyor 
        // yazısının da hata mesajıyla değişmesi kullanıcı deneyimi için iyidir.
        $('#editPermissionsContainer').html('<div class="text-danger">Bağlantı hatası. Lütfen tekrar deneyin.</div>');
    }
}

$(document).ready(function () {

    $('#roleCreateForm').on('submit', function (e) {
        e.preventDefault();

        var roleName = $('#roleName').val();

        Alert.fire({
            title: 'Emin misiniz?',
            text: `${roleName} rolünü eklemek istiyor musunuz?`,
            icon: 'question',
            confirmButtonText: 'Evet, Ekle',
            onConfirm: async () => {
                this.submit();
            },
            successMessage: 'Rol başarıyla eklendi!'
        });
    });

    // Düzenleme Formunu Gönderme İşlemi
    $('#roleEditForm').on('submit', function (e) {
        e.preventDefault();

        const roleId = $('#editRoleId').val();
        const selectedPermissions = [];

        $('.edit-perm-checkbox:checked').each(function () {
            selectedPermissions.push($(this).val());
        });

        const data = {
            Id: roleId,
            Permissions: selectedPermissions
        };

        Alert.fire({
            title: 'Yetkiler güncellenecek!',
            text: `Emin misiniz?`,
            icon: 'warning',
            confirmButtonText: 'Evet, Güncelle',
            onConfirm: async () => {
                // Burada senin CustomAjax post yapını kullanıyoruz
                const response = await CustomAjax.post('/Admin/Role/UpdatePermissions', data);
                if (response && response.success) {
                    location.reload();
                }
            },
            successMessage: 'Yetkiler başarıyla güncellendi!'
        });
    });
});