import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../../shared/models/order.model';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private baseUrl = 'https://localhost:44337/api/orders';

  constructor(private http: HttpClient) {}

  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.baseUrl);
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}/${id}`);
  }

  createOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(this.baseUrl, order);
  }

  updateOrder(id: number, order: Order): Observable<Order> {
    return this.http.put<Order>(`${this.baseUrl}/${id}`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // Aggiungi questo metodo per aggiornare solo lo stato dell'ordine
  updateOrderStatus(id: number, status: string): Observable<Order> {
    return this.http.patch<Order>(`${this.baseUrl}/${id}`, { status });
  }
}
