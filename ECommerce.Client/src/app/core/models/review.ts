export interface Review {
  id: number;
  productId: number;
  userId: string;
  userFullName: string;
  rating: number;
  comment: string;
  createdAt: Date;
}
