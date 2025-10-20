import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryTreeListResponse {
    items: ICategoryTreeListItemResponse[];
}

interface ICategoryTreeListItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
    parentCategoryId?: number;
    subCategories: ICategoryTreeListItemResponse[];
}

export { type ICategoryTreeListResponse, type ICategoryTreeListItemResponse };