import { ref } from 'vue';
import { apiCall, type ApiCallResult } from '@/utils/ApiCall';

interface Props {
    endpoint: string;
    onFullUpdateSuccess?: () => void;
    onFullUpdateError?: () => void;
    onPartialUpdateSuccess?: () => void;
    onPartialUpdateError?: () => void;
}

const useUpdateEntity = <TRequest extends { id: number }>({ endpoint, onFullUpdateSuccess, onFullUpdateError, onPartialUpdateSuccess, onPartialUpdateError }: Props) => {

    // #region Full update

    const fullUpdateResult = ref<ApiCallResult>();

    const fullUpdateEntity = async (data: TRequest): Promise<void> => {
        if (!data.id) {
            return Promise.reject('Error: Cannot update entity without id.');
        }

        const result = await apiCall<TRequest, void>(`${endpoint}/${data.id}`, { method: 'PUT', body: data });

        fullUpdateResult.value = result;

        if (result.isSuccess) {
            onFullUpdateSuccess?.();
        }
        else {
            onFullUpdateError?.();
        }
    };

    // #endregion Full update

    // #region Partial update

    const partialUpdateResult = ref<ApiCallResult>();

    const partialUpdateEntity = async (id: number, data: Partial<TRequest>): Promise<void> => {
        if (!id) {
            return Promise.reject('Error: Cannot update entity without id.');
        }

        const result = await apiCall<Partial<TRequest>, void>(`${endpoint}/${id}`, { method: 'PATCH', body: data });

        partialUpdateResult.value = result;

        if (result.isSuccess) {
            onPartialUpdateSuccess?.();
        }
        else {
            onPartialUpdateError?.();
        }
    };

    // #endregion Partial update

    return { fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult };
};

export default useUpdateEntity;
