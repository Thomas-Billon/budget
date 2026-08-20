<script setup lang="ts">

    import { Bank } from '@/enums/Bank';
    import { Currency } from '@/enums/Currency';
    import { getEnumValues } from '@/utils/Enum';
    import { type IAccountRequest } from '@/features/accounts/models/IAccountRequest';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';

    const { isNew, isAutoSave, isLoading, saveAllResult, savePartialResult, deleteResult } = defineProps<FormProps>();
    const model = defineModel<IAccountRequest>({ required: true });
    const emit = defineEmits<FormEmits<IAccountRequest>>();

    const bankOptions = getEnumValues(Bank, false);
    const currencyOptions = getEnumValues(Currency, false);

    // #region Events

    const formEvents = {
        onSaveAll: (data: IAccountRequest) => emit('saveAll', data),
        onSavePartial: (id: number, data: Partial<IAccountRequest>) => emit('savePartial', id, data),
        onDelete: (id: number) => emit('delete', id)
    };

    // #endregion Events

    // #region Form validation

    const isFormValid = (): boolean => {
        return model.value.name.trim().length > 0;
    };

    // #endregion Form validation

</script>

<template>
    <FormBase
        v-slot="{ onChange }"
        v-model="model"
        :is-new="isNew"
        :is-auto-save="isAutoSave"
        :is-loading="isLoading"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-bind="formEvents"
    >

        <div class="row">
            <div class="col-12">
                <div class="form-body card">

                    <div class="row">
                        <div class="col-12">
                            <label class="form-label" for="name">Account Name</label>
                            <input id="name" v-model="model.name" name="Name" type="text" class="form-control" placeholder="e.g. Everyday Account" required @input="onChange('name', model.name);" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 col-md-with-navbar-6">
                            <label class="form-label" for="bank">Bank</label>
                            <select id="bank" v-model="model.bank" name="Bank" class="form-select" @change="onChange('bank', model.bank);">
                                <option :value="Bank.None" disabled selected>Select Bank</option>
                                <option v-for="bank in bankOptions" :key="bank" :value="bank">{{ Bank[bank] }}</option>
                            </select>
                        </div>
                        <div class="col-12 col-md-with-navbar-6">
                            <label class="form-label" for="currency">Currency</label>
                            <select id="currency" v-model="model.currency" name="Currency" class="form-select" @change="onChange('currency', model.currency);">
                                <option :value="Currency.None" disabled selected>Select Currency</option>
                                <option v-for="currency in currencyOptions" :key="currency" :value="currency">{{ Currency[currency] }}</option>
                            </select>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </FormBase>
</template>
