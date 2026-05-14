import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AddressDto } from '../../../../core/models/address.model';

@Component({
  selector: 'app-address-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './address-modal.html',
  styleUrl: './address-modal.css',
})
export class AddressModal {
    @Input() address: AddressDto | null = null;
  form!: FormGroup;
  isEditMode = false;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.isEditMode = !!this.address;
    this.form = this.fb.group({
      fullName: [this.address?.fullName ?? '', Validators.required],
      street: [this.address?.street ?? '', Validators.required],
      city: [this.address?.city ?? '', Validators.required],
      state: [this.address?.state ?? '', Validators.required],
      country: [this.address?.country ?? '', Validators.required],
      zipCode: [this.address?.zipCode ?? '', Validators.required],
      phone: [this.address?.phone ?? '', Validators.required],
      isDefault: [this.address?.isDefault ?? false]
    });
  }

    submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.activeModal.close(this.form.value);
  }

  get fullName() { return this.form.get('fullName'); }
  get street() { return this.form.get('street'); }
  get city() { return this.form.get('city'); }
  get country() { return this.form.get('country'); }
}
