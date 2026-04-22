import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FinansService } from '../../services/finans.service';

@Component({
    selector: 'app-tahsilat',
    templateUrl: './tahsilat.component.html',
    styleUrls: ['./tahsilat.component.scss'],
    imports: [CommonModule, FormsModule, DecimalPipe]
})

export class TahsilatComponent {
    vknSorgu: string = '';
    krediler: any[] = [];
    mevduatHesaplari: any[] = [];

    seciliKredi: any = null;
    seciliHesapId: number | null = null;
    tahsilatTutari: number = 0;

    loading: boolean = false;

    constructor(private readonly finansService: FinansService, private readonly changeDetectorRef: ChangeDetectorRef) { }


    kredileriGetir() {
        const vkn = this.vknSorgu.trim();

        if (vkn.length === 10) {
            this.loading = true;
            this.finansService.getActiveLoans(vkn).subscribe({
                next: (res) => {
                    this.krediler = res;
                    this.seciliKredi = null;
                    this.loading = false;
                    this.changeDetectorRef.detectChanges();
                },
                error: (err) => {
                    this.loading = false;
                    this.changeDetectorRef.detectChanges();
                }
            });
        } else {
            alert("VKN 10 haneli değil!");
        }
    }


    krediSec(kredi: any) {
        this.seciliKredi = kredi;
        this.tahsilatTutari = kredi.kalanBorc;
        this.loading = true;

        this.finansService.getAccounts(kredi.krediId).subscribe({
            next: (res: any[]) => {
                this.mevduatHesaplari = res;
                this.loading = false;
                this.changeDetectorRef.detectChanges();
            },
            error: () => {
                this.loading = false;
                this.changeDetectorRef.detectChanges();
            }
        });
    }


    odemeYap() {
        if (!this.seciliHesapId || this.tahsilatTutari <= 0) return;

        const data = {
            krediId: this.seciliKredi.krediId,
            hesapId: this.seciliHesapId,
            tahsilatTutari: this.tahsilatTutari
        };

        this.finansService.makePayment(data).subscribe({
            next: (res) => {
                alert("Tahsilat başarıyla tamamlandı, bakiye ve borç güncellendi.");
                this.kredileriGetir();
                this.seciliKredi = null;
            },
            error: (err) => alert("Hata: " + err.error)
        });
    }
}