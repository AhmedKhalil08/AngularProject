import { Product } from './product';
import { Review } from './review';

export interface IProductDetails extends Product {
  reviews?: Review[];
}
