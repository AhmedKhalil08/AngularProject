import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { SellerProfileDto, UserDto } from '../../../core/models/auth.model';
import { PagedResult } from '../../../core/models/pagination.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private api:ApiService){}
  getUsers() {
    return this.api.get<UserDto[]>('admin/users');
  }

getCustomers(page = 1, pageSize = 9, search = '', status = '') {
  return this.api.get<PagedResult<UserDto>>(
    `admin/customers?page=${page}&pageSize=${pageSize}&search=${search}&status=${status}`
  );}

  getSellers() {
    return this.api.get<SellerProfileDto[]>('admin/sellers');
  }
    toggleUserStatus(id: string, isActive: boolean) {
    return this.api.put<any>(`admin/users/${id}/status`, isActive);
  }
    deleteUser(id: string) {
    return this.api.delete<any>(`admin/users/${id}`);
  }
  approveSeller(id: number, isApproved: boolean) {
    return this.api.put<any>(`admin/sellers/${id}/approve`, { isApproved });
  }
  restoreUser(id: string) {
  return this.api.put<any>(`admin/users/${id}/restore`, {});
}
}
