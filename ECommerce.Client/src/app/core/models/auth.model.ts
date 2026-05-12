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