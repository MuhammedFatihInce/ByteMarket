$(document).ready(function () {
    const roleModal = new bootstrap.Modal(document.getElementById('assignRoleModal'));

    $('.btn-manage-role').on('click', function () {
        const userId = $(this).data('id');
        const userName = $(this).data('username');
        const currentRoles = $(this).data('current-roles').split(',');

        $('#modalUserId').val(userId);
        $('#modalUserName').text(userName);

        $('.role-checkbox').prop('checked', false);
        currentRoles.forEach(roleName => {
            $(`.role-checkbox[value="${roleName}"]`).prop('checked', true);
        });

        roleModal.show();
    });

    $('#btnSaveRoles').on('click', async function () {
        const userId = $('#modalUserId').val();
        const selectedRoles = [];

        $('.role-checkbox:checked').each(function () {
            selectedRoles.push($(this).val());
        });

        const data = {
            UserId: userId,
            Roles: selectedRoles
        };

        try {
            const response = await CustomAjax.post('/User/AssignRole', data);

            if (response && response.success) {
                roleModal.hide();
                Alert.toast({ title: response.message, icon: 'success' });

                setTimeout(() => location.reload(), 1500);
            }
        } catch (error) {
            console.error("Rol atama hatası:", error);
        }
    });
});