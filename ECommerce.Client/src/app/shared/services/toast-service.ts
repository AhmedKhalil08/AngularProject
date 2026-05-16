import { Injectable } from '@angular/core';
import { Toast } from '../components/toast/toast';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private toastComponent ?:Toast;
 register(component: Toast) {
    this.toastComponent = component;
  }

    show(title: string, message: string, type: 'success' | 'error' | 'info' | 'warning' = 'info') {
    this.toastComponent?.addToast({
      id: Date.now(),
      title,
      message,
      type
    });
  }

   success(message: string, title = 'Success') {
    this.show(title, message, 'success');
  }

    error(message: string, title = 'Error') {
    this.show(title, message, 'error');
  }
    info(message: string, title = 'Info') {
    this.show(title, message, 'info');
  }


  warning(message: string, title = 'Warning') {
    this.show(title, message, 'warning');
  }

}
