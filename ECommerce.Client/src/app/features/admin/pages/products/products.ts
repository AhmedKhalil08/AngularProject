import { CommonModule } from '@angular/common';
import { Component, computed, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';
import { environment } from '../../../../environment/environment';

@Component({
  selector: 'app-products',
  imports: [FormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit  {
  allProducts = signal<any[]>([]);
  loading = signal(false);
  searchTerm = signal('');
  selectedCategory = signal('');
  selectedStatus = signal('all');
    categories = computed(() => {
    const cats = this.allProducts().map(p => p.categoryName).filter(Boolean);
    return [...new Set(cats)];
  });
    filteredProducts = computed(() => {
    let products = this.allProducts();
    
    if (this.searchTerm()) {
      products = products.filter(p => 
        p.name.toLowerCase().includes(this.searchTerm().toLowerCase())
      );
    }
    
    if (this.selectedCategory()) {
      products = products.filter(p => p.categoryName === this.selectedCategory());
    }
    
    if (this.selectedStatus() === 'active') {
      products = products.filter(p => !p.isDeleted);
    } else if (this.selectedStatus() === 'deleted') {
      products = products.filter(p => p.isDeleted);
    }
    
    return products;
  });

    constructor(private adminService: AdminService, private modalService: NgbModal) {}

     ngOnInit(): void {
    this.loadProducts();
  }

    loadProducts() {
    this.loading.set(true);
    this.adminService.getProducts().subscribe({
      next: (data) => { this.allProducts.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  deleteProduct(id: number) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Delete Product';
    modal.componentInstance.message = 'Are you sure you want to delete this product?';
    modal.componentInstance.confirmText = 'Delete';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.adminService.deleteProduct(id).subscribe({
          next: () => this.loadProducts()
        });
      }
    }).catch(() => {});
  }

  getProductImage(product: any): string {
    if (product.imageUrls && product.imageUrls.length > 0) {
      const url = product.imageUrls[0];
      if (url.startsWith('http')) return url;
      return `https://localhost:7018/${url}`;
    }
    return `https://ui-avatars.com/api/?name=${product.name}&background=7c6ff7&color=fff&size=100`;
  }

  restoreProduct(id: number) {
  const modal = this.modalService.open(ConfirmModal, { centered: true });
  modal.componentInstance.title = 'Restore Product';
  modal.componentInstance.message = 'Are you sure you want to restore this product?';
  modal.componentInstance.confirmText = 'Restore';
  modal.componentInstance.confirmClass = 'success';

  modal.result.then((confirmed) => {
    if (confirmed) {
      this.adminService.restoreProduct(id).subscribe({
        next: () => this.loadProducts()
      });
    }
  }).catch(() => {});
}

}
