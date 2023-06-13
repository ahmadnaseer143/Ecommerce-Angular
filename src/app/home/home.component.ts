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
      bannerImage: "Banner/bannerchair.jpg",
      category: {
        id: 1,
        category: 'furniture',
        subCategory: 'chairs'
      }
    },
    {
      bannerImage: "Banner/bannerlaptop.avif",
      category: {
        id: 1,
        category: 'electronics',
        subCategory: 'laptops'
      }
    },
    {
      bannerImage: "Banner/bannermobile.jpg",
      category: {
        id: 1,
        category: 'electronics',
        subCategory: 'mobiles'
      }
    }
  ]

}
