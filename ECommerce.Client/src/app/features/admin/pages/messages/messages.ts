import { Component, effect, OnInit, signal } from '@angular/core';
import { ContactMessageDto } from '../../../../core/models/contact.model';
import { AdminService } from '../../services/admin.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalrService } from '../../../../core/services/signalr-service';
import { DatePipe } from '@angular/common';
import { MessagesModal } from '../../components/messages-modal/messages-modal';

@Component({
  selector: 'app-messages',
  imports: [DatePipe, MessagesModal],
  templateUrl: './messages.html',
  styleUrl: './messages.css',
})
export class Messages implements OnInit {
  messages = signal<ContactMessageDto[]>([]);
  loading = signal(false);

  constructor(
    private adminService: AdminService,
    private modalService: NgbModal,
    private signalrService: SignalrService
  ) {
    effect(() => {
      const newMsgs = this.signalrService.newMessages();
      if (newMsgs.length > 0) {
        this.loadMessages();
      }
    });
  }

  ngOnInit(): void {
    this.loadMessages();
    this.signalrService.unreadCount.set(0);
  }

  loadMessages() {
    this.loading.set(true);
    this.adminService.getMessages().subscribe({
      next: (data) => { this.messages.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

openMessage(message: ContactMessageDto) {
  const modal = this.modalService.open(MessagesModal, { centered: true, size: 'lg' });
  modal.componentInstance.message = message;
  
  if (!message.isRead) {
    this.signalrService.unreadCount.update(count => Math.max(0, count - 1));
  }

  this.adminService.getMessageById(message.id).subscribe({
    next: () => {
      this.messages.update(msgs =>
        msgs.map(m => m.id === message.id ? { ...m, isRead: true } : m)
      );
    }
  });
}
}