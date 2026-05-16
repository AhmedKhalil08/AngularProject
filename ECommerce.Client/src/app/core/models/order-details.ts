import { Shipment } from './shipment';

export interface OrderDetails {
  id: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  shipments: Shipment[];
}
