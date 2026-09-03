import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({ selector: 'app-layout', standalone: true, imports: [RouterLink, RouterLinkActive], template: `
  <div class="layout" dir="rtl"><aside><div class="brand"><span>PR</span><strong>Premya</strong></div><p class="nav-label">ניהול מערכת</p>
    <a routerLink="/premium-methods" routerLinkActive="active">שיטות פרמיה</a><a routerLink="/metrics" routerLinkActive="active">מדדים</a><a routerLink="/metric-data" routerLinkActive="active">קליטת נתונים</a>
  </aside><div class="content"><ng-content /></div></div>`, styles: [`:host{display:block}.layout{min-height:100vh;display:flex;background:#f3f6fa}aside{width:235px;background:#14213d;color:#dce5f7;padding:28px 18px}.brand{display:flex;align-items:center;gap:10px;color:white;font-size:22px;margin-bottom:48px}.brand span{background:#5271ff;border-radius:9px;padding:7px 8px;font-size:14px}.nav-label{font-size:11px;color:#8594b3;margin:0 12px 12px}aside a{display:block;color:#bac7df;text-decoration:none;padding:13px 14px;border-radius:9px;margin:4px 0;font-size:14px}aside a.active,aside a:hover{background:#293b61;color:#fff}.content{flex:1;min-width:0}@media(max-width:700px){aside{width:170px}.brand{font-size:18px;margin-bottom:28px}}`] , changeDetection: ChangeDetectionStrategy.OnPush })
export class AppLayoutComponent {}
