import { TransactionType } from '@/enums/TransactionType.ts';
import { type IPagination } from '@/utils/IPagination';

interface ITransactionListResponse extends IPagination<ITransactionListItemResponse> {}

interface ITransactionListItemResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date?: string;
}

export { type ITransactionListResponse, type ITransactionListItemResponse };
