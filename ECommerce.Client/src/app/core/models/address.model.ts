export interface AddressDto {
  id: number;
  userId: string;
  fullName: string;
  street: string;
  city: string;
  state: string;
  country: string;
  zipCode: string;
  phone: string;
  isDefault: boolean;
}

export interface CreateAddressDto {
  fullName: string;
  street: string;
  city: string;
  state: string;
  country: string;
  zipCode: string;
  phone: string;
  isDefault: boolean;
}