// import { Component, OnInit, signal } from '@angular/core';
// import { BannerDto } from '../../../../core/models/banner.model';
// import { AdminService } from '../../services/admin.service';
// import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
// import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';
// import { FormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-banners',
//   imports: [FormsModule,CommonModule],
//   templateUrl: './banners.html',
//   styleUrl: './banners.css',
// })
// export class Banners implements OnInit{
//   banners = signal<BannerDto[]>([]);
//   loading = signal(false);
//   showForm = signal(false);
//   editingBanner = signal<BannerDto | null>(null);

//   form = {
//     title: '',
//     imageUrl: '',
//     link: '',
//     isActive: true,
//     displayOrder: 1
//   };

//   constructor(private adminService: AdminService, private modalService: NgbModal) {}

//   ngOnInit(): void {
//     this.loadBanners();
//   }

//   loadBanners() {
//     this.loading.set(true);
//     this.adminService.getBanners().subscribe({
//       next: (data) => { this.banners.set(data); this.loading.set(false); },
//       error: () => this.loading.set(false)
//     });
//   }

//   openCreateForm() {
//     this.editingBanner.set(null);
//     this.form = { title: '', imageUrl: '', link: '', isActive: true, displayOrder: 1 };
//     this.showForm.set(true);
//   }

//   openEditForm(banner: BannerDto) {
//     this.editingBanner.set(banner);
//     this.form = {
//       title: banner.title,
//       imageUrl: banner.imageUrl,
//       link: banner.link ?? '',
//       isActive: banner.isActive,
//       displayOrder: banner.displayOrder
//     };
//     this.showForm.set(true);
//   }

//   saveForm() {
//     if (this.editingBanner()) {
//       this.adminService.updateBanner(this.editingBanner()!.id, { id: this.editingBanner()!.id, ...this.form }).subscribe({
//         next: () => { this.loadBanners(); this.showForm.set(false); }
//       });
//     } else {
//       this.adminService.createBanner(this.form).subscribe({
//         next: () => { this.loadBanners(); this.showForm.set(false); }
//       });
//     }
//   }

// deleteBanner(id: number) {
//   console.log('deleteBanner called', id);
//   const modal = this.modalService.open(ConfirmModal, { centered: true });
//   modal.componentInstance.title = 'Delete Banner';
//   modal.componentInstance.message = 'Are you sure you want to delete this banner?';
//   modal.componentInstance.confirmText = 'Delete';
//   modal.componentInstance.confirmClass = 'danger';

//   modal.result.then((confirmed) => {
//     console.log('confirmed:', confirmed);
//     if (confirmed) {
//       this.adminService.deleteBanner(id).subscribe({
//         next: () => {
//              console.log('deleted!');
//           this.loadBanners()
//         },
//           error: (err) => console.error('delete error:', err)
//       });
//     }
//   }).catch(() => {});
// }

// }
