import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Navbar} from '../../shared/components/navbar/navbar';
import {Footer} from '../../shared/components/footer/footer'
import {Category} from '../../features/admin/pages/category/category'

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, Navbar,Footer,Category],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout {}
