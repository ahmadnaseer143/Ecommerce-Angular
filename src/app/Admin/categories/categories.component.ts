import { Component } from '@angular/core';
import { Category } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent {

  categories: Category[] = [];

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    // get all categories
    this.navigationService.getCategoryList().subscribe((res: any) => {
      this.categories = res;
    })
  }

}
