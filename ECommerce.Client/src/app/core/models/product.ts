export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  categoryId: number;
  categoryName?: string;
  images?: any[];
  imageUrls?: string[];
  rating?: number;
  sellerName?: string;
  storeDes?: string;
  isDeleted:boolean;
}
