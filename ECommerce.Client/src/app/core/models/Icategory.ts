export interface ICategory {
  id: number;
  name: string;
  description: string;
  parentCategoryId?: number | null;
  imageUrl?: string;
  parentCategoryName?: string;
}
