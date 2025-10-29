import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onDeleteSuccess?: () => void;
    onDeleteError?: () => void;
}

const useDeleteEntity = <TRequest extends { id: number }, TResponse>({ endpoint, onDeleteSuccess, onDeleteError }: Props) => {
    // #region Delete

    const deleteResult = ref<ApiCallResult>();

    const deleteEntity = async (id: number): Promise<void> => {
        apiCall<void, TResponse>(`${endpoint}/${id}`, { method: 'DELETE' })
            .then(() => {
                onDeleteSuccess?.();
            })
            .catch(() => {
                onDeleteError?.();

                deleteResult.value = {
                    isSuccess: false,
                    timestamp: Date.now(),
                };
            });
    };

    // #endregion Delete

    return { deleteEntity, deleteResult };
}

export default useDeleteEntity;
