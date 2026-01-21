import { Directive, ElementRef, Renderer2, OnInit } from '@angular/core';

@Directive({
    standalone: true, // ✅ standalone directive
    // selector: 'input:not([autocomplete]), textarea:not([autocomplete])'
    selector: '[disableAutofill]'
})
export class DisableAutofillDirective implements OnInit {
    constructor(
        private readonly el: ElementRef,
        private readonly renderer: Renderer2
    ) {}

    ngOnInit() {
        const element = this.el.nativeElement as HTMLInputElement | HTMLTextAreaElement;
        this.renderer.setAttribute(element, 'autocomplete', 'off');
        this.renderer.setAttribute(element, 'autocorrect', 'off');
        this.renderer.setAttribute(element, 'autocapitalize', 'off');
        this.renderer.setAttribute(element, 'spellcheck', 'false');
        this.renderer.setAttribute(element, 'name', 'no_autofill_' + Math.random().toString(36).substring(2, 15)); //
    }
}
