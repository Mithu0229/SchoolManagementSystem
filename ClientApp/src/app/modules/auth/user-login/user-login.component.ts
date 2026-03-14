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
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-user-login',
  imports: [
    CheckboxModule,
    Fluid,
    ButtonModule,
    ErrorMessageComponent,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    CommonModule,
    MessageModule,
  ],
  standalone: true,
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.scss',
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('500ms', style({ opacity: 1 })),
      ]),
    ]),
  ],
})
export class UserLoginComponent {
  showTwoFactor = signal(false);
  errorMessage = signal('');
  returnUrl: string = '/';
  formHeaderInfo = {
    title: 'Welcome to PersonalImp',
    description: 'Sign in to continue',
  };

  isSigningIn = signal<boolean>(false);
  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    rememberMe: new FormControl(false),
  });

  constructor(
    private readonly userService: UserService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
  ) {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  handleSubmit() {
    if (this.loginForm.valid) {
      const credentials = {
        email: this.loginForm.value.email ?? '',
        password: this.loginForm.value.password ?? '',
        rememberMe: this.loginForm.value.rememberMe ?? false,
      };
      this.isSigningIn.set(true);
      this.authService.login(credentials).subscribe({
        //userService
        next: (res) => {
          if (res.isSuccess) {
            if (res.isSuccess && res.data.token)
              this.handleTwoFactorVerified(res.data.id);
            // if (res.isSuccess) {
            if (res.data?.userType == 3) {
              this.router.navigateByUrl('/member');
            } else if (res.data?.userType == 2) {
              this.router.navigateByUrl('/admin');
            } else {
              this.router.navigateByUrl('');
            }
            //   console.log(res);

            //   //this.handleTwoFactorVerified();
            // }
          } else {
            this.isSigningIn.set(false);
            this.errorMessage.set(
              res.errors?.[0] ??
                'We couldn’t log you in. Please check your email and password.',
            );
          }
        },
        error: () => {
          this.isSigningIn.set(false);
          this.errorMessage.set('An error occurred during login.');
        },
      });
    }
  }

  // resendOtp() {
  //     this.errorMessage.set('');
  //     this.handleSubmit();
  // }
  isInvalid(controlName: string): boolean {
    const control = this.loginForm.get(controlName);
    return !!(control && control.invalid && control.touched);
  }

  handleTwoFactorVerified(userId: string = '') {
    // this.authService.getMenuList(userId ?? '').subscribe({
    //   next: (response: any) => {
    //     if (response.isSuccess) {
    //       localStorage.setItem('menuList', JSON.stringify(response.data ?? []));
    //     } else {
    //       console.error('Failed to load menu list:', response.errors);
    //     }
    //     this.isSigningIn.set(false);
    //     //this.router.navigateByUrl(this.returnUrl);
    //   },
    // });
  }
}
