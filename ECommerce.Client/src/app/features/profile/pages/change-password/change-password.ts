import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { matchPassword } from '../../../auth/validators/password-match.validator';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css',
})
export class ChangePassword {
  form: FormGroup;
  loading = signal(false);
  success = signal(false);
  error = signal<string | null>(null);
  showCurrentPassword = signal(false);
  showNewPassword = signal(false);
  showConfirmPassword = signal(false);

  constructor(private authService: AuthService, private fb: FormBuilder) {
    this.form = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])/)
      ]],
      confirmNewPassword: ['', Validators.required]
    }, { validators: matchPassword('newPassword', 'confirmNewPassword') });
  }
    submit() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.loading.set(true);
    this.error.set(null);
    this.success.set(false);

    this.authService.changePassword(this.form.value).subscribe({
      next: () => {
        this.success.set(true);
        this.loading.set(false);
        this.form.reset();
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Failed to change password');
        this.loading.set(false);
      }
    });
  }

    get currentPassword() { return this.form.get('currentPassword'); }
  get newPassword() { return this.form.get('newPassword'); }
  get confirmNewPassword() { return this.form.get('confirmNewPassword'); }
}
