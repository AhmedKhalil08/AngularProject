import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
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
import { ProductCatalog } from './features/products/components/product-catalog/product-catalog';
import { ProductDetails } from './features/productDetails/pages/product-details/product-details';
import { CartComp } from './features/cart/components/cart-comp/cart-comp';
import { SellerLayout } from './layouts/seller-layout/seller-layout';
import { sellerGuard } from './core/guards/seller-guard';
import { SellerOverview } from './features/seller/pages/seller-overview/seller-overview';
import { MyProducts } from './features/seller/pages/my-products/my-products';
import { OrderCheckOut } from './features/orders/components/order-check-out/order-check-out';
import { CheckoutSuccess } from './features/orders/components/checkout-success/checkout-success';
import { Checkoutfailed } from './features/orders/components/checkoutfailed/checkoutfailed';
import { Profile } from './features/profile/pages/profile/profile';
import { ProfileLayout } from './layouts/profile-layout/profile-layout';
import { authGuard } from './core/guards/auth-guard';
import { ChangePassword } from './features/profile/pages/change-password/change-password';
import { Addresses } from './features/profile/pages/addresses/addresses';
import { Messages } from './features/admin/pages/messages/messages';
import { HelpCenter } from './features/contact/pages/help-center/help-center';
import { CustomerService } from './features/contact/pages/customer-service/customer-service';
import { MyOrders } from './features/profile/pages/my-orders/my-orders';
import { Shipment } from './features/seller/pages/shipment/shipment';
import { Products } from './features/admin/pages/products/products';
import { guestGuard } from './core/guards/guest-guard';
import { AllOrders } from './features/admin/pages/all-orders/all-orders';
import { Subscribers } from './features/admin/pages/subscribers/subscribers';
// import { Subscribers } from './features/admin/pages/subscribers/subscribers';
import { Wishlist } from './features/wishlist/pages/wishlist/wishlist';
import { Unauthorized } from './features/auth/pages/unauthorized/unauthorized';
import { Banner } from './features/admin/pages/banner/banner';
import { PromoCode } from './features/admin/pages/promo-code/promo-code';
import { Category } from './features/admin/pages/category/category';

export const routes: Routes = [
  {
     
    
 
    path: '',
   
    component: MainLayout,
    children: [
      { path: '', component: Home },
      { path: 'products', component: ProductCatalog },
      { path: 'products/:id', component: ProductDetails },
      { path: 'cart', component: CartComp },
      {
        path: 'checkout',
        component: OrderCheckOut,
      },
      { path: 'checkout/success', component: CheckoutSuccess },
      { path: 'checkout/failed', component: Checkoutfailed },
      { path: 'help-center', component: HelpCenter },
      { path: 'contact', component: CustomerService },
      { path: 'wishlist', component: Wishlist, canActivate: [authGuard] },
    ],
  },
  {
    path: 'auth',
    component: AuthLayout,
    canActivate: [guestGuard],
    children: [
      { path: 'login', component: Login },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'register-type', component: RegisterType },
      { path: 'register', component: Register },
      { path: 'register-seller', component: RegisterSeller },
    ],
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
      { path: 'messages', component: Messages },
      { path: 'products', component: Products },
      { path: 'Orders', component: AllOrders },
      { path: 'subscribers', component: Subscribers },
      { path: '', redirectTo: 'overview', pathMatch: 'full' },
      { path: 'banners', component: Banner },
      { path: 'promocodes', component: PromoCode },
      { path: 'categories', component: Category },
      { path: 'products/:id',component: ProductDetails}
    ],
  },
  {
    path: 'seller',
    component: SellerLayout,
    canActivate: [sellerGuard],
    children: [
      { path: 'overview', component: SellerOverview },
      { path: 'products', component: MyProducts },
      { path: 'shipments', component: Shipment },
      { path: '', redirectTo: 'overview', pathMatch: 'full' },
    ],
  },
  {
    path: 'profile',
    component: ProfileLayout,
    canActivate: [authGuard],
    children: [
      { path: 'info', component: Profile },
      { path: 'password', component: ChangePassword }, // placeholder for now
      { path: 'addresses', component: Addresses },
      { path: 'myorders', component: MyOrders }, // placeholder for now
      { path: '', redirectTo: 'info', pathMatch: 'full' },
    ],
  },
  { path: 'unauthorized', component: Unauthorized },
  { path: '**', redirectTo: '' },
];
