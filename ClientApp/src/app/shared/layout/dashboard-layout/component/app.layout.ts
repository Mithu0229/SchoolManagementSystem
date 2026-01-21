import { Component, Renderer2, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { filter, Subscription } from 'rxjs';
// import { AppTopbar } from './app.topbar';
import { AppSidebar } from './app.sidebar';
import { LayoutService } from '../service/layout.service';
import { Breadcrumb } from 'primeng/breadcrumb';
import { AppTopbarNew } from './app-topbar-new/app-topbar-new.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, AppTopbarNew, AppSidebar],
  template: `
    <div class="layout-wrapper" [ngClass]="containerClass">
      <app-topbar-new></app-topbar-new>
      <app-sidebar></app-sidebar>
      <div class="layout-main-container">
        <div class="layout-main">
          <!-- <p-breadcrumb [home]="home" [model]="items"></p-breadcrumb> Breadcrumb-->
          <router-outlet></router-outlet>
        </div>
      </div>
      <div class="layout-mask animate-fadein"></div>
    </div>
  `,
  styles: `
        ::ng-deep .layout-main > .p-breadcrumb {
            margin-bottom: 1.5rem;
        }

        ::ng-deep .p-breadcrumb {
            background: transparent !important;
            border: none !important;
            padding: 0 !important;
            margin-bottom: 20px;
        }

        ::ng-deep .p-breadcrumb .p-menuitem-link {
            color: #23272f !important;
            font-weight: 500;
            font-size: 1rem;
            text-decoration: none;
        }

        ::ng-deep .p-breadcrumb-item-link {
            color: #b70f76 !important;
        }

        ::ng-deep .p-breadcrumb-list {
            color: #b70f76 !important;
        }

        ::ng-deep .p-breadcrumb-list li:last-child > a {
            color: #262626 !important;
        }
        ::ng-deep .p-breadcrumb-list li:last-child > a :hover {
            color: #262626 !important;
        }
        ::ng-deep .p-breadcrumb-list li :hover {
            //color: #b70f76 !important;
        }

        ::ng-deep .p-breadcrumb .p-menuitem-link:hover {
            text-decoration: underline !important;
        }

        ::ng-deep .p-breadcrumb .p-menuitem-link:last-child {
            color: #262626 !important; /* Current page */
            font-weight: 700;
            pointer-events: none;
            cursor: default;
            text-decoration: none !important;
        }

        ::ng-deep .p-breadcrumb li:last-child .p-menuitem-link {
            color: #262626 !important;
            font-weight: 700 !important;
            pointer-events: none !important;
            cursor: default !important;
            text-decoration: none !important;
        }

        ::ng-deep .p-breadcrumb .p-breadcrumb-chevron {
            color: #b6b8c3;
            font-size: 1rem;
            margin: 0 0.5rem;
        }

        .p-breadcrumb-separator {
            display: flex;
            align-items: center;
            color: var(--p-breadcrumb-separator-color);
        }

        .p-breadcrumb-item-icon {
            color: var(--p-breadcrumb-item-icon-color);
            transition: inherit;
        }

        .p-breadcrumb-item-icon:hover {
            color: var(--p-breadcrumb-item-icon-color);
            transition: inherit;
        }
        .p-breadcrumb-item-label {
            color: #262626 !important;
        }
        .p-breadcrumb .p-menuitem:last-child {
            font-weight: bold;
            color: #28a745 !important; /* Green color for the last item, representing "Tenant Registration" */
        }
    `,
})
export class AppLayout {
  overlayMenuOpenSubscription: Subscription;

  menuOutsideClickListener: any;

  items: any[] = [];

  home: any;

  @ViewChild(AppSidebar) appSidebar!: AppSidebar;

  constructor(
    public layoutService: LayoutService,
    public renderer: Renderer2,
    public router: Router
  ) {
    this.overlayMenuOpenSubscription =
      this.layoutService.overlayOpen$.subscribe(() => {
        if (!this.menuOutsideClickListener) {
          this.menuOutsideClickListener = this.renderer.listen(
            'document',
            'click',
            (event) => {
              if (this.isOutsideClicked(event)) {
                this.hideMenu();
              }
            }
          );
        }

        if (this.layoutService.layoutState().staticMenuMobileActive) {
          this.blockBodyScroll();
        }
      });

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.updateBreadcrumbs();
        this.hideMenu();
      });
    // Set home breadcrumb
    this.home = {
      // icon: 'pi pi-home',
      label: 'Home',
      routerLink: '/',
    };
  }

  updateBreadcrumbs() {
    const currentUrl = this.router.url;
    const urlSegments = currentUrl
      .split('/')
      .filter((segment) => segment !== '');

    // Always show breadcrumb for dashboard
    if (currentUrl === '/') {
      this.items = [
        {
          label: 'Dashboard',
          routerLink: '/',
        },
      ];
      return;
    }

    // Get the current route and extract breadcrumb data
    const route = this.router.routerState.snapshot.root;
    const breadcrumbItems: any[] = [];

    // Find the active route configuration
    let currentRoute = route;
    while (currentRoute.children.length) {
      const childWithData = currentRoute.children.find(
        (child) => child.outlet === 'primary'
      );
      if (childWithData) {
        currentRoute = childWithData;

        // Check if this route has breadcrumb data
        if (currentRoute.data['breadcrumb']) {
          breadcrumbItems.push({
            label: currentRoute.data['breadcrumb'],
            routerLink: currentRoute.routeConfig?.path
              ? this.buildRouterLink(currentRoute)
              : null,
          });
        }

        // Check for parent breadcrumb data
        if (currentRoute.data['parent']) {
          const parents = currentRoute.data['parent'];
          // Insert parents at the beginning of the breadcrumb trail
          for (let i = 0; i < parents.length; i++) {
            breadcrumbItems.splice(i, 0, {
              label: parents[i].label,
              routerLink: parents[i].url,
            });
          }
        }
      } else {
        break;
      }
    }

    // If breadcrumb data was found in the route, use it
    if (breadcrumbItems.length > 0) {
      this.items = breadcrumbItems;
      return;
    }

    // Default behavior for routes without breadcrumb data
    this.items = urlSegments.map((segment, index) => ({
      label: this.formatBreadcrumbLabel(segment),
      routerLink: '/' + urlSegments.slice(0, index + 1).join('/'),
    }));
  }

  buildRouterLink(route: any): string {
    let url = '';
    let current = route;

    while (current) {
      if (current.routeConfig && current.routeConfig.path) {
        url = `/${current.routeConfig.path}${url}`;
      }
      current = current.parent;
    }

    return url;
  }

  formatBreadcrumbLabel(segment: string): string {
    // Convert kebab-case or snake_case to Title Case
    return segment
      .split(/[-_]/)
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  isOutsideClicked(event: MouseEvent) {
    const sidebarEl = document.querySelector('.layout-sidebar');
    const topbarEl = document.querySelector('.layout-menu-button');
    const eventTarget = event.target as Node;

    return !(
      sidebarEl?.isSameNode(eventTarget) ||
      sidebarEl?.contains(eventTarget) ||
      topbarEl?.isSameNode(eventTarget) ||
      topbarEl?.contains(eventTarget)
    );
  }

  hideMenu() {
    this.layoutService.layoutState.update((prev) => ({
      ...prev,
      overlayMenuActive: false,
      staticMenuMobileActive: false,
      menuHoverActive: false,
    }));
    if (this.menuOutsideClickListener) {
      this.menuOutsideClickListener();
      this.menuOutsideClickListener = null;
    }
    this.unblockBodyScroll();
  }

  blockBodyScroll(): void {
    if (document.body.classList) {
      document.body.classList.add('blocked-scroll');
    } else {
      document.body.className += ' blocked-scroll';
    }
  }

  unblockBodyScroll(): void {
    if (document.body.classList) {
      document.body.classList.remove('blocked-scroll');
    } else {
      document.body.className = document.body.className.replace(
        new RegExp(
          '(^|\\b)' + 'blocked-scroll'.split(' ').join('|') + '(\\b|$)',
          'gi'
        ),
        ' '
      );
    }
  }

  get containerClass() {
    return {
      'layout-overlay':
        this.layoutService.layoutConfig().menuMode === 'overlay',
      'layout-static': this.layoutService.layoutConfig().menuMode === 'static',
      'layout-static-inactive':
        this.layoutService.layoutState().staticMenuDesktopInactive &&
        this.layoutService.layoutConfig().menuMode === 'static',
      'layout-overlay-active':
        this.layoutService.layoutState().overlayMenuActive,
      'layout-mobile-active':
        this.layoutService.layoutState().staticMenuMobileActive,
    };
  }

  ngOnDestroy() {
    if (this.overlayMenuOpenSubscription) {
      this.overlayMenuOpenSubscription.unsubscribe();
    }

    if (this.menuOutsideClickListener) {
      this.menuOutsideClickListener();
    }
  }
}
