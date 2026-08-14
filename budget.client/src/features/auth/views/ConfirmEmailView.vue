<script setup lang="ts">

    import './AuthView.scss';

    import { apiCall } from '@/utils/ApiCall';
    import { onMounted, ref } from 'vue';
    import { errorMessages } from '@/utils/Error';
    import { type IConfirmEmailRequest } from '@/features/auth/models/IConfirmEmailRequest';
    import { routes } from '@/router';
    import { useRoute } from 'vue-router';
    import BrandLogo from '@/components/brand-logo/BrandLogo.vue';

    const route = useRoute();

    const isLinkValid = ref<boolean>(false);
    const isLoading = ref<boolean>(true);
    const isSuccess = ref<boolean>(false);
    const serverError = ref<string>('');

    onMounted(async () => {
        const queryEmail = route.query.email;
        const queryToken = route.query.token;

        if (typeof queryEmail !== 'string' || typeof queryToken !== 'string' || !queryEmail || !queryToken) {
            isLoading.value = false;
            return;
        }

        isLinkValid.value = true;

        const request: IConfirmEmailRequest = { email: queryEmail, token: queryToken };

        const result = await apiCall<IConfirmEmailRequest, void>(
            'auth/confirm-email',
            { method: 'POST', body: request },
            { canRetryOnUnauthorized: false }
        );

        if (result.isSuccess) {
            isSuccess.value = true;
        }
        else if (result.error.type === 'failure') {
            serverError.value = result.error.code;
        }

        isLoading.value = false;
    });
</script>

<template>
    <div class="auth-screen">

        <BrandLogo class="stacked" />

        <div class="auth-card card">

            <div v-if="serverError" class="alert alert-danger">
                {{ errorMessages[serverError] }}
            </div>

            <div v-if="!isLinkValid" class="alert alert-danger">
                {{ errorMessages['error.auth.email_confirmation_failed'] }}
            </div>

            <div v-else-if="isSuccess" class="alert alert-info">
                Your email has been confirmed.<br />
                You can now <RouterLink :to="routes.auth.login" class="link text-600">sign in</RouterLink>.
            </div>

        </div>

        <div class="auth-foot">
            <p>
                <RouterLink :to="routes.auth.login" class="link">Back to sign in</RouterLink>
            </p>
        </div>

    </div>
</template>
