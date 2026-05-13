import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private endpoint = 'Cart';
  constructor(private apiService: ApiService) {}

  addToCart(productId: number, quantity: number) {
    return this.apiService.post(`${this.endpoint}/add-item`, { productId, quantity });
  }
  removeFromCart(productId: number) {
    return this.apiService.delete(`${this.endpoint}/item/${productId}`);
  }
  getCart() {
    return this.apiService.get(`${this.endpoint}`);
  }
  clearCart() {
    return this.apiService.delete(`${this.endpoint}/clear`);
  }
  syncCart(cartItems: { productId: number; quantity: number }[]) {
    return this.apiService.post(`${this.endpoint}/sync`, { items: cartItems });
  }
}
