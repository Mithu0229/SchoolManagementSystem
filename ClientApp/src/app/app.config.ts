import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import {
  provideRouter,
  withEnabledBlockingInitialNavigation,
  withInMemoryScrolling,
} from '@angular/router';
import { routes } from './app.routes';
import { MessageService } from 'primeng/api';
import { providePrimeNG } from 'primeng/config';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { apiPrefixInterceptor } from './core/interceptores/api-prefix.interceptor';
import Aura from '@primeng/themes/aura';
import { loaderInterceptor } from './core/interceptores/loader.interceptor';
import { authInterceptor } from './core/interceptores/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    // providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes)]
    provideRouter(
      routes,
      withInMemoryScrolling({
        anchorScrolling: 'enabled',
        scrollPositionRestoration: 'enabled',
      }),
      withEnabledBlockingInitialNavigation()
    ),
    provideHttpClient(
      withInterceptors([
        loaderInterceptor,
        authInterceptor,
        apiPrefixInterceptor,
      ]),
      withFetch()
    ),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } },
    }),
    MessageService,
  ],
};
