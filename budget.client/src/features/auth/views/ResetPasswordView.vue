<script setup lang="ts">

    import './AuthView.scss';

    import { apiCall } from '@/utils/ApiCall';
    import { computed, onMounted, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/Error';
    import { type IResetPasswordRequest } from '@/features/auth/models/IResetPasswordRequest';
    import { routes } from '@/router';
    import { useRoute } from 'vue-router';
    import { validatePassword } from '@/features/auth/AuthService';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

    const route = useRoute();

    const email = ref<string>('');
    const token = ref<string>('');

    const isLinkValid = ref<boolean>(false);
    const newPassword = ref<string>('');
    const confirmPassword = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isSuccess = ref<boolean>(false);
    const isNewPasswordVisible = ref<boolean>(false);
    const isConfirmPasswordVisible = ref<boolean>(false);

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
            'auth/reset-password',
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
    <div class="auth-screen">

        <BrandLogo class="stacked" />

        <div class="auth-card card">

            <div v-if="serverError" class="alert alert-danger">
                {{ errorMessages[serverError] }}
            </div>

            <div v-if="!isLinkValid" class="alert alert-danger">
                {{ errorMessages['error.auth.reset_password_failed'] }}
            </div>

            <div v-else-if="isSuccess" class="alert alert-info">
                Your password has been reset.<br />
                You can now <RouterLink :to="routes.auth.login" class="link text-600">sign in</RouterLink>.
            </div>

            <form v-else novalidate @submit.prevent="submit">
                <div class="row">
                    <div class="col-12">
                        <label for="newPassword" class="form-label">New password</label>
                        <div class="input-group">
                            <div class="input-group-text">
                                <font-awesome-icon icon="fa-solid fa-lock" fixed-width />
                            </div>
                            <input
                                id="newPassword"
                                v-model="newPassword"
                                :type="isNewPasswordVisible ? 'text' : 'password'"
                                :class="['form-control', 'password', { 'is-invalid': touched.newPassword && fieldErrors.newPassword && fieldErrors.newPassword.length > 0 }]"
                                placeholder="••••••••"
                                required
                                @blur="onNewPasswordBlur"
                            />
                            <button type="button" class="input-group-text" @click="isNewPasswordVisible = !isNewPasswordVisible">
                                <font-awesome-icon :icon="`fa-solid fa-${isNewPasswordVisible ? 'eye-slash' : 'eye'}`" fixed-width />
                            </button>
                        </div>
                        <div class="invalid-feedback">
                            <div v-for="(error, index) in fieldErrors.newPassword" :key="index">
                                {{ errorMessages[error] }}
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <label for="confirmPassword" class="form-label">Confirm new password</label>
                        <div class="input-group">
                            <div class="input-group-text">
                                <font-awesome-icon icon="fa-solid fa-lock" fixed-width />
                            </div>
                            <input
                                id="confirmPassword"
                                v-model="confirmPassword"
                                :type="isConfirmPasswordVisible ? 'text' : 'password'"
                                :class="['form-control', 'password', { 'is-invalid': touched.confirmPassword && !passwordsMatch }]"
                                placeholder="••••••••"
                                required
                                @blur="onConfirmPasswordBlur"
                            />
                            <button type="button" class="input-group-text" @click="isConfirmPasswordVisible = !isConfirmPasswordVisible">
                                <font-awesome-icon :icon="`fa-solid fa-${isConfirmPasswordVisible ? 'eye-slash' : 'eye'}`" fixed-width />
                            </button>
                        </div>
                        <div class="invalid-feedback">
                            Passwords do not match.
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 mt-6">
                        <button type="submit" class="btn btn-primary btn-lg w-100" :disabled="!isFormValid || isLoading">
                            {{ isLoading ? 'Resetting…' : 'Reset password' }}
                        </button>
                    </div>
                </div>
            </form>

        </div>

        <div class="auth-foot">
            <p>
                <RouterLink :to="routes.auth.login" class="link">Back to sign in</RouterLink>
            </p>
        </div>

    </div>
</template>
