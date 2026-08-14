import { type CategoryColor } from '@/enums/CategoryColor';
import { type CategoryIcon } from '@/enums/CategoryIcon';

interface ICategoryListResponse {
    items: ICategoryListItemResponse[];
}

interface ICategoryListItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    icon: CategoryIcon;
    transactionCount: number;
}

export { type ICategoryListResponse, type ICategoryListItemResponse };
