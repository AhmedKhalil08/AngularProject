import { Injectable, signal } from '@angular/core'; // استيراد signal العادي
import { ApiService } from '../../../core/services/api.service';
import { OrderResult } from '../../../core/models/order-result';
import { OrderRequest } from '../../../core/models/order-request';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private orderEndpoint = 'Orders';

  private orderResultSignal = signal<OrderResult | null>(null);
  public orderResult = this.orderResultSignal.asReadonly();

  constructor(
    private apiService: ApiService,
    private router: Router,
  ) {}

  placeOrder(orderData: OrderRequest): void {
    this.apiService.post<OrderResult>(this.orderEndpoint, orderData).subscribe({
      next: (res) => {
        console.log('Order placed successfully:', res);
        this.orderResultSignal.set(res);
        if (res.isSuccess) {
          alert('Order placed successfully!');
          if (res.paymentUrl) {
            window.location.href = res.paymentUrl;
          } else {
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
}
