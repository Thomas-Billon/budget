import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransactionRequest {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
    paymentMethod: PaymentMethod;
    comment: string;
    categoryIds: number[];
}

const getDefaultTransactionRequest = ():ITransactionRequest => JSON.parse(JSON.stringify({
    id: 0,
    type: TransactionType.None,
    amount: 0,
    reason: '',
    date: '',
    paymentMethod: PaymentMethod.None,
    comment: '',
    categoryIds: []
}));

export { type ITransactionRequest, getDefaultTransactionRequest };
