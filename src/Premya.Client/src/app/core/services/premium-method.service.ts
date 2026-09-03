import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PremiumMethod, PremiumMethodRequest } from '../models/premium-method.model';

@Injectable({ providedIn: 'root' })
export class PremiumMethodService {
  private readonly http = inject(HttpClient);
  private readonly url = 'http://localhost:5112/api/premium-methods';
  getAll(): Observable<PremiumMethod[]> { return this.http.get<PremiumMethod[]>(this.url); }
  create(request: PremiumMethodRequest): Observable<PremiumMethod> { return this.http.post<PremiumMethod>(this.url, request); }
}
