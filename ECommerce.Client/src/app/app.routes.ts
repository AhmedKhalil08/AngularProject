import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { authGuard } from './core/guards/auth-guard';
import { Login } from './features/auth/pages/login/login';
import { Home } from './features/home/pages/home/home';
import { RegisterType } from './features/auth/pages/register-type/register-type';
import { Register } from './features/auth/pages/register/register';
import { RegisterSeller } from './features/auth/pages/register-seller/register-seller';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { adminGuard } from './core/guards/admin-guard';
import { Overview } from './features/admin/pages/overview/overview';
import { Customers } from './features/admin/pages/customers/customers';
import { Sellers } from './features/admin/pages/sellers/sellers';
import { Admins } from './features/admin/pages/admins/admins';
import { SellerLayout } from './layouts/seller-layout/seller-layout';
import { sellerGuard } from './core/guards/seller-guard';
import { SellerOverview } from './features/seller/pages/seller-overview/seller-overview';
import { MyProducts } from './features/seller/pages/my-products/my-products';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', component: Home }
    ]
  },
  
{
    path: 'auth',
    component: AuthLayout,
    children: [
    { path: 'login', component: Login },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'register-type', component: RegisterType },
      { path: 'register', component: Register },
      { path: 'register-seller', component: RegisterSeller },
    ]
  },
{
  path: 'admin',
  component: AdminLayout,
  canActivate: [adminGuard],
  children: [
    { path: 'overview', component: Overview },
    { path: 'customers', component: Customers },
    { path: 'sellers', component: Sellers },
    { path: 'admins', component: Admins },
    { path: '', redirectTo: 'overview', pathMatch: 'full' }
  ]
},
{
  path: 'seller',
  component: SellerLayout,
  canActivate: [sellerGuard],
  children: [
    { path: 'overview', component: SellerOverview },  
    { path: '', redirectTo: 'overview', pathMatch: 'full' },
    { path: 'overview', component: SellerOverview },
    { path: 'products', component: MyProducts },
  ]
},
  { path: '**', redirectTo: '' }

];
