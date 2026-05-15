import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../../orders/services/order-service';

import { OrderModal } from '../order-modal/order-modal';
import { ApiService } from '../../../../core/services/api.service';
import { OrderDetails } from '../../../../core/models/order-details';
import { MyOrder } from '../../../../core/models/my-order';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, OrderModal, FormsModule],
  templateUrl: './my-orders.html',
  styleUrl: './my-orders.css',
})
export class MyOrders implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);
  private apiService = inject(ApiService);

  selectedOrder = signal<MyOrder | null>(null);
  selectedOrderDetails = signal<OrderDetails | null>(null);
  isModalOpen = signal<boolean>(false);

  // Filter signals
  startDate = signal<string>('');
  endDate = signal<string>('');
  selectedStatus = signal<string>('');
  sortBy = signal<string>('latest');

  readonly itemsPerPage = 10;
  currentPage = signal(1);

  orders = this.orderService.myOrders;

  // Computed filtered orders (with sorting)
  filteredOrders = computed(() => {
    let filtered = [...this.orders()];

    // Filter by status
    if (this.selectedStatus()) {
      filtered = filtered.filter(
        (order) => order.status.toLowerCase() === this.selectedStatus().toLowerCase(),
      );
    }

    // Filter by date range
    if (this.startDate() || this.endDate()) {
      filtered = filtered.filter((order) => {
        const orderDate = new Date(order.orderDate);
        if (this.startDate()) {
          const start = new Date(this.startDate());
          start.setHours(0, 0, 0, 0);
          if (orderDate < start) return false;
        }
        if (this.endDate()) {
          const end = new Date(this.endDate());
          end.setHours(23, 59, 59, 999);
          if (orderDate > end) return false;
        }
        return true;
      });
    }

    // Sort by selected option - create new array instead of mutating
    const sortBy = this.sortBy();
    const sorted = [...filtered];
    if (sortBy === 'latest') {
      sorted.sort((a, b) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime());
    } else if (sortBy === 'oldest') {
      sorted.sort((a, b) => new Date(a.orderDate).getTime() - new Date(b.orderDate).getTime());
    } else if (sortBy === 'highest') {
      sorted.sort((a, b) => (b.totalAmount || 0) - (a.totalAmount || 0));
    } else if (sortBy === 'lowest') {
      sorted.sort((a, b) => (a.totalAmount || 0) - (b.totalAmount || 0));
    }

    return sorted;
  });

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
    return Math.ceil(this.filteredOrders().length / this.itemsPerPage);
  });

  paginatedOrders = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.filteredOrders().slice(start, end);
  });

  ngOnInit(): void {
    this.orderService.MyOrders();
  }

  onOrderClick(order: MyOrder): void {
    this.selectedOrder.set(order);
    this.isModalOpen.set(true);

    this.selectedOrderDetails.set(null);

    this.apiService.get<OrderDetails>(`Orders/${order.id}`).subscribe({
      next: (details) => {
        this.selectedOrderDetails.set(details);
      },
      error: (err) => console.error('Error fetching order details', err),
    });
  }

  closeOrderModal(): void {
    this.isModalOpen.set(false);
    this.selectedOrder.set(null);
  }
  // ===============================

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

  onStatusFilterChange(status: string): void {
    this.selectedStatus.set(status);
    this.currentPage.set(1);
  }

  onStartDateChange(date: string): void {
    this.startDate.set(date);
    this.currentPage.set(1);
  }

  onEndDateChange(date: string): void {
    this.endDate.set(date);
    this.currentPage.set(1);
  }

  resetFilters(): void {
    this.startDate.set('');
    this.endDate.set('');
    this.selectedStatus.set('');
    this.sortBy.set('latest');
    this.currentPage.set(1);
  }

  onSortChange(sort: string): void {
    this.sortBy.set(sort);
    this.currentPage.set(1);
  }

  getStatusOptions(): string[] {
    const statuses = new Set(this.orders().map((order) => order.status));
    return Array.from(statuses).sort();
  }
}
