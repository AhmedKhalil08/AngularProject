import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  {path: '',
    component: MainLayout,
    // canActivate: [authGuard],
    children: []
  },
  
{
    path: 'auth',
    component: AuthLayout,
    children: [
    //   { path: 'login', component: LoginComponent },
    //   { path: 'register', component: RegisterComponent },
    //   { path: 'register-seller', component: RegisterSellerComponent }
    ]
  },
  //   {
//     path: 'admin',
//     component: AdminLayoutComponent,
//     canActivate: [adminGuard],
//     children: []
//   },
//   {
//     path: 'seller',
//     component: MainLayoutComponent,
//     canActivate: [sellerGuard],
//     children: []
//   },
//   { path: 'unauthorized', component: MainLayoutComponent },
  { path: '**', redirectTo: '' }

];
