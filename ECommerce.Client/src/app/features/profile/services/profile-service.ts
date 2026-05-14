import { Injectable } from '@angular/core';
import { UserDto } from '../../../core/models/auth.model';
import { ApiService } from '../../../core/services/api.service';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {

   constructor(private api: ApiService) {}

  getMyProfile() {
    return this.api.get<UserDto>('user');
  }

  updateMyProfile(data: { fullName: string; phoneNumber: string; profileImageUrl?: string; profileImage?: File }) {
    const formData = new FormData();
    formData.append('fullName', data.fullName);
    formData.append('phoneNumber', data.phoneNumber);
    if (data.profileImage) formData.append('profileImage', data.profileImage);
    else if (data.profileImageUrl) formData.append('profileImageUrl', data.profileImageUrl);
    return this.api.put<UserDto>('user', formData);
  }
}
