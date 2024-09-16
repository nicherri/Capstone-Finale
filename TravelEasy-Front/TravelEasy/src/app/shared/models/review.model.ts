import { Image } from './image.model';
import { Video } from './video.model';
import { User } from './user.model';
import { Product } from './product.model';

export interface Review {
  id: number;
  productId: number;
  product?: Product;
  userId: number;
  user?: User;
  rating: number;
  comment: string;
  createdAt: Date;
  reviewImages: Image[];
  reviewVideos: Video[];

}
