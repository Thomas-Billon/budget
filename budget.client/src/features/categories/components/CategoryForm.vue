<script setup lang="ts">

    import './CategoryForm.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { CategoryColor } from '@/enums/CategoryColor.ts';
    import { type ICategoryRequest } from '@/features/categories/models/ICategoryRequest';
    import type { ApiCallResult } from '@/utils/ApiCall';

    interface Props {
        isNew: boolean;
        saveAllResult?: ApiCallResult;
        savePartialResult?: ApiCallResult;
    }

    type Emits = {
        saveAll: [data: Partial<ICategoryRequest>];
        savePartial: [id: number, data: Partial<ICategoryRequest>];
    };
    
    const { isNew, saveAllResult, savePartialResult } = defineProps<Props>();
    const model = defineModel<Partial<ICategoryRequest>>({ required: true });
    const emit = defineEmits<Emits>();

    const submitLabel = ref<string>('');
    const isSubmitDisabled = ref<boolean>(false);

    let partialModel: Partial<ICategoryRequest> = {};

    // Init
    onMounted(() => {
        setSubmitButtonToDefaultState();
    });

    // On props change
    watch(() => [saveAllResult, savePartialResult], ([all, partial]) => {
        if (all !== undefined && !all.isSuccess) {
            setSubmitButtonToErrorState();
            waitAndResetSubmitButtonToDefaultState();
        }
        else {
            if (partial !== undefined && !partial.isSuccess || isNew) {
                setSubmitButtonToDefaultState();
            }
            else {
                setSubmitButtonToSavedState();
                waitAndResetSubmitButtonToDefaultState();
            }
        }
    });

    // On form submit
    const onSubmit = (): void => {
        setSubmitButtonToSavedState();
        saveAll(model.value);
    }

    // On all form fields input event (any text modification)
    const onFormFieldInput = <T extends keyof ICategoryRequest>(event: Event, fieldName: T): void => {
        setSubmitButtonToDefaultState();

        // Edit only for partial update
        if (!isNew) {
            const target = event.target as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null;
            if (target === null) {
                return;
            }

            partialModel[fieldName] = target.value as ICategoryRequest[T];
            debounceSavePartial();
        }
    }

    const setSubmitButtonToDefaultState = (): void => {
        submitLabel.value = isNew ? 'Add category' : 'Edit category';
        isSubmitDisabled.value = false;
    }

    const setSubmitButtonToSavedState = (): void => {
        submitLabel.value = 'Saved';
        isSubmitDisabled.value = true;
    }

    const setSubmitButtonToErrorState = (): void => {
        submitLabel.value = 'Error';
        isSubmitDisabled.value = false;
    }

    const waitAndResetSubmitButtonToDefaultState = debounce(setSubmitButtonToDefaultState, 5000);

    const saveAll = (data: Partial<ICategoryRequest>) => emit('saveAll', data);
    const savePartial = (id: number, data: Partial<ICategoryRequest>) => emit('savePartial', id, data);

    const debounceSavePartial = debounce(() => {
        if (model.value.id === undefined || Object.keys(partialModel).length === 0) {
            return;
        }

        savePartial(model.value.id, partialModel);
        partialModel = {};
    }, 1000);

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
        
        </div>

        <div class="category-form-foot">
            <button type="submit" class="category-form-submit btn btn-primary btn-lg" :disabled="isSubmitDisabled || !model.name">
                <span>{{ submitLabel }}</span>
            </button>
        </div>

    </form>
</template>
