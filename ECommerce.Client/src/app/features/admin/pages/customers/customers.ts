import { Component, computed, OnInit, signal } from '@angular/core';
import { UserDto } from '../../../../core/models/auth.model';
import {  AdminService } from '../../services/admin.service';
import { CustomerCard } from '../../components/customer-card/customer-card';
import { PagedResult } from '../../../../core/models/pagination.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';

@Component({
  selector: 'app-customers',
  imports: [CustomerCard],
  templateUrl: './customers.html',
  styleUrl: './customers.css',
})
export class Customers implements OnInit {


 pagedResult = signal<PagedResult<UserDto> | null>(null);
  customers = computed(() => this.pagedResult()?.items ?? []);
  loading = signal(false);
  
  // filters
  search = signal('');
  status = signal('');
  currentPage = signal(1);
  pageSize = 9;


    constructor(private adminService: AdminService,private modalService: NgbModal) {}

    ngOnInit(): void {
      this.loadCustomers();
    }
 loadCustomers() {
    this.loading.set(true);
    this.adminService.getCustomers(
      this.currentPage(), 
      this.pageSize, 
      this.search(), 
      this.status()
    ).subscribe({
      next: (data) => { this.pagedResult.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

toggleStatus(user: UserDto) {
  this.adminService.toggleUserStatus(user.id, !user.isActive).subscribe({
    next: () => {
      this.pagedResult.update(result => result ? {
        ...result,
        items: result.items.map(u => u.id === user.id ? { ...u, isActive: !u.isActive } : u)
      } : result);
    }
  });
}
  
deleteUser(user: UserDto) {
  const modal = this.modalService.open(ConfirmModal, { centered: true });
  modal.componentInstance.title = 'Delete Customer';
  modal.componentInstance.message = `Are you sure you want to delete ${user.fullName}?`;
  modal.componentInstance.confirmText = 'Delete';
  modal.componentInstance.confirmClass = 'danger';

  modal.result.then((confirmed) => {
    if (confirmed) {
      this.adminService.deleteUser(user.id).subscribe({
        next: () => this.loadCustomers()
      });
    }
  }).catch(() => {});
}

onSearch(value: string) {
    this.search.set(value);
    this.currentPage.set(1);
    this.loadCustomers();
  }

  onStatusFilter(value: string) {
    this.status.set(value);
    this.currentPage.set(1);
    this.loadCustomers();
  }

  goToPage(page: number) {
    this.currentPage.set(page);
    this.loadCustomers();
  }
  getPages(): number[] {
  const total = this.pagedResult()?.totalPages ?? 0;
  return Array.from({ length: total }, (_, i) => i + 1);
}
restoreUser(user: UserDto) {
  this.adminService.restoreUser(user.id).subscribe({
    next: () => {
      this.pagedResult.update(result => result ? {
        ...result,
        items: result.items.map(u => u.id === user.id ? { ...u, isActive: true, isDeleted: false } : u)
      } : result);
    }
  });
}
}
