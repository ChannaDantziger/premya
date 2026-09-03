import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MetricService } from '../../core/services/metric.service';
import { Metric } from '../../core/models/metric.model';

@Component({ selector: 'app-metrics', standalone: true, imports: [ReactiveFormsModule], templateUrl: './metrics.component.html', styleUrl: './metrics.component.scss', changeDetection: ChangeDetectionStrategy.OnPush })
export class MetricsComponent implements OnInit {
  private readonly service = inject(MetricService); private readonly fb = inject(FormBuilder);
  protected readonly metrics = signal<Metric[]>([]); protected readonly showForm = signal(false); protected readonly message = signal('');
  protected readonly form = this.fb.nonNullable.group({ premiumMethodId: [1, Validators.required], name: ['', Validators.required], description: ['', Validators.required], sourceType: ['Excel', Validators.required], sourceName: ['Employees.xlsx', Validators.required], ingestionFrequency: ['Quarterly', Validators.required] });
  ngOnInit(): void { this.load(); }
  load(): void { this.service.getAll(this.form.controls.premiumMethodId.value).subscribe({ next: data => this.metrics.set(data), error: () => this.message.set('לא ניתן לטעון את המדדים.') }); }
  create(): void { if (this.form.invalid) return; this.service.create(this.form.getRawValue()).subscribe({ next: () => { this.message.set('המדד נוצר בהצלחה.'); this.showForm.set(false); this.load(); }, error: () => this.message.set('יצירת המדד נכשלה. ודאו ששיטת הפרמיה קיימת.') }); }
}
