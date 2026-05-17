import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { OverviewStatsDto, SellerProfileDto, UserDto } from '../../../core/models/auth.model';
import { PagedResult } from '../../../core/models/pagination.model';
import { ContactMessageDto } from '../../../core/models/contact.model';
import { BannerDto } from '../../../core/models/banner.model';

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

getSellers(page = 1, pageSize = 9, search = '', status = '') {
  return this.api.get<PagedResult<SellerProfileDto>>(
    `admin/sellers?page=${page}&pageSize=${pageSize}&search=${search}&status=${status}`
  );
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
getAdmins() {
  return this.api.get<UserDto[]>('admin/admins');
}
createAdmin(dto: { fullName: string; email: string; password: string }) {
  return this.api.post<any>('admin/admins', dto);
}
getOverviewStats() {
  return this.api.get<OverviewStatsDto>('admin/overview');
}
getMessages() {
  return this.api.get<ContactMessageDto[]>('admin/messages');
}

getMessageById(id: number) {
  return this.api.get<ContactMessageDto>(`admin/messages/${id}`);
}

markAsRead(id: number) {
  return this.api.put<any>(`admin/messages/${id}/read`, {});
}
getProducts() {
  return this.api.get<any[]>('products');
}

deleteProduct(id: number) {
  return this.api.delete<any>(`products/${id}`);
}
restoreProduct(id: number) {
  return this.api.put<any>(`admin/products/${id}/restore`, {});
}
getSubscribers() {
  return this.api.get<any[]>('newsletter/subscribers');
}
getBanners() {
  return this.api.get<BannerDto[]>('admin/banners');
}

createBanner(command: any) {
  return this.api.post<BannerDto>('admin/banners', command);
}

updateBanner(id: number, command: any) {
  return this.api.put<BannerDto>(`admin/banners/${id}`, command);
}

deleteBanner(id: number) {
  return this.api.delete<any>(`admin/banners/${id}`);
}
getPromoCodes() {
  return this.api.get<any[]>('admin/promocodes');
}

createPromoCode(dto: any) {
  return this.api.post<any>('admin/promocodes', dto);
}

deletePromoCode(id: number) {
  return this.api.delete<any>(`admin/promocodes/${id}`);
}
getCategories() {
  return this.api.get<any[]>('category');
}

createCategory(dto: any) {
  return this.api.post<any>('category', dto);
}

updateCategory(id: number, dto: any) {
  return this.api.put<any>(`category/${id}`, dto);
}

deleteCategory(id: number) {
  return this.api.delete<any>(`category/${id}`);
}
}

