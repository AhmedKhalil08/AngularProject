import { Component, inject, OnInit } from '@angular/core';
import { ShimpmentService } from '../../services/shimpment-service';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, NgClass, CommonModule } from '@angular/common';

@Component({
  selector: 'app-shipment',
  imports: [FormsModule, NgClass, CurrencyPipe, CommonModule],
  templateUrl: './shipment.html',
  styleUrl: './shipment.css',
})
export class Shipment implements OnInit {
  shipmentService = inject(ShimpmentService);

  ngOnInit() {
    this.shipmentService.loadMyShipments();
  }

  onStatusChange(shipmentId: number, newStatus: string) {
    this.shipmentService.updateShipmentStatus(shipmentId, newStatus);
  }

  onFilterChange(status: string) {
    this.shipmentService.setStatusFilter(status);
  }

  onPageChange(page: number) {
    this.shipmentService.setPage(page);
  }

  onPageSizeChange(size: string) {
    this.shipmentService.setPageSize(Number(size));
  }

  getPages(): number[] {
    const totalPages = this.shipmentService.totalPages();
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }
}
