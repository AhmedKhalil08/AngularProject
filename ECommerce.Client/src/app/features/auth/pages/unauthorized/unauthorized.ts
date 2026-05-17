import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-unauthorized',
  imports: [RouterLink],
  templateUrl: './unauthorized.html',
  styleUrl: './unauthorized.css',
})
export class Unauthorized {
  constructor(private location: Location, public authService: AuthService) {}

  goBack() {
    this.location.back()
  }

}
