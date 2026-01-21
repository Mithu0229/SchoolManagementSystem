import { Component, signal } from '@angular/core';
import { Fluid } from 'primeng/fluid';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { MessageModule } from 'primeng/message';
import { animate, style, transition, trigger } from '@angular/animations';
import { UserService } from '../../../core/services/user.service';
import { ErrorMessageComponent } from '../../../shared/components/error-message/error-message.component';

@Component({
  selector: 'app-user-login',
  imports: [
    CheckboxModule,
    ButtonModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    CommonModule,
    MessageModule,
  ],
  standalone: true,
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent {
  constructor(
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {
    //this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
}
