export interface BannerDto {
  id: number;
  title: string;
  imageUrl: string;
  link?: string;
  isActive: boolean;
  displayOrder: number;
}