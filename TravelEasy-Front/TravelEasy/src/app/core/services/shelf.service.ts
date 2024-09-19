import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Shelf } from '../../shared/models/shelf.model';

@Injectable({
  providedIn: 'root',
})
export class ShelfService {
  private baseUrl = 'https://localhost:44337/api/shelf';

  constructor(private http: HttpClient) {}

  getAllShelves(): Observable<Shelf[]> {
    return this.http.get<Shelf[]>(this.baseUrl);
  }

  getShelfById(id: number): Observable<Shelf> {
    return this.http.get<Shelf>(`${this.baseUrl}/${id}`);
  }

  getShelvesByShelving(shelvingId: number): Observable<Shelf[]> {
    return this.http.get<Shelf[]>(`${this.baseUrl}/${shelvingId}`);
  }


  createShelf(shelf: Shelf): Observable<Shelf> {
    return this.http.post<Shelf>(this.baseUrl, shelf);
  }

  updateShelf(id: number, shelf: Shelf): Observable<Shelf> {
    return this.http.put<Shelf>(`${this.baseUrl}/${id}`, shelf);
  }

  deleteShelf(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
