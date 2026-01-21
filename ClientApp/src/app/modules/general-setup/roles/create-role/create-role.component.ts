import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ButtonModule } from 'primeng/button';
//import { FormErrorComponent } from '../../../../../shared/components/form-error.component';
//import { FormBase } from '../../../../../core/form-base/form-base';
//import { TextareaComponent } from '../../../../../shared/components/textarea/textarea.component';
// import { DivisionService } from '../../../../../core/services/division.service';
// import { TenantService } from '../../../../../core/services/tenant.service';
// import { ToastService } from '../../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { RoleService } from '../../../../core/services/role.service';
import { FormBase } from '../../../../core/enums/form-base';
import { TextareaComponent } from '../../../../shared/components/textarea/textarea.component';

@Component({
    selector: 'app-create-role',
    imports: [ReactiveFormsModule, CommonModule,TextareaComponent, InputTextModule, TextareaModule, SelectModule, DatePickerModule, RadioButtonModule, ButtonModule],
    templateUrl: './create-role.component.html',
    styleUrl: './create-role.component.scss'
})
export class CreateRoleComponent extends FormBase{
    
    divisions: any = [];
    characterCount: number = 0;
    form = new FormGroup({
        name: new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
        description: new FormControl('', [Validators.maxLength(1000)])
    });

    constructor(
        private readonly roleService: RoleService,
        private readonly route: Router
    ) {
         super();
    }

    onSubmit() {
        console.log('submitted: ', this.form);
        if (!this.form.valid) {
           // this.markAllAsTouched(); // show all errors
            return;
        }
        if (this.form.valid) {
            const payload = {
                name: this.form.value.name,
                description: this.form.value.description,
            };
            this.roleService.createRole(payload).subscribe({
                next: (res) => {
                    if (res.isSuccess) {
                        //this.toastService.success('Division has been created successfully.');
                        //this.resetForm();
                    } else {
                        let errorMessage = 'Failed to create division. Please try again later.';
                        if (res.notificationMessage && res.notificationMessage !== '') {
                            errorMessage = res.notificationMessage;
                        } else if (res.errors?.[0]) {
                            errorMessage = res.errors[0];
                        }
                      //  this.toastService.error(errorMessage);
                    }
                },
                error: () => {
                    //this.toastService.error('Failed to create division. Please try again later.');
                }
            });
        }
    }

    onCancel() {
        this.route.navigate(['/role']);
    }
}
