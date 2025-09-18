import { CategoryColor } from '@/enums/CategoryColor';

interface ICategoryRequest {
    id: number;
    name: string;
    color: CategoryColor;
}

export { type ICategoryRequest };
