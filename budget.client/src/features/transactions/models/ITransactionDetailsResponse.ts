import { type CategoryColor } from '@/enums/CategoryColor';
import { type CategoryIcon } from '@/enums/CategoryIcon';
import { type TransactionType } from '@/enums/TransactionType.ts';
import { type PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransactionDetailsResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
    paymentMethod: PaymentMethod;
    comment: string;
    accountId: number;

    categories: ITransactionDetailsCategoryItemResponse[];
}

interface ITransactionDetailsCategoryItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    icon: CategoryIcon;
}

export { type ITransactionDetailsResponse, type ITransactionDetailsCategoryItemResponse };
