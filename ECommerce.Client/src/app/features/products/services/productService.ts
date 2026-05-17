import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Product } from '../../../core/models/product';
import { IProductDetails } from '../../../core/models/product-details';
import { Review } from '../../../core/models/review';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private Endpoint = 'products';
  constructor(private apiService: ApiService) {}

  getProducts() {
    return this.apiService.get<Product[]>(this.Endpoint);
  }

  getProductById(id: number) {
    return this.apiService.get<IProductDetails>(`${this.Endpoint}/${id}`);
  }

  addReview(productId: number, review: Partial<Review>) {
    return this.apiService.post<Review>(`${this.Endpoint}/${productId}/reviews`, review);
  }

  getProductReviews(productId: number) {
    return this.apiService.get<Review[]>(`${this.Endpoint}/${productId}/reviews`);
  }
}
