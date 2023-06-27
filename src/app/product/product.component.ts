import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../models/models';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent {
  @Input() view: 'grid' | 'list' | 'currcartitem' | 'prevcartitem' = 'grid';

  @Input() product: Product = {
    id: 0,
    title: '',
    description: '',
    price: 0,
    productCategory: {
      id: 1,
      category: '',
      subCategory: '',
    },
    offer: {
      id: 1,
      title: '',
      discount: 0,
    },
    quantity: 0,
    imageName: '',
  };

  @Output() removeItem: EventEmitter<any> = new EventEmitter();

  constructor(public utilityService: UtilityService) {}

  ngOnInit() {
    // console.log('Received @Input() values:');
    // console.log(this.product);
  }

  onRemoveFromCart() {
    console.log(this.product);
    this.utilityService.removeFromCart(this.product);
    this.removeItem.emit(this.product);
  }
}
