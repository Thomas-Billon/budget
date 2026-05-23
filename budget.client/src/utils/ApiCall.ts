interface ApiCallOptions<TRequest> {
    method: string;
    body?: TRequest;
}

interface ApiCallResult {
    isSuccess: boolean;
    timestamp: number;
}

const apiCall = async <TRequest, TResponse>(
    urlPath: string,
    options: ApiCallOptions<TRequest> = { method: 'GET' }
): Promise<TResponse> => {
    const urlBase = import.meta.env.VITE_API_BASE_URL;

    const fetchOptions: RequestInit = {
        method: options.method,
        body: options.body ? JSON.stringify(options.body) : undefined,
        headers: {
            'Content-Type': 'application/json'
        }
    };

    const urlTarget = `${urlBase}/${urlPath}`;

    console.log('API call:', options.method, urlTarget);

    const response = await fetch(urlTarget, fetchOptions)
        .then(async response => {
            if (response.ok === false) {
                throw new Error(`Error: HTTP ${response.status}`);
            }

            return await response.json()
                .then(json => {
                    return json;
                })
                .catch(() => {
                    return undefined;
                });
        })
        .catch((error: unknown) => {
            const message = error instanceof Error ? error.message : 'Error: Network failure';
            throw new Error(message);
        });

    return (response as TResponse);
}

export { apiCall, type ApiCallOptions, type ApiCallResult };
