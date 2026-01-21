import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class HttpService {
    constructor(private readonly http: HttpClient) {}

    get<T>(endpoint: string): Observable<T> {
        return this.http.get<T>(`/${endpoint}`);
    }

    post<T>(endpoint: string, data: any): Observable<T> {
        return this.http.post<T>(`/${endpoint}`, data);
    }

    put<T>(endpoint: string, data: any): Observable<T> {
        return this.http.put<T>(`/${endpoint}`, data);
    }

    delete<T>(endpoint: string): Observable<T> {
        return this.http.delete<T>(`/${endpoint}`);
    }

    deactivate<T>(endpoint: string, data: any): Observable<T> {
        return this.http.delete<T>(`/${endpoint}`, { body: data });
    }

    deleteWithPayload<T>(endpoint: string, data: any): Observable<T> {
        return this.http.delete<T>(`/${endpoint}`, { body: data });
    }
}
