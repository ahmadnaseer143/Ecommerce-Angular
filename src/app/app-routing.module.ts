import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProductComponent } from './product/product.component';
import { ProductsComponent } from './products/products.component';
import { CartComponent } from './cart/cart.component';
import { OrderComponent } from './order/order.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { AuthGuard } from './services/auth.guard';
import { AdminDashboardComponent } from './Admin/admin-dashboard/admin-dashboard.component';
import { AddProductComponent } from './Admin/add-product/add-product.component';
import { EditProductComponent } from './Admin/edit-product/edit-product.component';
import { RoleGuard } from './services/role.guard';
import { OrdersComponent } from './Admin/orders/orders.component';
import { CategoriesComponent } from './Admin/categories/categories.component';
import { CustomersComponent } from './Admin/customers/customers.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [RoleGuard] },
  { path: 'admin/addproduct', component: AddProductComponent, canActivate: [RoleGuard] },
  { path: 'admin/editproduct/:id', component: EditProductComponent, canActivate: [RoleGuard] },
  { path: 'admin/orders', component: OrdersComponent, canActivate: [RoleGuard] },
  { path: 'admin/categories', component: CategoriesComponent, canActivate: [RoleGuard] },
  { path: 'admin/customers', component: CustomersComponent, canActivate: [RoleGuard] },
  { path: 'products', component: ProductsComponent },
  { path: 'product-details', component: ProductDetailsComponent },
  { path: 'cart', component: CartComponent, canActivate: [AuthGuard] },
  { path: 'orders', component: OrderComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
