import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5003/api/auth';

  private readonly loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private readonly http: HttpClient, private readonly router: Router) { }


  login(credentials: { kullaniciAdi: string, sifre: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(res => {
          if (res?.token && globalThis.window !== undefined && globalThis.localStorage) {
            localStorage.setItem('token', res.token);
            if (res.kullaniciAdSoyad) {
              localStorage.setItem('kullaniciAdSoyad', res.kullaniciAdSoyad);
            }
            this.loggedIn.next(true);
          }
        })
      );
  }


  logout() {
    if (globalThis.window !== undefined && globalThis.localStorage) {
      localStorage.removeItem('token');
      localStorage.removeItem('kullaniciAdSoyad');
    }
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }


  getToken(): string | null {
    if (typeof globalThis !== 'undefined' && globalThis.localStorage) {
      return localStorage.getItem('token');
    }
    return null;
  }

  private hasToken(): boolean {
    if (typeof globalThis !== 'undefined' && globalThis.localStorage) {
      return !!localStorage.getItem('token');
    }
    return false;
  }
}
