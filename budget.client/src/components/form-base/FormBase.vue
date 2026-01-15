<script setup lang="ts" generic="T extends { id: number }">

    import './FormBase.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { type FormProps, type FormEmits } from '@/components/form-base/FormBase';


    interface Props<T> extends FormProps {
        isFormValid: (model: T) => boolean
    }

    interface FormHeadProps {
        onChange: <K extends keyof T>(field: K, value: T[K]) => void;
    }

    interface FormBodyProps {
        onChange: <K extends keyof T>(field: K, value: T[K]) => void;
    }

    interface FormFootProps {
        isFormValid: (model: T) => boolean;
        onSubmit: () => void;
        onDelete: () => void;
        isNew: boolean;
        deleteButtonLabel: string;
        submitButtonLabel: string;
        isSubmitButtonDisabled: boolean;
        isDeleteButtonDisabled: boolean;
    }

    const { isNew, saveAllResult, savePartialResult, deleteResult, isFormValid } = defineProps<Props<T>>();
    const model = defineModel<T>({ required: true });
    const emit = defineEmits<FormEmits<T>>();

    const slots = defineSlots<{
        head?(props: FormHeadProps): any;
        body(props: FormBodyProps): any;
        foot?(props: FormFootProps): any;
    }>();

    let partialModel: Partial<T> = {};

    const submitButtonLabel = ref('');
    const deleteButtonLabel = ref('');
    const isSubmitButtonDisabled = ref(false);
    const isDeleteButtonDisabled = ref(false);

    // #region Init

    onMounted(() => {
        setButtonsToDefaultState();
    });

    // #endregion Init

    // #region Buttons

    const setButtonsToDefaultState = (): void => {
        submitButtonLabel.value = isNew ? 'Add transaction' : 'Edit transaction';
        deleteButtonLabel.value = 'Delete transaction';
        enableButtons();
    }

    const setSubmitButtonToSavedState = (): void => {
        submitButtonLabel.value = 'Saved';
        disableButtons();
    }

    const setSubmitButtonToErrorState = (): void => {
        submitButtonLabel.value = 'Error';
        enableButtons();
    }

    const setDeleteButtonToErrorState = (): void => {
        deleteButtonLabel.value = 'Error';
        enableButtons();
    }

    const disableButtons = (): void => {
        disableSubmitButton();
        disableDeleteButton();
    }

    const disableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = true;
    }

    const disableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = true;
    }

    const enableButtons = (): void => {
        enableSubmitButton();
        enableDeleteButton();
    }

    const enableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = false;
    }

    const enableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = false;
    }

    const waitAndResetButtonsToDefaultState = debounce(setButtonsToDefaultState, 5000);

    // #endregion Buttons

    // #region Actions

    // On form submit
    const onChange = <K extends keyof T>(fieldName: K, value: T[K]): void => {
        setButtonsToDefaultState();

        if (isNew) {
            return;
        }

        partialModel[fieldName] = value;
        debounceSavePartial();
    };

    const debounceSavePartial = debounce(() => {
        if (!isFormValid(model.value) || Object.keys(partialModel).length === 0) {
            return;
        }

        disableButtons();
        emit('savePartial', model.value.id, partialModel);

        partialModel = {};
    }, 1000);

    const onSubmit = (): void => {
        if (!isFormValid(model.value)) {
            return;
        }

        disableButtons();
        emit('saveAll', model.value);
    }

    // On delete button click
    const onDelete = (): void => {
        disableButtons();
        emit('delete', model.value.id);
    }

    // #endregion Actions

    // #region API call results

    watch(() => saveAllResult, (result) => {
        if (result && !result.isSuccess) {
            // Error on save all -> set submit button to error state for a while
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        // Success on save all -> nothing to do, form will be exited
    });

    watch(() => savePartialResult, (result) => {
        if (result && !result.isSuccess || isNew) {
            // Error on save partial -> set submit button to error state for a while
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else {
            // Success on save partial -> set submit button to saved state until next edit but keep delete button enabled
            setSubmitButtonToSavedState();
            enableDeleteButton();
        }
    });

    watch(() => deleteResult, (result) => {
        if (result !== undefined && !result.isSuccess) {
            // Error on delete -> set delete button to error state for a while
            setDeleteButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        // Success on delete -> nothing to do, form will be exited
    });

    // #endregion API call results

</script>

<template>
    <form class="form section-container-grow container" @submit.prevent="onSubmit">

        <input type="hidden" name="Id" v-model="model.id" />
        
        <slot name="head" :onChange="onChange"></slot>

        <slot name="body" :onChange="onChange"></slot>

        <slot name="foot"
            :isFormValid="isFormValid"
            :onSubmit="onSubmit"
            :onDelete="onDelete"
            :isNew="isNew"
            :deleteButtonLabel="deleteButtonLabel"
            :submitButtonLabel="submitButtonLabel"
            :isSubmitButtonDisabled="isSubmitButtonDisabled"
            :isDeleteButtonDisabled="isDeleteButtonDisabled">
            <div class="form-foot">
                <button v-if="!isNew" type="button" class="form-button btn btn-outline-danger btn-lg" :disabled="isDeleteButtonDisabled" @click="onDelete">
                    <font-awesome-icon icon="fa-solid fa-trash" />
                    <span>{{ deleteButtonLabel }}</span>
                </button>
                <button type="submit" class="form-button btn btn-primary btn-lg" :disabled="isSubmitButtonDisabled || !isFormValid(model)">
                    <font-awesome-icon icon="fa-solid fa-check" />
                    <span>{{ submitButtonLabel }}</span>
                </button>
            </div>
        </slot>
    </form>
</template>