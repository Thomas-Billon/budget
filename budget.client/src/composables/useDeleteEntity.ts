import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onDeleteSuccess?: () => void;
    onDeleteError?: () => void;
}

const useDeleteEntity = ({ endpoint, onDeleteSuccess, onDeleteError }: Props) => {

    const deleteResult = ref<ApiCallResult>();

    const deleteEntity = async (id: number): Promise<void> => {
        if (!id) {
            return Promise.reject('Error: Cannot delete entity without id.');
        }

        const result = await apiCall<void, void>(`${endpoint}/${id}`, { method: 'DELETE' });

        deleteResult.value = result;

        if (result.isSuccess) {
            onDeleteSuccess?.();
        }
        else {
            onDeleteError?.();
        }
    };

    return { deleteEntity, deleteResult };
};

export default useDeleteEntity;
