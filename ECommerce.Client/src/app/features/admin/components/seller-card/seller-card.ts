import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SellerProfileDto } from '../../../../core/models/auth.model';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-seller-card',
  imports: [DecimalPipe],
  templateUrl: './seller-card.html',
  styleUrl: './seller-card.css',
})
export class SellerCard {
  @Input() seller!: SellerProfileDto;
  @Output() onApprove = new EventEmitter<SellerProfileDto>();
  @Output() onDelete = new EventEmitter<SellerProfileDto>();
  @Output() onRestore = new EventEmitter<SellerProfileDto>();
  getInitials(name: string): string {
    return name.split(' ').map(n => n[0]).join('').substring(0, 2).toUpperCase();
  }

  getAvatarColor(id: number): string {
    const colors = ['#7c6ff7', '#f59e0b', '#10b981', '#f43f5e', '#3b82f6'];
    return colors[id % colors.length];
  }
}
