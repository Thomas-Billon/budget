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
    options: ApiCallOptions<TRequest> = { method: 'GET' }
): Promise<ApiCallResult<TResponse>> => {
    const urlBase = import.meta.env.VITE_API_BASE_URL;

    const fetchOptions: RequestInit = {
        method: options.method,
        body: options.body ? JSON.stringify(options.body) : undefined,
        headers: {
            'Content-Type': 'application/json'
        }
    };

    console.log('API call:', options.method, `${urlBase}/${urlPath}`);

    const response = await fetch(`${urlBase}/${urlPath}`, fetchOptions);

    // INFO: If the response is not ok, return the error
    if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        return { isSuccess: false, timestamp: Date.now(), error: apiError(response.status, errorData) };
            }

    const data = await response.json().catch(() => null);
    return { isSuccess: true, timestamp: Date.now(), data };
};

export { apiCall, type ApiCallOptions };
