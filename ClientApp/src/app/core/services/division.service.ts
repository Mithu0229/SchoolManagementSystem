import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http.service';

export interface ApiResponse<T = any> {
    isSuccess: boolean;
    statusCode: number;
    data: T | null;
    errors: string[];
    notificationMessage: string;
}

export interface IDivision {
    id: string;
    description: string;
    divisionCode: string;
}

export interface IDivisionList {
    id: string;
    name: string;
}

@Injectable({
    providedIn: 'root'
})
export class DivisionService {
    tenantId: string | null = '';
    constructor(
        private readonly http: HttpService,
        //private readonly tenantService: TenantService
    ) {
       // this.tenantId = this.tenantService.getCurrentTenantId();
    }
    deleteDivision(id: string): Observable<ApiResponse<any>> {
        return this.http.delete(`division/delete-division/${id}`);
    }

    getDivisions(): Observable<ApiResponse<IDivisionList[]>> {
        return this.http.get(`division/get-division-by-teanantId/${this.tenantId}`);
    }

    createDivision(payload: any): Observable<ApiResponse<any>> {
        return this.http.post<ApiResponse<any>>('division/save-division', payload);
    }

    editDivision(payload: any): Observable<ApiResponse<any>> {
        return this.http.put<ApiResponse<any>>('division/update-division', payload);
    }
}
