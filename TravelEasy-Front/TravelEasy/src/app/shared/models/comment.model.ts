import { User } from './user.model';
import { BlogPost } from './/blog-post.model';

export interface Comment {
  id: number;
  content: string;
  createdAt: Date;
  blogPostId: number;
  blogPost: BlogPost;
  userId: number;
  user: User;
}
