import { APP_INITIALIZER, ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { lastValueFrom, of } from 'rxjs';

import { routes } from './app.routes';
import { withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import { errorInterceptor } from './core/interceptors/error-interceptor';
import { AuthService } from './core/services/auth.service';


function initializeApp(authService: AuthService) {
  return () => lastValueFrom(
    authService.loadCurrentUser().pipe(catchError(() => of(null)))
  );
}
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor,errorInterceptor])),
    {
      provide:APP_INITIALIZER ,
      useFactory: initializeApp,
      deps:[AuthService],
      multi:true
    },
    //provideHttpClient(withInterceptors([authInterceptor,errorInterceptor]),)
    provideHttpClient(withFetch())
  ]
};
