import type { Ref } from 'vue';

interface UseSelectionResult<TValue> {
    isSelected: (value: TValue) => boolean;
    select: (value: TValue) => void;
}

type UseSelection = {
    <TValue>(model: Ref<TValue | undefined>, emit: (event: 'change', value: TValue) => void, options: { isMultiple: false }): UseSelectionResult<TValue>;
    <TValue>(model: Ref<TValue[]>, emit: (event: 'change', value: TValue[]) => void, options: { isMultiple: true }): UseSelectionResult<TValue>;
};

const useSelection: UseSelection = <TValue>(
    model: Ref<TValue | undefined> | Ref<TValue[]>,
    emit: (event: 'change', value: TValue | TValue[]) => void,
    { isMultiple }: { isMultiple: boolean }
): UseSelectionResult<TValue> => {
    const isSelected = (value: TValue): boolean => {
        if (isMultiple) {
            return (model.value as TValue[]).includes(value);
        }
        else {
            return model.value === value;
        }
    };

    const select = (value: TValue): void => {
        if (isMultiple) {
            const values = model.value as TValue[];
            const newValues = isSelected(value) ? values.filter(v => v !== value) : [...values, value];
            (model as Ref<TValue[]>).value = newValues;
            emit('change', newValues);
        }
        else {
            (model as Ref<TValue | undefined>).value = value;
            emit('change', value);
        }
    };

    return { isSelected, select };
};

export default useSelection;
