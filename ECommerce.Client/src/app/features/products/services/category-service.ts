import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Category } from '../../../core/models/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private Endpoint = 'Category';
  constructor(private apiService: ApiService) {}
  getCategories() {
    return this.apiService.get<Category[]>(this.Endpoint);
  }
  getCategoryById(id: number) {
    return this.apiService.get<Category>(`${this.Endpoint}/${id}`);
  }
}
