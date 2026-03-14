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

async function updateUserRolesBulk(roleName, changes) {

    const data = {
        RoleName: roleName,
        Changes: changes.map(c => ({
            UserId: c.userId,
            IsAdding: c.status
        }))
    };

    try {
        const response = await CustomAjax.post('/Admin/Role/AssignRole', data);

        if (response && response.success) {
            Alert.toast({ title: response.message, icon: 'success' });
            return true;
        } else {
            Alert.toast({ title: response.message, icon: 'error' });
            return false;
        }
    } catch (error) {
        Alert.toast({ title: "Sunucuyla bağlantı kurulamadı!", icon: 'error' });
        return false;
    }

    
};

async function getUserByRole(roleName) {

    try {
        const response = await CustomAjax.get(`/Admin/Role/GetAllUsersWithRolesAsync/${roleName}`);

        if (response && response.success) {
            return response.data;

        } else {
            Alert.toast({ title: "Hata", icon: 'error' });
            return null;
        }
    } catch (error) {
        Alert.toast({ title: "Hata", icon: 'error' });
        return null;
    } 


};
function createUserRow(user) {
    const templateHtml = $('#user-row-template').html();
    const $row = $(templateHtml);

    $row.find('.user-namesurname').text(user.nameSurname || '---');
    $row.find('.user-id').text(`ID: ${user.id}`);
    $row.find('.user-username').text(user.userName);
    $row.find('.user-email').text(user.email);

    const $rolesContainer = $row.find('.user-roles');

    $.each(user.roles || [], function (i, role) {
        const isAdmin = role === "Admin";
        const $badge = $('<span></span>')
            .addClass(`badge bg-opacity-10 border border-opacity-25 px-2 py-1 me-1`)
            .addClass(isAdmin ? 'bg-danger text-danger border-danger' : 'bg-primary text-primary border-primary')
            .text(role);
        $rolesContainer.append($badge);
    });

    $row.attr('id', `user-row-${user.id}`);
    return $row;
}

async function refreshUserTable(roleName) {
    const $tbody = $('#user-table-body');
    const roleTitle = `<i class="bi bi-people-fill me-2 text-ty-orange"></i>${roleName} Rolü Listesi`;

    $('#tableUsersListTitle').html(roleTitle);

    await $tbody.fadeOut(200).promise();

    const users = await getUserByRole(roleName);

    if (!users) {
        $tbody.fadeIn(200);
        return;
    }

    $tbody.empty();

    if (users.length === 0) {
        $tbody.append('<tr><td colspan="4" class="text-center py-4">Kullanıcı bulunamadı.</td></tr>');
        $tbody.fadeIn(400);
        return;
    }

    let rows = [];
    $.each(users, function (index, user) {
        const $row = createUserRow(user);
        $row.hide(); 
        rows.push($row);
    });

   
    $tbody.append(rows);

    
    $tbody.show(); 

   
    $tbody.find('tr').each(function (i) {
        $(this).delay(i * 30).fadeIn(400);
    });
}

let pendingChanges = {};
function refreshPendingChangesTable() {
    const $body = $('#pending-changes-body');
    const $area = $('#pendingChangesArea');
    const templateHtml = $('#pending-row-template').html();
    const changes = Object.values(pendingChanges);

    if (changes.length === 0) {
        $area.fadeOut(200, function () { $(this).addClass('d-none'); });
        return;
    }

    $area.removeClass('d-none').fadeIn(200);
    $('#pendingCount').text(changes.length);
    $body.empty();

    let rows = [];

    $.each(changes, function (index, user) {
        const $row = $(templateHtml);

        $row.find('.pending-namesurname').text(user.nameSurname || '---');
        $row.find('.pending-id').text(`ID: ${user.userId}`);
        $row.find('.pending-username').text(user.userName);
        $row.find('.pending-email').text(user.email);

        $row.addClass(user.status ? 'table-success' : 'table-danger');

        const isAdding = user.status;
        const $badge = $('<span></span>')
            .addClass(`badge bg-opacity-10 border border-opacity-25 px-2 py-1`)
            .addClass(isAdding ? 'bg-success text-success border-success' : 'bg-danger text-danger border-danger')
            .html(`<i class="bi ${isAdding ? 'bi-plus-circle' : 'bi-dash-circle'} me-1"></i> ${isAdding ? 'Role Ekle' : 'Rolden Çıkar'}`);

        $row.find('.pending-status-container').append($badge);

        $row.hide();
        rows.push($row);
    });

    $body.append(rows);
    $body.find('tr').fadeIn(300);
}

async function openUserAuthorizeModal(roleId, roleName) {
    $('#createUserAuthorizeModal').data('active-role-name', roleName);
    pendingChanges = {};

    await refreshUserTable(roleName);

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
                        userName: item.userName,
                        roles: item.roleNames,
                        selected: alreadyHasRole,
                        wasSelected: alreadyHasRole
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


        data.selected = !data.selected;

        if (data.selected === data.wasSelected) {
            delete pendingChanges[userId];
        } else {
            
            pendingChanges[userId] = {
                userId: userId,
                status: data.selected, 
                userName: data.userName,
                nameSurname: data.text,
                email: data.email,
                roles: data.roles || []
            };
        }

        refreshPendingChangesTable();

        $('.user-switch-' + userId).prop('checked', data.selected);

        var currentValues = $(this).val() || [];
        if (data.selected) {
            currentValues.push(userId);
        } else {
            
            currentValues = currentValues.filter(v => v.toLowerCase() !== userId.toLowerCase());
        }
        $(this).val(currentValues).trigger('change.select2');

       
    });


    $('#createUserAuthorizeModal').modal('show');
}

$(document).ready(function () {

    $('#btnUserRoles').off('click').on('click', async function () {
        const changesArray = Object.values(pendingChanges);
        const activeRoleName = $('#createUserAuthorizeModal').data('active-role-name');

        if (changesArray.length === 0) {
            Alert.toast({ title: "Herhangi bir değişiklik yapmadınız.", icon: 'warning' });
            return;
        }

        Alert.fire({
            title: 'Emin misiniz?',
            text: `${changesArray.length} adet yetkilendirme işlemi tek seferde uygulanacaktır.`,
            icon: 'question',
            confirmButtonText: 'Evet, Onayla',
            onConfirm: async () => {
                isConfirming = true;
                $(this).prop('disabled', true).html('<span class="spinner-border spinner-border-sm"></span> İşleniyor...');

                const isSuccess = await updateUserRolesBulk(activeRoleName, changesArray);

                if (isSuccess) {
                    pendingChanges = {};
                    refreshPendingChangesTable();
                    await refreshUserTable(activeRoleName);
                }

                isConfirming = false;
                $(this).prop('disabled', false).text('Yetkiyi Tanımla');
            }
        });
    });


    $('#createUserAuthorizeModal').on('hidden.bs.modal', function () {
        pendingChanges = {};
        refreshPendingChangesTable();
        const $select = $('.select2-users');
        $select.off('select2:selecting select2:closing');

        if ($select.data('select2')) {
            $select.select2('destroy');
            $select.empty();
        }
    });
});





