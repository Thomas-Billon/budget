import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryHierarchyResponse {
    items: ICategoryHierarchyItemResponse[];
}

interface ICategoryHierarchyItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
    parentCategoryId?: number;
    subCategories: ICategoryHierarchyItemResponse[];
}

export { type ICategoryHierarchyResponse, type ICategoryHierarchyItemResponse };
