import { Component } from '@angular/core';
import { User } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent {

  customers: User[] = [];
  itemsPerPage: number = 8;
  p: number = 1;

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.loadCustomers();
  }

  loadCustomers() {
    this.navigationService.getAllUsers().subscribe((res: any) => {
      // console.log(res);
      this.customers = res;
    },
      error => console.log("Error in getting all users", error))
  }

}
