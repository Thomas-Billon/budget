<script setup lang="ts">
    import { apiCall } from '@/utils/ApiCall';
    import { computed, onMounted, ref, watch } from 'vue';
    import { errorMessages, passwordMaxLength, passwordMinLength } from '@/utils/errorMessages';
    import { type IResetPasswordRequest } from '@/features/auth/models/IResetPasswordRequest';
    import { routes } from '@/router';
    import { useAuthStore } from '@/stores/useAuthStore';
    import { useRoute } from 'vue-router';
    import { validatePassword } from '@/utils/AuthValidation';

    const route = useRoute();
    const authStore = useAuthStore();

    const email = ref<string>('');
    const token = ref<string>('');
    const isLinkValid = ref<boolean>(false);

    const newPassword = ref<string>('');
    const confirmPassword = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isSuccess = ref<boolean>(false);

    const fieldErrors = ref<Record<string, string[]>>({ newPassword: [] });

    const touched = ref<Record<string, boolean>>({ newPassword: false, confirmPassword: false });

    const passwordsMatch = computed(() => newPassword.value === confirmPassword.value);

    const isFormValid = computed(() =>
        !!newPassword.value &&
        !!confirmPassword.value &&
        passwordsMatch.value &&
        Object.values(fieldErrors.value).every(e => e.length === 0)
    );

    onMounted(() => {
        const queryEmail = route.query.email;
        const queryToken = route.query.token;

        if (typeof queryEmail === 'string' && typeof queryToken === 'string' && queryEmail && queryToken) {
            email.value = queryEmail;
            token.value = queryToken;
            isLinkValid.value = true;
        }
    });

    watch(newPassword, (value) => {
        fieldErrors.value.newPassword = validatePassword(value);
    });

    const submit = async (): Promise<void> => {
        if (!isFormValid.value) {
            return;
        }

        serverError.value = '';
        isLoading.value = true;

        const request: IResetPasswordRequest = {
            email: email.value,
            token: token.value,
            newPassword: newPassword.value
        };

        const result = await apiCall<IResetPasswordRequest, void>(
            `auth/reset-password`,
            { method: 'POST', body: request },
            { canRetryOnUnauthorized: false }
        );

        if (result.isSuccess) {
            isSuccess.value = true;
        }
        else if (result.error.type === 'invalidModel') {
            fieldErrors.value = result.error.codes;
        }
        else if (result.error.type === 'failure') {
            serverError.value = result.error.code;
        }

        isLoading.value = false;
    };

    const onNewPasswordBlur = (): void => {
        touched.value.newPassword = true;
    };

    const onConfirmPasswordBlur = (): void => {
        touched.value.confirmPassword = true;
    };
</script>

<template>
    <div class="container">
        <div v-if="!isLinkValid" class="alert alert-danger" role="alert">
            This password reset link is invalid or has expired. Please request a new one.
        </div>

        <div v-else-if="isSuccess" class="alert alert-success" role="alert">
            Your password has been reset. You can now <RouterLink :to="routes.auth.login">sign in</RouterLink>.
        </div>

        <form v-else novalidate @submit.prevent="submit">
            <div class="mb-3">
                <label for="newPassword" class="form-label">New password</label>
                <input
                    v-model="newPassword"
                    type="password"
                    class="form-control"
                    :class="{ 'is-invalid': touched.newPassword && fieldErrors.newPassword && fieldErrors.newPassword.length > 0 }"
                    autocomplete="new-password"
                    required
                    :minlength="passwordMinLength"
                    :maxlength="passwordMaxLength"
                    @blur="onNewPasswordBlur"
                />
                <div v-if="touched.newPassword && fieldErrors.newPassword && fieldErrors.newPassword.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.newPassword" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
            </div>

            <div class="mb-3">
                <label for="confirmPassword" class="form-label">Confirm new password</label>
                <input
                    v-model="confirmPassword"
                    type="password"
                    class="form-control"
                    :class="{ 'is-invalid': touched.confirmPassword && !passwordsMatch }"
                    autocomplete="new-password"
                    required
                    @blur="onConfirmPasswordBlur"
                />
                <div v-if="touched.confirmPassword && !passwordsMatch" class="invalid-feedback">
                    Passwords do not match.
                </div>
            </div>

            <div v-if="serverError" class="alert alert-danger" role="alert">
                {{ errorMessages[serverError] }}
            </div>

            <button type="submit" class="btn btn-primary w-100" :disabled="!isFormValid || isLoading">
                {{ isLoading ? 'Resetting…' : 'Reset password' }}
            </button>
        </form>
    </div>
</template>
