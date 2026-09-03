import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'metric-data', pathMatch: 'full' },
  { path: 'premium-methods', loadComponent: () => import('./features/premium-methods/premium-methods.component').then(m => m.PremiumMethodsComponent) },
  { path: 'metrics', loadComponent: () => import('./features/metrics/metrics.component').then(m => m.MetricsComponent) },
  { path: 'metric-data', loadComponent: () => import('./features/metric-data/metric-data.component').then(m => m.MetricDataComponent) },
  { path: '**', redirectTo: 'metric-data' }
];
