export interface ApiCallOptions<TCommand> {
    method: string;
    body?: TCommand;
}

export async function apiCall<TCommand, TResponse>(
    urlPath: string,
    options: ApiCallOptions<TCommand> = { method: 'GET' }
): Promise<TResponse> {
    const urlBase = import.meta.env.VITE_API_BASE_URL;

    const fetchOptions: RequestInit = {
        method: options.method,
        body: options.body ? JSON.stringify(options.body) : undefined,
        headers: {
            //'Content-Type': 'application/json',
        }
    };

    console.log(`API Call: ${urlBase}/${urlPath}`, fetchOptions);

    const response = await fetch(`${urlBase}/${urlPath}`, fetchOptions);

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(
            `API call failed: ${response.status} ${response.statusText} - ${errorText}`
        );
    }

    // Try to parse JSON, fallback to text
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
        return response.json() as Promise<TResponse>;
    } else {
        return (response.text() as TResponse) as Promise<TResponse>;
    }
}
