<script setup lang="ts">

    import './AuthView.scss';

    import { computed, ref, watch } from 'vue';
    import { errorMessages } from '@/utils/Error';
    import { type IRegisterRequest } from '@/features/auth/models/IRegisterRequest';
    import { routes } from '@/router';
    import { useAuthStore } from '@/stores/useAuthStore';
    import { validateEmail, validateOptionalName, validatePassword } from '@/features/auth/AuthService';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

    const authStore = useAuthStore();

    const email = ref<string>('');
    const password = ref<string>('');
    const firstName = ref<string>('');
    const lastName = ref<string>('');
    const serverError = ref<string>('');
    const isLoading = ref<boolean>(false);
    const isRegistered = ref<boolean>(false);
    const isPasswordVisible = ref<boolean>(false);

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
    <div class="auth-screen">

        <BrandLogo class="stacked" />

        <div class="auth-card card ">

            <div v-if="serverError" class="alert alert-danger">
                {{ errorMessages[serverError] }}
            </div>

            <div v-if="isRegistered" class="alert alert-info">
                Account created!<br />
                Please check your email to confirm your address before signing in.
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
                                :class="['form-control', 'password', { 'is-invalid': touched.password && fieldErrors.password && fieldErrors.password.length > 0 }]"
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
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <label for="firstName" class="form-label">First name</label>
                        <input
                            id="firstName"
                            v-model="firstName"
                            type="text"
                            :class="['form-control', { 'is-invalid': touched.firstName && fieldErrors.firstName && fieldErrors.firstName.length > 0 }]"
                            @blur="onFirstNameBlur"
                        />
                        <div class="invalid-feedback">
                            <div v-for="(error, index) in fieldErrors.firstName" :key="index">
                                {{ errorMessages[error] }}
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <label for="lastName" class="form-label">Last name</label>
                        <input
                            id="lastName"
                            v-model="lastName"
                            type="text"
                            :class="['form-control', { 'is-invalid': touched.lastName && fieldErrors.lastName && fieldErrors.lastName.length > 0 }]"
                            @blur="onLastNameBlur"
                        />
                        <div class="invalid-feedback">
                            <div v-for="(error, index) in fieldErrors.lastName" :key="index">
                                {{ errorMessages[error] }}
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 mt-6">
                        <button type="submit" class="btn btn-primary btn-lg w-100" :disabled="!isFormValid || isLoading">
                            {{ isLoading ? 'Creating account…' : 'Create account' }}
                        </button>
                    </div>
                </div>
            </form>

        </div>

        <div class="auth-foot">
            <p>
                Already have an account?
                <RouterLink :to="routes.auth.login" class="link">Sign in</RouterLink>
            </p>
        </div>

    </div>
</template>
