import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

export interface NotificationMessage {
  id: number;
  name: string;
  subject: string;
  sentAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection: signalR.HubConnection | null = null;
  newMessages = signal<NotificationMessage[]>([]);
  unreadCount = signal(0);

startConnection() {
  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7018/hubs/notifications', {
      withCredentials: true
    })
    .withAutomaticReconnect()
    .build();

  this.hubConnection.start()
    .then(() => {
      console.log('SignalR Connected');
      this.joinAdminGroup(); // move here!
    })
    .catch(err => console.error('SignalR Error:', err));

  this.hubConnection.on('NewMessage', (message: NotificationMessage) => {
    this.newMessages.update(msgs => [message, ...msgs]);
    this.unreadCount.update(count => count + 1);
  });
}

joinAdminGroup() {
    this.hubConnection?.invoke('JoinAdminGroup')
      .catch(err => console.error('Error joining admin group:', err));
  }

  stopConnection() {
    this.hubConnection?.stop();
  }
}