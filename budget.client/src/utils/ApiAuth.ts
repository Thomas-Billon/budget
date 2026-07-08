import { type AuthStore } from '@/stores/useAuthStore';
import { type NavigationFailure, type Router } from 'vue-router';
import { routes } from '@/router';

let _authStore: AuthStore | null = null;
let _router: Router | null = null;

// INFO: Concurrent api calls must share a single refresh, or every call but the first would find their refresh token already revoked and would log the user out.
let _refreshPromise: Promise<boolean> | null = null;

const initApiAuth = (authStore: AuthStore, router: Router): void => {
    _authStore = authStore;
    _router = router;
};

const getAccessToken = (): string | null => _authStore?.accessToken ?? null;

const refreshAccessToken = (): Promise<boolean> => {
    if (!_authStore) {
        return Promise.resolve(false);
    }

    if (!_refreshPromise) {
        _refreshPromise = _authStore.refresh().finally(() => {
            _refreshPromise = null;
        });
    }

    return _refreshPromise;
};

const handleSessionExpired = (): Promise<NavigationFailure | void | undefined> => _router?.push(routes.auth.login) ?? Promise.resolve();

export {
    initApiAuth,
    getAccessToken,
    refreshAccessToken,
    handleSessionExpired
};
