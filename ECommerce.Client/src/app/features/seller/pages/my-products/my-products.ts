import { Component, OnInit, signal } from '@angular/core';
import { Product } from '../../../../core/models/product';
import { Category } from '../../../../core/models/category';
import { SellerService } from '../../services/seller-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModal } from '../../../admin/components/confirm-modal/confirm-modal';
import { ProductModal } from '../../components/product-modal/product-modal';
import { ProductCard } from '../../components/product-card/product-card';

@Component({
  selector: 'app-my-products',
  imports: [ProductModal,ProductCard],
  templateUrl: './my-products.html',
  styleUrl: './my-products.css',
})
export class MyProducts implements OnInit {
    products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  loading = signal(false);

    constructor(private sellerService: SellerService, private modalService: NgbModal) {}

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
  }
  loadProducts() {
    this.loading.set(true);
    this.sellerService.getMyProducts().subscribe({
      next: (data) => { this.products.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  } 
  
  loadCategories() {
    this.sellerService.getCategories().subscribe({
      next: (data) => this.categories.set(data)
    });
  }
  deleteProduct(product: Product) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Delete Product';
    modal.componentInstance.message = `Are you sure you want to delete ${product.name}?`;
    modal.componentInstance.confirmText = 'Delete';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.sellerService.deleteProduct(product.id).subscribe({
          next: () => this.loadProducts()
        });
      }
    }).catch(() => {});
  }
  openCreateModal() {
  const modal = this.modalService.open(ProductModal, { centered: true, size: 'lg' });
  modal.componentInstance.categories = this.categories();

  modal.result.then((result) => {
    if (result) {
      this.sellerService.createProduct(result.formData).subscribe({
        next: () => this.loadProducts()
      });
    }
  }).catch(() => {});
}
openEditModal(product: Product) {
  const modal = this.modalService.open(ProductModal, { centered: true, size: 'lg' });
  modal.componentInstance.product = product;
  modal.componentInstance.categories = this.categories();

  modal.result.then((result) => {
    if (result) {
      this.sellerService.updateProduct(product.id, result.formData).subscribe({
        next: () => this.loadProducts()
      });
    }
  }).catch(() => {});
}
}
