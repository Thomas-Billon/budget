import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onDeleteSuccess?: () => void;
    onDeleteError?: () => void;
}

const useDeleteEntity = ({ endpoint, onDeleteSuccess, onDeleteError }: Props) => {

    const deleteEntity = async (id: number): Promise<void> => {
        if (!id) {
            return Promise.reject('Error: Cannot delete entity without id.');
        }

        apiCall<void, void>(`${endpoint}/${id}`, { method: 'DELETE' })
            .then(() => {
                onDeleteSuccess?.();

                deleteResult.value = {
                    isSuccess: true,
                    timestamp: Date.now(),
                };
            })
            .catch(() => {
                onDeleteError?.();

                deleteResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    };

    const deleteResult = ref<ApiCallResult>();

    return { deleteEntity, deleteResult };
}

export default useDeleteEntity;
