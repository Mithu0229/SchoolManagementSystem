import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TrialBalanceItemDto {
  particulars: string;
  openingBalance: number;
  debitTrans: number;
  creditTrans: number;
  netTrans: number;
  dr: number;
  cr: number;
}

export interface TrialBalanceReportDto {
  items: TrialBalanceItemDto[];
}

@Injectable({
  providedIn: 'root',
})
export class TrialBalanceService {
  // baseUrl = environment.apiUrl + 'TrialBalance';
  private apiUrl = '/TrialBalance';

  constructor(private http: HttpClient) {}

  getTrialBalance(
    fromDate: string,
    toDate: string,
  ): Observable<TrialBalanceReportDto> {
    let params = new HttpParams();
    if (fromDate) params = params.append('FromDate', fromDate);
    if (toDate) params = params.append('ToDate', toDate);

    return this.http.get<TrialBalanceReportDto>(this.apiUrl, { params });
  }
}
