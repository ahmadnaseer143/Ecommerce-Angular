import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Product } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {

  productForm !: FormGroup;
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
        discount: [0, Validators.required]
      }),
      quantity: [0, Validators.required],
      imageName: ['', Validators.required]
    })
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
