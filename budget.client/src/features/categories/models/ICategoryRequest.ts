import { CategoryColor } from '@/enums/CategoryColor';
import { CategoryIcon } from '@/enums/CategoryIcon';

interface ICategoryRequest {
    id: number;
    name: string;
    color: CategoryColor;
    icon: CategoryIcon;
}

const getDefaultCategoryRequest = (): ICategoryRequest => JSON.parse(JSON.stringify({
    id: 0,
    name: '',
    color: CategoryColor.None,
    icon: CategoryIcon.None
}));

export { type ICategoryRequest, getDefaultCategoryRequest };
