import { TransactionType } from '@/enums/TransactionType.ts';
import { type IPagination } from '@/utils/IPagination';

interface ITransactionHistoryResponse extends IPagination<ITransactionHistoryItemResponse> {}

interface ITransactionHistoryItemResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date?: string;
}

export { type ITransactionHistoryResponse, type ITransactionHistoryItemResponse };
