import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AllOrder } from '../../../../core/models/all-order';
import { OrderDetails } from '../../../../core/models/order-details';
import { CommonModule, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-all-order-modal',
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './all-order-modal.html',
  styleUrl: './all-order-modal.css',
})
export class AllOrderModal {
  @Input() order: AllOrder | null = null;
  @Input() orderDetails: OrderDetails | null = null;

  @Input() isOpen: boolean = false;
  @Output() closeModal = new EventEmitter<void>();
  onClose() {
    this.isOpen = false;
    this.closeModal.emit();
  }
}
