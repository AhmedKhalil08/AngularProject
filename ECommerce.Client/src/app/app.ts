import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { Toast } from './shared/components/toast/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,Toast],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('ECommerce.Client');
constructor(private authService:AuthService){}
  ngOnInit() {
    this.authService.loadCurrentUser().subscribe({
      error : ()=> {}
    });
  };
}
