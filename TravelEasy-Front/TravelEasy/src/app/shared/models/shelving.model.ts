import { Area } from './area.model';
import { Shelf } from './shelf.model';
import { Product } from './product.model';

export interface Shelving {
  id: number;
  name: string;
  areaId: number;
  area: Area;
  isOccupied: boolean;
  shelves: Shelf[];
  products: Product[];
}
