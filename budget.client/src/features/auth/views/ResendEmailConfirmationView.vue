<script setup lang="ts">
    import { apiCall } from '@/utils/ApiCall';
    import { computed, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/errorMessages';
    import { type IResendEmailConfirmationRequest } from '@/features/auth/models/IResendEmailConfirmationRequest';
    import { routes } from '@/router';
    import { validateEmail } from '@/utils/AuthValidation';

    const email = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isSubmitted = ref<boolean>(false);

    const fieldErrors = ref<Record<string, string[]>>({ email: [] });

    const touched = ref<Record<string, boolean>>({ email: false });

    const isFormValid = computed(() =>
        !!email.value &&
        Object.values(fieldErrors.value).every(e => e.length === 0)
    );

    watch(email, (value) => {
        fieldErrors.value.email = validateEmail(value);
    });

    const submit = async (): Promise<void> => {
        if (!isFormValid.value) {
            return;
        }

        serverError.value = '';
        isLoading.value = true;

        const request: IResendEmailConfirmationRequest = { email: email.value };

        const result = await apiCall<IResendEmailConfirmationRequest, void>(
            'auth/resend-email-confirmation',
            { method: 'POST', body: request },
            { canRetryOnUnauthorized: false }
        );

        if (result.isSuccess) {
            isSubmitted.value = true;
        }
        else if (result.error.type === 'invalidModel') {
            fieldErrors.value = result.error.codes;
        }
        else if (result.error.type === 'failure') {
            serverError.value = result.error.code;
        }

        isLoading.value = false;
    };

    const onEmailBlur = (): void => {
        touched.value.email = true;
    };
</script>

<template>
    <div class="container">
        <div v-if="isSubmitted" class="alert alert-success" role="alert">
            If an account exists for that email and hasn't been confirmed yet, we've sent a new confirmation link.
        </div>

        <form v-else novalidate @submit.prevent="submit">
            <div class="mb-3">
                <label for="email" class="form-label">Email</label>
                <input
                    v-model="email"
                    type="email"
                    class="form-control"
                    :class="{ 'is-invalid': touched.email && fieldErrors.email && fieldErrors.email.length > 0 }"
                    autocomplete="email"
                    required
                    @blur="onEmailBlur"
                />
                <div v-if="touched.email && fieldErrors.email && fieldErrors.email.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.email" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
            </div>

            <div v-if="serverError" class="alert alert-danger" role="alert">
                {{ errorMessages[serverError] }}
            </div>

            <button type="submit" class="btn btn-primary w-100" :disabled="!isFormValid || isLoading">
                {{ isLoading ? 'Sending…' : 'Resend confirmation email' }}
            </button>
        </form>

        <p class="mt-3 text-center">
            <RouterLink :to="routes.auth.login">
                Back to sign in
            </RouterLink>
        </p>
    </div>
</template>
