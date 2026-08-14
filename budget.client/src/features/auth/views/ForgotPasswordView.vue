<script setup lang="ts">

    import './AuthView.scss';

    import { apiCall } from '@/utils/ApiCall';
    import { computed, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/Error';
    import { type IForgotPasswordRequest } from '@/features/auth/models/IForgotPasswordRequest';
    import { routes } from '@/router';
    import { validateEmail } from '@/features/auth/AuthService';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

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

        const request: IForgotPasswordRequest = { email: email.value };

        const result = await apiCall<IForgotPasswordRequest, void>(
            'auth/forgot-password',
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
    <div class="auth-screen">

        <BrandLogo class="stacked" />

        <div class="auth-card card">

            <div v-if="serverError" class="alert alert-danger">
                {{ errorMessages[serverError] }}
            </div>

            <div v-if="isSubmitted" class="alert alert-info">
                If an account exists for that email, we've sent a reset link.
            </div>

            <form v-else novalidate @submit.prevent="submit">
                <div class="row">
                    <div class="col-12">
                        <label for="email" class="form-label">Email</label>
                        <div class="input-group">
                            <div class="input-group-text">
                                <font-awesome-icon icon="fa-solid fa-envelope" fixed-width />
                            </div>
                            <input
                                id="email"
                                v-model="email"
                                type="email"
                                :class="['form-control', { 'is-invalid': touched.email && fieldErrors.email && fieldErrors.email.length > 0 }]"
                                placeholder="name@company.com"
                                autocomplete="email"
                                required
                                @blur="onEmailBlur"
                            />
                        </div>
                        <div class="invalid-feedback">
                            <div v-for="(error, index) in fieldErrors.email" :key="index">
                                {{ errorMessages[error] }}
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 mt-6">
                        <button type="submit" class="btn btn-primary btn-lg w-100" :disabled="!isFormValid || isLoading">
                            {{ isLoading ? 'Sending…' : 'Send reset link' }}
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
