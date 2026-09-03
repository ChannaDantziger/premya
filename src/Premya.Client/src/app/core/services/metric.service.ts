import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Metric, MetricRequest } from '../models/metric.model';

@Injectable({ providedIn: 'root' })
export class MetricService {
  private readonly http = inject(HttpClient);
  private readonly url = 'http://localhost:5112/api/metrics';
  getAll(premiumMethodId: number): Observable<Metric[]> { return this.http.get<Metric[]>(this.url, { params: { premiumMethodId } }); }
  create(request: MetricRequest): Observable<Metric> { return this.http.post<Metric>(this.url, request); }
}
