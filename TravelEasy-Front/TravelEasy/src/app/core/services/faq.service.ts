import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FAQ } from '../../shared/models/faq.model';

@Injectable({
  providedIn: 'root',
})
export class FAQService {
  private baseUrl = 'https://localhost:44337/api/FAQ';

  constructor(private http: HttpClient) {}

  getAllFAQs(): Observable<FAQ[]> {
    return this.http.get<FAQ[]>(this.baseUrl);
  }

  getFAQById(id: number): Observable<FAQ> {
    return this.http.get<FAQ>(`${this.baseUrl}/${id}`);
  }

  createFAQ(faq: FAQ): Observable<FAQ> {
    return this.http.post<FAQ>(this.baseUrl, faq);
  }

  updateFAQ(id: number, faq: FAQ): Observable<FAQ> {
    return this.http.put<FAQ>(`${this.baseUrl}/${id}`, faq);
  }

  deleteFAQ(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
