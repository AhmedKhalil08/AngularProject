export interface AuthResponse {
  email: string;
  fullName: string;
  role: 'Admin' | 'Seller' | 'Customer';
  expiration: string;
}

export interface CurrentUser {
  email: string;
  fullName: string;
  role: 'Admin' | 'Seller' | 'Customer';
  expiration: string;
}

export interface LoginDto {
  emailOrUserName: string;
  password: string;
}

export interface RegisterDto {
  fullName: string;
  userName: string;
  email: string;
  password: string;
  phoneNumber: string;
}
export interface RegisterSellerDto {
  fullName: string;
  userName: string;
  email: string;
  password: string;
  phoneNumber: string;
  storeName: string;
  storeDescription?: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
export interface UserDto {
  id: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  profileImageUrl?: string;
  role: string;
  isActive: boolean;
  isDeleted: boolean;
}

export interface SellerProfileDto {
  id: number;
  storeName: string;
  storeDescription?: string;
  logoUrl?: string;
  isApproved: boolean;
  totalEarnings: number;
  userId: string;
  fullName: string;
  email: string;
  isDeleted: boolean;
    isActive: boolean; 
}
export interface OverviewStatsDto {
  totalCustomers: number;
  totalSellers: number;
  totalAdmins: number;
  totalOrders: number;
  totalProducts: number;
  totalCategories: number;
  totalRevenue: number;
  pendingSellers: number;
  bannedUsers: number;
  monthlySales: MonthlySalesDto[];
  orderStatusStats: OrderStatusStatsDto[];
    topProducts: TopProductDto[]; 
}

export interface MonthlySalesDto {
  month: string;
  revenue: number;
  orders: number;
}

export interface OrderStatusStatsDto {
  status: string;
  count: number;
}
export interface TopProductDto {
  productName: string;
  totalSold: number;
  revenue: number;
}
export interface SellerStatsDto {
  totalProducts: number;
  totalEarnings: number;
  totalOrders: number;
  monthlySales: MonthlySalesDto[];
  orderStatusStats: OrderStatusStatsDto[];
  topProducts: TopProductDto[];
}
