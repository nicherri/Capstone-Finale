import { Product } from './product.model';
import { Wishlist } from './wishlist.model';

export interface WishlistItem {
  id: number;
  wishlistId: number;
  wishlist: Wishlist;
  productId: number;
  product: Product;
  quantity: number;
}
