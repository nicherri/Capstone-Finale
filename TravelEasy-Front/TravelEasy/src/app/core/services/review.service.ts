import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review } from '../../shared/models/review.model';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private baseUrl = 'https://localhost:44337/api/review';

  constructor(private http: HttpClient) {}

  getAllReviews(): Observable<Review[]> {
    return this.http.get<Review[]>(this.baseUrl);
  }

  getReviewById(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.baseUrl}/${id}`);
  }

  createReview(review: Review): Observable<Review> {
    return this.http.post<Review>(this.baseUrl, review);
  }

  updateReview(id: number, review: Review): Observable<Review> {
    return this.http.put<Review>(`${this.baseUrl}/${id}`, review);
  }

  deleteReview(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
