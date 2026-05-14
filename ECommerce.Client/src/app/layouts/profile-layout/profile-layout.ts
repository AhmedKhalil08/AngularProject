import { Component } from '@angular/core';
import { ProfileSidebar } from '../../features/profile/components/profile-sidebar/profile-sidebar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-profile-layout',
  imports: [RouterOutlet, ProfileSidebar],
  templateUrl: './profile-layout.html',
  styleUrl: './profile-layout.css',
})
export class ProfileLayout {}
