import { shipementItem } from './Shipement-item-detail';

export interface Shipment {
  id: number;
  sellerId: string;
  sellerName: string;
  status: string;
  shippingFee: number;
  totalAmount: number;
  items: shipementItem[];
}
