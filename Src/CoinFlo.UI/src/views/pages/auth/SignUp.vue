<script setup>
import { useLayout } from '@/layout/composables/layout';
import { ref, computed } from 'vue';
import AppConfig from '@/layout/AppConfig.vue';
import { useToast } from 'primevue/usetoast';

const toast = useToast();
const { layoutConfig } = useLayout();
const email = ref('');
const password = ref('');
const fullName = ref('');

const logoUrl = computed(() => {
    return `/layout/images/${layoutConfig.darkTheme.value ? 'logo-white' : 'logo-dark'}.svg`;
});

function doSignUp() {
    const isFormValid = formDataValidation();

    if (isFormValid === true) {
        toast.add({ severity: 'success', summary: 'Success', detail: 'Signup Successfully Done', life: 3000 });
    } else {
        toast.add({ severity: 'warn', summary: 'Internal Server Error', detail: 'Something Went WWrong. Please Reload & Try Again', life: 3000 });
    }
}

function formDataValidation() {
    if (!fullName.value) {
        toast.add({ severity: 'error', summary: 'Error Message', detail: 'Please Add Your Full Name', life: 3000 });
        return false;
    }
    if (!email.value) {
        toast.add({ severity: 'error', summary: 'Error Message', detail: 'Please Add Your Email', life: 3000 });
        return false;
    }
    if (!password.value) {
        toast.add({ severity: 'error', summary: 'Error Message', detail: 'Please Add Your Account Password', life: 3000 });
        return false;
    }
    return true;
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
                        <span class="text-600 font-medium">Signup Your Account</span>
                    </div>

                    <div>
                        <label for="fullName" class="block text-900 text-xl font-medium mb-2">Name</label>
                        <InputText id="fullName" type="text" placeholder="Full Name" class="w-full md:w-30rem mb-5" style="padding: 1rem" v-model="fullName" />

                        <label for="email1" class="block text-900 text-xl font-medium mb-2">Email</label>
                        <InputText id="email1" type="text" placeholder="Email address" class="w-full md:w-30rem mb-5" style="padding: 1rem" v-model="email" />

                        <label for="password1" class="block text-900 font-medium text-xl mb-2">Password</label>
                        <Password id="password1" v-model="password" placeholder="Password" :toggleMask="true" class="w-full mb-3" inputClass="w-full" :inputStyle="{ padding: '1rem' }"></Password>
                        <Button label="Sign Up" class="w-full p-3 text-xl" @click="doSignUp"></Button>
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
