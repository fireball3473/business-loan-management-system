import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Kredi onay, müşteri arama ve tahsilat işlemleri için backend API ile iletişimi kuran servis.
 */
@Injectable({ providedIn: 'root' })
export class FinansService {
    private readonly apiUrl = 'http://localhost:5003/api';

    constructor(private readonly http: HttpClient) { }

    /** VKN'ye göre veritabanından müşteri getirir */
    getMusteri(vkn: string): Observable<any> {
        return this.http.get(`${this.apiUrl}/kredi/musteri-ara/${vkn}`);
    }

    /** Müşteriye yeni bir kredi hesabı oluşturulması için backend'e veri gönderir */
    createLoan(loanData: any): Observable<any> {
        return this.http.post(`${this.apiUrl}/kredi/kredi-olustur`, loanData);
    }

    /** Müşterinin henüz ödenmemiş (kalan borcu olan) kredilerini listeler */
    getActiveLoans(vkn: string): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/tahsilat/aktif-krediler/${vkn}`);
    }

    /** Seçilen kredi borcunu ödemek için müşterinin kullanılabileceği mevduat hesaplarını getirir */
    getAccounts(krediId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/tahsilat/musteri-hesaplari/${krediId}`);
    }
    /** Seçili hesaptan krediye tahsilat (ödeme) işleminin yapılmasını sağlar */
    makePayment(paymentData: any): Observable<any> {
        return this.http.post(`${this.apiUrl}/tahsilat/tahsilat-yap`, paymentData);
    }
}