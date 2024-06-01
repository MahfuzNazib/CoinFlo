<script setup>
import { ref, onBeforeMount, reactive } from 'vue';
import { FilterMatchMode, FilterOperator } from 'primevue/api';
import { CustomerService } from '@/service/CustomerService';
import { ProductService } from '@/service/ProductService';
import Dialog from 'primevue/dialog';
import { useToast } from 'primevue/usetoast';

const toast = useToast();
const visible = ref(false);
const customer1 = ref(null);
const customer2 = ref(null);
const customer3 = ref(null);
const filters1 = ref(null);
const loading1 = ref(null);
const loading2 = ref(null);
const products = ref(null);
const categoryName = ref(null);
const transactionTypeValue = ref(null);
const statuses = reactive(['unqualified', 'qualified', 'new', 'negotiation', 'renewal', 'proposal']);
const customerService = new CustomerService();
const productService = new ProductService();
const getSeverity = (status) => {
    switch (status) {
        case 'unqualified':
            return 'danger';

        case 'qualified':
            return 'success';

        case 'new':
            return 'info';

        case 'negotiation':
            return 'warning';

        case 'renewal':
            return null;
    }
};

onBeforeMount(() => {
    productService.getProductsWithOrdersSmall().then((data) => (products.value = data));
    customerService.getCustomersLarge().then((data) => {
        customer1.value = data;
        loading1.value = false;
        customer1.value.forEach((customer) => (customer.date = new Date(customer.date)));
    });
    customerService.getCustomersLarge().then((data) => (customer2.value = data));
    customerService.getCustomersMedium().then((data) => (customer3.value = data));
    loading2.value = false;

    initFilters1();
});

const initFilters1 = () => {
    filters1.value = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
        'country.name': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
        representative: { value: null, matchMode: FilterMatchMode.IN },
        date: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.DATE_IS }] },
        balance: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] },
        status: { operator: FilterOperator.OR, constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] },
        activity: { value: [0, 100], matchMode: FilterMatchMode.BETWEEN },
        verified: { value: null, matchMode: FilterMatchMode.EQUALS }
    };
};

const formatDate = (value) => {
    return value.toLocaleDateString('en-US', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
};

const transactionTypeValues = ref([
    { name: 'Cash In', code: 'CASH_IN' },
    { name: 'Cash Out', code: 'CASH_OUT' }
]);

function saveCategory() {
    const validateForm = formValidation();
    if (validateForm === true) {
        toast.add({ severity: 'success', summary: 'Success', detail: 'New Record Saved Successfully', life: 3500 });
    }
}

function formValidation() {
    if (!categoryName.value || categoryName.value === null || categoryName.value === '') {
        visible.value = true;
        toast.add({ severity: 'error', summary: 'Error Message', detail: 'Category Name Cannot be Blank', life: 3000 });
        return false;
    } else if (!transactionTypeValue.value || transactionTypeValue.value === null || transactionTypeValue.value === '') {
        visible.value = true;
        toast.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please Select Type Value', life: 3000 });
        return false;
    } else {
        visible.value = false;
        resetFormModalValues();
        return true;
    }
}

function resetFormModalValues() {
    categoryName.value = null;
    transactionTypeValue.value = null;
}
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>Categories</h5>
                <Dialog v-model:visible="visible" modal header="Add New Category" :style="{ width: '28rem' }">
                    <span class="p-text-danger block mb-5">*Add New Category with Name & Transaction Type.</span>
                    <div class="flex align-items-center gap-3 mb-3">
                        <label for="name" class="font-semibold w-6rem">Name</label>
                        <InputText id="name" class="flex-auto" v-model="categoryName" autocomplete="off" />
                    </div>
                    <div class="flex align-items-center gap-3 mb-5">
                        <label for="type" class="font-semibold w-6rem">Type</label>
                        <Dropdown id="type" class="flex-auto" v-model="transactionTypeValue" :options="transactionTypeValues" optionLabel="name" placeholder="Select" />
                    </div>
                    <div class="flex justify-content-end gap-2">
                        <Button type="button" label="Cancel" severity="secondary" @click="visible = false"></Button>
                        <Button type="button" label="Save" @click="saveCategory"></Button>
                    </div>
                </Dialog>
                <DataTable
                    :value="customer1"
                    :paginator="true"
                    :rows="10"
                    dataKey="id"
                    :rowHover="true"
                    v-model:filters="filters1"
                    filterDisplay="menu"
                    :loading="loading1"
                    :filters="filters1"
                    :globalFilterFields="['name', 'country.name', 'representative.name', 'balance', 'status']"
                    showGridlines
                >
                    <template #header>
                        <div class="flex justify-content-between flex-column sm:flex-row">
                            <Button label="Add New" icon="pi pi-plus" @click="visible = true" />
                            <IconField iconPosition="left">
                                <InputIcon class="pi pi-search" />
                                <InputText v-model="filters1['global'].value" placeholder="Keyword Search" style="width: 100%" />
                            </IconField>
                        </div>
                    </template>
                    <template #empty> No categories found. </template>
                    <template #loading> Loading category data. Please wait. </template>
                    <Column field="name" header="Name" style="min-width: 12rem">
                        <template #body="{ data }">
                            {{ data.name }}
                        </template>
                        <template #filter="{ filterModel }">
                            <InputText type="text" v-model="filterModel.value" class="p-column-filter" placeholder="Search by name" />
                        </template>
                    </Column>
                    <Column header="Transaction Type" filterField="date" dataType="date" style="min-width: 10rem">
                        <template #body="{ data }">
                            {{ formatDate(data.date) }}
                        </template>
                        <template #filter="{ filterModel }">
                            <Calendar v-model="filterModel.value" dateFormat="mm/dd/yy" placeholder="mm/dd/yyyy" />
                        </template>
                    </Column>
                    <Column field="status" header="Status" :filterMenuStyle="{ width: '14rem' }" style="min-width: 12rem">
                        <template #body="{ data }">
                            <Tag :severity="getSeverity(data.status)">{{ data.status.toUpperCase() }}</Tag>
                        </template>
                        <template #filter="{ filterModel }">
                            <Dropdown v-model="filterModel.value" :options="statuses" placeholder="Any" class="p-column-filter" :showClear="true">
                                <template #value="slotProps">
                                    <Tag :severity="getSeverity(slotProps.value)" v-if="slotProps.value">{{ slotProps.value }} </Tag>
                                    <span v-else>{{ slotProps.placeholder }}</span>
                                </template>
                                <template #option="slotProps">
                                    <Tag :severity="getSeverity(slotProps.option)">{{ slotProps.option.toUpperCase() }}</Tag>
                                </template>
                            </Dropdown>
                        </template>
                    </Column>
                </DataTable>
            </div>
        </div>
    </div>
</template>

<style scoped lang="scss">
:deep(.p-datatable-frozen-tbody) {
    font-weight: bold;
}

:deep(.p-datatable-scrollable .p-frozen-column) {
    font-weight: bold;
}
</style>
