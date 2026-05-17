// import { CommonModule } from '@angular/common';
// import { Component, OnInit, signal } from '@angular/core';
// import { FormsModule } from '@angular/forms';
// import { AdminService } from '../../services/admin.service';
// import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
// import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';

// @Component({
//   selector: 'app-categories',
//   imports: [FormsModule,CommonModule],
//   templateUrl: './categories.html',
//   styleUrl: './categories.css',
// })
// export class Categories implements OnInit{

//      categories = signal<any[]>([]);
//   loading = signal(false);
//   showForm = signal(false);
//   editingCategory = signal<any | null>(null);

//   form = {
//     name: '',
//     description: '',
//     imageUrl: '',
//     parentCategoryId: null as number | null
//   };

//   constructor(private adminService: AdminService, private modalService: NgbModal) {}

//   ngOnInit(): void {
//     this.loadCategories();
//   }

//   loadCategories() {
//     this.loading.set(true);
//     this.adminService.getCategories().subscribe({
//       next: (data) => { this.categories.set(data); this.loading.set(false); },
//       error: () => this.loading.set(false)
//     });
//   }

//   openCreateForm() {
//     this.editingCategory.set(null);
//     this.form = { name: '', description: '', imageUrl: '', parentCategoryId: null };
//     this.showForm.set(true);
//   }

//   openEditForm(category: any) {
//     this.editingCategory.set(category);
//     this.form = {
//       name: category.name,
//       description: category.description ?? '',
//       imageUrl: category.imageUrl ?? '',
//       parentCategoryId: category.parentCategoryId ?? null
//     };
//     this.showForm.set(true);
//   }

//   saveForm() {
//     if (!this.form.name) return;
//     if (this.editingCategory()) {
//       this.adminService.updateCategory(this.editingCategory().id, { id: this.editingCategory().id, ...this.form }).subscribe({
//         next: () => { this.loadCategories(); this.showForm.set(false); }
//       });
//     } else {
//       this.adminService.createCategory(this.form).subscribe({
//         next: () => { this.loadCategories(); this.showForm.set(false); }
//       });
//     }
//   }

//   deleteCategory(id: number) {
//     const modal = this.modalService.open(ConfirmModal, { centered: true });
//     modal.componentInstance.title = 'Delete Category';
//     modal.componentInstance.message = 'Are you sure you want to delete this category?';
//     modal.componentInstance.confirmText = 'Delete';
//     modal.componentInstance.confirmClass = 'danger';

//     modal.result.then((confirmed) => {
//       if (confirmed) {
//         this.adminService.deleteCategory(id).subscribe({
//           next: () => this.loadCategories()
//         });
//       }
//     }).catch(() => {});
//   }

//   getCategoryImage(imageUrl?: string): string {
//     if (!imageUrl) return '';
//     if (imageUrl.startsWith('http')) return imageUrl;
//     return `https://localhost:7018${imageUrl}`;
//   }
// }
