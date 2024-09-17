import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Product } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'http://localhost:5000/api/product';

  constructor(private http: HttpClient) {}
  private fixMediaUrls(product: Product): Product {
    const baseUrl = 'http://localhost:5000'; // Corretto prefisso

    // Correggi URL delle immagini
    product.images.forEach((image) => {
      if (image.coverImageUrl && !image.coverImageUrl.startsWith(baseUrl)) {
        image.coverImageUrl = `${baseUrl}${image.coverImageUrl}`;
      }
      if (image.image1Url && !image.image1Url.startsWith(baseUrl)) {
        image.image1Url = `${baseUrl}${image.image1Url}`;
      }
      if (image.image2Url && !image.image2Url.startsWith(baseUrl)) {
        image.image2Url = `${baseUrl}${image.image2Url}`;
      }
      if (image.image3Url && !image.image3Url.startsWith(baseUrl)) {
        image.image3Url = `${baseUrl}${image.image3Url}`;
      }
    });

    // Correggi URL dei video
    if (product.videos) {
      product.videos.forEach((video) => {
        if (video.videoUrl && !video.videoUrl.startsWith(baseUrl)) {
          video.videoUrl = `${baseUrl}${video.videoUrl}`;
        }
      });
    }

    return product;
  }

  getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`).pipe(
      map(product => this.fixMediaUrls(product))
    );
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProduct(id: number, product: any, images: File[], videos: File[]): Observable<Product> {
    const formData = new FormData();

    // Controllo e aggiunta delle proprietà del prodotto con fallback se undefined/null
    formData.append('id', product.id ? product.id.toString() : '');
    formData.append('title', product.title || '');
    formData.append('subtitle', product.subtitle || '');
    formData.append('description', product.description || '');
    formData.append('price', product.price !== undefined ? product.price.toString() : '0');
    formData.append('numberOfPieces', product.numberOfPieces !== undefined ? product.numberOfPieces.toString() : '0');
    formData.append('categoryId', product.categoryId !== undefined ? product.categoryId.toString() : '');
    formData.append('areaId', product.areaId !== undefined ? product.areaId.toString() : '');

    formData.append('areaId', product.areaId !== undefined ? product.areaId.toString() : ''); // Usa areaId corretto

    formData.append('areaName', product.areaName || ''); // Assicurati che esista product.areaName
    formData.append('categoryName', product.categoryName || ''); // Assicurati che esista product.categoryName
    formData.append('relatedProducts', product.relatedProducts ? JSON.stringify(product.relatedProducts) : '[]');
    formData.append('shelfName', product.shelfName || ''); // Assicurati che esista product.shelfName
    formData.append('shelvingName', product.shelvingName || ''); // Assicurati che esista product.shelvingName


    // Aggiunta condizionale dei campi shelvingId e shelfId se presenti


    if (product.shelvingId) {
      formData.append('shelvingId', product.shelvingId.toString());
    }
    if (product.shelfId) {
      formData.append('shelfId', product.shelfId.toString());
    }

    // Aggiunta di immagini selezionate
    images.forEach((image, index) => {
      formData.append(`images[${index}]`, image);
    });

    // Aggiunta di video selezionati
    videos.forEach((video, index) => {
      formData.append(`videos[${index}]`, video);
    });

    // Aggiunta di FAQ, benefits e reviews come JSON string
    formData.append('faQs', product.faQs ? JSON.stringify(product.faQs) : '[]');
    formData.append('benefits', product.benefits ? JSON.stringify(product.benefits) : '[]');
    formData.append('reviews', product.reviews ? JSON.stringify(product.reviews) : '[]');

    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData);
  }




  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }



}
