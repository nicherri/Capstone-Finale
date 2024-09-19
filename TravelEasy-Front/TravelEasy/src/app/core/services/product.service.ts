import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Product } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'https://localhost:44337/api/product';

  constructor(private http: HttpClient) {}
  private fixMediaUrls(product: Product): Product {
    const baseUrl = 'https://localhost:44337'; // Corretto prefisso

    // Correggi URL delle immagini
    product.images.forEach((image) => {
      if (image.imageUrl && !image.imageUrl.startsWith(baseUrl)) {
        image.imageUrl = `${baseUrl}${image.imageUrl}`;
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

  getProducts(pageNumber: number, pageSize: number): Observable<Product[]> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<Product[]>(this.apiUrl, { params });
  }
  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`).pipe(
      map(product => this.fixMediaUrls(product))
    );
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProductWithMedia(id: number, product: any, images: File[], videos: File[]): Observable<Product> {
    const formData = new FormData();

    // Aggiungi i campi obbligatori del prodotto al FormData
    formData.append('id', product.id ? product.id.toString() : '');
    formData.append('title', product.title || '');
    formData.append('subtitle', product.subtitle || '');
    formData.append('description', product.description || '');
    formData.append('price', product.price !== undefined ? product.price.toString() : '0');
    formData.append('numberOfPieces', product.numberOfPieces !== undefined ? product.numberOfPieces.toString() : '0');
    formData.append('categoryId', product.categoryId !== undefined ? product.categoryId.toString() : '');
    formData.append('categoryName', product.categoryName || '');
    formData.append('areaId', product.areaId !== undefined ? product.areaId.toString() : '');
    formData.append('areaName', product.areaName || '');
    formData.append('shelfId', product.shelfId !== undefined ? product.shelfId.toString() : '');
    formData.append('shelfName', product.shelfName || '');
    formData.append('shelvingId', product.shelvingId !== undefined ? product.shelvingId.toString() : '');
    formData.append('shelvingName', product.shelvingName || '');

    // Estrai immagini dalla descrizione del prodotto
    const imageRegex = /<img.*?src="(.*?)"/g;
    let match;
    while ((match = imageRegex.exec(product.description)) !== null) {
      formData.append('descriptionImages', match[1]);  // Aggiungi le immagini dalla descrizione
    }

    // Estrai video dalla descrizione del prodotto
    const videoRegex = /<video.*?src="(.*?)"/g;
    while ((match = videoRegex.exec(product.description)) !== null) {
      formData.append('descriptionVideos', match[1]);  // Aggiungi i video dalla descrizione
    }

    // Aggiungi immagini selezionate al FormData
    images.forEach((image, index) => {
      formData.append(`images[${index}]`, image);
    });

    // Aggiungi video selezionati al FormData
    videos.forEach((video, index) => {
      formData.append(`videos[${index}]`, video);
    });

    // Aggiungi FAQ, benefits e reviews come JSON string
    formData.append('faqs', product.faqs ? JSON.stringify(product.faqs) : '[]');
    formData.append('benefits', product.benefits ? JSON.stringify(product.benefits) : '[]');
    formData.append('reviews', product.reviews ? JSON.stringify(product.reviews) : '[]');

    // Aggiungi prodotti correlati
    formData.append('relatedProducts', product.relatedProducts ? JSON.stringify(product.relatedProducts) : '[]');

    return this.http.put<Product>(`${this.apiUrl}/${id}`, formData);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


}
