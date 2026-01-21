import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
// import { FormErrorComponent } from '../../../../../shared/components/form-error.component';
// import { TextareaComponent } from '../../../../../shared/components/textarea/textarea.component';
import { ButtonModule } from 'primeng/button';


//import { ToastService } from '../../../../../core/services/toast.service';
import { Router } from '@angular/router';
import { FormBase } from '../../../../core/enums/form-base';
import { IRole, RoleService } from '../../../../core/services/role.service';

@Component({
    selector: 'app-edit-role',
    imports: [ReactiveFormsModule, CommonModule, InputTextModule, TextareaModule, SelectModule, ButtonModule],
    templateUrl: './edit-role.component.html',
    styleUrl: './edit-role.component.scss'
})
export class EditRoleComponent extends FormBase {
    tenantId: string | null = '';
    currentState: IRole | null = null;
    form = new FormGroup({
        name: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
        description: new FormControl('', [Validators.maxLength(1000)])
    });

    constructor(
        private readonly roleService: RoleService,
        //private readonly toastService: ToastService,
        private readonly route: Router
    ) {
        super();

        const windowState = window.history.state;
        const state = windowState.role;
        if (state) {
            this.currentState = state;
            this.form.patchValue({
                name: state.name,
                description: state.description
            });
            this.form.get('role')?.disable();
        }
    }

    onSubmit() {
        if (!this.form.valid) {
            this.markAllAsTouched(); // show all errors
            return;
        }
        if (this.form.valid) {
            const payload: IRole = {
                ...this.currentState!,
                name: this.form.value.name ?? '',
                description: this.form.value.description ?? ''
                // isActive: this.form.value.isActive ?? false
            };

            this.roleService.editRole(payload).subscribe({
                next: (res) => {
                    if (res.isSuccess) {
                        //this.toastService.success('Division has been updated successfully.');
                    } else {
                        let errorMessage = 'Failed to update division. Please try again later.';
                        if (res.notificationMessage && res.notificationMessage !== '') {
                            errorMessage = res.notificationMessage;
                        } else if (res.errors?.[0]) {
                            errorMessage = res.errors[0];
                        }
                        //this.toastService.error(errorMessage);
                    }
                },
                error: () => {
                    //this.toastService.error('Failed to update division. Please try again later.');
                }
            });
        }
    }

    onCancel() {
        this.route.navigate(['/role']);
    }
}
