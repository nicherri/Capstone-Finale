import { Shelving } from './shelving.model';
import { Product } from './product.model';

export interface Shelf {
  id: number;
  name: string;
  isOccupied: boolean;
  shelvingId: number;
  shelving: Shelving;
  products: Product[];
}
