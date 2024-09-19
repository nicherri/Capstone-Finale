import { Product } from './product.model';

export interface Benefit {
id: number;
  description: string;
  productId: number;
  product?: Product;
}
