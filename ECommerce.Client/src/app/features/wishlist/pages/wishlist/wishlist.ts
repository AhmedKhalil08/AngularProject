import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { WishlistDto } from '../../../../core/models/wishlist.model';
import { WishlistService } from '../../services/wishlist-service';
import { CartService } from '../../../cart/services/cart-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModal } from '../../../admin/components/confirm-modal/confirm-modal';

@Component({
  selector: 'app-wishlist',
  imports: [RouterLink,CommonModule],
  templateUrl: './wishlist.html',
  styleUrl: './wishlist.css',
})
export class Wishlist {
   items = signal<WishlistDto[]>([]);
  loading = signal(false);

  constructor(
    private wishlistService: WishlistService,
    private cartService: CartService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadWishlist();
  }

  loadWishlist() {
    this.loading.set(true);
    this.wishlistService.getWishlist().subscribe({
      next: (data) => { this.items.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  removeItem(id: number) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Remove from Wishlist';
    modal.componentInstance.message = 'Are you sure you want to remove this item?';
    modal.componentInstance.confirmText = 'Remove';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.wishlistService.removeFromWishlist(id).subscribe({
          next: () => this.loadWishlist()
        });
      }
    }).catch(() => {});
  }

  getImageUrl(url?: string): string {
    if (!url) return `https://ui-avatars.com/api/?name=Product&background=7c6ff7&color=fff&size=300`;
    if (url.startsWith('http')) return url;
    return `https://localhost:7018${url}`;
  }

  addToCart(item: WishlistDto) {
  this.cartService.addToCart({ 
    id: item.productId, 
    name: item.productName, 
    price: item.productPrice,
    imageUrls: item.productImageUrl ? [item.productImageUrl] : []
  } as any, 1);
}
clearWishlist() {
  const modal = this.modalService.open(ConfirmModal, { centered: true });
  modal.componentInstance.title = 'Clear Wishlist';
  modal.componentInstance.message = 'Are you sure you want to remove all items from your wishlist?';
  modal.componentInstance.confirmText = 'Clear All';
  modal.componentInstance.confirmClass = 'danger';

  modal.result.then((confirmed) => {
    if (confirmed) {
      this.wishlistService.clearWishlist().subscribe({
        next: () => this.items.set([])
      });
    }
  }).catch(() => {});
}
}
