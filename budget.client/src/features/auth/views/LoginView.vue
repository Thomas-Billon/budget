<script setup lang="ts">

    import './AuthView.scss';

    import { computed, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/Error';
    import { type ILoginRequest } from '@/features/auth/models/ILoginRequest';
    import { routes } from '@/router';
    import { useAuthStore } from '@/stores/useAuthStore';
    import { useRoute, useRouter } from 'vue-router';
    import { validateEmail, validatePasswordLength } from '@/features/auth/AuthService';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

    const router = useRouter();
    const route = useRoute();
    const authStore = useAuthStore();

    const email = ref<string>('');
    const password = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isPasswordVisible = ref<boolean>(false);

    const fieldErrors = ref<Record<string, string[]>>({ email: [], password: [] });

    const touched = ref<Record<string, boolean>>({ email: false, password: false });

    const isFormValid = computed(() =>
        !!email.value &&
        !!password.value &&
        Object.values(fieldErrors.value).every(e => e.length === 0)
    );

    watch(email, (value) => {
        fieldErrors.value.email = validateEmail(value);
    });

    watch(password, (value) => {
        fieldErrors.value.password = validatePasswordLength(value);
    });

    const submit = async (): Promise<void> => {
        if (!isFormValid.value) {
            return;
        }

        serverError.value = '';
        isLoading.value = true;

        const credentials: ILoginRequest = {
            email: email.value,
            password: password.value
        };

        const result = await authStore.login(credentials);

        if (result.isSuccess) {
            const redirect = route.query.redirect as string | undefined;
            await router.push(redirect ?? routes.home);
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

    const onPasswordBlur = (): void => {
        touched.value.password = true;
    };
</script>

<template>
    <div class="auth-screen">

        <BrandLogo class="stacked" />

        <div class="auth-card card">

            <div v-if="serverError" class="alert alert-danger">
                {{ errorMessages[serverError] }}
            </div>

            <form novalidate @submit.prevent="submit">
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
                    <div class="col-12">
                        <label for="password" class="form-label">Password</label>
                        <div class="input-group">
                            <div class="input-group-text">
                                <font-awesome-icon icon="fa-solid fa-lock" fixed-width />
                            </div>
                            <input
                                id="password"
                                v-model="password"
                                :type="isPasswordVisible ? 'text' : 'password'"
                                :class="['form-control password', { 'is-invalid': touched.password && fieldErrors.password && fieldErrors.password.length > 0 }]"
                                placeholder="••••••••"
                                required
                                @blur="onPasswordBlur"
                            />
                            <button type="button" class="input-group-text" @click="isPasswordVisible = !isPasswordVisible">
                                <font-awesome-icon :icon="`fa-solid fa-${isPasswordVisible ? 'eye-slash' : 'eye'}`" fixed-width />
                            </button>
                        </div>
                        <div class="invalid-feedback">
                            <div v-for="(error, index) in fieldErrors.password" :key="index">
                                {{ errorMessages[error] }}
                            </div>
                        </div>
                        <RouterLink :to="routes.auth.forgotPassword" class="link text-sm text-600 mt-1">Forgot password?</RouterLink>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 mt-6">
                        <button type="submit" class="btn btn-primary btn-lg w-100" :disabled="!isFormValid || isLoading">
                            {{ isLoading ? 'Signing in…' : 'Sign in' }}
                        </button>
                    </div>
                </div>
            </form>

        </div>

        <div class="auth-foot">
            <p>
                No account yet?
                <RouterLink :to="routes.auth.register" class="link">Create one</RouterLink>
            </p>
            <p>
                Didn't receive the confirmation email?
                <RouterLink :to="routes.auth.resendEmailConfirmation" class="link">Resend it</RouterLink>
            </p>
        </div>

    </div>
</template>
