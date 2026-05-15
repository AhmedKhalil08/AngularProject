import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-become-seller-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './become-seller-modal.html',
  styleUrl: './become-seller-modal.css',
})
export class BecomeSellerModal {
  form: FormGroup;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {
    this.form = this.fb.group({
      storeName: ['', [Validators.required, Validators.minLength(3)]],
      storeDescription: ['']
    });
  }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.activeModal.close(this.form.value);
  }

  get storeName() { return this.form.get('storeName'); }


}
