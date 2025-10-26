import { CategoryColor } from "@/enums/CategoryColor";

interface ICategoryOptionsResponse {
    items: ICategoryOptionsItemResponse[];
}

interface ICategoryOptionsItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type ICategoryOptionsResponse, type ICategoryOptionsItemResponse };
