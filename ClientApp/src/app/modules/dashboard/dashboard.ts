import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  imports: [ButtonModule, CardModule, FormsModule, ReactiveFormsModule],
  template: `
    <div class="flex flex-col">
      <div class="card">
        <div class="font-semibold text-xl mb-4">Home</div>
      </div>
    </div>
  `,
})
export class Dashboard {}
