import { Component } from '@angular/core';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent {
  view: 'grid' | 'list' = 'list';
  sortBy: 'default' | 'htl' | 'lth' = 'default';
}
