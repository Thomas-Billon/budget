import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onCreateSuccess?: () => void;
    onCreateError?: () => void;
}

const useCreateEntity = <TRequest extends { id: number }, TResponse>({ endpoint, onCreateSuccess, onCreateError }: Props) => {
    const entity = ref<TRequest>({} as TRequest); // TODO: Fix casting

    // #region Create

    const createResult = ref<ApiCallResult>();

    const createEntity = async (data: Partial<TRequest>): Promise<void> => {
        return apiCall<Partial<TRequest>, void>(endpoint, { method: 'POST', body: data })
            .then(() => {
                onCreateSuccess?.();
            })
            .catch(() => {
                onCreateError?.();
                
                createResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    }

    // #endregion Create

    return { entity, createEntity, createResult };
}

export default useCreateEntity;
