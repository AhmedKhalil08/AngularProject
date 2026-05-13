import { Component, computed, OnInit, signal } from '@angular/core';
import { SellerCard } from '../../components/seller-card/seller-card';
import { PagedResult } from '../../../../core/models/pagination.model';
import { SellerProfileDto } from '../../../../core/models/auth.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AdminService } from '../../services/admin.service';
import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';

@Component({
  selector: 'app-sellers',
  imports: [SellerCard],
  templateUrl: './sellers.html',
  styleUrl: './sellers.css',
})
export class Sellers implements OnInit {
    pagedResult = signal<PagedResult<SellerProfileDto> | null>(null);
  sellers = computed(() => this.pagedResult()?.items ?? []);
  loading = signal(false);

  search = signal('');
  status = signal('');
  currentPage = signal(1);
  pageSize = 9;
  constructor(private adminService: AdminService, private modalService: NgbModal) {}

    ngOnInit(): void {
    this.loadSellers();
  }

    loadSellers() {
    this.loading.set(true);
    this.adminService.getSellers(
      this.currentPage(),
      this.pageSize,
      this.search(),
      this.status()
    ).subscribe({
      next: (data) => { this.pagedResult.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
    onSearch(value: string) {
    this.search.set(value);
    this.currentPage.set(1);
    this.loadSellers();
  }
  onStatusFilter(value: string) {
    this.status.set(value);
    this.currentPage.set(1);
    this.loadSellers();
  } 
  
  goToPage(page: number) {
    this.currentPage.set(page);
    this.loadSellers();
  }
    getPages(): number[] {
    const total = this.pagedResult()?.totalPages ?? 0;
    return Array.from({ length: total }, (_, i) => i + 1);
  }

    approveSeller(seller: SellerProfileDto) {
    this.adminService.approveSeller(seller.id, !seller.isApproved).subscribe({
      next: () => {
        this.pagedResult.update(result => result ? {
          ...result,
          items: result.items.map(s => s.id === seller.id ? { ...s, isApproved: !s.isApproved } : s)
        } : result);
      }
    });
  }

  toggleStatus(seller: SellerProfileDto) {
    this.adminService.toggleUserStatus(seller.userId, !seller.isActive).subscribe({
      next: () => {
        this.pagedResult.update(result => result ? {
          ...result,
          items: result.items.map(s => s.id === seller.id ? { ...s, isActive: !s.isActive } : s)
        } : result);
      }
    });
  }

  deleteSeller(seller: SellerProfileDto) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Delete Seller';
    modal.componentInstance.message = `Are you sure you want to delete ${seller.storeName}?`;
    modal.componentInstance.confirmText = 'Delete';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.adminService.deleteUser(seller.userId).subscribe({
          next: () => this.loadSellers()
        });
      }
    }).catch(() => {});
  }
 restoreSeller(seller: SellerProfileDto) {
    this.adminService.restoreUser(seller.userId).subscribe({
      next: () => {
        this.pagedResult.update(result => result ? {
          ...result,
          items: result.items.map(s => s.id === seller.id ? { ...s, isDeleted: false, isActive: true } : s)
        } : result);
      }
    });
  }
}
