import { type CategoryColor } from '@/enums/CategoryColor';
import { type CategoryIcon } from '@/enums/CategoryIcon';
import { type TransactionType } from '@/enums/TransactionType.ts';
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
    icon: CategoryIcon;
}

export { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse, type ITransactionHistoryCategoryItemResponse };
