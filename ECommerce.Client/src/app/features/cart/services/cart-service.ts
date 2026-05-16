import { Injectable, signal, computed } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';
import { CartItem } from '../../../core/models/cart-item';
import { Product } from '../../../core/models/product';
import { Cart } from '../../../core/models/cart';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cartItemsSignal = signal<CartItem[]>([]);
  public cartItems = this.cartItemsSignal.asReadonly();

  private endpoint = 'Cart';

  constructor(
    private apiService: ApiService,
    private authService: AuthService,
  ) {
    this.initializeCart();
  }

  initializeCart() {
    if (this.authService.isLoggedIn()) {
      this.loadCartFromApi();
    } else {
      this.loadCartFromStorage();
    }
  }

  // private loadCartFromStorage() {
  //   const storedCart = localStorage.getItem('cart');
  //   if (storedCart) {
  //     this.cartItemsSignal.set(JSON.parse(storedCart));
  //   }
  // }

  // private saveToStorage(items: CartItem[]) {
  //   localStorage.setItem('cart', JSON.stringify(items));
  //   this.cartItemsSignal.set(items);
  // }

  public cartCount = computed(() => {
    const items = this.cartItemsSignal() || [];
    return items.reduce((acc, item) => acc + (item.quantity || 0), 0);
  });

  private cartTotalSignal = signal<number>(0);
  public cartTotal = this.cartTotalSignal.asReadonly();
  private loadCartFromStorage() {
    const storedCart = localStorage.getItem('cart');
    if (storedCart) {
      const items: CartItem[] = JSON.parse(storedCart);
      this.cartItemsSignal.set(items);

      // تحديث إجمالي السعر للمستخدم الغير مسجل
      const total = items.reduce((acc, item) => acc + (item.subTotal || 0), 0);
      this.cartTotalSignal.set(total);
    }
  }
  applyDiscount(discountPercent: number) {
    const currentTotal = this.cartTotalSignal();
    const newTotal = currentTotal * (1 - discountPercent / 100);
    this.cartTotalSignal.set(newTotal);
  }

  private saveToStorage(items: CartItem[]) {
    localStorage.setItem('cart', JSON.stringify(items));
    this.cartItemsSignal.set(items);

    // تحديث إجمالي السعر كل مرة بنعدل فيها الكارت
    const total = items.reduce((acc, item) => acc + (item.subTotal || 0), 0);
    this.cartTotalSignal.set(total);
  }

  loadCartFromApi() {
    this.apiService.get<Cart>(this.endpoint).subscribe({
      next: (res) => {
        this.cartItemsSignal.set(res?.items || []);

        const total = res?.totalPrice || 0;
        this.cartTotalSignal.set(total);
      },
      error: (err) => {
        this.cartTotalSignal.set(0);
      },
    });
  }

  addToCart(product: Product, quantity: number = 1) {
    if (this.authService.isLoggedIn()) {
      const payload = { productId: product.id, quantity: quantity };
      this.apiService.post(`${this.endpoint}/add-item`, payload).subscribe({
        next: () => this.loadCartFromApi(),
        error: (err) => console.error('Error adding item', err),
      });
    } else {
      const currentItems = [...this.cartItemsSignal()];
      const existingItem = currentItems.find((item) => item.productId === product.id);

      if (existingItem) {
        const newQuantity = existingItem.quantity + quantity;

        // فحص المخزون
        if (newQuantity > existingItem.stockQuantity) {
          return;
        }
        existingItem.quantity += quantity;
        existingItem.subTotal = existingItem.quantity * existingItem.unitPrice;
      } else {
        currentItems.push({
          productId: product.id,
          productName: product.name,
          productImage: product.imageUrls?.[0] || 'assets/placeholder.jpg',
          unitPrice: product.price,
          quantity: quantity,
          subTotal: product.price * quantity,
          stockQuantity: product.stock,
        });
      }
      this.saveToStorage(currentItems);
    }
  }

  updateCartItem(id: number | undefined, quantity: number) {
    if (this.authService.isLoggedIn()) {
      const payload = { cartItemId: id, quantity };
      this.apiService.put(`${this.endpoint}/update-item`, payload).subscribe({
        next: () => this.loadCartFromApi(),
        error: (err) => console.error('Error updating item', err),
      });
    } else {
      const currentItems = [...this.cartItemsSignal()];
      const item = currentItems.find((i) => (i.id === id ? id : 0));
      if (item) {
        item.quantity = quantity;
        item.subTotal = item.quantity * item.unitPrice;
        this.saveToStorage(currentItems);
      }
    }
  }

  removeCartItem(id: number | undefined) {
    if (this.authService.isLoggedIn()) {
      this.apiService.delete(`${this.endpoint}/items/${id ? id : 0}`).subscribe({
        next: () => this.loadCartFromApi(),
        error: (err) => console.error('Error removing item', err),
      });
    } else {
      const currentItems = this.cartItemsSignal().filter((i) => (i.id !== id ? id : 0));
      this.saveToStorage(currentItems);
    }
  }

  clearCart() {
    if (this.authService.isLoggedIn()) {
      this.apiService.delete(`${this.endpoint}/clear`).subscribe({
        next: () => {
          this.cartItemsSignal.set([]);
          this.cartTotalSignal.set(0);
        },
        error: (err) => console.error('Error clearing cart', err),
      });
    } else {
      this.cartItemsSignal.set([]);
      localStorage.removeItem('cart');
    }
  }
  clearLocalCart() {
    this.cartItemsSignal.set([]);
    localStorage.removeItem('cart');
    this.cartTotalSignal.set(0);
  }

  syncLocalCartToDb() {
    const storedCart = localStorage.getItem('cart');
    if (storedCart) {
      const items: CartItem[] = JSON.parse(storedCart);
      this.apiService.post(`${this.endpoint}/sync`, { items }).subscribe({
        next: () => {
          localStorage.removeItem('cart');
          this.loadCartFromApi();
        },
        error: (err) => console.error('Error syncing cart', err),
      });
    } else {
      this.loadCartFromApi();
    }
  }
}
