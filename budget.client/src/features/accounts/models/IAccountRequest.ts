import { Bank } from '@/enums/Bank';
import { Currency } from '@/enums/Currency';

interface IAccountRequest {
    id: number;
    name: string;
    bank: Bank;
    currency: Currency;
}

const getDefaultAccountRequest = (): IAccountRequest => JSON.parse(JSON.stringify({
    id: 0,
    name: '',
    bank: Bank.None,
    currency: Currency.None
}));

export { type IAccountRequest, getDefaultAccountRequest };
