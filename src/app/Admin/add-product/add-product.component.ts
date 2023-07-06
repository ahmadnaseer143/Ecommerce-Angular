import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Category, NavigationItem, Offer, Product } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {

  productForm !: FormGroup;
  categoryList: NavigationItem[] = [];
  subCategoryList: any = [];
  offerList: Offer[] = [];

  constructor(private router: Router, private navigationService: NavigationService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.productForm = this.formBuilder.group({
      id: [1, Validators.required],
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      productCategory: this.formBuilder.group({
        id: [1, Validators.required],
        category: ['', Validators.required],
        subCategory: ['', Validators.required]
      }),
      offer: this.formBuilder.group({
        id: [1, Validators.required],
        title: ['', Validators.required],
        discount: [{ value: 0, disabled: true }, Validators.required]
      }),
      quantity: [0, Validators.required],
      imageName: ['', Validators.required]
    })

    this.loadCategories();

    this.loadOffers();

  }

  loadOffers() {
    this.navigationService.getAllOffers().subscribe((res: any) => {
      this.offerList = res;
      console.log(res);
    },
      error => console.log("Error in getting all offers in add product", error))
  }

  loadCategories() {
    this.navigationService.getCategoryList().subscribe(
      (res: Category[]) => {
        for (let item of res) {
          let present = false;
          for (let navItem of this.categoryList) {
            if (navItem.category === item.category) {
              navItem.subCategories.push(item.subCategory);
              present = true;
            }
          }
          if (!present) {
            this.categoryList.push({
              category: item.category,
              subCategories: [item.subCategory],
            });
          }
        }
        // console.log(res);
      },
      error => console.log("Error in Getting Category List in add product", error)
    );
  }

  onOfferChange() {
    const selectedOfferTitle = this.productForm.get('offer.title')?.value;

    const selectedOffer = this.offerList.find(offer => offer.title === selectedOfferTitle);

    if (selectedOffer) {
      this.productForm.get('offer.discount')?.setValue(selectedOffer.discount);
    } else {
      this.productForm.get('offer.discount')?.setValue(0);
    }
  }


  onCategoryChange() {
    const selectedCategoryId = this.productForm.get('productCategory.category')?.value;

    const selectedCategory = this.categoryList.find(category => category.category === selectedCategoryId);

    if (selectedCategory) {
      // console.log(selectedCategory?.subCategories)
      this.subCategoryList = selectedCategory?.subCategories;
    } else {
      this.subCategoryList = [];
    }
  }


  addProduct() {
    if (this.productForm.invalid) {
      alert("Validation Error");
      return;
    }

    const product = { ...this.productForm.value } as Product;

    console.log(product);

    this.router.navigate(['admin'])
  }

}
