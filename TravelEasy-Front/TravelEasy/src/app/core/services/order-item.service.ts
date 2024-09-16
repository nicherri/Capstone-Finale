import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderItem } from '../../shared/models/order-item.model';

@Injectable({
  providedIn: 'root',
})
export class OrderItemService {
  private baseUrl = 'http://localhost:5000/api/orderitems';

  constructor(private http: HttpClient) {}

  getAllOrderItems(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(this.baseUrl);
  }

  getOrderItemById(id: number): Observable<OrderItem> {
    return this.http.get<OrderItem>(`${this.baseUrl}/${id}`);
  }

  createOrderItem(orderItem: OrderItem): Observable<OrderItem> {
    return this.http.post<OrderItem>(this.baseUrl, orderItem);
  }

  updateOrderItem(id: number, orderItem: OrderItem): Observable<OrderItem> {
    return this.http.put<OrderItem>(`${this.baseUrl}/${id}`, orderItem);
  }

  deleteOrderItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

