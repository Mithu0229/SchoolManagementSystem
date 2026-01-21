import { Component } from '@angular/core';

@Component({
    selector: 'app-background-image',
    template: `
        <div class="background-image">
            <img src="assets/images/background-image.png" alt="Background Image" />
        </div>
    `,
    styles: [
        `
            .background-image {
                width: 100%;
                height: 100%;
                position: fixed;
                top: 0;
                left: 0;
                z-index: -1;
                img {
                    width: 100%;
                    height: 100%;
                    object-fit: cover;
                }
            }
        `
    ]
})
export class BackgroundImage {}
