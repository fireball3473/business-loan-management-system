import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/interceptors/auth.interceptor';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { serverRoutes } from './app/app.routes.server';


const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(App, {
    providers: [
      provideRouter(routes),
      provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
      provideServerRendering(withRoutes(serverRoutes)),
    ],
  }, context);

export default bootstrap;