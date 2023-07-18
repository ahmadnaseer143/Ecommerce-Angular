import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NavigationService } from '../services/navigation.service';
import { UtilityService } from '../services/utility.service';
import { Router } from '@angular/router';
import { Cart, Order, Payment, PaymentMethod } from '../models/models';
import { timer } from 'rxjs';
import { environment } from 'src/environment/environment';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css'],
})
export class OrderComponent {
  paymentHandler: any = null;
  selectedPaymentMethodName = '';
  selectedPaymentMethod = new FormControl('0');

  address = '';
  mobileNumber = '';
  displaySpinner = false;
  message = '';
  classname = '';

  paymentMethods: PaymentMethod[] = [];

  usersCart: Cart = {
    id: 0,
    user: this.utilityService.getUser(),
    cartItems: [],
    ordered: false,
    orderedOn: '',
  };

  usersPaymentInfo: Payment = {
    id: 0,
    user: this.utilityService.getUser(),
    paymentMethod: {
      id: 0,
      type: '',
      provider: '',
      available: false,
      reason: '',
    },
    totalAmount: 0,
    shippingCharges: 0,
    amountReduced: 0,
    amountPaid: 0,
    createdAt: '',
  };

  constructor(
    private navigationService: NavigationService,
    public utilityService: UtilityService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Get Payment Methods
    this.navigationService.getPaymentMethods().subscribe((res) => {
      this.paymentMethods = res;
    });

    this.selectedPaymentMethod.valueChanges.subscribe((res: any) => {
      if (res === '0') this.selectedPaymentMethodName = '';
      else this.selectedPaymentMethodName = res.toString();
    });

    // Get Cart
    this.navigationService
      .getActiveCartOfUser(this.utilityService.getUser().id)
      .subscribe((res: any) => {
        this.usersCart = res;
        this.utilityService.calculatePayment(res, this.usersPaymentInfo);
      });

    // Set address and phone number
    this.address = this.utilityService.getUser().address;
    this.mobileNumber = this.utilityService.getUser().mobile;

    // stripe
    this.invokeStripe();
  }

  invokeStripe() {
    if (!window.document.getElementById('stripe-script')) {
      const script = window.document.createElement('script');
      script.id = 'stripe-script';
      script.type = 'text/javascript';
      script.src = 'https://checkout.stripe.com/checkout.js';
      script.onload = () => {
        this.paymentHandler = (<any>window).StripeCheckout.configure({
          key: environment.stripe.publicKey,
          locale: 'auto',
          token: function (stripeToken: any) {
            console.log(stripeToken);
            alert('Payment has been successfull!');
          },
        });
      };
      window.document.body.appendChild(script);
    }
  }

  getPaymentMethod(id: string) {
    let x = this.paymentMethods.find((v) => v.id === parseInt(id));
    return x?.type + ' - ' + x?.provider;
  }

  placeOrder() {
    // Open the Stripe checkout popup
    this.paymentHandler.open({
      name: 'Nas Ecommerce',
      description: '',
      amount: parseInt(this.usersPaymentInfo.amountPaid.toString()) * 100,
      token: (stripeToken: any) => {
        // This callback will be executed when the user completes the payment in the Stripe popup
        console.log(stripeToken); // You can use the token to process the payment on the server if needed
        this.displaySpinner = true; // Show the spinner while processing the order

        // Call the storeOrder method to store the order and complete the purchase process
        this.storeOrder();
        this.router.navigateByUrl('/home');
      },
    });

    // let step = 0;
    // let count = timer(0, 3000).subscribe((res) => {
    //   ++step;
    //   if (step === 1) {
    //     this.message = 'Processing Payment';
    //     this.classname = 'text-success';
    //   }
    //   if (step === 2) {
    //     this.message = 'Payment Successfull, Order is being placed.';
    //     this.storeOrder();
    //   }
    //   if (step === 3) {
    //     this.message = 'Your Order has been placed';
    //     this.displaySpinner = false;
    //   }
    //   if (step === 4) {
    //     this.router.navigateByUrl('/home');
    //     count.unsubscribe();
    //   }
    // });
  }

  // payMoney() {
  //   return true;
  // }

  storeOrder() {
    let pmid = 0;
    if (this.selectedPaymentMethod.value)
      pmid = parseInt(this.selectedPaymentMethod.value);

    const body = {
      id: 0,
      paymentMethod: {
        id: pmid,
      },
      user: {
        id: this.utilityService.getUser().id,
      },
      totalAmount: parseInt(this.usersPaymentInfo.totalAmount.toString()),
      shippingCharges: parseInt(this.usersPaymentInfo.totalAmount.toString()),
      amountReduced: parseInt(this.usersPaymentInfo.amountReduced.toString()),
      amountPaid: parseInt(this.usersPaymentInfo.amountPaid.toString()),
    };

    this.navigationService.insertPayment(body).subscribe(
      (response) => {
        // console.log('Response:', response);
        const order = {
          user: {
            id: this.utilityService.getUser().id,
          },
          cart: {
            id: this.usersCart.id,
          },
          payment: {
            id: response,
          },
        };
        this.navigationService.insertOrder(order).subscribe((orderResponse) => {
          // console.log('orderResponse:', orderResponse);
          this.utilityService.changeCart.next(0);
        });
      },
      (error) => {
        console.error('Error:', error);
      }
    );
  }
}
