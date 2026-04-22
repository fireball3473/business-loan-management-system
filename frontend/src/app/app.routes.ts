import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { KrediGirisComponent } from './components/kredi-giris/kredi-giris.component';
import { TahsilatComponent } from './components/tahsilat/tahsilat.component';
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './guards/auth.guard';

/**
 * Uygulamanın sayfaları (route'ları) ve bu sayfalara erişim yetkilerini (guard) tanımlar.
 */
export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'kredi-giris', component: KrediGirisComponent, canActivate: [authGuard] },
  { path: 'tahsilat', component: TahsilatComponent, canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
