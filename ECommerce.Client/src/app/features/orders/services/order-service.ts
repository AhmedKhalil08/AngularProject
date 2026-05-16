import { Injectable, signal } from '@angular/core'; // استيراد signal العادي
import { ApiService } from '../../../core/services/api.service';
import { OrderResult } from '../../../core/models/order-result';
import { OrderRequest } from '../../../core/models/order-request';
import { Router } from '@angular/router';
import { MyOrder } from '../../../core/models/my-order';
import { AllOrder } from '../../../core/models/all-order';
import { CartService } from '../../cart/services/cart-service';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private orderEndpoint = 'Orders';
  private MyorderResultSignal = signal<MyOrder[]>([]);
  private AllOrderResultSignal = signal<AllOrder[]>([]);

  public myOrders = this.MyorderResultSignal.asReadonly();
  public allOrders = this.AllOrderResultSignal.asReadonly();

  private orderResultSignal = signal<OrderResult | null>(null);
  public orderResult = this.orderResultSignal.asReadonly();

  constructor(
    private apiService: ApiService,
    private router: Router,
    private cartService: CartService,
  ) {}

  loadAllOrders(): void {
    this.apiService.get<AllOrder[]>('Orders/AllOrders').subscribe({
      next: (res) => {
        this.AllOrderResultSignal.set(res || []);
      },
      error: (err) => {
        console.error('Failed to load orders:', err);
      },
    });
  }
  placeOrder(orderData: OrderRequest): void {
    this.apiService.post<OrderResult>(this.orderEndpoint, orderData).subscribe({
      next: (res) => {
        this.orderResultSignal.set(res);
        if (res.isSuccess) {
          alert('Order placed successfully!');
          if (res.paymentUrl) {
            window.location.href = res.paymentUrl;
          } else {
            this.cartService.clearCart();
            this.router.navigate(['/checkout/success']);
          }
        } else {
          alert('Failed to place order: ' + res.message);
          this.router.navigate(['/checkout/failed']);
        }
      },
      error: (err) => {
        console.error('Failed to place order:', err);
        this.orderResultSignal.set({
          isSuccess: false,
          message: 'Failed to place order. Please try again.',
        });
      },
    });
  }

  MyOrders() {
    this.apiService.get<MyOrder[]>('Orders/MyOrder').subscribe({
      next: (res) => {
        this.MyorderResultSignal.set(res || []);
      },
      error: (err) => {
        console.error('Failed to load orders:', err);
      },
    });
  }
}
