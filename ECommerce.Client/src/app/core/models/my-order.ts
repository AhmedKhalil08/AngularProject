export interface MyOrder {
  id: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  userName: string;
  payment: {
    method: string;
    status: string;
    amount: number;
  };
  orderItems: {
    productName: string;
    quantity: number;
  }[];
}
