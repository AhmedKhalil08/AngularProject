import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SellerProfileDto } from '../../../../core/models/auth.model';

@Component({
  selector: 'app-edit-store-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-store-modal.html',
  styleUrl: './edit-store-modal.css',
})
export class EditStoreModal implements OnInit {

 constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {}
  @Input() profile!: SellerProfileDto;
  form!: FormGroup;
  selectedLogo: File | null = null;
  previewUrl: string | null = null;
  
  ngOnInit(): void {
    this.form = this.fb.group({
      storeName: [this.profile.storeName, [Validators.required, Validators.minLength(3)]],
      storeDescription: [this.profile.storeDescription ?? ''],
      logoUrl: [this.profile.logoUrl ?? '']
    });
    this.previewUrl = this.profile.logoUrl ?? null;
  }
    onLogoSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedLogo = input.files[0];
      this.previewUrl = URL.createObjectURL(this.selectedLogo);
    }
  }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.activeModal.close({
      storeName: this.form.value.storeName,
      storeDescription: this.form.value.storeDescription,
      logoUrl: this.form.value.logoUrl,
      logo: this.selectedLogo
    });
  }
  get storeName() { return this.form.get('storeName'); }
}
