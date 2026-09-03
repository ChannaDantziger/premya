import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface MetricDataResponse { fields: string[]; records: Record<string, unknown>[]; totalRecords: number; }

@Injectable({ providedIn: 'root' })
export class MetricDataService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5112';

  getData(metricId: number, search: string, sortBy: string, descending: boolean): Observable<MetricDataResponse> {
    let params = new HttpParams().set('page', 1).set('pageSize', 50).set('descending', descending);
    if (search) params = params.set('search', search);
    if (sortBy) params = params.set('sortBy', sortBy);
    return this.http.get<MetricDataResponse>(`${this.apiUrl}/api/metrics/${metricId}/data`, { params });
  }

  upload(metricId: number, dataYear: number, period: string, file: File): Observable<unknown> {
    const form = new FormData();
    form.append('metricId', metricId.toString()); form.append('dataYear', dataYear.toString());
    form.append('calculationPeriod', period); form.append('file', file);
    return this.http.post(`${this.apiUrl}/api/imports`, form);
  }
}
