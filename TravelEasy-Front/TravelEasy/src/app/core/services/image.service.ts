import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Image } from '../../shared/models/image.model';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private baseUrl = 'http://localhost:5000/api/image';

  constructor(private http: HttpClient) {}

  getAllImages(): Observable<Image[]> {
    return this.http.get<Image[]>(this.baseUrl);
  }

  getImageById(id: number): Observable<Image> {
    return this.http.get<Image>(`${this.baseUrl}/${id}`);
  }

  createImage(image: Image): Observable<Image> {
    return this.http.post<Image>(this.baseUrl, image);
  }

  updateImage(id: number, image: Image): Observable<Image> {
    return this.http.put<Image>(`${this.baseUrl}/${id}`, image);
  }

  deleteImage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
