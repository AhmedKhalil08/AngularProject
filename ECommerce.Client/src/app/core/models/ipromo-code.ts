import { DecimalPipe } from "@angular/common";

export interface IPromoCode {
    id: number;
  code: string;
  discountPercent: number;
  maxUsageCount: number;
  currentUsageCount: number;
  expiryDate: Date;
  isActive: boolean;
}
