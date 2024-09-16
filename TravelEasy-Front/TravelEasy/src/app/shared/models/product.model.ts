import { Image } from './/image.model';
import { Review } from './review.model';
import { Video } from './video.model';
import { FAQ } from './faq.model';
import { Benefit } from './benefit.model';


export interface Product {
  id: number;
  title: string;
  subtitle: string;
  description: string;
  price: number;
  numberOfPieces: number;
  categoryId: number;
  categoryName: string;
  areaid:number;
  areaName?: string;
  shelvingId?: number;
  shelvingName?: string;
  shelfId?: number;
  shelfName?: string;
  averageRating: number;
  images: Image[];
  videos: Video[];
  reviews: Review[];
  faqs: FAQ[];
  relatedProducts: Product[];
  benefits: Benefit[];
}
