import { getAccessToken, handleSessionExpired, refreshAccessToken } from '@/utils/ApiAuth';

const HTTP_UNAUTHORIZED = 401;

interface ApiCallOptions<TRequest> {
    method: string;
    body?: TRequest;
}

export type ApiCallResult<T = void> =
    | { isSuccess: true; timestamp: number; data: T }
    | { isSuccess: false; timestamp: number; error: ApiCallError };

export type ApiCallError =
    | { status: number; type: 'failure'; code: string }
    | { status: number; type: 'invalidModel'; codes: Record<string, string[]> }
    | { status: number; type: 'unknown' };

function apiError(status: number, errorData: unknown): ApiCallError {
    if (typeof errorData === 'object' && errorData !== null) {
        const data = errorData as Record<string, unknown>;

        if (typeof data.error === 'string') {
            return { status, type: 'failure', code: data.error };
        }
        if (typeof data.errors === 'object' && data.errors !== null) {
            return { status, type: 'invalidModel', codes: data.errors as Record<string, string[]> };
        }
    }

    return { type: 'unknown', status };
}

const apiCall = async <TRequest, TResponse = void>(
    urlPath: string,
    options: ApiCallOptions<TRequest> = { method: 'GET' },
    { canRetryOnUnauthorized = true } = {}
): Promise<ApiCallResult<TResponse>> => {
    const urlBase = import.meta.env.VITE_API_BASE_URL;

    const accessToken = getAccessToken();

    const headers: Record<string, string> = {
        'Content-Type': 'application/json',
        'Authorization': accessToken ? `Bearer ${accessToken}` : ''
    };

    const fetchOptions: RequestInit = {
        method: options.method,
        body: options.body ? JSON.stringify(options.body) : undefined,
        headers,
        credentials: 'include'
    };

    console.log('API call:', options.method, `${urlBase}/${urlPath}`);

    const response = await fetch(`${urlBase}/${urlPath}`, fetchOptions);

    // INFO: If unauthorized, refresh the access token and retry the API call
    if (response.status === HTTP_UNAUTHORIZED && canRetryOnUnauthorized) {
        const isRefreshed = await refreshAccessToken();

        if (isRefreshed) {
            // INFO: Retry the API call with the new access token
            return apiCall(urlPath, options, { canRetryOnUnauthorized: false });
        }

        // INFO: If the access token cannot be refreshed, need to login again
        await handleSessionExpired();
        return { isSuccess: false, timestamp: Date.now(), error: { type: 'unknown', status: HTTP_UNAUTHORIZED }};
    }

    // INFO: If the response is not ok, return the error
    if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        return { isSuccess: false, timestamp: Date.now(), error: apiError(response.status, errorData) };
    }

    const data = await response.json().catch(() => null);
    return { isSuccess: true, timestamp: Date.now(), data };
};

export { apiCall, type ApiCallOptions };
