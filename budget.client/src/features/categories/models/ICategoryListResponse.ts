import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryListResponse {
    items: ICategoryListItemResponse[];
}

interface ICategoryListItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type ICategoryListResponse, type ICategoryListItemResponse };