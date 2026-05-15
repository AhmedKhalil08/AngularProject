import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-profile-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './profile-sidebar.html',
  styleUrl: './profile-sidebar.css',
})
export class ProfileSidebar {}
