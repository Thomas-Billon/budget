interface IPagination<T> {
    page: T[];
    isLastPage: boolean;
}

export { type IPagination };
