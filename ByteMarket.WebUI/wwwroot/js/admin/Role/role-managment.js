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


function formatUser(user) {
    if (!user.id) return user.text;

    var initial = user.text.charAt(0).toUpperCase();

    var isChecked = user.selected ? 'checked' : '';
;

    var $container = $(
        '<div class="d-flex align-items-center justify-content-between py-2 w-100">' +
            '<div class="d-flex align-items-center">' +
                '<div class="rounded-circle bg-light d-flex align-items-center justify-content-center me-3" ' +
                     'style="width: 38px; height: 38px; border: 1px solid #eee; font-weight: bold; color: #ff6600;">' +
                      initial +
                  '</div>' +
                  '<div class="d-flex flex-column">' +
                    '<span class="fw-bold text-dark">' + user.text + '</span>' +
                    '<small class="text-muted">' + (user.email || "") + '</small>' +
                '</div>' +
            '</div>' +
            '' +
            '<div class="form-check form-switch ms-auto">' +
        '<input class="form-check-input custom-switch user-switch-' + user.id + '" type="checkbox" ' + isChecked + ' style="pointer-events: none;">' +
            '</div>' +
        '</div>'
    );
    return $container;
}

function formatUserSelection(user) {
    if (!user.id) return user.text;
    return $('<span>' + user.text + '</span>');
}


let isConfirming = false;

async function updateUserRole(roleName, userId, isAdding) {

    isConfirming = true;

    const data = {
        UserId: userId,
        RoleName: roleName,
        IsAdding: isAdding
    };

    return new Promise((resolve) => {

        Alert.fire({
            title: 'Yetki güncellenecek!',
            text: `Kullanıcı yetkisini değiştirmek istediğinize emin misiniz?`,
            icon: 'warning',
            confirmButtonText: 'Evet, Güncelle',
            onConfirm: async () => {
                try {
                    const response = await CustomAjax.post('/Admin/Role/AssignRole', data);

                    if (response && response.success) {
                        Alert.toast({ title: response.message, icon: 'success' });
                        resolve(true);

                    } else {
                        Alert.error({ title: "Hata", text: response.message });
                        resolve(false);
                    }
                } catch (error) {
                    Alert.error({ title: "Hata", text: "Bağlantı kurulamadı!" });
                    resolve(false);
                } finally {
                    isConfirming = false;
                }
            },
            onCancel: () => {
                resolve(false);
            },
            didClose: () => {
                isConfirming = false; 
                console.log("Kilit kaldırıldı!");
            },
            successMessage: 'Yetkiler başarıyla güncellendi!'
        });


    });

    
};

async function openUserAuthorizeModal(roleId, roleName) {

    $('#createUserAuthorizeModalLabel').html(`<i class="bi bi-people me-2 text-ty-orange"></i>Kullanıcıya ${roleName} Rolünü Tanımla`);


    const $selectElement = $('.select2-users');

    $selectElement.off('select2:selecting select2:closing');

    if ($selectElement.data('select2')) {
        $selectElement.empty();
        $selectElement.select2('destroy');
    }


    $selectElement.select2({
        theme: 'bootstrap-5',
        dropdownParent: $('#createUserAuthorizeModal'),
        width: '100%',
        placeholder: "Kullanıcı ismine göre arayın...",
        ajax: {
            url: '/Admin/Role/GetAllUsersForSelect',
            dataType: 'json',
            delay: 250, 
            data: function (params) {
                return { q: params.term || '' };
            },
            processResults: function (data) {

                var targetRoleId = roleId.toLowerCase();

                var result = data.map(function (item) {
                    var alreadyHasRole = false;

                    if (item.roleIds && Array.isArray(item.roleIds)) {
                        alreadyHasRole = item.roleIds.some(r => r.toLowerCase() === targetRoleId);
                    }

                    return {
                        id: item.id.toLowerCase(),
                        text: item.text,
                        email: item.email,
                        selected: alreadyHasRole
                    };

                });

                return { results: result };
            },
            cache: false
        },
        closeOnSelect: false,
        templateResult: formatUser, 
        templateSelection: formatUserSelection,
        minimumInputLength: 2
    });

    console.log(isConfirming);

    $selectElement.on('select2:closing', function (e) {
        if (isConfirming) {
            e.preventDefault();
        }
    });

    $selectElement.on('select2:selecting', async function (e) {

        if (isConfirming) {
            e.preventDefault();
            return;
        }

        var data = e.params.args.data;
        const userId = data.id;

        e.preventDefault();

        const isSuccess = await updateUserRole(roleName, userId, !data.selected);

        if (isSuccess) {
            data.selected = !data.selected;

            $('.user-switch-' + userId).prop('checked', data.selected);

            var currentValues = $(this).val() || [];
            if (data.selected) {
                currentValues.push(userId);
            } else {
                currentValues = currentValues.filter(v => v.toLowerCase() !== userId.toLowerCase());
            }
            $(this).val(currentValues).trigger('change.select2');
            
        }
    });


    $('#createUserAuthorizeModal').modal('show');
}

$('#createUserAuthorizeModal').on('hidden.bs.modal', function () {
    const $select = $('.select2-users');
    $select.off('select2:selecting select2:closing');

    if ($select.data('select2')) {
        $select.select2('destroy');
        $select.empty();
    }

    location.reload();
});


