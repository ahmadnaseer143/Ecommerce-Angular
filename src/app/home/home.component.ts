import { Component } from '@angular/core';
import { SuggestedProduct } from '../models/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  suggestedProducts: SuggestedProduct[] = [
    {
      bannerImage: 'Banner/Banner_Mobile.png',
      category: {
        id: 0,
        category: 'Electronics',
        subCategory: 'Mobiles',
      },
    },
    {
      bannerImage: 'Banner/Banner_Laptop.png',
      category: {
        id: 1,
        category: 'Electronics',
        subCategory: 'Laptops',
      },
    },
    {
      bannerImage: 'Banner/Banner_Chair.png',
      category: {
        id: 2,
        category: 'Furniture',
        subCategory: 'Chairs',
      },
    },
    {
      bannerImage: 'Banner/Banner_Table.png',
      category: {
        id: 3,
        category: 'Furniture',
        subCategory: 'Tables',
      },
    },
  ];

}
