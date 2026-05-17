import { SellerShipmentItem } from './seller-shipment-item';

export interface SellerShipment {
  id: number;
  sellerId: string;
  status: string;
  shippingFee: number;
  totalAmount: number;
  items: SellerShipmentItem[];
}
