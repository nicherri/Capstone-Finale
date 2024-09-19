import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Area } from '../../shared/models/area.model';


@Injectable({
  providedIn: 'root',
})
export class AreaService {
  private apiUrl = 'https://localhost:44337/api/area';

  constructor(private http: HttpClient) {}

  getAllAreas(): Observable<Area[]> {
    return this.http.get<Area[]>(this.apiUrl);
  }

  getAreaById(id: number): Observable<Area> {
    return this.http.get<Area>(`${this.apiUrl}/${id}`);
  }

  createArea(area: Area): Observable<Area> {
    return this.http.post<Area>(this.apiUrl, area);
  }

  updateArea(id: number, area: Area): Observable<Area> {
    return this.http.put<Area>(`${this.apiUrl}/${id}`, area);
  }

  deleteArea(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
