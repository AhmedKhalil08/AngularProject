import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Product } from '../../../core/models/product';
import { Category } from '../../../core/models/category';

@Injectable({
  providedIn: 'root',
})
export class SellerService {
  
  constructor(private api:ApiService){}

    getMyProducts() {
    return this.api.get<Product[]>('products/myproducts');
  }
    getCategories() {
    return this.api.get<Category[]>('category');
  }
    createProduct(formData: FormData) {
    return this.api.post<Product>('products', formData);
  }
  updateProduct(id: number, formData: FormData) {
    return this.api.put<Product>(`products/${id}`, formData);
  }
    deleteProduct(id: number) {
    return this.api.delete<boolean>(`products/${id}`);
  }
}
