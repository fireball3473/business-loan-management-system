import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [CommonModule, ReactiveFormsModule]
})
/**
 * Kullanıcıların sisteme güvenli giriş (login) yapmasını sağlayan arayüz bileşeni.
 */
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;

  constructor(
    private readonly fb: FormBuilder, 
    private readonly authService: AuthService, 
    private readonly router: Router
  ) {
    this.loginForm = this.fb.group({
      kullaniciAdi: ['', Validators.required],
      sifre: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.authService.getToken()) { 
      this.router.navigate(['/kredi-giris']);
    }
  }

  /* Kullanıcının girdiği kimlik bilgileri geçerliyse, auth servisine yönlendirerek token bazlı oturum açma işlemini tetikler */
  onSubmit() {
    if (this.loginForm.valid) {
      this.loading = true;
      this.errorMessage = '';

      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          this.loading = false;
          this.router.navigate(['/kredi-giris']);
        },
        error: (err) => {
          this.loading = false;
          if (err.status === 401) {
            this.errorMessage = 'Kullanıcı adı veya şifre hatalı!';
          } else {
            this.errorMessage = 'Sunucuya bağlanılırken bir hata oluştu.';
          }
        }
      });
    }
  }
}
