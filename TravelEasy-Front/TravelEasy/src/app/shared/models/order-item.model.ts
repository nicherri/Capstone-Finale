import { Product } from './product.model';
import { Order } from './order.model';

export interface OrderItem {
  id: number;
  orderId: number;
  order: Order;
  productId: number;
  product: Product;
  quantity: number;
}
