import { type ApiCallResult } from "@/utils/ApiCall";

interface FormProps {
    isNew: boolean;
    saveAllResult?: ApiCallResult;
    savePartialResult?: ApiCallResult;
    deleteResult?: ApiCallResult;
};

interface FormEmits<T> {
    saveAll: [data: T];
    savePartial: [id: number, data: Partial<T>];
    delete: [id: number];
};

export { type FormProps, type FormEmits };