// import { CommonModule } from '@angular/common';
// import { Component, OnInit, signal } from '@angular/core';
// import { FormsModule } from '@angular/forms';
// import { AdminService } from '../../services/admin.service';
// import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
// import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';

// @Component({
//   selector: 'app-promo-codes',
//   imports: [FormsModule,CommonModule],
//   templateUrl: './promo-codes.html',
//   styleUrl: './promo-codes.css',
// })
// export class PromoCodes implements OnInit {
//   promoCodes = signal<any[]>([]);
//   loading = signal(false);
//   showForm = signal(false);

//   form = {
//     code: '',
//     discountPercent: 10,
//     maxUsageCount: 100,
//     expiryDate: ''
//   };

//   constructor(private adminService: AdminService, private modalService: NgbModal) {}

//   ngOnInit(): void {
//     this.loadPromoCodes();
//   }

//   loadPromoCodes() {
//     this.loading.set(true);
//     this.adminService.getPromoCodes().subscribe({
//       next: (data) => { this.promoCodes.set(data); this.loading.set(false); },
//       error: () => this.loading.set(false)
//     });
//   }

//   saveForm() {
//     if (!this.form.code || !this.form.expiryDate) return;
//     this.adminService.createPromoCode(this.form).subscribe({
//       next: () => {
//         this.loadPromoCodes();
//         this.showForm.set(false);
//         this.form = { code: '', discountPercent: 10, maxUsageCount: 100, expiryDate: '' };
//       }
//     });
//   }

//   deletePromoCode(id: number) {
//     const modal = this.modalService.open(ConfirmModal, { centered: true });
//     modal.componentInstance.title = 'Delete Promo Code';
//     modal.componentInstance.message = 'Are you sure you want to delete this promo code?';
//     modal.componentInstance.confirmText = 'Delete';
//     modal.componentInstance.confirmClass = 'danger';

//     modal.result.then((confirmed) => {
//       if (confirmed) {
//         this.adminService.deletePromoCode(id).subscribe({
//           next: () => this.loadPromoCodes()
//         });
//       }
//     }).catch(() => {});
//   }

//   isExpired(expiryDate: string): boolean {
//     return new Date(expiryDate) < new Date();
//   }


// }
