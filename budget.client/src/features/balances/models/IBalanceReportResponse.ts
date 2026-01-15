import { CategoryColor } from '@/enums/CategoryColor';
import { TransactionType } from '@/enums/TransactionType.ts';

interface IBalanceReportResponse {
    totalIncome: number;
    totalExpense: number;
    netBalance: number;
    mostLucrativeTransactions: IBalanceReportTransactionItemResponse[];
    mostExpensiveTransactions: IBalanceReportTransactionItemResponse[];
    categories: IBalanceReportCategoryItemResponse[];
    incomeTransactionsByCategory: IBalanceReportTransactionsByCategoryItemResponse[];
    expenseTransactionsByCategory: IBalanceReportTransactionsByCategoryItemResponse[];
}

interface IBalanceReportTransactionItemResponse {
    id: number;
    type: TransactionType;
    amount: number;
    reason: string;
    date: string;
}

interface IBalanceReportCategoryItemResponse {
    id: number;
    name: string;
    color: CategoryColor;
    colorHex: string;
}

interface IBalanceReportTransactionsByCategoryItemResponse {
    categoryId: number;
    categoryShare: number;
    transactions: IBalanceReportTransactionItemResponse[];
}

export { type IBalanceReportResponse, type IBalanceReportTransactionItemResponse, type IBalanceReportCategoryItemResponse };
