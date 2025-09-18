import { CategoryColor } from "@/enums/CategoryColor";

interface CategoryListResponse {
    items: ICategoryListItemResponse[];
}

interface ICategoryListItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type CategoryListResponse, type ICategoryListItemResponse };