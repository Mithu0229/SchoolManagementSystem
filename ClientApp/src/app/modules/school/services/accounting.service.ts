import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BankBookReportDto, CashBookReportDto } from '../models/accounting.model';

@Injectable({
  providedIn: 'root'
})
export class AccountingService {
  // private baseUrl = environment.baseUrl + 'api/accounting';
  private baseUrl = '/accounting';

  constructor(private http: HttpClient) { }

  getCashBookReport(startDate?: string, endDate?: string): Observable<CashBookReportDto[]> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    return this.http.get<CashBookReportDto[]>(`${this.baseUrl}/cashbook`, { params });
  }

  getBankBookReport(startDate?: string, endDate?: string): Observable<BankBookReportDto[]> {
    let params = new HttpParams();
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);
    return this.http.get<BankBookReportDto[]>(`${this.baseUrl}/bankbook`, { params });
  }

  getIncomeStatement(fromDate: string, toDate: string): Observable<any> {
    let params = new HttpParams().set('fromDate', fromDate).set('toDate', toDate);
    return this.http.get<any>(`${this.baseUrl}/income-statement`, { params });
  }

  getBalanceSheet(asOfDate: string): Observable<any> {
    let params = new HttpParams().set('asOfDate', asOfDate);
    return this.http.get<any>(`${this.baseUrl}/balance-sheet`, { params });
  }
}
