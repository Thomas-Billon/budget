const getEnumValues = <T extends number>(enumObject: Record<string, string | T>, includeNone: boolean = true): T[] => {
    return Object.values(enumObject).filter((value): value is T => typeof value === 'number' && (includeNone || value !== 0));
};

export { getEnumValues };
