import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface Transaction {
    id: number;
    type: TransactionType;
    amount: number;
    title: string;
    date?: string;
    paymentMethod: PaymentMethod;
    comment: string;
}

export type { Transaction };

export const DefaultTransaction: Transaction = {
    id: 0,
    type: TransactionType.None,
    amount: 0,
    title: '',
    date: undefined,
    paymentMethod: PaymentMethod.None,
    comment: ''
};
