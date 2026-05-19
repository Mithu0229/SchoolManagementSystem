import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

export interface Shift {
  id?: string;
  shiftName: string;
  isActive: boolean;
}

export interface ShiftList {
  items: Shift[];
  totalRecord: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class ShiftService {
  private http = inject(HttpClient);
  private apiUrl = '/Shift';

  getShifts(): Observable<ApiResponse<ShiftList>> {
    return this.http.post<ApiResponse<ShiftList>>(`${this.apiUrl}/get-shift-list`, {});
  }

  getShift(id: string): Observable<ApiResponse<Shift>> {
    return this.http.get<ApiResponse<Shift>>(`${this.apiUrl}/${id}`);
  }

  createShift(shift: Shift): Observable<ApiResponse<Shift>> {
    return this.http.post<ApiResponse<Shift>>(`${this.apiUrl}/save-shift`, shift);
  }

  updateShift(shift: Shift): Observable<ApiResponse<Shift>> {
    return this.http.put<ApiResponse<Shift>>(`${this.apiUrl}/update-shift`, shift);
  }

  deleteShift(id: string): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/delete-shift/${id}`);
  }

  getShiftDropdown(): Observable<ApiResponse<any[]>> {
    return this.http.get<ApiResponse<any[]>>(`${this.apiUrl}/get-shift-dropdown`);
  }
}
