import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../models/models';
import { UtilityService } from '../services/utility.service';
import { NavigationService } from '../services/navigation.service';

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
    imageFile: ''
  };

  imageSrc: string | undefined;

  @Output() removeItem: EventEmitter<any> = new EventEmitter();

  constructor(public utilityService: UtilityService, public navigationService: NavigationService) { }

  ngOnInit() {
    // this.navigationService.getImage(this.product.id).subscribe(
    //   (imageBlob: Blob) => {
    //     this.imageSrc = URL.createObjectURL(imageBlob);
    //   },
    //   (error: any) => {
    //     console.error('Failed to load product image:', error);
    //   }
    // );
  }

  onRemoveFromCart() {
    console.log(this.product);
    this.utilityService.removeFromCart(this.product);
    this.removeItem.emit(this.product);
  }
}
