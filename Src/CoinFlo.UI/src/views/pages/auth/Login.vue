<script setup>
import { useLayout } from '@/layout/composables/layout';
import { ref, computed, getCurrentInstance } from 'vue';
import AppConfig from '@/layout/AppConfig.vue';
import Swal from 'sweetalert2';
import axios from 'axios';
import { useRouter } from 'vue-router';

const { layoutConfig } = useLayout();
const email = ref('');
const password = ref('');
const checked = ref(false);
const { proxy } = getCurrentInstance();
const router = useRouter();

const logoUrl = computed(() => {
    return `/layout/images/${layoutConfig.darkTheme.value ? 'logo-white' : 'logo-dark'}.svg`;
});

function doLogin() {
    const isFormValid = loginFormValidation();
    if (isFormValid == true) {
        const loginRequest = {
            email: email.value,
            password: password.value
        };

        axios
            .post(proxy.$BASE_API_URL + 'Auth/Login', loginRequest)
            .then(function (response) {
                if (response.data.status == false) {
                    swalNotificationAlert('Error', response.data.message, 'error', 'OK');
                } else {
                    router.push({ path: '/dashboard' });
                }
            })
            .catch(function (error) {
                if (error.response) {
                    swalNotificationAlert('Error', error.response.data.message, 'error', 'OK');
                } else {
                    swalNotificationAlert('Error', 'Internal Server Error (' + error.message + ')', 'error', 'OK');
                }
            });
        swalNotificationAlert('Authenticating', 'Please wait. System is authenticating', 'information', 'OK');
    }
}

function loginFormValidation() {
    if (!email.value) {
        swalNotificationAlert('Warning', 'Enter Your Email', 'warning', 'OK');
        return false;
    }

    if (!password.value) {
        swalNotificationAlert('Warning', 'Enter Your Password', 'warning', 'OK');
        return false;
    }

    return true;
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
                        <div class="text-900 text-3xl font-medium mb-3">Welcome, CoinFlo!</div>
                        <span class="text-600 font-medium">Sign in to continue</span>
                    </div>

                    <div>
                        <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                        <InputText id="email1" type="text" placeholder="Email address" class="w-full md:w-30rem mb-5" style="padding: 1rem" v-model="email" />

                        <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                        <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true" class="w-full mb-3" inputClass="w-full" :inputStyle="{ padding: '1rem' }"></Password>

                        <div class="flex align-items-center justify-content-between mb-5 gap-5">
                            <div class="flex align-items-center">
                                <Checkbox v-model="checked" id="rememberme1" binary class="mr-2"></Checkbox>
                                <label for="rememberme1">Remember me</label>
                            </div>
                            <a class="font-medium no-underline ml-2 text-right cursor-pointer" style="color: var(--primary-color)">Forgot password?</a>
                        </div>
                        <Button label="Sign In" class="w-full p-3 text-xl" @click="doLogin"></Button>
                        <div class="text-center mt-2">
                            <span class="text-600 font-medium mb-3">Or</span>
                            <br />
                            <router-link to="/auth/signup" class="font-medium no-underline ml-2 text-right cursor-pointer" style="color: var(--primary-color)"> Create New Account </router-link>
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
