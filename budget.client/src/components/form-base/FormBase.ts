import { type ApiCallResult } from '@/utils/ApiCall';

interface FormProps {
    isNew: boolean;
    isLoading: boolean;
    isAutoSave?: boolean;
    saveAllResult?: ApiCallResult;
    savePartialResult?: ApiCallResult;
    deleteResult?: ApiCallResult;
};

interface FormBaseProps extends FormProps {
    isFormValid: () => boolean
}

interface FormEmits<T> {
    saveAll: [data: T];
    savePartial: [id: number, data: Partial<T>];
    delete: [id: number];
};

export { type FormProps, type FormBaseProps, type FormEmits };
