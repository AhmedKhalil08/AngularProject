import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Review } from '../../../core/models/review';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  endPoint = 'Review';
  apiService = inject(ApiService);

  addReview(productId: number, rating: number, comment: string) {
    const reviewData = { productId, rating, comment };
    return this.apiService.post<Review>(`${this.endPoint}`, reviewData);
  }
}
