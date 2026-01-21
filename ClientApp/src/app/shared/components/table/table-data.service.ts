import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, shareReplay } from 'rxjs/operators';
import { TableAPIPayload } from './table.interface';
import { TableService } from '../../../core/services/table.service';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';

dayjs.extend(utc);
@Injectable({ providedIn: 'root' })
export class TableDataService {
    private cache = new Map<string, Observable<any>>();

    constructor(private tableService: TableService) {}

    private getFromCache<T>(key: string, fetchFn: () => Observable<T>, forceRefresh = false): Observable<T> {
        if (forceRefresh || !this.cache.has(key)) {
            const request$ = fetchFn().pipe(
                catchError((error) => {
                    console.error(`Error fetching table data for key ${key}:`, error);
                    return of({ items: [], totalRecord: 0 } as any as T);
                }),
                shareReplay(1)
            );
            this.cache.set(key, request$);
        }
        return this.cache.get(key)! as Observable<T>;
    }

    getTableData(apiUrl: string, state: any, columns: any[], capitalize: any, forceRefresh = false): Observable<{ items: any[]; totalRecord: number }> {
        let params: TableAPIPayload = {
            page: state.page,
            pageSize: state.pageSize,
            sortColumn: '',
            sortDirection: '',
            search: state.searchQuery ?? '',
            filters: []
        };

        // Custom filters
        if (state.customFilters && Object.keys(state.customFilters).length > 0) {
            for (const [field, value] of Object.entries(state.customFilters)) {
                if (value !== null && value !== undefined) {
                    params.filters?.push({ field, value });
                }
            }
        }

        // Sorting
        if (state.sortField) {
            let sortField = capitalize.transform(state.sortField.toString() ?? '');
            if (sortField === 'Serial') {
                sortField = capitalize.transform(columns[0].field);
            }
            params.sortColumn = sortField ?? '';
            params.sortDirection = state.sortOrder ?? '';
        }

        // Date filters
        if (state.startDate && state.endDate) {
            params.startDate = dayjs(state.startDate).format('YYYY-MM-DD');
            params.endDate = dayjs(state.endDate).format('YYYY-MM-DD');
        }

        const key = `${apiUrl}_${JSON.stringify(params)}`;

        return this.getFromCache(
            key,
            () =>
                this.tableService.getTableData(apiUrl, params).pipe(
                    map((response) => {
                        const parsed = response.data ?? {};
                        return {
                            items: parsed.items ?? [],
                            totalRecord: parsed.totalRecord ?? 0
                        };
                    })
                ),
            forceRefresh
        );
    }

    clearCache(): void {
        this.cache.clear();
        console.log('Table data cache cleared.');
    }
}
