import { BlogPost } from "./blog-post.model";
import { Product } from "./product.model";
import { Review } from "./review.model";

export interface Video {
  id: number;
  videoUrl: string;
  altText?: string;

  productId?: number;
  product?: Product;
  Reviewid?: number;
  Review?: Review;
  BlogPostid?:number;
  BlogPost?:BlogPost;
}
