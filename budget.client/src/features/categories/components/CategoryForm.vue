<script setup lang="ts">

    import './CategoryForm.scss';

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { CategoryColor } from '@/enums/CategoryColor.ts';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import { type ICategoryOptionsResponse, type ICategoryOptionsItemResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import FormBase from '@/components/form-base/FormBase.vue';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';

    const { isNew, saveAllResult, savePartialResult, deleteResult } = defineProps<FormProps>();
    const model = defineModel<ICategoryRequest>({ required: true });
    const emit = defineEmits<FormEmits<ICategoryRequest>>();

    const categoryOptions = ref<ICategoryOptionsItemResponse[]>([]);

    // #region Init

    onMounted(() => {
        getCategoryOptions();
    });

    // #endregion Init

    // #region Category options

    const getCategoryOptions = (): void => {
        if (isNew) {
            return;
        }

        apiCall<undefined, ICategoryOptionsResponse>('category/options', { method: 'GET' })
            .then(response => {
                if (response.isSuccess) {
                    categoryOptions.value = response.data.items;
                }
                // TODO: Handle error in else case
            });
    };

    // #endregion Category options

    // #region Events

    const formEvents = {
        onSaveAll: (data: ICategoryRequest) => emit('saveAll', data),
        onSavePartial: (id: number, data: Partial<ICategoryRequest>) => emit('savePartial', id, data),
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
        v-model="model"
        :is-new="isNew"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-bind="formEvents"
    >

        <template #body="{ onChange }">
            <div class="form-body">

                <input v-model="model.name" name="Name" type="text" class="form-control form-control-lg" placeholder="Name" required @input="onChange('name', model.name);" />

                <select v-model="model.color" name="Color" class="form-select form-select-lg" @input="onChange('color', model.color);">
                    <option :value="CategoryColor.None" disabled selected>Select Color</option>
                    <option :value="CategoryColor.Blue">{{ CategoryColor[CategoryColor.Blue] }}</option>
                    <option :value="CategoryColor.Green">{{ CategoryColor[CategoryColor.Green] }}</option>
                    <option :value="CategoryColor.Yellow">{{ CategoryColor[CategoryColor.Yellow] }}</option>
                    <option :value="CategoryColor.Orange">{{ CategoryColor[CategoryColor.Orange] }}</option>
                    <option :value="CategoryColor.Red">{{ CategoryColor[CategoryColor.Red] }}</option>
                </select>

                <select v-if="!isNew" v-model="model.parentCategoryId" name="ParentCategoryId" class="form-select form-select-lg" @input="onChange('parentCategoryId', model.parentCategoryId);">
                    <option :value="null" selected>No parent category</option>
                    <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
                </select>

                <input v-if="isNew" v-model="model.parentCategoryId" name="ParentCategoryId" type="hidden" />

            </div>
        </template>

    </FormBase>
</template>
