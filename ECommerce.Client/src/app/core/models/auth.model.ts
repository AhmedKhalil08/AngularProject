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
