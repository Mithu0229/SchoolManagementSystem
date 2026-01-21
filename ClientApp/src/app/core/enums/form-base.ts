import { FormGroup, AbstractControl } from '@angular/forms';

export abstract class FormBase {
    abstract form: FormGroup;

    /** Mark all fields as touched to show errors */
    markAllAsTouched(): void {
        Object.values(this.form.controls).forEach((control) => {
            control.markAsTouched();
            control.updateValueAndValidity();
        });
    }

    /** Check if a control is invalid and touched or dirty */
    isInvalid(controlName: string): boolean {
        const control = this.getControl(controlName);
        return !!(control && control.invalid && (control.dirty || control.touched));
    }

    /** Get a form control by name */
    getControl<T = any>(controlName: string): AbstractControl<T> | null {
        return this.form.get(controlName);
    }

    /** Get raw value of the form */
    getFormValue<T = any>(): T {
        return this.form.getRawValue();
    }

    /** Reset form with optional new values */
    resetForm(values: Record<string, any> = {}): void {
        this.form.reset(values);
    }

    /** Patch values to form controls */
    patchForm(values: Record<string, any>): void {
        this.form.patchValue(values);
    }

    /** Disable all controls in the form */
    disableForm(): void {
        this.form.disable();
    }

    /** Enable all controls in the form */
    enableForm(): void {
        this.form.enable();
    }

    /** Check if the entire form is valid */
    isFormValid(): boolean {
        return this.form.valid;
    }
}
