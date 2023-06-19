import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UtilityService } from '../services/utility.service';
import { NavigationService } from '../services/navigation.service';
import { Product } from '../models/models';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
})
export class ProductsComponent {
  view: 'grid' | 'list' = 'list';
  sortBy: 'default' | 'htl' | 'lth' = 'default';

  products: Product[] = [];

  constructor(
    private activatedRoute: ActivatedRoute,
    private navigationService: NavigationService,
    private utilityService: UtilityService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(
      ({ category, subcategory }: any) => {
        if (category && subcategory) {
          this.navigationService
            .getProducts(category, subcategory, 10)
            .subscribe((res: any) => {
              this.products = res;
            });
        }
      }
    );
  }

  sortByPrice(sortKey: string) {
    this.products.sort((a, b) => {
      if (sortKey === 'default') {
        return a.id > b.id ? 1 : -1;
      }
      return (
        (sortKey === 'htl' ? 1 : -1) *
        (this.utilityService.applyDiscount(a.price, a.offer.discount) >
        this.utilityService.applyDiscount(b.price, b.offer.discount)
          ? -1
          : 1)
      );
    });
  }
}
