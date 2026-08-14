import { type CategoryColor } from '@/enums/CategoryColor';
import { type CategoryIcon } from '@/enums/CategoryIcon';

interface ICategoryOptionsResponse {
    items: ICategoryOptionsItemResponse[];
}

interface ICategoryOptionsItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    icon: CategoryIcon;
}

export { type ICategoryOptionsResponse, type ICategoryOptionsItemResponse };
