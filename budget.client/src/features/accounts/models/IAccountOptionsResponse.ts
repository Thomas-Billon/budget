import { type Bank } from '@/enums/Bank';
import { type Currency } from '@/enums/Currency';

interface IAccountOptionsResponse {
    items: IAccountOptionsItemResponse[];
}

interface IAccountOptionsItemResponse {
    id: number;
    name: string;
    bank: Bank;
    currency: Currency;
}

export { type IAccountOptionsResponse, type IAccountOptionsItemResponse };
