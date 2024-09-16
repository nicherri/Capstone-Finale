import { Shelving } from './shelving.model';
import { Product } from './product.model';

export interface Area {
  id: number;
  name: string;
  isOccupied: boolean;
  shelvings: Shelving[];
  products: Product[];
}
