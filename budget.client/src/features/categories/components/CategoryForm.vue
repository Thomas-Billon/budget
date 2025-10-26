<script setup lang="ts">

    import './CategoryForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { apiCall } from '@/utils/ApiCall.ts';
    import { CategoryColor } from '@/enums/CategoryColor.ts';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import { type ICategoryOptionsResponse, type ICategoryOptionsItemResponse } from '@/features/categories/models/ICategoryOptionsResponse';
    import { type ApiCallResult } from '@/utils/ApiCall';

    interface Props {
        isNew: boolean;
        saveAllResult?: ApiCallResult;
        savePartialResult?: ApiCallResult;
        deleteResult?: ApiCallResult;
    };

    type Emits = {
        saveAll: [data: ICategoryRequest];
        savePartial: [id: number, data: Partial<ICategoryRequest>];
        delete: [id: number];
    };

    const { isNew, saveAllResult, savePartialResult, deleteResult } = defineProps<Props>();
    const model = defineModel<ICategoryRequest>({ required: true });
    const emit = defineEmits<Emits>();

    const submitLabel = ref<string>('');
    const deleteLabel = ref<string>('');
    const areButtonsDisabled = ref<boolean>(false);

    const categoryOptions = ref<ICategoryOptionsItemResponse[]>([]);

    let partialModel: Partial<ICategoryRequest> = {};

    // Init
    onMounted(() => {
        getCategoryOptions();
        setButtonsToDefaultState();
    });

    // On props change
    watch(() => [saveAllResult, savePartialResult, deleteResult], ([newSaveAll, newSavePartial, newDelete]) => {
        if (newSaveAll !== undefined && !newSaveAll.isSuccess) {
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else if (newSavePartial !== undefined && !newSavePartial.isSuccess || isNew) {
            setButtonsToDefaultState();
        }
        else if (newDelete !== undefined && !newDelete.isSuccess) {
            setDeleteButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else {
            setSubmitButtonToSavedState();
            waitAndResetButtonsToDefaultState();
        }
    });

    // On form submit
    const onSubmit = (): void => {
        saveAll(model.value);
        disableButtons();
    }

    // On delete button click
    const onDelete = (): void => {
        emitDelete(model.value.id);
        disableButtons();
    }

    // #region Partial update

    // On all form fields input event (any text modification)
    const onFormFieldInput = <T extends keyof ICategoryRequest>(event: Event, fieldName: T): void => {
        const target = event.target as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null;
        if (target === null) {
            return;
        }

        setButtonsToDefaultState();
        fillPartialModel(fieldName, target.value as ICategoryRequest[T]);
    }

    const fillPartialModel = <T extends keyof ICategoryRequest>(fieldName: T, value: ICategoryRequest[T]): void => {
        if (isNew) {
            return;
        }

        partialModel[fieldName] = value;
        debounceSavePartial();
    };

    // #endregion Partial update

    // #region Options

    const getCategoryOptions = (): void => {
        if (!isNew) {
            apiCall<undefined, ICategoryOptionsResponse>(`category/options`, { method: 'GET' })
            .then(response => {
                categoryOptions.value = response.items;
            })
            .catch(() => {
                // TODO: Add error
            });
        }
    }

    // #endregion Options

    // #region Buttons

    const setButtonsToDefaultState = (): void => {
        submitLabel.value = isNew ? 'Add category' : 'Edit category';
        deleteLabel.value = 'Delete category';
        enableButtons();
    }

    const setSubmitButtonToSavedState = (): void => {
        submitLabel.value = 'Saved';
        disableButtons();
    }

    const setSubmitButtonToErrorState = (): void => {
        submitLabel.value = 'Error';
        enableButtons();
    }

    const setDeleteButtonToErrorState = (): void => {
        deleteLabel.value = 'Error';
        enableButtons();
    }

    const disableButtons = (): void => {
        areButtonsDisabled.value = true;
    }

    const enableButtons = (): void => {
        areButtonsDisabled.value = false;
    }

    const waitAndResetButtonsToDefaultState = debounce(setButtonsToDefaultState, 5000);

    // #endregion Buttons

    // #region Emits

    const saveAll = (data: ICategoryRequest) => emit('saveAll', data);
    const savePartial = (id: number, data: Partial<ICategoryRequest>) => emit('savePartial', id, data);
    const emitDelete = (id: number) => emit('delete', id);

    const debounceSavePartial = debounce(() => {
        if (model.value.id === undefined || Object.keys(partialModel).length === 0) {
            return;
        }

        savePartial(model.value.id, partialModel);
        partialModel = {};
    }, 1000);

    // #endregion Emits

</script>

<template>
    <form class="category-form section-container-grow container" @submit.prevent="onSubmit">

        <input type="hidden" id="category-id" name="Id" v-model="model.id" />

        <div class="category-form-body">

            <input class="form-control form-control-lg" type="text" id="category-name" name="Name" v-model="model.name" placeholder="Name" required @input="onFormFieldInput($event, 'name');" />

            <select class="form-select form-select-lg" id="category-color" name="Color" v-model="model.color" @input="onFormFieldInput($event, 'color');">
                <option :value="undefined" disabled selected>Select Color</option>
                <option :value="CategoryColor.Blue">{{ CategoryColor[CategoryColor.Blue] }}</option>
                <option :value="CategoryColor.Green">{{ CategoryColor[CategoryColor.Green] }}</option>
                <option :value="CategoryColor.Yellow">{{ CategoryColor[CategoryColor.Yellow] }}</option>
                <option :value="CategoryColor.Orange">{{ CategoryColor[CategoryColor.Orange] }}</option>
                <option :value="CategoryColor.Red">{{ CategoryColor[CategoryColor.Red] }}</option>
            </select>
            
            <select v-if="!isNew" class="form-select form-select-lg" id="category-parent-category-id" name="ParentCategoryId" v-model="model.parentCategoryId" @input="onFormFieldInput($event, 'parentCategoryId');">
                <option v-if="model.parentCategoryId === undefined" :value="undefined" selected>No parent category</option>
                <option v-if="model.parentCategoryId !== undefined" :value="null">No parent category</option>
                <option v-for="option in categoryOptions" :key="option.id" :value="option.id">{{ option.name }}</option>
            </select>

            <input v-if="isNew" type="hidden" id="category-parent-category-id" name="ParentCategoryId" v-model="model.parentCategoryId" />

        </div>

        <div class="category-form-foot">
            <button v-if="!isNew" class="category-form-button btn btn-outline-danger btn-lg" :disabled="areButtonsDisabled" @click="onDelete">
                <font-awesome-icon icon="fa-solid fa-trash" />
                <span>{{ deleteLabel }}</span>
            </button>
            <button type="submit" class="category-form-button btn btn-primary btn-lg" :disabled="areButtonsDisabled || !model.name">
                <font-awesome-icon icon="fa-solid fa-check" />
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
