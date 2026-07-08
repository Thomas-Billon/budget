import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onCreateSuccess?: () => void;
    onCreateError?: () => void;
}

const useCreateEntity = <TRequest extends { id: number }>({ endpoint, onCreateSuccess, onCreateError }: Props) => {

    const createResult = ref<ApiCallResult>();

    const createEntity = async (data: Partial<TRequest>): Promise<void> => {
        const result = await apiCall<Partial<TRequest>, void>(endpoint, { method: 'POST', body: data });

        createResult.value = result;

        if (result.isSuccess) {
            onCreateSuccess?.();
        }
        else {
            onCreateError?.();
        }
    };

    return { createEntity, createResult };
};

export default useCreateEntity;
