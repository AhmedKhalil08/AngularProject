import { computed, inject, Inject, Injectable, signal } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { SellerShipment } from '../../../core/models/seller-shipment';

@Injectable({
  providedIn: 'root',
})
export class ShimpmentService {
  private endpoint = 'orders';
  apiService = inject(ApiService);

  private shipmentsSignal = signal<SellerShipment[]>([]);
  private isLoadingSignal = signal<boolean>(false);
  private pageSignal = signal<number>(1);
  private pageSizeSignal = signal<number>(5);
  private selectedStatusSignal = signal<string>('');

  public shipments = this.shipmentsSignal.asReadonly();
  public isLoading = this.isLoadingSignal.asReadonly();
  public currentPage = this.pageSignal.asReadonly();
  public pageSize = this.pageSizeSignal.asReadonly();
  public selectedStatus = this.selectedStatusSignal.asReadonly();

  public filteredShipments = computed(() => {
    const selectedStatus = this.selectedStatusSignal();
    let filtered = selectedStatus
      ? this.shipmentsSignal().filter((s) => s.status === selectedStatus)
      : this.shipmentsSignal();
    // Create a copy before sorting to avoid mutation
    return [...filtered].sort((a, b) => b.id - a.id);
  });

  public paginatedShipments = computed(() => {
    const filtered = this.filteredShipments();
    const page = this.pageSignal();
    const size = this.pageSizeSignal();
    const startIndex = (page - 1) * size;
    return filtered.slice(startIndex, startIndex + size);
  });

  public totalPages = computed(() =>
    Math.ceil(this.filteredShipments().length / this.pageSizeSignal()),
  );

  public totalShipmentsCount = computed(() => this.shipmentsSignal().length);

  public pendingShipmentsCount = computed(
    () => this.shipmentsSignal().filter((s) => s.status === 'Pending').length,
  );

  public totalRevenue = computed(() =>
    this.shipmentsSignal().reduce((sum, shipment) => sum + shipment.totalAmount, 0),
  );

  loadMyShipments() {
    this.isLoadingSignal.set(true);
    this.apiService.get<SellerShipment[]>(`${this.endpoint}/my-shipments`).subscribe({
      next: (res) => {
        this.shipmentsSignal.set(res);
        this.isLoadingSignal.set(false);
      },
      error: (err) => {
        console.error('Error fetching seller shipments', err);
        this.isLoadingSignal.set(false);
      },
    });
  }
  updateShipmentStatus(shipmentId: number, newStatus: string) {
    this.shipmentsSignal.update((currentShipments) =>
      currentShipments.map((shipment) =>
        shipment.id === shipmentId ? { ...shipment, shipmentStatus: newStatus } : shipment,
      ),
    );

    const payload = { shipmentId: shipmentId, shipmentStatus: newStatus };
    this.apiService.put(`${this.endpoint}/update-shipment`, payload).subscribe({
      next: () => {
        this.loadMyShipments();
      },
      error: (err) => {
        console.error('Error updating status', err);
        this.loadMyShipments();
      },
    });
  }

  setStatusFilter(status: string) {
    this.selectedStatusSignal.set(status);
    this.pageSignal.set(1);
  }

  setPage(page: number) {
    this.pageSignal.set(page);
  }

  setPageSize(size: number) {
    this.pageSizeSignal.set(size);
    this.pageSignal.set(1);
  }

  getStatusOptions(): string[] {
    return ['', 'Pending', 'Processing', 'Confirmed', 'Shipped', 'Delivered'];
  }
}
