<script setup>
import { useLayout } from '@/layout/composables/layout';
import { ref, computed } from 'vue';
import AppConfig from '@/layout/AppConfig.vue';
import Swal from 'sweetalert2';

const { layoutConfig } = useLayout();
const otpCode = ref('');

const logoUrl = computed(() => {
    return `/layout/images/${layoutConfig.darkTheme.value ? 'logo-white' : 'logo-dark'}.svg`;
});

function verifyOTPCode() {
    if (otpCode.value === '1122') {
        swalNotificationAlert('Success', 'OTP Verified Successfully', 'success', 'OK');
    } else {
        swalNotificationAlert('Error', 'Invalid OTP Code', 'error', 'OK');
    }
}

function swalNotificationAlert(txtTitle, txtMessage, txtIcon, txtConfirmButtonText) {
    Swal.fire({
        title: txtTitle,
        text: txtMessage,
        icon: txtIcon,
        confirmButtonText: txtConfirmButtonText
    });
}
</script>

<template>
    <div class="surface-ground flex align-items-center justify-content-center min-h-screen min-w-screen overflow-hidden">
        <div class="flex flex-column align-items-center justify-content-center">
            <img :src="logoUrl" alt="Sakai logo" class="mb-5 w-6rem flex-shrink-0" />
            <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                <div class="w-full surface-card py-8 px-5 sm:px-8" style="border-radius: 53px">
                    <div class="text-center mb-5">
                        <img src="/demo/images/login/avatar.png" alt="Image" height="50" class="mb-3" />
                        <div class="text-900 text-3xl font-medium mb-3">Verify Your OTP Code</div>
                        <span class="text-600 font-medium">We send a OTP code in you mail. Check & verify it</span>
                    </div>

                    <div>
                        <InputText id="otpCode" type="text" placeholder="OTP Code" class="w-full md:w-30rem mb-5" style="padding: 1rem" v-model="otpCode" />
                        <Button label="Verify" class="w-full p-3 text-xl" @click="verifyOTPCode"></Button>
                        <div class="text-center mt-2">
                            <span class="text-600 font-medium mb-3">Or</span>
                            <br />
                            <router-link to="/auth/login" class="font-medium no-underline ml-2 text-right cursor-pointer" style="color: var(--primary-color)"> Go Back To Login </router-link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <AppConfig simple />
</template>

<style scoped>
.pi-eye {
    transform: scale(1.6);
    margin-right: 1rem;
}

.pi-eye-slash {
    transform: scale(1.6);
    margin-right: 1rem;
}
</style>
