import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { PromoCode } from '../../../core/models/promo-code';

@Injectable({
  providedIn: 'root',
})
export class PromoCodeService {
  API_ENDPOINT = 'PromoCode';
  constructor(private apiService: ApiService) {}
  getAllPromoCodes() {
    return this.apiService.get<PromoCode[]>(this.API_ENDPOINT);
  }
}
