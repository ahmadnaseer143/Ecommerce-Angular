import { Component } from '@angular/core';
import { Order } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent {

  orders: Order[] = [];
  itemsPerPage: number = 8;
  p: number = 1;

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    // get al the orders
    this.navigationService.getAllOrders().subscribe(
      (res: any) => {
        this.orders = res;
      },
      error => {
        console.log('Error retrieving orders:', error);
      }
    );

  }

}
