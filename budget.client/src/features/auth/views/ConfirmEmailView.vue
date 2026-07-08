<script setup lang="ts">
    import { apiCall } from '@/utils/ApiCall';
    import { onMounted, ref } from 'vue';
    import { errorMessages } from '@/utils/errorMessages';
    import { type IConfirmEmailRequest } from '@/features/auth/models/IConfirmEmailRequest';
    import { routes } from '@/router';
    import { useRoute } from 'vue-router';

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
        else {
            serverError.value = 'error.auth.email_confirmation_failed';
        }

        isLoading.value = false;
    });
</script>

<template>
    <div class="container">
        <div v-if="isLoading" class="text-center">
            Confirming your email…
        </div>

        <div v-else-if="!isLinkValid || serverError" class="alert alert-danger" role="alert">
            {{ errorMessages['error.auth.email_confirmation_failed'] }}
            <RouterLink :to="routes.auth.resendEmailConfirmation">
                Resend confirmation email
            </RouterLink>
        </div>

        <div v-else-if="isSuccess" class="alert alert-success" role="alert">
            Your email has been confirmed. You can now <RouterLink :to="routes.auth.login">sign in</RouterLink>.
        </div>
    </div>
</template>
