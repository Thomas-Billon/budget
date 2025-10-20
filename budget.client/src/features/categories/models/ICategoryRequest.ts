import { CategoryColor } from '@/enums/CategoryColor';

interface ICategoryRequest {
    id: number;
    name: string;
    color: CategoryColor;
    parentCategoryId?: number;
}

export { type ICategoryRequest };
