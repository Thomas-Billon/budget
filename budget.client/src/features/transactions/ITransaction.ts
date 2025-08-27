import { TransactionType } from '@/enums/TransactionType.ts';
import { PaymentMethod } from '@/enums/PaymentMethod.ts';

interface ITransaction {
    id: number;
    type: TransactionType;
    amount: number;
    title: string;
    date?: string;
    paymentMethod: PaymentMethod;
    comment: string;
}

export type { ITransaction };

export const DefaultTransaction: ITransaction = {
    id: 0,
    type: TransactionType.None,
    amount: 0,
    title: '',
    date: undefined,
    paymentMethod: PaymentMethod.None,
    comment: ''
};
