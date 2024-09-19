import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Video } from '../../shared/models/video.model';

@Injectable({
  providedIn: 'root',
})
export class VideoService {
  private baseUrl = 'https://localhost:44337/api/videos';

  constructor(private http: HttpClient) {}

  getAllVideos(): Observable<Video[]> {
    return this.http.get<Video[]>(this.baseUrl);
  }

  getVideoById(id: number): Observable<Video> {
    return this.http.get<Video>(`${this.baseUrl}/${id}`);
  }

  createVideo(video: Video): Observable<Video> {
    return this.http.post<Video>(this.baseUrl, video);
  }

  updateVideo(productId: number, formData: FormData): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/product/${productId}/videos`, formData);
  }


  deleteVideo(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
