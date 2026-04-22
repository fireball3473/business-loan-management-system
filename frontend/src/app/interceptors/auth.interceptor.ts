import { HttpInterceptorFn } from '@angular/common/http';

/**
 * Giden tüm HTTP isteklerini araya girip yakalar ve Authorization Header'ı (Bearer Token) otomatik ekler.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');

  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  return next(req);
};