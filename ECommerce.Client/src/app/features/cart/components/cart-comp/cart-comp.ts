import { Component, computed, inject } from '@angular/core';
import { CartService } from '../../services/cart-service';
import { Cart } from '../../../../core/models/cart';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cart-comp',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './cart-comp.html',
  styleUrl: './cart-comp.css',
})
export class CartComp {
  public readonly CartService = inject(CartService);
  Mycart = computed(() => {
    const id = this.CartService.cartItems().map((item) => item.id);
    const items = this.CartService.cartItems() || [];
    const totalPrice = items.reduce((sum, item) => sum + item.subTotal || 0, 0);
    return { items, totalPrice };
  });

  buttonRemoveItem(id: number | undefined) {
    this.CartService.removeCartItem(id ? id : 0);
  }
  buttonUpdateQuantity(id: number | undefined, quantity: number) {
    this.CartService.updateCartItem(id ? id : 0, quantity);
  }
}
