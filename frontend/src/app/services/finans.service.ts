import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class FinansService {
    private readonly apiUrl = 'http://localhost:5003/api';

    constructor(private readonly http: HttpClient) { }


    getMusteri(vkn: string): Observable<any> {
        return this.http.get(`${this.apiUrl}/kredi/musteri-ara/${vkn}`);
    }


    createLoan(loanData: any): Observable<any> {
        return this.http.post(`${this.apiUrl}/kredi/kredi-olustur`, loanData);
    }


    getActiveLoans(vkn: string): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/tahsilat/aktif-krediler/${vkn}`);
    }


    getAccounts(krediId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/tahsilat/musteri-hesaplari/${krediId}`);
    }

    makePayment(paymentData: any): Observable<any> {
        return this.http.post(`${this.apiUrl}/tahsilat/tahsilat-yap`, paymentData);
    }
}