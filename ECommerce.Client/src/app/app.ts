import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import {PromoCode} from './features/admin/pages/promo-code/promo-code'
import {Category} from './features/admin/pages/category/category'
import {BnrCarousel} from './features/home/components/bnr-carousel/bnr-carousel'
import {Footer} from './shared/components/footer/footer'



@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PromoCode,Category,BnrCarousel,Footer],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('ECommerce.Client');
constructor(private authService:AuthService){}
   ngOnInit() {}
  //   this.authService.loadCurrentUser().subscribe({
  //     error : ()=> {}
  //   });
  // };
}
