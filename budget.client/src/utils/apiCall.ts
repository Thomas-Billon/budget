import { env } from 'process';

export interface ApiCallOptions {
    method?: string;
    headers?: Record<string, string>;
    body?: any;
    authToken?: string;
}

export async function apiCall<T>(
    urlPath: string,
    options: ApiCallOptions = {}
): Promise<T> {
    const urlBase = env.API_BASE_URL;

    const { method = 'GET', headers = {}, body, authToken } = options;

    const fetchHeaders: Record<string, string> = {
        'Content-Type': 'application/json',
        ...headers,
    };

    if (authToken) {
        fetchHeaders['Authorization'] = `Bearer ${authToken}`;
    }

    const fetchOptions: RequestInit = {
        method,
        headers: fetchHeaders,
    };

    if (body !== undefined) {
        fetchOptions.body = JSON.stringify(body);
    }

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
        return response.json() as Promise<T>;
    } else {
        return (response.text() as unknown) as Promise<T>;
    }
}
