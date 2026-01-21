import { CommonModule } from '@angular/common';
import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { TextareaModule } from 'primeng/textarea';

@Component({
    selector: 'app-textarea',
    imports: [TextareaModule, CommonModule, FormsModule],
    templateUrl: './textarea.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TextareaComponent),
            multi: true
        }
    ]
})
export class TextareaComponent implements ControlValueAccessor {
    @Input() maxLength: string = '1000';
    @Input() placeholder: string = '';
    inputValue: string = '';
    characterCount: number = 0;

    onChange = (_: any) => {};
    onTouched = () => {};

    writeValue(value: string): void {
        this.inputValue = value || '';
        this.updateCharacterCount();
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    updateCharacterCount(): void {
        this.characterCount = this.inputValue.length;
        this.onTouched();
        this.onChange(this.inputValue);
    }
}
