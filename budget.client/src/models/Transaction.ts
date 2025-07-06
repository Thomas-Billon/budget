import { TransactionType } from '@/enums/TransactionType.ts'
import { PaymentMethod } from '@/enums/PaymentMethod.ts'

interface Transaction {
    id: number;
    type: TransactionType;
    amount: number;
    date?: string;
    paymentMethod?: PaymentMethod;
    comment?: string;
}

export type { Transaction };

export const DefaultTransaction: Transaction = {
    id: 0,
    type: TransactionType.None,
    amount: 0,
    date: new Date().toISOString().split('T')[0],
    paymentMethod: PaymentMethod.None,
    comment: '',
};
