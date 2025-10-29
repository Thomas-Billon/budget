import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onCreateSuccess?: () => void;
    onCreateError?: () => void;
}

const useCreateEntity = <TRequest extends { id: number }>({ endpoint, onCreateSuccess, onCreateError }: Props) => {
    
    const createEntity = async (data: Partial<TRequest>): Promise<void> => {
        return apiCall<Partial<TRequest>, void>(endpoint, { method: 'POST', body: data })
            .then(() => {
                onCreateSuccess?.();

                createResult.value = {
                    isSuccess: true,
                    timestamp: Date.now(),
                };
            })
            .catch(() => {
                onCreateError?.();
                
                createResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    }

    const createResult = ref<ApiCallResult>();

    return { createEntity, createResult };
}

export default useCreateEntity;
