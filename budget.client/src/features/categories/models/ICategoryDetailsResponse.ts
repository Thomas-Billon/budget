import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryDetailsBaseResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

interface ICategoryDetailsResponse extends ICategoryDetailsBaseResponse {
    parentCategoryId?: number;
    subCategories: ICategoryDetailsBaseResponse[];
}

export { type ICategoryDetailsResponse, type ICategoryDetailsBaseResponse };
