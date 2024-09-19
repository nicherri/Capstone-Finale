import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WishlistItem } from '../../shared/models/wishlist-item.model';

@Injectable({
  providedIn: 'root',
})
export class WishlistItemService {
  private baseUrl = 'https://localhost:44337/api/wishlistitem';

  constructor(private http: HttpClient) {}

  getAllWishlistItems(): Observable<WishlistItem[]> {
    return this.http.get<WishlistItem[]>(this.baseUrl);
  }

  getWishlistItemById(id: number): Observable<WishlistItem> {
    return this.http.get<WishlistItem>(`${this.baseUrl}/${id}`);
  }

  createWishlistItem(wishlistItem: WishlistItem): Observable<WishlistItem> {
    return this.http.post<WishlistItem>(this.baseUrl, wishlistItem);
  }

  updateWishlistItem(id: number, wishlistItem: WishlistItem): Observable<WishlistItem> {
    return this.http.put<WishlistItem>(`${this.baseUrl}/${id}`, wishlistItem);
  }

  deleteWishlistItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
