import { Product } from './product.model';
import { Image } from './image.model';

export interface Category {
  id: number;
  name: string;
  description?: string;
  images: Image[];
  products: Product[];
}
