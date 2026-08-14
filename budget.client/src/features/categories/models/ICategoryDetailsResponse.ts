import { type CategoryColor } from '@/enums/CategoryColor';
import { type CategoryIcon } from '@/enums/CategoryIcon';

interface ICategoryDetailsResponse {
    id: number;
    name: string;
    color: CategoryColor;
    icon: CategoryIcon;
}

export { type ICategoryDetailsResponse };
