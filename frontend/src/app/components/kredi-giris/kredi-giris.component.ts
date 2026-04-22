import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FinansService } from '../../services/finans.service';

@Component({
  selector: 'app-kredi-giris',
  templateUrl: './kredi-giris.component.html',
  styleUrls: ['./kredi-giris.component.scss'],
  imports: [CommonModule, ReactiveFormsModule, DecimalPipe]
})
/**
 * Kredi giriş işlemlerinin yapıldığı, Reactive Forms tabanlı Angular bileşeni.
 * Müşteri doğrulama ve kredi kayıt süreçlerini yönetir.
 */
export class KrediGirisComponent implements OnInit {
  krediForm!: FormGroup;
  musteriUnvan: string = '';
  hesaplananToplamBorc: number = 0;
  loading: boolean = false;

  constructor(private readonly fb: FormBuilder, private readonly finansService: FinansService) { } 

  ngOnInit(): void { 
    this.initForm();

    this.krediForm.valueChanges.subscribe(() => {
      this.toplamBorcHesapla();
    });
  }

  /** Kredi giriş formunu (özellikler ve doğrulama validasyonları) başlatır */
  initForm() {
    this.krediForm = this.fb.group({
      vkn: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      krediTuru: ['Ticari', Validators.required],
      anaPara: [0, [Validators.required, Validators.min(100)]],
      dovizCinsi: ['TRY', Validators.required],
      odemeTipi: ['Aylık', Validators.required],
      vadeSayisi: [1, [Validators.required, Validators.min(1)]],
      faizOrani: [0, [Validators.required, Validators.min(0)]]
    });
  }

  /** VKN 10 haneye ulaştığında backend'e gidip müşterinin ad/unvan bilgisini çeker */
  vknSorgula() {
    const vknValue = this.krediForm.get('vkn')?.value;
    if (vknValue?.length === 10) {
      this.loading = true;
      this.finansService.getMusteri(vknValue).subscribe({
        next: (res) => {
          this.musteriUnvan = res.unvan;
          this.loading = false;
        },
        error: () => {
          this.musteriUnvan = 'Müşteri bulunamadı!';
          this.loading = false;
        }
      });
    } else {
      this.musteriUnvan = ''; 
    }
  }

  /** Girilen ana para, faiz oranı ve vade limitine göre ödenecek toplam maksimum borcu hesaplar */
  toplamBorcHesapla() {
    const { anaPara, faizOrani, vadeSayisi } = this.krediForm.value;
    this.hesaplananToplamBorc = anaPara + (anaPara * (faizOrani / 100) * vadeSayisi);
  }

  /** Form geçerliyse ve müşteri bulunduysa onaya sunmak için API'ye kredi datasını gönderir */
  kaydet() {
    if (this.krediForm.valid && this.musteriUnvan !== 'Müşteri bulunamadı!') {
      this.finansService.createLoan(this.krediForm.value).subscribe({
        next: (res) => {
          alert(`Kredi onaylandı! Toplam Borç: ${this.hesaplananToplamBorc} TL`);
          this.krediForm.reset({ krediTuru: 'Ticari', dovizCinsi: 'TRY', odemeTipi: 'Aylık', anaPara: 0, vadeSayisi: 1, faizOrani: 0 });
          this.musteriUnvan = '';
        },
        error: (err) => alert("Hata oluştu: " + err.error)
      });
    }
  }
}