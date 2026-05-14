import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AdminSidebar } from '../../features/admin/components/admin-sidebar/admin-sidebar';
import { SignalrService } from '../../core/services/signalr-service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet,AdminSidebar],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css',
})
export class AdminLayout  implements OnInit, OnDestroy  {
    constructor(private signalrService: SignalrService, private authService: AuthService) {}

  ngOnInit(): void {
    if (this.authService.isAdmin()) {
      this.signalrService.startConnection();
      // this.signalrService.joinAdminGroup();
    }
  }

  ngOnDestroy(): void {
    this.signalrService.stopConnection();
  }
}
