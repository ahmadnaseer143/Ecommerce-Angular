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
  selectedFile !: File;

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
      imageName: ['', Validators.required],
      imageFile: ['', Validators.required],
    });



    this.route.params.subscribe((params) => {
      const id = params['id'];
      this.navigationService.getProduct(id).subscribe((res: any) => {
        console.log(res);
        this.selectedFile = res?.imageFile;
        this.productForm.patchValue(res);
      },
        error => {
          console.log('Error retrieving product:', error);
        }
      );
    });
  }

  onFileChange(event: any) {
    this.selectedFile = event.target.files[0];
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        const fileContent = e.target.result;
        // Extract the base64 string (remove the data URL prefix)
        const base64String = fileContent.split(',')[1];

        // Set the base64 string to form control
        this.productForm.controls['imageFile'].setValue(base64String);
      };

      reader.readAsDataURL(file);
    }
  }

  getImageDataUrl(imageFile: string | File): string {
    if (imageFile instanceof File) {
      // Handle the file object differently
      const reader = new FileReader();
      reader.readAsDataURL(imageFile);
      reader.onload = (e) => {
        return e.target?.result as string;
      };
    } else if (typeof imageFile === 'string') {
      if (imageFile.startsWith('data:image')) {
        // Image file is already a base64 data URL
        return imageFile;
      } else {
        // Convert base64-encoded image file to data URL
        return 'data:image/jpeg;base64,' + imageFile;
        // Replace 'image/jpeg' with the appropriate MIME type for your image file
      }
    }

    // Return a placeholder image URL or a default image URL
    return 'https://platinumlist.net/guide/wp-content/uploads/2023/03/IMG-worlds-of-adventure.webp';
  }



  updateProduct() {
    if (this.productForm.invalid) {
      alert("Validation error");
      return;
    }
    const updatedProduct = { ...this.productForm.value } as Product;
    console.log(updatedProduct);
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
