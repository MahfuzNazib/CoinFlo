<script setup>
import { ref } from 'vue';

const firstName = ref(null);
const lastName = ref(null);
const countryCode = ref(null);
const countryCodes = ref([
    { name: 'New York', code: 'NY' },
    { name: 'Rome', code: 'RM' },
    { name: 'London', code: 'LDN' },
    { name: 'Istanbul', code: 'IST' },
    { name: 'Paris', code: 'PRS' }
]);
const dailyCashInOutReminder = ref(null);
const monthlyBudgetPlanner = ref(null);
const autoPaymentDays = ref([{ day: '01' }, { day: '02' }, { day: '03' }, { day: '04' }, { day: '05' }]);
const autoPaymentDay = ref(null);
</script>
<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>Settings</h5>
                <div class="grid">
                    <div class="col">
                        <h2>Test1</h2>
                    </div>
                    <div class="col">
                        <h2>Test 2</h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-12 lg:col-6">
            <!-- User Profile Start -->
            <div class="card">
                <h5>Profile</h5>
                <div class="grid p-fluid mt-3">
                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <InputText type="text" id="firstName" v-model="firstName" />
                            <label for="firstName">First Name</label>
                        </FloatLabel>
                    </div>
                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <InputText type="text" id="lastName" v-model="lastName" />
                            <label for="lastName">Last Name</label>
                        </FloatLabel>
                    </div>

                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <InputText type="text" id="lastName" v-model="lastName" />
                            <label for="lastName">Phone No</label>
                        </FloatLabel>
                    </div>
                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <InputText type="text" id="lastName" v-model="lastName" />
                            <label for="lastName">Gmail</label>
                        </FloatLabel>
                    </div>

                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <Dropdown id="dropdown" :options="countryCodes" v-model="countryCode" optionLabel="name"></Dropdown>
                            <label for="dropdown">Country Code</label>
                        </FloatLabel>
                    </div>
                    <div class="field col-12 md:col-6">
                        <FloatLabel>
                            <Dropdown id="dropdown" :options="countryCodes" v-model="countryCode" optionLabel="name"></Dropdown>
                            <label for="dropdown">Currency</label>
                        </FloatLabel>
                    </div>
                </div>
            </div>
            <!-- User Profile End -->
        </div>
        <div class="col-12 lg:col-6">
            <div class="card">
                <h5>Configuration</h5>
                <br />
                <div class="grid">
                    <div class="col-12 lg:col-8">
                        <label>Daily Cash In/Out Reminder</label>
                    </div>
                    <div class="col-12 lg:col-4">
                        <InputSwitch v-model="dailyCashInOutReminder" />
                    </div>
                </div>
                <div class="grid">
                    <div class="col-12 lg:col-8">
                        <label>Monthly Budget Planner</label>
                    </div>
                    <div class="col-12 lg:col-4">
                        <InputSwitch v-model="monthlyBudgetPlanner" />
                    </div>
                </div>

                <div class="grid">
                    <div class="col-12 lg:col-8">
                        <label>Enable Auto Payment</label>
                    </div>
                    <div class="col-12 lg:col-4">
                        <InputSwitch v-model="monthlyBudgetPlanner" />
                    </div>
                </div>

                <div class="grid">
                    <div class="col-12 lg:col-8">
                        <label>Adjust Auto Payment in Budget</label>
                    </div>
                    <div class="col-12 lg:col-4">
                        <InputSwitch v-model="monthlyBudgetPlanner" />
                    </div>
                </div>

                <div class="grid">
                    <div class="col-12 lg:col-8">
                        <label>Auto Payment Day</label>
                    </div>
                    <div class="col-12 lg:col-4">
                        <Dropdown v-model="autoPaymentDay" :options="autoPaymentDays" optionLabel="day" placeholder="Select" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
