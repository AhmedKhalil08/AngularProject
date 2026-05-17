import {
  Component,
  inject,
  OnInit,
  signal,
  computed,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/productService';
import { IProductDetails } from '../../../../core/models/product-details';
import { Review } from '../../../../core/models/review';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../../../../core/services/auth.service';
import { CartService } from '../../../cart/services/cart-service';
import { ToastService } from '../../../../shared/services/toast-service';
import { ReviewService } from '../../services/review-service';

@Component({
  selector: 'app-product-details',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductDetails implements OnInit {
  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private fb = inject(FormBuilder);
  private cartService = inject(CartService);
  private toastService = inject(ToastService);
  private reviewService = inject(ReviewService);
  private cdr = inject(ChangeDetectorRef);
  private readonly backendUrl = 'https://localhost:7018/';

  // Signals
  product = signal<IProductDetails | null>(null);
  reviews = signal<Review[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  selectedImageIndex = signal(0);
  quantity = signal(1);
  isSubmittingReview = signal(false);
  averageRating = computed(() => {
    const reviewsList = this.reviews();
    if (reviewsList.length === 0) return 0;
    const total = reviewsList.reduce((sum, r) => sum + r.rating, 0);
    return Math.round((total / reviewsList.length) * 10) / 10;
  });

  // Review form
  reviewForm = this.fb.group({
    rating: [5, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]],
  });

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const productId = +params['id'];
      if (productId) {
        this.loadProduct(productId);
      }
    });
  }

  private loadProduct(productId: number) {
    this.isLoading.set(true);
    this.error.set(null);

    this.productService.getProductById(productId).subscribe({
      next: (data) => {
        this.product.set(data);
        if (data.reviews && data.reviews.length > 0) {
          this.reviews.set(data.reviews);
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error loading product:', err);
        this.error.set('Failed to load product details. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  getProductImage(): string {
    const prod = this.product();
    if (!prod) return '';

    const images = prod.images || prod.imageUrls || [];
    if (Array.isArray(images) && images.length > 0) {
      const currentImage = images[this.selectedImageIndex()];
      if (currentImage && typeof currentImage === 'string' && currentImage.startsWith('http')) {
        return currentImage;
      }
      if (currentImage && typeof currentImage === 'object' && currentImage.url) {
        return currentImage.url;
      }
      return currentImage || '';
    }
    return '';
  }

  selectImage(index: number) {
    const prod = this.product();
    if (prod) {
      const images = prod.images || prod.imageUrls || [];
      if (index >= 0 && index < images.length) {
        this.selectedImageIndex.set(index);
      }
    }
  }

  getThumbnails(): string[] {
    const prod = this.product();
    if (!prod) return [];

    const images = prod.images || prod.imageUrls || [];
    return (Array.isArray(images) ? images : []).map((img) => {
      if (typeof img === 'string') return img;
      if (typeof img === 'object' && img.url) return img.url;
      return '';
    });
  }

  incrementQuantity() {
    const prod = this.product();
    if (prod && this.quantity() < prod.stock) {
      this.quantity.update((q) => q + 1);
    }
  }

  decrementQuantity() {
    if (this.quantity() > 1) {
      this.quantity.update((q) => q - 1);
    }
  }

  getRatingStars(rating: number): string {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 !== 0;
    let stars = '⭐'.repeat(fullStars);
    if (hasHalfStar) stars += '½';
    const emptyStars = 5 - Math.ceil(rating);
    stars += '☆'.repeat(emptyStars);
    return stars;
  }

  submitReview() {
    console.log('submitReview called');
    console.log('Form valid:', this.reviewForm.valid);
    console.log('Is logged in:', this.authService.isLoggedIn());

    // Check if logged in first
    if (!this.authService.isLoggedIn()) {
      this.toastService.error('Please log in to submit a review', 'Login Required');
      return;
    }

    // Mark all fields as touched to show validation errors
    if (!this.reviewForm.valid) {
      Object.keys(this.reviewForm.controls).forEach((key) => {
        this.reviewForm.get(key)?.markAsTouched();
      });
      this.cdr.markForCheck();
      this.toastService.error('Please fill all required fields correctly', 'Invalid Form');
      console.log('Form errors:', this.reviewForm.errors);
      console.log('Comment control:', this.reviewForm.get('comment')?.errors);
      return;
    }

    const prod = this.product();
    if (!prod) return;

    this.isSubmittingReview.set(true);
    this.cdr.markForCheck();
    const formValue = this.reviewForm.value;
    const rating = formValue.rating ?? 5;
    const comment = formValue.comment ?? '';

    console.log('Submitting review:', { productId: prod.id, rating, comment });

    this.reviewService.addReview(prod.id, rating, comment).subscribe({
      next: (newReview) => {
        console.log('Review submitted successfully:', newReview);
        this.reviews.update((reviews) => [newReview, ...reviews]);
        this.reviewForm.reset({ rating: 5, comment: '' });
        this.isSubmittingReview.set(false);
        this.cdr.markForCheck();
        this.toastService.success('Your review has been submitted successfully!', 'Review Posted');
      },
      error: (err) => {
        console.error('Error submitting review:', err);
        this.isSubmittingReview.set(false);
        this.cdr.markForCheck();

        // Handle specific error cases
        if (err.status === 500 && err.error?.message?.includes('already submitted')) {
          this.toastService.error(
            err.error.message || 'You have already submitted a review for this product.',
            'Already Reviewed',
          );
        } else if (err.error?.message) {
          this.toastService.error(err.error.message, 'Error');
        } else {
          this.toastService.error('Failed to submit review. Please try again.', 'Error');
        }
      },
    });
  }

  goBack() {
    this.router.navigate(['/products']);
  }

  addToCart() {
    const prod = this.product();
    if (prod) {
      this.cartService.addToCart(prod, this.quantity());
      // Reset quantity to 1 after adding
      this.quantity.set(1);
    }
  }
}
