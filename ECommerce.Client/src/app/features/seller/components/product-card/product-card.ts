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
    const url = this.product.imageUrls[0];
    if (url.startsWith('http')) return url;
    return `https://localhost:7018/${url}`;
  }
  return 'https://ui-avatars.com/api/?name=' + this.product.name + '&background=7c6ff7&color=fff&size=300';
}
}
