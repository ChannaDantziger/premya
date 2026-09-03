import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppLayoutComponent } from './layout/components/app-layout.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, AppLayoutComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {}
