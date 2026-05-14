import { CurrencyPipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../../core/models/product';

@Component({
  selector: 'app-product-card',
  imports: [CurrencyPipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {
    @Input() product!: Product;
  @Output() onEdit = new EventEmitter<Product>();
  @Output() onDelete = new EventEmitter<Product>();
  getMainImage(): string {
    if (this.product.imageUrls && this.product.imageUrls.length > 0) {
      return this.product.imageUrls[0];
    }
    return 'https://via.placeholder.com/300x200?text=No+Image';
  }
}
