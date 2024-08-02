<script setup>
import { useLayout } from '@/layout/composables/layout';
import { ref, computed, getCurrentInstance } from 'vue';
import AppConfig from '@/layout/AppConfig.vue';
import Swal from 'sweetalert2';
import axios from 'axios';
import { useRouter } from 'vue-router';

const { layoutConfig } = useLayout();
const { proxy } = getCurrentInstance();
const router = useRouter();
const otpCode = ref('');

const logoUrl = computed(() => {
    return `/layout/images/${layoutConfig.darkTheme.value ? 'logo-white' : 'logo-dark'}.svg`;
});

const userEmail = localStorage.getItem('userEmail') || '';

function verifyOTPCode() {
    if (otpCode.value) {
        const otpVerificationParam = {
            otpCode: otpCode.value,
            userEmail: userEmail
        };

        axios.post(proxy.$BASE_API_URL + 'Auth/OTP-Verification', otpVerificationParam).then(function (response) {
            console.log(response);
            if (response.data.status === true) {
                swalNotificationAlert('Success', response.data.message, 'success', 'OK');
                router.push({ path: '/' });
            } else {
                swalNotificationAlert('Faild', response.data.message, 'error', 'OK');
            }
        });
    } else {
        swalNotificationAlert('Warning', 'Enter OTP Code', 'warning', 'OK');
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
                        <div class="text-900 text-3xl font-medium mb-3">OTP Verification</div>
                        <span class="text-600 font-medium">We sent a OTP code in you email. Please check & verify it</span>
                    </div>

                    <div>
                        <InputText id="otpCode" v-model="otpCode" placeholder="Enter OTP Code" :toggleMask="true" class="w-full mb-3" inputClass="w-full" :inputStyle="{ padding: '1rem' }" style="padding: 1rem" />
                        <Button label="Verify OTP" class="w-full p-3 text-xl" @click="verifyOTPCode"></Button>
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
