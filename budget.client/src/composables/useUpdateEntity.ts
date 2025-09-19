import { onMounted, ref, watch } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';
import { useRoute } from 'vue-router';

interface Props {
    endpoint: string;
    onGetByIdSuccess?: <TResponse>(response: TResponse) => void;
    onGetByIdError?: () => void;
    onUpdateSuccess?: () => void;
    onUpdateError?: () => void;
    onPartialUpdateSuccess?: () => void;
    onPartialUpdateError?: () => void;
}

const useUpdateEntity = <TRequest, TResponse>({ endpoint, onGetByIdSuccess, onGetByIdError, onUpdateSuccess, onUpdateError, onPartialUpdateSuccess, onPartialUpdateError }: Props) => {
    const route = useRoute();

    const entity = ref<Partial<TResponse>>({});
    
    // Init
    onMounted(() => {
        const entityId = parseInt(route.params.id as string);
        getTransactionById(entityId);
    });

    // On route change
    watch(() => route.params.id, (id) => {
        const entityId = parseInt(id as string);
        getTransactionById(entityId);
    });

    const getTransactionById = async (id: number) => {
        apiCall<void, TResponse>(`${endpoint}/${id}`, { method: 'GET' })
            .then(response => {
                onGetByIdSuccess?.(response);

                entity.value = response;
            })
            .catch(() => {
                onGetByIdError?.();
            });
    };

    const updateEntity = async (data: Partial<TRequest>): Promise<void> => {
        return apiCall<Partial<TRequest>, void>(endpoint, { method: 'POST', body: data })
            .then(() => {
                onUpdateSuccess?.();

                updateResult.value = {
                    isSuccess: true,
                    timestamp: Date.now(),
                };
            })
            .catch(() => {
                onUpdateError?.();

                updateResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    }

    const updateResult = ref<ApiCallResult>();

    const updateEntityPartial = async (id: number, data: Partial<TRequest>) => {
        apiCall<Partial<TRequest>, void>(`${endpoint}/${id}`, { method: 'PATCH', body: data })
            .then(_ => {
                onPartialUpdateSuccess?.();

                updatePartialResult.value = {
                    isSuccess: true,
                    timestamp: Date.now()
                };
            })
            .catch(() => {
                onPartialUpdateError?.();
                    
                updatePartialResult.value = {
                    isSuccess: false,
                    timestamp: Date.now()
                };
            });
    };

    const updatePartialResult = ref<ApiCallResult>();

    return { entity, updateEntity, updateResult, updateEntityPartial, updatePartialResult };
}

export default useUpdateEntity;
