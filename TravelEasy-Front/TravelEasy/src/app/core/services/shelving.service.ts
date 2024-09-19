import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Shelving } from '../../shared/models/shelving.model';

@Injectable({
  providedIn: 'root',
})
export class ShelvingService {
  private baseUrl = 'https://localhost:44337/api/shelving';

  constructor(private http: HttpClient) {}

  getAllShelvings(): Observable<Shelving[]> {
    return this.http.get<Shelving[]>(this.baseUrl);
  }

  getShelvingsByArea(areaId: number): Observable<Shelving[]> {
    return this.http.get<Shelving[]>(`${this.baseUrl}/${areaId}`);
  }

  getShelvingById(id: number): Observable<Shelving> {
    return this.http.get<Shelving>(`${this.baseUrl}/${id}`);
  }

  createShelving(shelving: Shelving): Observable<Shelving> {
    return this.http.post<Shelving>(this.baseUrl, shelving);
  }

  updateShelving(id: number, shelving: Shelving): Observable<Shelving> {
    return this.http.put<Shelving>(`${this.baseUrl}/${id}`, shelving);
  }

  deleteShelving(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
