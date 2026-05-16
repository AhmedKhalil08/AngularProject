import { DatePipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-subscribers',
  imports: [DatePipe],
  templateUrl: './subscribers.html',
  styleUrl: './subscribers.css',
})
export class Subscribers implements OnInit {
  subscribers = signal<any[]>([]);
  loading = signal(false);
  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadSubscribers();
  }

  loadSubscribers() {
    this.loading.set(true);
    this.adminService.getSubscribers().subscribe({
      next: (data) => { this.subscribers.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  
}
