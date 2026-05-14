import { Component, signal } from '@angular/core';
import { AddressDto } from '../../../../core/models/address.model';
import { AddressService } from '../../services/address-service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModal } from '../../../admin/components/confirm-modal/confirm-modal';
import { AddressModal } from '../../components/address-modal/address-modal';

@Component({
  selector: 'app-addresses',
  imports: [],
  templateUrl: './addresses.html',
  styleUrl: './addresses.css',
})
export class Addresses {
 addresses = signal<AddressDto[]>([]);
  loading = signal(false);

  constructor(private addressService: AddressService, private modalService: NgbModal) {}

  ngOnInit(): void {
    this.loadAddresses();
  }

  loadAddresses() {
    this.loading.set(true);
    this.addressService.getAddresses().subscribe({
      next: (data) => { this.addresses.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  openAddModal() {
    const modal = this.modalService.open(AddressModal, { centered: true, size: 'lg' });
    modal.result.then((result) => {
      if (result) {
        this.addressService.createAddress(result).subscribe({
          next: () => this.loadAddresses()
        });
      }
    }).catch(() => {});
  }

  openEditModal(address: AddressDto) {
    const modal = this.modalService.open(AddressModal, { centered: true, size: 'lg' });
    modal.componentInstance.address = address;
    modal.result.then((result) => {
      if (result) {
        this.addressService.updateAddress(address.id, result).subscribe({
          next: () => this.loadAddresses()
        });
      }
    }).catch(() => {});
  }

  deleteAddress(id: number) {
    const modal = this.modalService.open(ConfirmModal, { centered: true });
    modal.componentInstance.title = 'Delete Address';
    modal.componentInstance.message = 'Are you sure you want to delete this address?';
    modal.componentInstance.confirmText = 'Delete';
    modal.componentInstance.confirmClass = 'danger';

    modal.result.then((confirmed) => {
      if (confirmed) {
        this.addressService.deleteAddress(id).subscribe({
          next: () => this.loadAddresses()
        });
      }
    }).catch(() => {});
  }
}
