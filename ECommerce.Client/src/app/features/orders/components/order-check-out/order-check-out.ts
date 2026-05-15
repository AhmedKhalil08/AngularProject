import { Component, inject, OnInit } from '@angular/core';
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
import { DecimalPipe } from '@angular/common';
import { OrderService } from '../../services/order-service';
import { OrderResult } from '../../../../core/models/order-result';
import { RedirectCommand } from '@angular/router';

@Component({
  selector: 'app-order-check-out',
  imports: [FormsModule, ReactiveFormsModule, DecimalPipe],
  templateUrl: './order-check-out.html',
  styleUrl: './order-check-out.css',
})
export class OrderCheckOut implements OnInit {
  checkoutForm: FormGroup;
  paymentMethods = PaymentMethod;
  cartservice = inject(CartService);
  orderService = inject(OrderService);
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
    }, 100);
  }

  loadCart() {
    this.cartservice.loadCartFromApi();
  }

  onSubmit() {
    if (this.checkoutForm.valid) {
      const orderData: OrderRequest = this.checkoutForm.value;
      this.orderService.placeOrder(orderData);
    }
  }
}
