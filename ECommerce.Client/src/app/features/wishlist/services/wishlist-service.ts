import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { WishlistDto } from '../../../core/models/wishlist.model';

@Injectable({
  providedIn: 'root',
})
export class WishlistService {

   constructor(private api: ApiService) {}

  getWishlist() {
    return this.api.get<WishlistDto[]>('wishlist');
  }

  addToWishlist(productId: number) {
    return this.api.post<WishlistDto>('wishlist', { productId });
  }

  removeFromWishlist(id: number) {
    return this.api.delete<boolean>(`wishlist/${id}`);
  }
  clearWishlist() {
  return this.api.delete<any>('wishlist/clear');
}
}
