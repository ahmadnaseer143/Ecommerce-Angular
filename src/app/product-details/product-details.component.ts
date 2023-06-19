import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UtilityService } from '../services/utility.service';
import { NavigationService } from '../services/navigation.service';
import { Product } from '../models/models';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent {
  imageIndex: string = '1.jpg';
  product!: Product;
  reviewControl = new FormControl('');
  showError = false;
  reviewSaved = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    public utilityService: UtilityService,
    private navigationService: NavigationService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params: any) => {
      let id = params.id;
      this.navigationService.getProduct(id).subscribe((res: any) => {
        this.product = res;
        console.log(res);
      });
    });
  }

  submitReview() {
    let review = this.reviewControl.value;

    if (review === '' || review === null) {
      this.showError = true;
      return;
    }
  }
}
