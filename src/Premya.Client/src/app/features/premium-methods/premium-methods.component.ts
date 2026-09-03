import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PremiumMethodService } from '../../core/services/premium-method.service';
import { PremiumMethod } from '../../core/models/premium-method.model';

@Component({ selector: 'app-premium-methods', standalone: true, imports: [ReactiveFormsModule], templateUrl: './premium-methods.component.html', styleUrl: './premium-methods.component.scss', changeDetection: ChangeDetectionStrategy.OnPush })
export class PremiumMethodsComponent implements OnInit {
  private readonly service = inject(PremiumMethodService); private readonly fb = inject(FormBuilder);
  protected readonly methods = signal<PremiumMethod[]>([]); protected readonly showForm = signal(false); protected readonly message = signal('');
  protected readonly form = this.fb.nonNullable.group({ methodNumber: ['', Validators.required], description: ['', Validators.required], premiumRate: [5, [Validators.required, Validators.min(0), Validators.max(100)]], calculationPeriod: ['Quarter', Validators.required] });
  ngOnInit(): void { this.load(); }
  load(): void { this.service.getAll().subscribe({ next: data => this.methods.set(data), error: () => this.message.set('לא ניתן לטעון את שיטות הפרמיה.') }); }
  create(): void { if (this.form.invalid) return; this.service.create(this.form.getRawValue()).subscribe({ next: () => { this.message.set('שיטת הפרמיה נוצרה בהצלחה.'); this.form.reset({ methodNumber: '', description: '', premiumRate: 5, calculationPeriod: 'Quarter' }); this.showForm.set(false); this.load(); }, error: () => this.message.set('יצירת שיטת הפרמיה נכשלה.') }); }
}
