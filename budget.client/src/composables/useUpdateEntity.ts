import { onMounted, ref, watch } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
import { useRoute } from 'vue-router';

interface Props<TRequest, TResponse> {
    endpoint: string;
    defaultEntity: TRequest;
    mapResponseToRequest: (response: TResponse) => TRequest;
    onGetDetailsSuccess?: (response: TResponse) => void;
    onGetDetailsError?: () => void;
    onFullUpdateSuccess?: () => void;
    onFullUpdateError?: () => void;
    onPartialUpdateSuccess?: () => void;
    onPartialUpdateError?: () => void;
}

const useUpdateEntity = <TRequest extends { id: number }, TResponse>({
    endpoint,
    defaultEntity,
    mapResponseToRequest: mapRequestToResponse,
    onGetDetailsSuccess,
    onGetDetailsError,
    onFullUpdateSuccess,
    onFullUpdateError,
    onPartialUpdateSuccess,
    onPartialUpdateError
}: Props<TRequest, TResponse>) => {
    const route = useRoute();

    const entity = ref<TRequest>(defaultEntity);
    
    // #region Get details

    // Init
    onMounted(() => {
        getFromRoute(route.params?.id);
    });

    // On route change
    watch(() => route.params?.id, (id) => {
        getFromRoute(id);
    });

    const getFromRoute = (id?: string | string[]) => {
        if (id === undefined || typeof id !== 'string') {
            return;
        }
        const entityId = parseInt(id);
        getEntityDetails(entityId);
    };

    const getEntityDetails = async (id: number): Promise<void> => {
        return apiCall<void, TResponse>(`${endpoint}/${id}`, { method: 'GET' })
            .then(response => {
                onGetDetailsSuccess?.(response);

                entity.value = mapRequestToResponse(response);
            })
            .catch(() => {
                onGetDetailsError?.();
            });
    };

    // #endregion Get details

    // #region Full update

    const fullUpdateEntity = async (data: TRequest): Promise<void> => {
        if (!data.id) {
            return Promise.reject('Error: Cannot update entity without id.');
        }

        return apiCall<TRequest, void>(`${endpoint}/${data.id}`, { method: 'PUT', body: data })
            .then(() => {
                onFullUpdateSuccess?.();

                fullUpdateResult.value = {
                    isSuccess: true,
                    timestamp: Date.now(),
                };
            })
            .catch(() => {
                onFullUpdateError?.();

                fullUpdateResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    }

    const fullUpdateResult = ref<ApiCallResult>();
    
    // #endregion Full update

    // #region Partial update

    const partialUpdateEntity = async (id: number, data: Partial<TRequest>): Promise<void> => {
        return apiCall<Partial<TRequest>, void>(`${endpoint}/${id}`, { method: 'PATCH', body: data })
            .then(_ => {
                onPartialUpdateSuccess?.();

                partialUpdateResult.value = {
                    isSuccess: true,
                    timestamp: Date.now()
                };
            })
            .catch(() => {
                onPartialUpdateError?.();
                    
                partialUpdateResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

    const partialUpdateResult = ref<ApiCallResult>();

    // #endregion Partial update

    return { entity, fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult };
}

export default useUpdateEntity;
