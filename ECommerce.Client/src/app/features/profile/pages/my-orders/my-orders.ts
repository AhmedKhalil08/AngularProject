import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../../orders/services/order-service';

interface Order {
  id: number;
  date: Date;
  total: number;
  status: string;
  itemCount: number;
  shippingCity: string;
  shippingCountry: string;
  paymentMethod: string;
}

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-orders.html',
  styleUrl: './my-orders.css',
})
export class MyOrders implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);

  readonly itemsPerPage = 10;
  currentPage = signal(1);

  orders = this.orderService.myOrders;
  totalOrderCount = computed(() => this.orders().length);
  totalSpent = computed(() => {
    return this.orders().reduce((sum, order) => sum + (order.totalAmount || 0), 0);
  });

  completedCount = computed(() => {
    return this.orders().filter(
      (order) =>
        order.status.toLowerCase() === 'completed' || order.status.toLowerCase() === 'delivered',
    ).length;
  });
  pendingCount = computed(() => {
    return this.orders().filter(
      (order) =>
        order.status.toLowerCase() === 'pending' || order.status.toLowerCase() === 'processing',
    ).length;
  });

  totalPages = computed(() => {
    return Math.ceil(this.orders().length / this.itemsPerPage);
  });

  paginatedOrders = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.orders().slice(start, end);
  });
  ngOnInit(): void {
    this.orderService.MyOrders();
  }

  loadOrders(): void {
    // Call the service to fetch orders
    // Note: Update this based on how your service returns data
    // this.orders = this.orderService.getOrders();
  }

  onOrderClick(orderId: number): void {
    // Navigate to order details with order ID
    this.router.navigate(['/orders/details', orderId]);
  }

  getTotalOrderCount(): number {
    console.log('Total Orders:', this.orders().length);
    return this.orders().length;
  }

  navigateToProducts(): void {
    this.router.navigate(['/products']);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page);
    }
  }

  nextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.set(this.currentPage() + 1);
    }
  }

  previousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.set(this.currentPage() - 1);
    }
  }

  getPageNumbers(): number[] {
    const total = this.totalPages();
    const pages: number[] = [];
    for (let i = 1; i <= total; i++) {
      pages.push(i);
    }
    return pages;
  }
}
