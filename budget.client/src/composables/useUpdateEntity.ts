import { onMounted, ref, watch } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
import { useRoute } from 'vue-router';

interface Props {
    endpoint: string;
    onGetByIdSuccess?: <TResponse>(response: TResponse) => void;
    onGetByIdError?: () => void;
    onFullUpdateSuccess?: () => void;
    onFullUpdateError?: () => void;
    onPartialUpdateSuccess?: () => void;
    onPartialUpdateError?: () => void;
}

const useUpdateEntity = <TRequest extends { id: number }, TResponse>({ endpoint, onGetByIdSuccess, onGetByIdError, onFullUpdateSuccess, onFullUpdateError, onPartialUpdateSuccess, onPartialUpdateError }: Props) => {
    const route = useRoute();

    const entity = ref<TRequest>({} as TRequest); // TODO: Fix casting
    
    // #region Get by id

    // Init
    onMounted(() => {
        getFromUrl(route.params?.id);
    });

    // On route change
    watch(() => route.params?.id, (id) => {
        getFromUrl(id);
    });

    const getFromUrl = (id?: string | string[]) => {
        if (id === undefined || typeof id !== 'string') {
            return;
        }
        const entityId = parseInt(id);
        getById(entityId);
    };

    const getById = async (id: number) => {
        apiCall<void, TResponse>(`${endpoint}/${id}`, { method: 'GET' })
            .then(response => {
                onGetByIdSuccess?.(response);

                entity.value = response;
            })
            .catch(() => {
                onGetByIdError?.();
            });
    };

    // #endregion Get by id

    // #region Full update

    const fullUpdate = async (data: TRequest): Promise<void> => {
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

    const partialUpdate = async (id: number, data: Partial<TRequest>) => {
        apiCall<Partial<TRequest>, void>(`${endpoint}/${id}`, { method: 'PATCH', body: data })
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

    return { entity, fullUpdate, fullUpdateResult, partialUpdate, partialUpdateResult };
}

export default useUpdateEntity;
