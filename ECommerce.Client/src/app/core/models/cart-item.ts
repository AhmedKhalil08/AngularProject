export interface CartItem {
  id?: number;
  productId: number;
  quantity: number;
  unitPrice: number;
  productName: string;
  productImage: string;
  subTotal: number;
  stockQuantity: number;
}
