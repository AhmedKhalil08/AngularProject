import { CommonModule } from '@angular/common';
import { Component, effect, signal } from '@angular/core';
import { SignalrService } from '../../../core/services/signalr-service';
import { ToastService } from '../../services/toast-service';

export interface ToastMessage {
  id: number;
  title: string;
  message: string;
  type: 'success' | 'error' | 'info' | 'warning';
}
@Component({
  selector: 'app-toast',
  imports: [CommonModule],
  templateUrl: './toast.html',
  styleUrl: './toast.css',
  standalone:true
})
export class Toast {
toasts = signal<ToastMessage[]>([]);
lastCount=0;
  constructor(private signalrService: SignalrService,private toastService: ToastService) {
effect(() => {
  const messages = this.signalrService.newMessages();
  if (messages.length > 0) {
    const latest = messages[0];
    // only show if this is a NEW message (just added)
    if (messages.length === this.lastCount + 1) {
      this.addToast({
        id: Date.now(),
        title: '📩 New Message',
        message: `${latest.name}: ${latest.subject}`,
        type: 'info'
      });
    }
    this.lastCount = messages.length;
  }
});
}

addToast(toast: ToastMessage) {
  console.log('addToast called', toast);
  this.toasts.update(t => [toast, ...t]);
  console.log('toasts after update:', this.toasts());
  setTimeout(() => this.removeToast(toast.id), 5000);
}
  removeToast(id: number) {
    this.toasts.update(t => t.filter(toast => toast.id !== id));
  }

}
