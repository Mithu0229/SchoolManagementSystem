import { Injectable } from '@angular/core';
import { HttpClient, HttpContext } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TableAPIPayload } from '../../shared/components/table/table.interface';
import { SKIP_LOADING } from '../interceptores/loader.interceptor';

@Injectable({
    providedIn: 'root'
})
export class TableService {
    constructor(private readonly http: HttpClient) {}
    getTableData(endpoint: string, params: TableAPIPayload): Observable<any> {
        const context = new HttpContext().set(SKIP_LOADING, true);
        return this.http.post(endpoint, params, { context });
    }
}
