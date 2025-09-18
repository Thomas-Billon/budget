import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransactionRequest {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date?: string;
    paymentMethod: PaymentMethod;
    comment: string;
}

export { type ITransactionRequest };
