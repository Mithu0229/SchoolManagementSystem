import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { FormBase } from '../../../../core/enums/form-base';
import { TextareaComponent } from '../../../../shared/components/textarea/textarea.component';
import { FormErrorComponent } from '../../../../shared/components/form-error.component';
import { ToastService } from '../../../../core/services/toast.service';
import { UserTypes } from '../../../../core/enums/fixedIds';
import { UserService } from '../../../../core/services/user.service';

@Component({
  selector: 'app-create-user',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    FormErrorComponent,
    TextareaComponent,
    InputTextModule,
    TextareaModule,
    SelectModule,
    DatePickerModule,
    RadioButtonModule,
    ButtonModule,
  ],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.scss',
})
export class CreateUserComponent extends FormBase {
  userTypes: any[] = Object.values(UserTypes);
  form = new FormGroup({
    firstName: new FormControl(null),
    lastName: new FormControl(null),
    email: new FormControl(null, [
      Validators.required,
      Validators.maxLength(150),
    ]),
    phoneNumber: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    password: new FormControl(null, [
      Validators.required,
      Validators.maxLength(50),
    ]),
    address: new FormControl('', [Validators.maxLength(1000)]),
    isActive: new FormControl(false),
  });

  constructor(
    private readonly userService: UserService,
    private readonly route: Router,
    private readonly toastService: ToastService
  ) {
    super();
  }

  onSubmit() {
    console.log('submitted: ', this.form);
    if (!this.form.valid) {
      this.markAllAsTouched(); // show all errors
      return;
    }
    if (this.form.valid) {
      const payload = {
        firstName: this.form.value.firstName,
        lastName: this.form.value.lastName,
        email: this.form.value.email,
        phoneNumber: this.form.value.phoneNumber,
        password: this.form.value.password,
        userType: 4,
        address: this.form.value.address,
        isActive: false,
      };
      this.userService.createUser(payload).subscribe({
        next: (res) => {
          if (res.isSuccess) {
            this.toastService.success('User has been created successfully.');
            this.resetForm();
          } else {
            let errorMessage = 'Failed to create User. Please try again later.';
            if (res.notificationMessage && res.notificationMessage !== '') {
              errorMessage = res.notificationMessage;
            } else if (res.errors?.[0]) {
              errorMessage = res.errors[0];
            }
            this.toastService.error(errorMessage);
          }
        },
        error: () => {
          this.toastService.error(
            'Failed to create User. Please try again later.'
          );
        },
      });
    }
  }

  onCancel() {
    this.route.navigate(['/user']);
  }
}
