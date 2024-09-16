import { User } from './user.model';
import { OrderItem } from './order-item.model';

export interface Order {
  id: number;
  userId: number;
  user: User;
  orderDate: Date;
  status: string;
  shippingAddress: string;
  orderItems: OrderItem[];
}
