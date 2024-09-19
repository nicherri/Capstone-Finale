import { BlogPost } from './blog-post.model';
import { Review } from './review.model';
import { Category } from './category.model';
import { Product } from "./product.model";

export interface Image {
  id: number;
  imageUrl?: string;
  altText?: string;
  isCover: boolean;
  productId?: number;
  product?: Product;
  categoryid?:number;
  Category?: Category;
  Reviewid?: number;
  Review?: Review;
  BlogPostid?:number;
  BlogPost?:BlogPost;
}
