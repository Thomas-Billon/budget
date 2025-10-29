import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';
import { type ICategoryDetailsBaseResponse } from '@/features/categories/models/ICategoryDetailsResponse';

interface ITransactionDetailsResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
    paymentMethod: PaymentMethod;
    comment: string;

    categories: ICategoryDetailsBaseResponse[];
}

export { type ITransactionDetailsResponse };
