import { CategoryColor } from '@/enums/CategoryColor';

interface ICategoryRequest {
    id: number;
    name: string;
    color: CategoryColor;
    parentCategoryId?: number;
}

const defaultCategoryRequest: ICategoryRequest = {
    id: 0,
    name: '',
    color: CategoryColor.None,
    parentCategoryId: undefined
};

export { type ICategoryRequest, defaultCategoryRequest };
