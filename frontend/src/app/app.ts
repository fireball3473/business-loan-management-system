import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  imports: [CommonModule, RouterModule],
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
  isLoggedIn$!: Observable<boolean>;

  constructor(private readonly authService: AuthService) {
    this.isLoggedIn$ = this.authService.isLoggedIn$;
  }

  kullaniciAdSoyad() {
    return localStorage.getItem('kullaniciAdSoyad') || 'Kullanıcı';
  }

  cikisYap() {
    this.authService.logout();
  }
}
