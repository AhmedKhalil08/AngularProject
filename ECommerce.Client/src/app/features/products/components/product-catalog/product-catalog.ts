import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  inject,
  signal,
  computed,
} from '@angular/core';
import { Product } from '../../../../core/models/product';
import { ProductService } from '../../services/productService';
import { CategoryService } from '../../services/category-service';
import { Category } from '../../../../core/models/category';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { TruncateWordsPipe } from '../../../../shared/pipes/truncate-words.pipe';
import { CartService } from '../../../cart/services/cart-service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-catalog',
  imports: [ReactiveFormsModule, CommonModule, NgOptimizedImage, TruncateWordsPipe],
  templateUrl: './product-catalog.html',
  styleUrl: './product-catalog.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductCatalog implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  public CartService = inject(CartService);
  private readonly backendUrl = 'https://localhost:7018/';
  readonly minRangePrice = 0;
  maxRangePrice = signal(100000);
  private route=inject(ActivatedRoute);
  private router = inject(Router);

  // Signals
  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  searchTerm = signal<string>('');
  selectedCategory = signal<number | null>(null);
  minPrice = signal<number>(0);
  maxPrice = signal<number>(this.maxRangePrice());
  selectedRating = signal<number | null>(null);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  // Track quantity for each product
  productQuantities = signal<Map<number, number>>(new Map());

  // Track wishlist items
  wishlistItems = signal<Set<number>>(new Set());

  // Computed - Get filtered products based on all filter signals
  filteredProducts = computed(() => {
    const products = this.products();
    const searchTerm = this.searchTerm().toLowerCase().trim();
    const categoryId = this.selectedCategory();
    const minPrice = this.minPrice();
    const maxPrice = this.maxPrice();
    const rating = this.selectedRating();

    return products.filter((product) => {
      // Search filter
      const matchesSearchTerm =
        !searchTerm || (product.name && product.name.toLowerCase().includes(searchTerm));

      // Category filter
      const matchesCategory = categoryId === null || product.categoryId === categoryId;

      // Price range filter
      const matchesMinPrice = product.price >= minPrice;
      const matchesMaxPrice = product.price <= maxPrice;

      // Rating filter (if product has rating and filter is applied)
      const matchesRating = !rating || !product.rating || product.rating >= rating;

      return (
        matchesSearchTerm && matchesCategory && matchesMinPrice && matchesMaxPrice && matchesRating
      );
    });
  });

  ngOnInit(): void {
    // Load categories first, then products
    this.loadCategories();
    this.loadProducts();

    // Log selected category changes for debugging
    this.selectedCategory.set(null);

      // read category query param
  const categoryId = this.route.snapshot.queryParams['category'];
  if (categoryId) {
    this.selectedCategory.set(Number(categoryId));
  }
    // Debug: Check data after 2 seconds
    setTimeout(() => {
      console.log('=== DEBUG INFO ===');
      console.log('Total products in signal:', this.products().length);
      console.log('Filtered products:', this.filteredProducts().length);
      console.log('First product:', this.products()[0]);
      console.log('Categories count:', this.categories().length);
    }, 2000);
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data) => {
        if (data.length > 0) {
        }
        this.products.set(data);
              const maxProductPrice = Math.max(...data.map(p => p.price));
      this.maxRangePrice.set(maxProductPrice);
      this.maxPrice.set(maxProductPrice);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load products. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
      },
      error: (err) => {
        this.error.set('Failed to load categories.');
      },
    });
  }

  // Helper to get category name by ID
  getCategoryName(categoryId: number | undefined | null, categoryName?: string): string {
    // If categoryName is provided by API, use it
    if (categoryName) {
      return categoryName;
    }

    if (!categoryId || categoryId === 0) {
      return 'Uncategorized';
    }
    const category = this.categories().find((c) => c.id === categoryId);
    if (category) {
      return category.name;
    }
    return `Category ${categoryId}`;
  }

  getFullImageUrl(imageUrl: string | undefined): string {
    if (!imageUrl) {
      return 'assets/placeholder.jpg';
    }
    if (imageUrl.startsWith('http')) {
      return imageUrl;
    }

    const cleanPath = imageUrl.startsWith('/') ? imageUrl.substring(1) : imageUrl;
    return `${this.backendUrl}${cleanPath}`;
  }

  // Helper to get first image from product
  getProductImage(product: Product): string {
    // Try imageUrls array first (from API)
    if (product.imageUrls && product.imageUrls.length > 0) {
      return this.getFullImageUrl(product.imageUrls[0]);
    }

    // Try images array (alternative format)
    if (product.images && product.images.length > 0) {
      const imageUrl = product.images[0]?.imageUrl || product.images[0];
      return this.getFullImageUrl(imageUrl);
    }

    return 'assets/placeholder.jpg';
  }

  selectCategory(categoryId: number | null): void {
    console.log('Category selected:', categoryId);
    console.log('Available categories:', this.categories());
    console.log('Products before filter:', this.products().length);
    console.log('Filtered products after selection:', this.filteredProducts().length);
    this.selectedCategory.set(categoryId);
      this.router.navigate([], {
    queryParams: { category: categoryId ?? null },
    queryParamsHandling: 'merge'
  });
  }

  selectStarRating(rating: number): void {
    this.selectedRating.set(this.selectedRating() === rating ? null : rating);
  }

  onSearchChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm.set(target.value);
  }

onMinPriceChange(event: Event): void {
  const target = event.target as HTMLInputElement;
  const value = target.value;
  if (value === '') {
    this.minPrice.set(this.minRangePrice);
    return;
  }
  const numValue = parseFloat(value);
  if (!isNaN(numValue)) {
    // Clamp to valid range instead of silently ignoring
    this.minPrice.set(Math.max(this.minRangePrice, Math.min(numValue, this.maxPrice())));
  }
}

onMaxPriceChange(event: Event): void {
  const target = event.target as HTMLInputElement;
  const value = target.value;
  if (value === '') {
    this.maxPrice.set(this.maxRangePrice());
    return;
  }
  const numValue = parseFloat(value);
  if (!isNaN(numValue)) {
    this.maxPrice.set(Math.min(this.maxRangePrice(), Math.max(numValue, this.minPrice())));
  }
}

onRangeSliderChange(event: Event): void {
  const target = event.target as HTMLInputElement;
  const value = parseFloat(target.value);
  // Don't let slider go below minPrice
  this.maxPrice.set(Math.max(value, this.minPrice()));
}

  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedCategory.set(null);
    this.minPrice.set(this.minRangePrice);
    this.maxPrice.set(this.maxRangePrice());
    this.selectedRating.set(null);
      this.router.navigate([], {
    queryParams: {},
    queryParamsHandling: ''
  });
  }

  // Quantity management
  getProductQuantity(productId: number): number {
    return this.productQuantities().get(productId) ?? 1;
  }

  incrementQuantity(productId: number): void {
    const current = this.getProductQuantity(productId);
    const product = this.products().find((p) => p.id === productId);
    if (product && current < product.stock) {
      const newMap = new Map(this.productQuantities());
      newMap.set(productId, current + 1);
      this.productQuantities.set(newMap);
    }
  }

  decrementQuantity(productId: number): void {
    const current = this.getProductQuantity(productId);
    if (current > 1) {
      const newMap = new Map(this.productQuantities());
      newMap.set(productId, current - 1);
      this.productQuantities.set(newMap);
    }
  }

  // Wishlist management
  isInWishlist(productId: number): boolean {
    return this.wishlistItems().has(productId);
  }

  toggleWishlist(productId: number): void {
    const newSet = new Set(this.wishlistItems());
    if (newSet.has(productId)) {
      newSet.delete(productId);
    } else {
      newSet.add(productId);
    }
    this.wishlistItems.set(newSet);
  }
}
