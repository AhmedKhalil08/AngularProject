import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { SignalrService } from '../../../../core/services/signalr-service';

@Component({
  selector: 'app-admin-sidebar',
  imports: [RouterLink,RouterLinkActive],
  templateUrl: './admin-sidebar.html',
  styleUrl: './admin-sidebar.css',
})
export class AdminSidebar {
    constructor(public signalrService: SignalrService) {}
}
