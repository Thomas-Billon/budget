import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransaction {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date?: string;
    paymentMethod: PaymentMethod;
    comment: string;
}

export type { ITransaction };

export const DefaultTransaction: ITransaction = {
    id: 0,
    type: TransactionType.None,
    amount: 0,
    reason: '',
    date: undefined,
    paymentMethod: PaymentMethod.None,
    comment: ''
};
