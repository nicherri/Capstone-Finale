import { Product } from "./product.model";

export interface Image {
  id: number;
  coverImageUrl: string;
  image1Url?: string;
  image2Url?: string;
  image3Url?: string;
  altText?: string;

  productId?: number;
  product?: Product;
}
