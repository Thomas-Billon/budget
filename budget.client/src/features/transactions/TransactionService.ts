export const parseAmount = (value: string): number => {
    return parseFloat(value.replace(/[^0-9.,]/g, '').replace(/,/g, '.'));
};

export const formatAmount = (value?: number, { isFalsyValueAllowed } = { isFalsyValueAllowed: false }): string => {
    return value || isFalsyValueAllowed ? (value ?? 0).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '';
};
