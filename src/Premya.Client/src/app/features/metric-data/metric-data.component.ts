import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MetricDataResponse, MetricDataService } from './metric-data.service';

@Component({ selector: 'app-metric-data', standalone: true, imports: [ReactiveFormsModule], templateUrl: './metric-data.component.html', styleUrl: './metric-data.component.scss', changeDetection: ChangeDetectionStrategy.OnPush })
export class MetricDataComponent {
  private readonly fb = inject(FormBuilder); private readonly service = inject(MetricDataService);
  protected readonly form = this.fb.nonNullable.group({ metricId: [1, [Validators.required, Validators.min(1)]], dataYear: [2026, Validators.required], period: ['Quarter 1', Validators.required], search: [''], sortBy: [''], descending: [false] });
  protected readonly selectedFile = signal<File | null>(null); protected readonly result = signal<MetricDataResponse | null>(null); protected readonly message = signal(''); protected readonly loading = signal(false);
  loadData(): void { this.loading.set(true); const v = this.form.getRawValue(); this.service.getData(v.metricId, v.search, v.sortBy, v.descending).subscribe({ next: data => { this.result.set(data); this.loading.set(false); }, error: () => { this.message.set('לא נמצאו נתונים עבור המדד שנבחר.'); this.loading.set(false); } }); }
  onFileSelected(event: Event): void { const input = event.target as HTMLInputElement; this.selectedFile.set(input.files?.[0] ?? null); }
  upload(): void { const file = this.selectedFile(); if (!file || this.form.invalid) return; const v = this.form.getRawValue(); this.loading.set(true); this.service.upload(v.metricId, v.dataYear, v.period, file).subscribe({ next: () => { this.message.set('הקובץ נקלט בהצלחה.'); this.loadData(); }, error: () => { this.message.set('קליטת הקובץ נכשלה.'); this.loading.set(false); } }); }
  sort(field: string): void { const current = this.form.controls.sortBy.value; this.form.patchValue({ sortBy: field, descending: current === field ? !this.form.controls.descending.value : false }); this.loadData(); }
}
