import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { AddressDto, CreateAddressDto } from '../../../core/models/address.model';

@Injectable({
  providedIn: 'root',
})
export class AddressService {

    constructor(private api: ApiService) {}

      getAddresses() {
    return this.api.get<AddressDto[]>('addresses');
  }
    createAddress(dto: CreateAddressDto) {
    return this.api.post<AddressDto>('addresses', dto);
  }
    updateAddress(id: number, dto: CreateAddressDto) {
    return this.api.put<AddressDto>(`addresses/${id}`, dto);
  }
    deleteAddress(id: number) {
    return this.api.delete<boolean>(`addresses/${id}`);
  }
}
