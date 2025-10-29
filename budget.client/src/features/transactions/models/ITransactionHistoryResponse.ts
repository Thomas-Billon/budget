import { TransactionType } from '@/enums/TransactionType.ts';
import { type ICategoryDetailsBaseResponse } from '@/features/categories/models/ICategoryDetailsResponse';
import { type IPagination } from '@/utils/IPagination';

interface ITransactionHistoryResponse extends IPagination<ITransactionHistoryItemResponse> {}

interface ITransactionHistoryItemResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
    
    categories: ICategoryDetailsBaseResponse[];
}

export { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse };
