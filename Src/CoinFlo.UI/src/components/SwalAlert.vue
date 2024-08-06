<script>
import Swal from 'sweetalert2';

export default {
    name: 'SwalAlert',
    methods: {
        swalNotificationAlert(txtTitle, txtMessage, txtIcon, txtConfirmButtonText) {
            return Swal.fire({
                title: txtTitle,
                text: txtMessage,
                icon: txtIcon,
                confirmButtonText: txtConfirmButtonText
            });
        },

        swalConfirmationAlert(txtTitle, txtMessage, confirmButtonText, cancelButtonText) {
            return Swal.fire({
                title: txtTitle,
                text: txtMessage,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: confirmButtonText,
                cancelButtonText: cancelButtonText
            });
        }
    }
};
</script>

<template>
    <div>
        <!-- NO UI -->
    </div>
</template>
