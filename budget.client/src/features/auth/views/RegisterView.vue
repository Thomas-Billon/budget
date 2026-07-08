<script setup lang="ts">
    import { computed, ref, watch } from 'vue';
    import { errorMessages, passwordMaxLength, passwordMinLength } from '@/utils/errorMessages';
    import { type IRegisterRequest } from '@/features/auth/models/IRegisterRequest';
    import { routes } from '@/router';
    import { useAuthStore } from '@/stores/useAuthStore';
    import { validateEmail, validateOptionalName, validatePassword } from '@/utils/AuthValidation';

    const authStore = useAuthStore();

    const email = ref<string>('');
    const password = ref<string>('');
    const firstName = ref<string>('');
    const lastName = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isRegistered = ref<boolean>(false);

    const fieldErrors = ref<Record<string, string[]>>({ email: [], password: [], firstName: [], lastName: [] });

    const touched = ref<Record<string, boolean>>({ email: false, password: false, firstName: false, lastName: false });

    const isFormValid = computed(() =>
        !!email.value &&
        !!password.value &&
        Object.values(fieldErrors.value).every(e => e.length === 0)
    );

    watch(email, (value) => {
        fieldErrors.value.email = validateEmail(value);
    });

    watch(password, (value) => {
        fieldErrors.value.password = validatePassword(value);
    });

    watch(firstName, (value) => {
        fieldErrors.value.firstName = validateOptionalName(value);
    });

    watch(lastName, (value) => {
        fieldErrors.value.lastName = validateOptionalName(value);
    });

    const submit = async (): Promise<void> => {
        if (!isFormValid.value) {
            return;
        }

        serverError.value = '';
        isLoading.value = true;

        const credentials: IRegisterRequest = {
            email: email.value,
            password: password.value,
            firstName: firstName.value || undefined,
            lastName: lastName.value || undefined
        };

        const result = await authStore.register(credentials);

        if (result.isSuccess) {
            isRegistered.value = true;
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

    const onFirstNameBlur = (): void => {
        touched.value.firstName = true;
    };

    const onLastNameBlur = (): void => {
        touched.value.lastName = true;
    };
</script>

<template>
    <div class="container">
        <div v-if="isRegistered" class="alert alert-success" role="alert">
            Account created! Please check your email to confirm your address before signing in.
        </div>

        <form v-else novalidate @submit.prevent="submit">
            <div class="mb-3">
                <label for="email" class="form-label">Email <span class="text-danger">*</span></label>
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
                <label for="password" class="form-label">Password <span class="text-danger">*</span></label>
                <input
                    v-model="password"
                    type="password"
                    class="form-control"
                    :class="{ 'is-invalid': touched.password && fieldErrors.password && fieldErrors.password.length > 0 }"
                    autocomplete="new-password"
                    required
                    :minlength="passwordMinLength"
                    :maxlength="passwordMaxLength"
                    @blur="onPasswordBlur"
                />
                <div v-if="touched.password && fieldErrors.password && fieldErrors.password.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.password" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
                <div class="form-text">
                    Min. {{ passwordMinLength }} characters, with uppercase, lowercase, digit, and special character.
                </div>
            </div>

            <div class="mb-3">
                <label for="firstName" class="form-label">First name <span class="text-muted">(optional)</span></label>
                <input
                    v-model="firstName"
                    type="text"
                    class="form-control"
                    :class="{ 'is-invalid': touched.firstName && fieldErrors.firstName && fieldErrors.firstName.length > 0 }"
                    autocomplete="given-name"
                    @blur="onFirstNameBlur"
                />
                <div v-if="touched.firstName && fieldErrors.firstName && fieldErrors.firstName.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.firstName" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
            </div>

            <div class="mb-3">
                <label for="lastName" class="form-label">Last name <span class="text-muted">(optional)</span></label>
                <input
                    v-model="lastName"
                    type="text"
                    class="form-control"
                    :class="{ 'is-invalid': touched.lastName && fieldErrors.lastName && fieldErrors.lastName.length > 0 }"
                    autocomplete="family-name"
                    @blur="onLastNameBlur"
                />
                <div v-if="touched.lastName && fieldErrors.lastName && fieldErrors.lastName.length > 0" class="invalid-feedback">
                    <div v-for="(error, index) in fieldErrors.lastName" :key="index">
                        {{ errorMessages[error] }}
                    </div>
                </div>
            </div>

            <div v-if="serverError" class="alert alert-danger" role="alert">
                {{ errorMessages[serverError] }}
            </div>

            <button type="submit" class="btn btn-primary w-100" :disabled="!isFormValid || isLoading">
                {{ isLoading ? 'Creating account…' : 'Create account' }}
            </button>
        </form>

        <p v-if="!isRegistered" class="mt-3 text-center">
            Already have an account?
            <RouterLink :to="routes.auth.login">
                Sign in
            </RouterLink>
        </p>
    </div>
</template>
