import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ImportHistoryItem, MetricDataResponse, MetricDataService } from './metric-data.service';

@Component({
  selector: 'app-metric-data',
  standalone: true,
  imports: [DatePipe, ReactiveFormsModule],
  templateUrl: './metric-data.component.html',
  styleUrl: './metric-data.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MetricDataComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(MetricDataService);

  protected readonly form = this.fb.nonNullable.group({
    metricId: [1, [Validators.required, Validators.min(1)]],
    dataYear: [2026, Validators.required],
    period: ['Quarter 1', Validators.required],
    search: [''],
    sortBy: [''],
    descending: [false]
  });
  protected readonly selectedFile = signal<File | null>(null);
  protected readonly result = signal<MetricDataResponse | null>(null);
  protected readonly history = signal<ImportHistoryItem[]>([]);
  protected readonly selectedImportBatchId = signal<number | null>(null);
  protected readonly message = signal('');
  protected readonly loading = signal(false);

  ngOnInit(): void { this.refresh(); }

  refresh(): void {
    this.selectedImportBatchId.set(null);
    this.loadHistory();
    this.loadData(null);
  }

  loadHistory(): void {
    const metricId = this.form.controls.metricId.value;
    this.service.getHistory(metricId).subscribe({
      next: items => this.history.set(items),
      error: () => this.message.set('לא ניתן לטעון את היסטוריית הקליטות.')
    });
  }

  loadData(importBatchId: number | null = this.selectedImportBatchId()): void {
    this.loading.set(true);
    const value = this.form.getRawValue();
    this.service.getData(value.metricId, value.search, value.sortBy, value.descending, importBatchId).subscribe({
      next: data => { this.result.set(data); this.loading.set(false); },
      error: () => { this.message.set('לא נמצאו נתונים עבור המדד שנבחר.'); this.loading.set(false); }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile.set(input.files?.[0] ?? null);
  }

  upload(): void {
    const file = this.selectedFile();
    if (!file || this.form.invalid) return;
    const value = this.form.getRawValue();
    this.loading.set(true);
    this.service.upload(value.metricId, value.dataYear, value.period, file).subscribe({
      next: () => {
        this.message.set('הקובץ נקלט בהצלחה והתווסף להיסטוריה.');
        this.loadHistory();
        this.loadData(null);
      },
      error: () => { this.message.set('קליטת הקובץ נכשלה.'); this.loading.set(false); }
    });
  }

  selectImport(item: ImportHistoryItem): void {
    this.selectedImportBatchId.set(item.id);
    this.message.set(`נבחרה קליטה: ${item.fileName}`);
    this.loadData(item.id);
  }

  statusLabel(status: string): string {
    if (status === 'Succeeded') return 'הצלחה';
    if (status === 'Failed') return 'נכשלה';
    if (status === 'Pending') return 'בתהליך';
    return status;
  }

  sort(field: string): void {
    const current = this.form.controls.sortBy.value;
    this.form.patchValue({ sortBy: field, descending: current === field ? !this.form.controls.descending.value : false });
    this.loadData();
  }
}
