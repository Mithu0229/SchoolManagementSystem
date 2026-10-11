export interface CashBookReportDto {
  id: string;
  transactionDate: Date;
  particulars: string;
  voucherNo: string;
  accountNo: string;
  debit: number;
  credit: number;
  balance: number;
}

export interface BankBookReportDto {
  id: string;
  transactionDate: Date;
  particulars: string;
  voucherNo: string;
  bankName: string;
  accountNo: string;
  transactionNo: string;
  debit: number;
  credit: number;
  balance: number;
}

export interface IncomeStatementItemDto {
  particulars: string;
  amount: number;
}

export interface IncomeStatementDto {
  fromDate: Date;
  toDate: Date;
  revenues: IncomeStatementItemDto[];
  expenses: IncomeStatementItemDto[];
  totalRevenue: number;
  totalExpense: number;
  netIncome: number;
}

export interface BalanceSheetItemDto {
  particulars: string;
  amount: number;
}

export interface BalanceSheetDto {
  asOfDate: Date;
  assets: BalanceSheetItemDto[];
  liabilitiesAndEquity: BalanceSheetItemDto[];
  totalAssets: number;
  totalLiabilitiesAndEquity: number;
}
