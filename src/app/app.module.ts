import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductComponent } from './product/product.component';
import { SuggestedProductComponent } from './suggested-product/suggested-product.component';
import { HomeComponent } from './home/home.component';
import { ProductsComponent } from './products/products.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { CartComponent } from './cart/cart.component';
import { OrderComponent } from './order/order.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { OpenProductsDirective } from './directives/open-products.directive';
import { OpenProductDetailsDirective } from './directives/open-product-details.directive';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { AdminDashboardComponent } from './Admin/admin-dashboard/admin-dashboard.component';
import { AddProductComponent } from './Admin/add-product/add-product.component';
import { EditProductComponent } from './Admin/edit-product/edit-product.component';
import { OrdersComponent } from './Admin/orders/orders.component';
import { CategoriesComponent } from './Admin/categories/categories.component';
import { AddCategoriesComponent } from './Admin/add-categories/add-categories.component';
import { CustomersComponent } from './Admin/customers/customers.component';
import { OffersComponent } from './Admin/offers/offers.component';
import { NgxStripeModule } from 'ngx-stripe';
import { environment } from 'src/environment/environment';
import { NgxPaginationModule } from 'ngx-pagination';
import { AngularFireModule } from '@angular/fire/compat';
import { AngularFireStorageModule } from '@angular/fire/compat/storage';

@NgModule({
  declarations: [
    AppComponent,
    ProductComponent,
    SuggestedProductComponent,
    HomeComponent,
    ProductsComponent,
    ProductDetailsComponent,
    CartComponent,
    OrderComponent,
    HeaderComponent,
    FooterComponent,
    PageNotFoundComponent,
    OpenProductsDirective,
    OpenProductDetailsDirective,
    LoginComponent,
    RegisterComponent,
    AdminDashboardComponent,
    AddProductComponent,
    EditProductComponent,
    OrdersComponent,
    CategoriesComponent,
    AddCategoriesComponent,
    CustomersComponent,
    OffersComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem('user');
        },
        allowedDomains: ['localhost:4200'],
      },
    }),
    NgxStripeModule.forRoot(environment.stripe.publicKey),
    NgxPaginationModule,
    AngularFireModule.initializeApp(environment.firebaseConfig),
    AngularFireStorageModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
