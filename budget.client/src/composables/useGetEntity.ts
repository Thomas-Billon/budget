import { ref } from 'vue';
import { apiCall } from '@/utils/ApiCall';

interface Props<TResponse> {
    endpoint: string;
    onGetSuccess?: (response: TResponse) => void;
    onGetError?: () => void;
}

const useGetEntity = <TResponse>({ endpoint, onGetSuccess, onGetError }: Props<TResponse>) => {
    const entity = ref<TResponse>();

    const getEntity = async (id: number): Promise<void> => {
        if (!id) {
            return Promise.reject('Error: Cannot get entity without id.');
        }

        const result = await apiCall<void, TResponse>(`${endpoint}/${id}`, { method: 'GET' });

        if (result.isSuccess) {
            entity.value = result.data;
            onGetSuccess?.(result.data);
        }
        else {
            onGetError?.();
        }
    };

    return { entity, getEntity };
};

export default useGetEntity;
