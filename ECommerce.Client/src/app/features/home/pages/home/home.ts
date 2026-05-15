import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgxParticlesModule } from '@tsparticles/angular';
import { BannerDto } from '../../../../core/models/banner.model';
import { Product } from '../../../../core/models/product';
import { Category } from '../../../../core/models/category';
import { ApiService } from '../../../../core/services/api.service';
import { CartService } from '../../../cart/services/cart-service';

@Component({
  selector: 'app-home',
  imports: [NgxParticlesModule, RouterLink,CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {

 banners = signal<BannerDto[]>([]);
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  currentBannerIndex = signal(0);
  showBackToTop = signal(false);
  autoSlideInterval: any;

  constructor(private api: ApiService, private cartService : CartService) {}

  ngOnInit(): void {
    this.loadBanners();
    this.loadProducts();
    this.loadCategories();
    window.addEventListener('scroll', this.onScroll.bind(this));
  }

  ngOnDestroy(): void {
    clearInterval(this.autoSlideInterval);
    window.removeEventListener('scroll', this.onScroll.bind(this));
  }

  loadBanners() {
    this.api.get<BannerDto[]>('banner').subscribe({
      next: (data) => {
        this.banners.set(data.filter(b => b.isActive).sort((a, b) => a.displayOrder - b.displayOrder));
        if (this.banners().length > 1) this.startAutoSlide();
      }
    });
  }

  loadProducts() {
    this.api.get<Product[]>('products').subscribe({
      next: (data) => this.products.set(data.slice(0, 8))
    });
  }

  loadCategories() {
    this.api.get<Category[]>('category').subscribe({
      next: (data) => this.categories.set(data.slice(0, 6))
    });
  }

  startAutoSlide() {
    this.autoSlideInterval = setInterval(() => this.nextBanner(), 4000);
  }

  nextBanner() {
    const total = this.banners().length;
    if (total === 0) return;
    this.currentBannerIndex.set((this.currentBannerIndex() + 1) % total);
  }

  prevBanner() {
    const total = this.banners().length;
    if (total === 0) return;
    this.currentBannerIndex.set((this.currentBannerIndex() - 1 + total) % total);
  }

  goToBanner(index: number) {
    this.currentBannerIndex.set(index);
    clearInterval(this.autoSlideInterval);
    this.startAutoSlide();
  }

  onScroll() {
    this.showBackToTop.set(window.scrollY > 400);
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  getProductImage(product: Product): string {
    if (product.imageUrls && product.imageUrls.length > 0) {
      const url = product.imageUrls[0];
      if (url.startsWith('http')) return url;
      return `https://localhost:7018/${url}`;
    }
    return `https://ui-avatars.com/api/?name=${product.name}&background=7c6ff7&color=fff&size=300`;
  }  

  addToCart(product: Product) {
  this.cartService.addToCart(product, 1);
}
}