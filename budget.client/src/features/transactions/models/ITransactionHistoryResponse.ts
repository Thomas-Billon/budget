import { CategoryColor } from '@/enums/CategoryColor';
import { TransactionType } from '@/enums/TransactionType.ts';
import { type IPagination } from '@/utils/IPagination';

interface ITransactionHistoryResponse extends IPagination<ITransactionHistoryItemResponse> {}

interface ITransactionHistoryItemResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;

    categories: ITransactionHistoryCategoryItemResponse[];
}

interface ITransactionHistoryCategoryItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse, type ITransactionHistoryCategoryItemResponse };
