import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-admin-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './create-admin-modal.html',
  styleUrl: './create-admin-modal.css',
})
export class CreateAdminModal {
  form: FormGroup;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {
    this.form = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])/)
      ]]
    });
  }

  submit() {
    if (this.form.valid) {
      this.activeModal.close(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }

  get fullName() { return this.form.get('fullName'); }
  get email() { return this.form.get('email'); }
  get password() { return this.form.get('password'); }
}