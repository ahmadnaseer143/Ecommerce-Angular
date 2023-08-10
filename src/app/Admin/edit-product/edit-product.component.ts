import { Component, OnInit } from '@angular/core';
import { AngularFireStorage } from '@angular/fire/compat/storage';
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
    private router: Router,
    private fireStorage: AngularFireStorage
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
      imageFile: [''],
    });



    this.route.params.subscribe((params) => {
      const id = params['id'];
      this.navigationService.getProduct(id).subscribe((res: any) => {
        // console.log(res);
        // this.selectedFile = res?.imageFile;
        this.productForm.patchValue(res);
      },
        error => {
          console.log('Error retrieving product:', error);
        }
      );
    });
  }

  onFileChange(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        return;
      }
      // console.log(file);
      this.selectedFile = file;
    }
    // this.selectedFile = event.target.files[0];
    // const file = event.target.files[0];
    // if (file) {
    //   const reader = new FileReader();

    //   reader.onload = (e: any) => {
    //     const fileContent = e.target.result;
    //     // Extract the base64 string (remove the data URL prefix)
    //     const base64String = fileContent.split(',')[1];

    //     // Set the base64 string to form control
    //     this.productForm.controls['imageFile'].setValue(base64String);
    //   };

    //   reader.readAsDataURL(file);
    // }
  }

  // getImageDataUrl(imageFile: string | File): string {
  //   if (imageFile instanceof File) {
  //     // Handle the file object differently
  //     const reader = new FileReader();
  //     reader.readAsDataURL(imageFile);
  //     reader.onload = (e) => {
  //       return e.target?.result as string;
  //     };
  //   } else if (typeof imageFile === 'string') {
  //     if (imageFile.startsWith('data:image')) {
  //       // Image file is already a base64 data URL
  //       return imageFile;
  //     } else {
  //       // Convert base64-encoded image file to data URL
  //       return 'data:image/jpeg;base64,' + imageFile;
  //       // Replace 'image/jpeg' with the appropriate MIME type for your image file
  //     }
  //   }

  //   // Return a placeholder image URL or a default image URL
  //   return 'https://platinumlist.net/guide/wp-content/uploads/2023/03/IMG-worlds-of-adventure.webp';
  // }

  getImageDataUrl() {
    if (this.selectedFile) {
      return URL.createObjectURL(this.selectedFile);
    }
    const temp = this.productForm.get('imageFile')?.value;
    if (temp) {
      return temp;
    }
    return null;
  }

  async deleteImage(imageUrl: string) {
    if (imageUrl) {
      const storageRef = this.fireStorage.refFromURL(imageUrl);
      try {
        await storageRef.delete();
        console.log('Image deleted successfully');
      } catch (error) {
        console.error('Error deleting image:', error);
      }
    }
  }

  async updateProduct() {
    if (this.productForm.invalid) {
      alert("Validation error");
      return;
    }
    if (this.selectedFile) {
      const originalImageUrl = this.productForm.value.imageFile;
      await this.deleteImage(originalImageUrl);

      const path = `products/${this.selectedFile.name}`;
      const uploadTask = await this.fireStorage.upload(path, this.selectedFile);
      const url = await uploadTask.ref.getDownloadURL();
      const updatedFormValue = { ...this.productForm.value, imageFile: url } as Product;
      this.navigationService.updateProduct(updatedFormValue).subscribe((res: any) => {
        console.log('Product updated successfully With Image');
        this.router.navigate(['/admin']);
      },
        error => {
          console.log('Error updating product:', error);
        }
      );

    } else {
      this.navigationService.updateProduct(this.productForm.value).subscribe((res: any) => {
        console.log('Product updated successfully Without Image');
        this.router.navigate(['/admin']);
      },
        error => {
          console.log('Error updating product:', error);
        }
      );
    }
  }
}
