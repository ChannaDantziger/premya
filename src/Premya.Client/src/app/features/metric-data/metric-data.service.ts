import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface MetricDataResponse { fields: string[]; records: Record<string, unknown>[]; totalRecords: number; }

export interface ImportHistoryItem {
  id: number;
  metricId: number;
  fileStructureVersionId: number;
  fileName: string;
  dataYear: number;
  calculationPeriod: string;
  importedAt: string;
  status: string;
  recordCount: number;
  errorMessage: string | null;
}

@Injectable({ providedIn: 'root' })
export class MetricDataService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5112';

  getHistory(metricId: number): Observable<ImportHistoryItem[]> {
    return this.http.get<ImportHistoryItem[]>(`${this.apiUrl}/api/imports`, {
      params: new HttpParams().set('metricId', metricId)
    });
  }

  getData(metricId: number, search: string, sortBy: string, descending: boolean, importBatchId: number | null): Observable<MetricDataResponse> {
    let params = new HttpParams().set('page', 1).set('pageSize', 50).set('descending', descending);
    if (importBatchId !== null) params = params.set('importBatchId', importBatchId);
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
