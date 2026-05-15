export interface ContactMessageDto {
  id: number;
  name: string;
  email: string;
  subject: string;
  message: string;
  sentAt: string;
  isRead: boolean;
}

export interface CreateContactMessageDto {
  name: string;
  email: string;
  subject: string;
  message: string;
}