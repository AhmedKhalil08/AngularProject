import { Address } from './address';

export interface OrderRequest {
  promocode?: string;
  paymentMethod: PaymentMethod;
  address: Address;
}

export enum PaymentMethod {
  CashOnDelivery = 'CashOnDelivery',
  CreditCard = 'CreditCard',
  PayPal = 'PayPal',
}
