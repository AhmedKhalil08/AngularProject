import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import {
  AuthResponse,
  ChangePasswordDto,
  CurrentUser,
  LoginDto,
  RegisterDto,
  RegisterSellerDto,
} from '../models/auth.model';
import { tap } from 'rxjs';
import { CartService } from '../../features/cart/services/cart-service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private api: ApiService,
    private router: Router,
  ) {}

  private currentUserSignal = signal<CurrentUser | null>(null);

  currentUser = this.currentUserSignal.asReadonly();
  isLoggedIn = computed(() => !!this.currentUserSignal());
  isAdmin = computed(() => this.currentUserSignal()?.role === 'Admin');
  isSeller = computed(() => this.currentUserSignal()?.role === 'Seller');

  login(dto: LoginDto) {
    return this.api.post<AuthResponse>('auth/login', dto).pipe(
      tap((res) => {
        const user: CurrentUser = {
          email: res.email,
          fullName: res.fullName,
          role: res.role,
          expiration: res.expiration,
        };
        this.currentUserSignal.set(user);

        // redirect based on role
        if (res.role === 'Admin') {
          this.router.navigate(['/admin/overview']);
        } else if (res.role === 'Seller') {
          this.router.navigate(['/seller/overview']);
        } else {
          this.router.navigate(['/']);
        }
      }),
    );
  }
  logout() {
    this.api.post('auth/logout', {}).subscribe({
      next: () => {
        this.currentUserSignal.set(null);
        this.router.navigate(['/']);
      },
      error: () => {
        console.error('Logout failed');
        // we'll replace this with a toast later
      },
    });
  }

  // register customer
  registerCustomer(dto: RegisterDto) {
    return this.api.post<AuthResponse>('auth/register/customer', dto);
  }

  // register seller
  registerSeller(dto: RegisterSellerDto) {
    return this.api.post<AuthResponse>('auth/register/seller', dto);
  }

  changePassword(dto: ChangePasswordDto) {
    return this.api.post<void>('auth/change-password', dto);
  }
  loadCurrentUser() {
    return this.api.get<AuthResponse>('auth/me').pipe(
      tap((res) => {
        const user: CurrentUser = {
          email: res.email,
          fullName: res.fullName,
          role: res.role,
          expiration: res.expiration,
        };
        this.currentUserSignal.set(user);
      }),
    );
  }
}
