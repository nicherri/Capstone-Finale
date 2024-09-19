import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Image } from '../../shared/models/image.model';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private baseUrl = 'https://localhost:44337/api/image';

  constructor(private http: HttpClient) {}

  getAllImages(): Observable<Image[]> {
    return this.http.get<Image[]>(this.baseUrl);
  }

  setImageAsCover(imageId: number): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/set-cover/${imageId}`, {});
  }

  getImageById(id: number): Observable<Image> {
    return this.http.get<Image>(`${this.baseUrl}/${id}`);
  }

  createImage(image: Image): Observable<Image> {
    return this.http.post<Image>(this.baseUrl, image);
  }

  updateImage(productId: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}${productId}/images`, formData);
  }


  deleteImage(imageId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${imageId}`);
  }

}
