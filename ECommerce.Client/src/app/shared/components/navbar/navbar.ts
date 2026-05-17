import { Component, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { CartService } from '../../../features/cart/services/cart-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, NgbModule, CommonModule, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  isLoggedIn: any;
  isAdmin: any;
  isSeller: any;
  isMenuOpen = signal(false);

  // Placeholder cart and wishlist data (will be replaced with service)

  wishlistCount = signal(0);

  constructor(
    private authService: AuthService,
    private router: Router,
    public cartService: CartService,
  ) {
    this.isAdmin = this.authService.isAdmin;
    this.isLoggedIn = this.authService.isLoggedIn;
    this.isSeller = this.authService.isSeller;
  }
  ngOnInit() {
    // Load cart and wishlist counts on init
    // this.cartService.loadCart();
    // this.cartService.cartCount = signal(0);
    // this.cartService.cartTotal = signal(0);
    // this.cartService.loadWishlist();
  }

  logout() {
    this.authService.logout();
    this.cartService.clearLocalCart(); // Clear local cart on logout
  }

  toggleMenu() {
    this.isMenuOpen.set(!this.isMenuOpen());
  }
}
