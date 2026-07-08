import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
import { defineStore } from 'pinia';
import { type IAccessTokenResponse } from '@/features/auth/models/IAccessTokenResponse';
import { type ILoginRequest } from '@/features/auth/models/ILoginRequest';
import { type IRegisterRequest } from '@/features/auth/models/IRegisterRequest';
import { type IUser } from '@/features/auth/models/IUser';
import { jwtDecode } from 'jwt-decode';
import { ref } from 'vue';

interface JwtPayload {
    sub: string;
    email: string;
    given_name?: string;
    family_name?: string;
}

export type AuthStore = ReturnType<typeof useAuthStore>;

export const useAuthStore = defineStore('auth', () => {
    const endpoint = 'auth';

    const accessToken = ref<string | null>(null);
    const user = ref<IUser | null>(null);

    const save = (response: IAccessTokenResponse): void => {
        accessToken.value = response.token;

        const payload = jwtDecode<JwtPayload>(response.token);

        user.value = {
            id: payload.sub,
            email: payload.email,
            firstName: payload.given_name ?? null,
            lastName: payload.family_name ?? null
        };
    };

    const clear = (): void => {
        accessToken.value = null;
        user.value = null;
    };

    function register(credentials: IRegisterRequest): Promise<ApiCallResult<void>> {
        return apiCall<IRegisterRequest, void>(
            `${endpoint}/register`,
            { method: 'POST', body: credentials },
            { canRetryOnUnauthorized: false }
        );
    }

    async function login(credentials: ILoginRequest): Promise<ApiCallResult<IAccessTokenResponse>> {
        const result = await apiCall<ILoginRequest, IAccessTokenResponse>(
            `${endpoint}/login`,
            { method: 'POST', body: credentials },
            { canRetryOnUnauthorized: false }
        );

        if (result.isSuccess) {
            save(result.data);
        }

        return result;
    }

    async function logout(): Promise<void> {
        await apiCall<void, void>(
            `${endpoint}/logout`,
            { method: 'POST' },
            { canRetryOnUnauthorized: false }
        );

        clear();
    }

    async function refresh(): Promise<boolean> {
        const result = await apiCall<void, IAccessTokenResponse>(
            `${endpoint}/refresh`,
            { method: 'POST' },
            { canRetryOnUnauthorized: false }
        );

        if (result.isSuccess) {
            save(result.data);
        }
        else {
            clear();
        }

        return result.isSuccess;
    }

    return {
        accessToken,
        user,
        register,
        login,
        logout,
        refresh
    };
});
