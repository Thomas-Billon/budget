import { CategoryColor } from '@/enums/CategoryColor';
import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransactionDetailsResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
    paymentMethod: PaymentMethod;
    comment: string;

    categories: ITransactionDetailsCategoryItemResponse[];
}

interface ITransactionDetailsCategoryItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

export { type ITransactionDetailsResponse, type ITransactionDetailsCategoryItemResponse };
