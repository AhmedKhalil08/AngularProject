import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../../../core/services/api.service';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-customer-service',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './customer-service.html',
  styleUrl: './customer-service.css',
})
export class CustomerService {
  form: FormGroup;
  loading = signal(false);
  success = signal(false);
  error = signal<string | null>(null);

  constructor(private fb: FormBuilder, private api: ApiService, private authService: AuthService) {
    const user = this.authService.currentUser();
    this.form = this.fb.group({
      name: [user?.fullName ?? '', Validators.required],
      email: [user?.email ?? '', [Validators.required, Validators.email]],
      subject: ['', Validators.required],
      message: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set(null);

    this.api.post('contact', this.form.value).subscribe({
      next: () => {
        this.success.set(true);
        this.loading.set(false);
        this.form.reset();
      },
      error: () => {
        this.error.set('Failed to send message. Please try again.');
        this.loading.set(false);
      }
    });
  }

  get name() { return this.form.get('name'); }
  get email() { return this.form.get('email'); }
  get subject() { return this.form.get('subject'); }
  get message() { return this.form.get('message'); }


}
