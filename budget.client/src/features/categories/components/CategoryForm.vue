<script setup lang="ts">

    import './CategoryForm.scss';

    import { onMounted, ref } from 'vue';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { CategoryColor } from '@/enums/CategoryColor.ts';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import { type ICategoryOptionsResponse, type ICategoryOptionsItemResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import FormBase from '@/components/form-base/FormBase.vue';
    import type { FormEmits, FormProps } from '@/utils/Form';

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

        apiCall<undefined, ICategoryOptionsResponse>(`category/options`, { method: 'GET' })
            .then(response => {
                categoryOptions.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
    }

    // #endregion Category options

    // #region Check form

    const isFormValid = (): boolean => {
        return model.value.name.trim().length > 0;
    };

    // #endregion Check form

</script>

<template>
    <FormBase
        :is-new="isNew"
        :save-all-result="saveAllResult"
        :save-partial-result="savePartialResult"
        :delete-result="deleteResult"
        :is-form-valid="isFormValid"
        v-model="model"
        @save-all="(data) => emit('saveAll', data)"
        @save-partial="(id, data) => emit('savePartial', id, data)"
        @delete="(id) => emit('delete', id)">

        <template #body="{ onChange }">
            <div class="category-form-body">

                <input class="form-control form-control-lg" type="text" id="category-name" name="Name" v-model="model.name" placeholder="Name" required @input="onChange('name', model.name);" />

                <select class="form-select form-select-lg" id="category-color" name="Color" v-model="model.color" @input="onChange('color', model.color);">
                    <option :value="CategoryColor.None" disabled selected>Select Color</option>
                    <option :value="CategoryColor.Blue">{{ CategoryColor[CategoryColor.Blue] }}</option>
                    <option :value="CategoryColor.Green">{{ CategoryColor[CategoryColor.Green] }}</option>
                    <option :value="CategoryColor.Yellow">{{ CategoryColor[CategoryColor.Yellow] }}</option>
                    <option :value="CategoryColor.Orange">{{ CategoryColor[CategoryColor.Orange] }}</option>
                    <option :value="CategoryColor.Red">{{ CategoryColor[CategoryColor.Red] }}</option>
                </select>

                <select v-if="!isNew" class="form-select form-select-lg" id="category-parent-category-id" name="ParentCategoryId" v-model="model.parentCategoryId" @input="onChange('parentCategoryId', model.parentCategoryId);">
                    <option :value="null" selected>No parent category</option>
                    <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
                </select>

                <input v-if="isNew" type="hidden" id="category-parent-category-id" name="ParentCategoryId" v-model="model.parentCategoryId" />

            </div>
        </template>

    </FormBase>
</template>
