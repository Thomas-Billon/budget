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

    const fullUpdateEntity = async (data: TRequest): Promise<void> => {
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

    const partialUpdateEntity = async (id: number, data: Partial<TRequest>): Promise<void> => {
        if (!id) {
            return Promise.reject('Error: Cannot update entity without id.');
        }

        return apiCall<Partial<TRequest>, void>(`${endpoint}/${id}`, { method: 'PATCH', body: data })
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

    return { fullUpdateEntity, fullUpdateResult, partialUpdateEntity, partialUpdateResult };
}

export default useUpdateEntity;
