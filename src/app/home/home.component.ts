import { Component } from '@angular/core';
import { SuggestedProduct } from '../models/models';
import { NavigationService } from '../services/navigation.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  suggestedProducts: SuggestedProduct[] = [];

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    // get all categories and their images
    this.navigationService.getCategoryList().subscribe((res: any) => {
      console.log(res);
      this.suggestedProducts = res;
    }, error => console.log("Error in getting categroies list", error))
  }

}
