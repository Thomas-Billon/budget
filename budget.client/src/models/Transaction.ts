import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface Transaction {
    id: number;
    type: TransactionType;
    amount: number;
    date?: string;
    paymentMethod: PaymentMethod;
    comment: string;
}

export type { Transaction };

export const DefaultTransaction: Transaction = {
    id: 0,
    type: TransactionType.None,
    amount: 0,
    date: undefined,
    paymentMethod: PaymentMethod.None,
    comment: ''
};
