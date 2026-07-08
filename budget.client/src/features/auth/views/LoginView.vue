<script setup lang="ts">
    import { computed, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/errorMessages';
    import { type ILoginRequest } from '@/features/auth/models/ILoginRequest';
    import { routes } from '@/router';
    import { useAuthStore } from '@/stores/useAuthStore';
    import { useRoute, useRouter } from 'vue-router';
    import { validateEmail, validatePasswordLength } from '@/utils/AuthValidation';

    const router = useRouter();
    const route = useRoute();
    const authStore = useAuthStore();

    const email = ref<string>('');
    const password = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);

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
    <div class="container">
        <form novalidate @submit.prevent="submit">
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

            <div class="mb-3">
                <label for="password" class="form-label">Password</label>
                <input
                    v-model="password"
                    type="password"
                    class="form-control"
                    :class="{ 'is-invalid': touched.password && fieldErrors.password && fieldErrors.password.length > 0 }"
                    autocomplete="current-password"
                    required
                    @blur="onPasswordBlur"
                />
                <div v-if="touched.password && fieldErrors.password && fieldErrors.password.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.password" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
            </div>

            <p class="mb-3 text-end">
                <RouterLink :to="routes.auth.forgotPassword">
                    Forgot password?
                </RouterLink>
            </p>

            <div v-if="serverError" class="alert alert-danger" role="alert">
                {{ errorMessages[serverError] }}
            </div>

            <button type="submit" class="btn btn-primary w-100" :disabled="!isFormValid || isLoading">
                {{ isLoading ? 'Signing in…' : 'Sign in' }}
            </button>
        </form>

        <p class="mt-3 text-center">
            No account yet?
            <RouterLink :to="routes.auth.register">
                Create one
            </RouterLink>
        </p>
        <p class="mt-2 text-center">
            Didn't receive the confirmation email?
            <RouterLink :to="routes.auth.resendEmailConfirmation">
                Resend it
            </RouterLink>
        </p>
    </div>
</template>
