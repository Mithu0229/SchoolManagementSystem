import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response';

@Injectable({
  providedIn: 'root',
})
export class SitemapService {
  constructor(private readonly http: HttpService) {}

  createSitemap(payload: any): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>('sitemap', payload);
  }
  editSitemap(payload: any): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>('sitemap', payload);
  }
  getParentMenuList(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>('sitemap/get-parent-menu-list');
  }

  getFeatureList(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>('Sitemap/get-feature-list');
  }
}
