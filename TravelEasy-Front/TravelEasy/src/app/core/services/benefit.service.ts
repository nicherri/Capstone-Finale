import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Benefit } from '../../shared/models/benefit.model';

@Injectable({
  providedIn: 'root',
})
export class BenefitService {
  private baseUrl = 'https://localhost:44337/api/benefit';

  constructor(private http: HttpClient) {}

  getAllBenefits(): Observable<Benefit[]> {
    return this.http.get<Benefit[]>(this.baseUrl);
  }

  getBenefitById(id: number): Observable<Benefit> {
    return this.http.get<Benefit>(`${this.baseUrl}/${id}`);
  }

  addBenefit(benefit: Benefit): Observable<Benefit> {
    return this.http.post<Benefit>(this.baseUrl, benefit);
  }

  updateBenefit(id: number, benefit: Benefit): Observable<Benefit> {
    return this.http.put<Benefit>(`${this.baseUrl}/${id}`, benefit);
  }

  deleteBenefit(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
