import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Product } from '../../../core/models/product';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private Endpoint = 'products';
  constructor(private apiService: ApiService) {}

  getProducts() {
    return this.apiService.get<Product[]>(this.Endpoint);
  }

  getProductById(id: number) {
    return this.apiService.get<Product>(`${this.Endpoint}/${id}`);
  }
}
