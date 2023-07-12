import { Component } from '@angular/core';
import { User } from 'src/app/models/models';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent {

  customers: User[] = [];

  constructor() { }

  ngOnInit() { }

}
