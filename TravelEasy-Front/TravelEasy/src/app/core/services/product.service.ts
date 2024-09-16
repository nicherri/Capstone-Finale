import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'http://localhost:5000/api/product';

  constructor(private http: HttpClient) {}

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProduct(id: number, product: Product, images: File[], videos: File[]): Observable<Product> {
    const formData = new FormData();

    // Aggiungi i campi di testo al FormData
    formData.append('title', product.title);
    formData.append('price', product.price.toString());
    formData.append('description', product.description);
    formData.append('categoryId', product.categoryId.toString());
    formData.append('areaId', product.areaid.toString());
    formData.append('shelvingId', product.shelvingId?.toString() || '');
    formData.append('shelfId', product.shelfId?.toString() || '');

    // Aggiungi immagini al FormData
    images.forEach((image, index) => {
      formData.append(`images[${index}]`, image);
    });

    // Aggiungi video al FormData
    videos.forEach((video, index) => {
      formData.append(`videos[${index}]`, video);
    });

    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
