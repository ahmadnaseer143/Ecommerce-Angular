import { Injectable } from '@angular/core';
import { Cart, Payment, Product, User } from '../models/models';
import { NavigationService } from './navigation.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UtilityService {
  changeCart = new Subject();
  constructor(
    private navigationService: NavigationService,
    private jwt: JwtHelperService,
    private router: Router
  ) { }

  applyDiscount(price: number, discount: number): number {
    let finalPrice: number = price - price * (discount / 100);

    return finalPrice;
  }

  getUser(): User {
    let token = this.jwt.decodeToken();
    let user: User = {
      id: token.id,
      firstName: token.firstName,
      lastName: token.lastName,
      address: token.address,
      mobile: token.mobile,
      email: token.email,
      password: '',
      createdAt: token.createdAt,
      modifiedAt: token.modifiedAt,
    };
    return user;
  }

  getUserRole() {
    return localStorage.getItem('role');
  }

  isAdmin() {
    let token = this.jwt.decodeToken();
    return token?.role == 'admin' ? true : false;
  }

  setUser(token: string) {
    localStorage.setItem('user', token);
  }

  isLoggedIn() {
    return localStorage.getItem('user') ? true : false;
  }

  logoutUser() {
    localStorage.removeItem('user');
    this.router.navigate(['home'])
  }

  addToCart(product: Product) {
    let productid = product.id;
    let userid = this.getUser().id;

    this.navigationService.addToCart(userid, productid).subscribe((res) => {
      if (res.toString() === 'inserted') this.changeCart.next(1);
    });
  }

  removeFromCart(product: Product): any {
    let productid = product.id;
    let userid = this.getUser().id;

    this.navigationService
      .removeFromCart(userid, productid)
      .subscribe((res) => {
        if (res.toString() === 'removed') this.changeCart.next(-1);
      });
  }

  calculatePayment(cart: Cart, payment: Payment) {
    payment.totalAmount = 0;
    payment.amountPaid = 0;
    payment.amountReduced = 0;

    for (let cartitem of cart.cartItems) {
      payment.totalAmount += cartitem.product.price;

      payment.amountReduced +=
        cartitem.product.price -
        this.applyDiscount(
          cartitem.product.price,
          cartitem.product.offer.discount
        );

      payment.amountPaid += this.applyDiscount(
        cartitem.product.price,
        cartitem.product.offer.discount
      );
    }

    if (payment.amountPaid > 50000) payment.shippingCharges = 2000;
    else if (payment.amountPaid > 20000) payment.shippingCharges = 1000;
    else if (payment.amountPaid > 5000) payment.shippingCharges = 500;
    else payment.shippingCharges = 200;
  }

  calculatePricePaid(cart: Cart) {
    let pricepaid = 0;
    for (let cartitem of cart.cartItems) {
      pricepaid += this.applyDiscount(
        cartitem.product.price,
        cartitem.product.offer.discount
      );
    }
    return pricepaid;
  }
}
