import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {
  productForm!: FormGroup;
  product !: Product;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private navigationService: NavigationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.productForm = this.formBuilder.group({
      id: ['', Validators.required],
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      productCategory: this.formBuilder.group({
        id: ['', Validators.required],
        category: ['', Validators.required],
        subCategory: ['', Validators.required]
      }),
      offer: this.formBuilder.group({
        id: ['', Validators.required],
        title: ['', Validators.required],
        discount: [0, Validators.required]
      }),
      quantity: [0, Validators.required],
      imageName: ['', Validators.required]
    });



    this.route.params.subscribe((params) => {
      const id = params['id'];
      this.navigationService.getProduct(id).subscribe((res: any) => {
        this.productForm.patchValue(res);
      },
        error => {
          console.log('Error retrieving product:', error);
        }
      );
    });
  }

  updateProduct() {
    if (this.productForm.invalid) {
      alert("Validation error");
      return;
    }
    const updatedProduct = { ...this.productForm.value } as Product;
    console.log(updatedProduct)
    this.navigationService.updateProduct(updatedProduct).subscribe((res: any) => {
      console.log('Product updated successfully');
      this.router.navigate(['/admin']);
    },
      error => {
        console.log('Error updating product:', error);
      }
    );
  }
}
