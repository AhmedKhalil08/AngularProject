import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { CartService } from '../../../cart/services/cart-service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm: FormGroup;
  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private cartService: CartService,
  ) {
    this.loginForm = this.fb.group({
      emailOrUserName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  isLoading = false;
  errorMessage = '';
  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.cartService.loadCartFromApi(); // Load cart from API after login
        this.cartService.syncLocalCartToDb();
        // Sync local cart with API on login
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error.message || 'Login failed';
        this.cdr.detectChanges();
      },
    });
  }

  loginWithGoogle() {
    window.location.href = 'https://localhost:7018/api/auth/google-login';
  }
  loginWithFacebook() {
    window.location.href = 'https://localhost:7018/api/auth/facebook-login';
  }
}
