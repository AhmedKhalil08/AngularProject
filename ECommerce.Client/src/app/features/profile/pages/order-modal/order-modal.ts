import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MyOrder } from '../../../../core/models/my-order';
import { OrderDetails } from '../../../../core/models/order-details';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';

@Component({
  selector: 'app-order-modal',
  imports: [FormsModule, CurrencyPipe, NgClass, DatePipe],
  templateUrl: './order-modal.html',
  styleUrl: './order-modal.css',
})
export class OrderModal {
  @Input() order: MyOrder | null = null;
  @Input() orderDetails: OrderDetails | null = null;

  @Input() isOpen: boolean = false;
  @Output() closeModal = new EventEmitter<void>();
  onClose() {
    this.isOpen = false;
    this.closeModal.emit();
  }
}
