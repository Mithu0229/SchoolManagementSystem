import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BackgroundImage } from './home.backgroundimage';

@Component({
    selector: 'home-layout',
    standalone: true,
    imports: [CommonModule, RouterModule, BackgroundImage],
    template: `
        <div class="layout-wrapper">
            <app-background-image></app-background-image>
            <div class="layout-main-container">
                <div class="layout-main">
                    <router-outlet></router-outlet>
                </div>
            </div>
        </div>
    `
})
export class HomeLayout {}
