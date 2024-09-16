import { WishlistItem } from './wishlist-item.model';
import { User } from './user.model';

export interface Wishlist {
  id: number;
  name: string;
  userId: number;
  user: User;
  wishlistItems: WishlistItem[];
}
