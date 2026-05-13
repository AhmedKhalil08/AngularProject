import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink,NgbModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
isLoggedIn:any;
isAdmin:any;
isSeller:any;
  isMenuOpen = false;
  constructor(private authService:AuthService, private router:Router){
  this.isAdmin=this.authService.isAdmin;
  this.isLoggedIn=this.authService.isLoggedIn;
  this.isSeller=this.authService.isSeller;
  }

logout() {
 this.authService.logout();
}
}
