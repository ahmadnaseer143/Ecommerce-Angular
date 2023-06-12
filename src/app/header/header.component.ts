import { Component } from '@angular/core';
import { NavigationItem } from '../models/models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  navigationList: NavigationItem[] = [
    {
      category: "Electronics",
      subCategories: ["Mobiles", "Laptops"]
    },
    {
      category: "Furniture",
      subCategories: ["Chairs", "Tables"]
    },
  ]

}
