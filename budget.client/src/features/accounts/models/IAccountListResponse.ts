import { type Bank } from '@/enums/Bank';
import { type Currency } from '@/enums/Currency';

interface IAccountListResponse {
    items: IAccountListItemResponse[];
}

interface IAccountListItemResponse {
    id: number;
    name: string;
    bank: Bank;
    currency: Currency;
    transactionCount: number;
}

export { type IAccountListResponse, type IAccountListItemResponse };
