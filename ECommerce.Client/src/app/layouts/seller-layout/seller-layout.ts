import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SellerSidebar } from '../../features/seller/components/seller-sidebar/seller-sidebar';

@Component({
  selector: 'app-seller-layout',
  imports: [RouterOutlet,SellerSidebar],
  templateUrl: './seller-layout.html',
  styleUrl: './seller-layout.css',
})
export class SellerLayout {}
