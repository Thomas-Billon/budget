import { CategoryColor } from '@/enums/CategoryColor';

interface ICategoryRequest {
    id: number;
    name: string;
    color: CategoryColor;
    parentCategoryId: number | null;
}

const getDefaultCategoryRequest = (): ICategoryRequest => JSON.parse(JSON.stringify({
    id: 0,
    name: '',
    color: CategoryColor.None
}));

export { type ICategoryRequest, getDefaultCategoryRequest };
