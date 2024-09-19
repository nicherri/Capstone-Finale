import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BlogPost } from '../../shared/models/blog-post.model';

@Injectable({
  providedIn: 'root',
})
export class BlogPostService {
  private baseUrl = 'https://localhost:44337/api//blogposts';

  constructor(private http: HttpClient) {}

  getAllBlogPosts(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(this.baseUrl);
  }

  getBlogPostById(id: number): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${this.baseUrl}/${id}`);
  }

  createBlogPost(blogPost: BlogPost): Observable<BlogPost> {
    return this.http.post<BlogPost>(this.baseUrl, blogPost);
  }

  updateBlogPost(id: number, blogPost: BlogPost): Observable<BlogPost> {
    return this.http.put<BlogPost>(`${this.baseUrl}/${id}`, blogPost);
  }

  deleteBlogPost(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
