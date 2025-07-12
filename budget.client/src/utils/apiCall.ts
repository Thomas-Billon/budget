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
            'Content-Type': 'application/json',
        }
    };

    const response = await fetch(`${urlBase}/${urlPath}`, fetchOptions)
        .then(async response => {
            if (response.status !== 200) {
                throw await response.json();
            }
            return response.json();
        })
        .catch(err => {
            console.error(err);
        });

    return (response as TResponse);
}
