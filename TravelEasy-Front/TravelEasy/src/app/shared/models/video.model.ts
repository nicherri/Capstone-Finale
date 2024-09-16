import { Product } from "./product.model";

export interface Video {
  id: number;
  videoUrl: string;
  altText?: string;

  productId?: number; // Aggiungi questa proprietà
  product?: Product;
}
