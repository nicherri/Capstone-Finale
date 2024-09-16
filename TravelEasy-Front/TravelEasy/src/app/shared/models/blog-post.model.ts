import { User } from './user.model';
import { Image } from './image.model';
import { Video } from './video.model';
import { Comment } from './comment.model';

export interface BlogPost {
  id: number;
  title: string;
  content: string;
  createdAt: Date;
  authorId: number;
  author: User;
  images: Image[];
  videos: Video[];
  comments: Comment[];
}
