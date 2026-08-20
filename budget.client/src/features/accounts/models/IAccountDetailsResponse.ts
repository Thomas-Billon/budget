import { type Bank } from '@/enums/Bank';
import { type Currency } from '@/enums/Currency';

interface IAccountDetailsResponse {
    id: number;
    name: string;
    bank: Bank;
    currency: Currency;
}

export { type IAccountDetailsResponse };
