import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryFlatListResponse {
    items: ICategoryFlatListItemResponse[];
}

interface ICategoryFlatListItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type ICategoryFlatListResponse, type ICategoryFlatListItemResponse };