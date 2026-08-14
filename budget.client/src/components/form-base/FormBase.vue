<script setup lang="ts" generic="T extends { id: number }">

    import './FormBase.scss';

    import { onMounted, ref, watch } from 'vue';
    import { debounce } from '@/utils/Utils';
    import { type FormEmits, type FormBaseProps } from '@/components/form-base/FormBase';
    import useNavigation from '@/composables/useNavigation';

    const { isNew, isLoading, isAutoSave = false, saveAllResult, savePartialResult, deleteResult, isFormValid } = defineProps<FormBaseProps>();
    const model = defineModel<T>({ required: true });
    const emit = defineEmits<FormEmits<T>>();

    let partialModel: Partial<T> = {};

    const { goBack } = useNavigation();

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
        submitButtonLabel.value = isNew ? 'Add' : 'Edit';
        deleteButtonLabel.value = 'Delete';
        enableButtons();
    };

    const setSubmitButtonToSavedState = (): void => {
        submitButtonLabel.value = 'Saved';
        disableButtons();
    };

    const setSubmitButtonToErrorState = (): void => {
        submitButtonLabel.value = 'Error';
        enableButtons();
    };

    const setDeleteButtonToErrorState = (): void => {
        deleteButtonLabel.value = 'Error';
        enableButtons();
    };

    const disableButtons = (): void => {
        disableSubmitButton();
        disableDeleteButton();
    };

    const disableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = true;
    };

    const disableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = true;
    };

    const enableButtons = (): void => {
        enableSubmitButton();
        enableDeleteButton();
    };

    const enableSubmitButton = (): void => {
        isSubmitButtonDisabled.value = false;
    };

    const enableDeleteButton = (): void => {
        isDeleteButtonDisabled.value = false;
    };

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
        if (!isAutoSave || !isFormValid() || Object.keys(partialModel).length === 0) {
            return;
        }

        disableButtons();
        emit('savePartial', model.value.id, partialModel);

        partialModel = {};
    }, 1000);

    const onSubmit = (): void => {
        if (!isFormValid()) {
            return;
        }

        disableButtons();
        emit('saveAll', model.value);
    };

    // On delete button click
    const onDelete = (): void => {
        disableButtons();
        emit('delete', model.value.id);
    };

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
        if (result && !result.isSuccess) {
            // Error on save partial -> set submit button to error state for a while
            setSubmitButtonToErrorState();
            waitAndResetButtonsToDefaultState();
        }
        else if (result && result.isSuccess) {
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
    <form v-if="!isLoading" novalidate class="form" @submit.prevent="onSubmit">

        <input v-model="model.id" type="hidden" name="Id" />

        <div class="form-content">
            <Teleport defer to=".form-body">
                <div class="form-close-container">
                    <button type="button" class="form-close" @click="goBack()">
                        <font-awesome-icon icon="fa-solid fa-xmark" />
                    </button>
                </div>
            </Teleport>
            <slot :on-change="onChange"></slot>
        </div>

        <div class="form-foot">
            <button v-if="!isNew" type="button" class="form-button btn btn-outline-danger btn-lg" :disabled="isDeleteButtonDisabled" @click="onDelete">
                <font-awesome-icon icon="fa-solid fa-trash" />
                <span>{{ deleteButtonLabel }}</span>
            </button>
            <button type="submit" class="form-button btn btn-primary btn-lg" :disabled="isSubmitButtonDisabled || !isFormValid()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                <span>{{ submitButtonLabel }}</span>
            </button>
        </div>

    </form>

    <div v-else>Loading…</div>
</template>
