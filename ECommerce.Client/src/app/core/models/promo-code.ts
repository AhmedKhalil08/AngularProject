export interface PromoCode {
  id: number;
  code: string;
  discountPercent: number;
  maxUsageCount: number;
  currentUsageCount: number;
  expiryDate: string;
  isActive: boolean;
}
