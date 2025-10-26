const debounce = <T extends (...args: any[]) => void>(callback: T, delay: number): ((...args: Parameters<T>) => void) => {
    let timeoutId: ReturnType<typeof setTimeout>;
    return (...args: Parameters<T>) => {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => callback(...args), delay);
    };
};

const getIsoDay = (date: Date): number => {
    return (date.getDay() + 6) % 7;
};

export { debounce, getIsoDay };
