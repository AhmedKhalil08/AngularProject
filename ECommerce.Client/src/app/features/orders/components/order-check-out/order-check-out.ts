import { Component, inject, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { OrderRequest, PaymentMethod } from '../../../../core/models/order-request';
import { CartService } from '../../../cart/services/cart-service';
import { Cart } from '../../../../core/models/cart';
import { CommonModule, DecimalPipe } from '@angular/common';
import { OrderService } from '../../services/order-service';
import { OrderResult } from '../../../../core/models/order-result';
import { RedirectCommand } from '@angular/router';
import { PromoCodeService } from '../../services/promo-code-service';
import { PromoCode } from '../../../../core/models/promo-code';
import { sign } from 'chart.js/helpers';

@Component({
  selector: 'app-order-check-out',
  imports: [FormsModule, ReactiveFormsModule, DecimalPipe, CommonModule],
  templateUrl: './order-check-out.html',
  styleUrl: './order-check-out.css',
})
export class OrderCheckOut implements OnInit {
  BackendUrl = 'https://localhost:7018/';
  checkoutForm: FormGroup;
  paymentMethods = PaymentMethod;
  cartservice = inject(CartService);
  orderService = inject(OrderService);
  promocodeService = inject(PromoCodeService);
  promocodes: PromoCode[] = [];
  isPromoValid: boolean = false;
  appliedPromo: PromoCode | null = null;
  promoMessage: string = '';
  orderResult = this.orderService.orderResult;
  private TotalBeforeDiscount = signal<number>(0);
  totalBeforeDiscount = this.TotalBeforeDiscount;

  constructor(private fb: FormBuilder) {
    this.checkoutForm = this.fb.group({
      promoCode: [''],
      paymentMethod: [PaymentMethod.CashOnDelivery, Validators.required],
      address: this.fb.group({
        fullName: ['', Validators.required],
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        zipCode: ['', Validators.required],
        isDefault: [true],
        country: ['', Validators.required],
        phone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      }),
    });
  }
  ngOnInit() {
    setTimeout(() => {
      this.loadCart();
      this.loadPromoCodes();
    }, 100);
    setTimeout(() => {
      this.totalBeforeDiscount.set(this.cartservice.cartTotal());
    }, 500);
  }

  loadCart() {
    this.cartservice.loadCartFromApi();
  }

  loadPromoCodes() {
    this.promocodeService.getAllPromoCodes().subscribe({
      next: (res) => {
        this.promocodes = res;
      },
      error: (err) => {
        console.error('Failed to load promo codes:', err);
      },
    });
  }
  checkPromoCode() {
    const enteredCode = this.checkoutForm.get('promoCode')?.value?.trim();

    if (!enteredCode) {
      this.promoMessage = 'Please enter a promo code first.';
      this.isPromoValid = false;
      return;
    }
    const foundPromo = this.promocodes.find(
      (p) => p.code.toLowerCase() === enteredCode.toLowerCase(),
    );

    if (foundPromo) {
      if (!foundPromo.isActive) {
        this.isPromoValid = false;
        this.appliedPromo = null;
        this.promoMessage = 'This promo code is no longer active.';
        this.checkoutForm.get('promoCode')?.setValue('');
        return;
      }
      if (new Date(foundPromo.expiryDate) < new Date()) {
        this.isPromoValid = false;
        this.appliedPromo = null;
        this.promoMessage = 'This promo code has expired.';
        this.checkoutForm.get('promoCode')?.setValue('');
        return;
      }
      if (foundPromo.currentUsageCount >= foundPromo.maxUsageCount) {
        this.isPromoValid = false;
        this.appliedPromo = null;
        this.promoMessage = 'This promo code has reached its maximum usage limit.';
        this.checkoutForm.get('promoCode')?.setValue('');
        return;
      }
      if (this.cartservice.cartTotal() === 0) {
        this.isPromoValid = false;
        this.appliedPromo = null;
        this.promoMessage = 'Please add items to your cart before applying a promo code.';
        this.checkoutForm.get('promoCode')?.setValue('');
        return;
      }
      if (this.cartservice.cartTotal() < 50) {
        this.isPromoValid = false;
        this.appliedPromo = null;
        this.promoMessage = 'Your cart total must be at least $50 to apply this promo code.';
        this.checkoutForm.get('promoCode')?.setValue('');
        return;
      }

      this.isPromoValid = true;
      this.TotalBeforeDiscount.set(this.cartservice.cartTotal());
      this.cartservice.applyDiscount(foundPromo.discountPercent);
      this.appliedPromo = foundPromo;
      this.promoMessage = 'Promo code applied successfully!';
    } else {
      this.isPromoValid = false;
      this.appliedPromo = null;
      this.promoMessage = 'Invalid promo code.';
      this.checkoutForm.get('promoCode')?.setValue('');
    }
  }

  onSubmit() {
    if (this.checkoutForm.valid) {
      const orderData: OrderRequest = this.checkoutForm.value;
      const enteredCode = this.checkoutForm.get('promoCode')?.value?.trim();
      if (enteredCode && !this.isPromoValid) {
        this.promoMessage = 'Please apply a valid promo code or clear the field.';
        return;
      }
      console.log('Order Data:', orderData);
      this.orderService.placeOrder(orderData);
    }
  }
  //   onSubmit() {
  //   if (this.checkoutForm.valid) {
  //     const orderData: OrderRequest = this.checkoutForm.value;
  //     console.log('Order Data:', orderData);
  //     this.orderService.placeOrder(orderData);
  //   }
  // }
}
