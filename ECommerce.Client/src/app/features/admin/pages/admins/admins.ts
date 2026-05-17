import { Component, OnInit, signal } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { CustomerCard } from '../../components/customer-card/customer-card';
import { UserDto } from '../../../../core/models/auth.model';
import { AdminService } from '../../services/admin.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CreateAdminModal } from '../../components/create-admin-modal/create-admin-modal';
import { ConfirmModal } from '../../components/confirm-modal/confirm-modal';

@Component({
  selector: 'app-admins',
  imports: [CustomerCard],
  templateUrl: './admins.html',
  styleUrl: './admins.css',
})
export class Admins implements OnInit {
  admins = signal<UserDto[]>([]);
  loading=signal(false);
  constructor(private adminService:AdminService, private modalService:NgbModal){}

  ngOnInit(): void {
    this.loadAdmins();
  }
   
  loadAdmins(){
    this.loading.set(true);
    this.adminService.getAdmins().subscribe({
      next: (data) => {this.admins.set(data); this.loading.set(false)},
      error:()=>this.loading.set(false)
    });
      
    }
     openCreateModal() {
    const modal = this.modalService.open(CreateAdminModal, { centered: true });
    modal.result.then((adminData) => {
      if (adminData) {
        this.adminService.createAdmin(adminData).subscribe({
          next: () => this.loadAdmins()
        });
      }
    }).catch(() => {});
  }


  deleteAdmin(user: UserDto) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Delete Admin';
    modal.componentInstance.message = `Are you sure you want to delete ${user.fullName}?`;
    modal.componentInstance.confirmText = 'Delete';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.adminService.deleteUser(user.id).subscribe({
          next: () => this.loadAdmins()
        });
      }
    }).catch(() => {});
  }
  toggleStatus(user: UserDto) {
    this.adminService.toggleUserStatus(user.id, !user.isActive).subscribe({
      next: () => {
        this.admins.update(admins =>
          admins.map(a => a.id === user.id ? { ...a, isActive: !a.isActive } : a)
        );
      }
    });
  }

   restoreAdmin(user: UserDto) {
    this.adminService.restoreUser(user.id).subscribe({
      next: () => this.loadAdmins()
    });
  }
}
  


