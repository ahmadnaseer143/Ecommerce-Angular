import { Component } from '@angular/core';
import { AngularFireStorage } from '@angular/fire/compat/storage';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Category, CategoryAndSubCategory, Offer, Product } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {

  productForm !: FormGroup;
  categoryList: CategoryAndSubCategory[] = [];
  subCategoryList: any = [];
  offerList: Offer[] = [];
  photoFile !: File;

  constructor(private router: Router, private navigationService: NavigationService, private formBuilder: FormBuilder, private fireStorage: AngularFireStorage) { }

  ngOnInit() {
    this.productForm = this.formBuilder.group({
      id: [1, Validators.required],
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      productCategory: this.formBuilder.group({
        id: [0, Validators.required],
        category: ['', Validators.required],
        subCategory: ['', Validators.required]
      }),
      offer: this.formBuilder.group({
        id: [1, Validators.required],
        title: ['', Validators.required],
        discount: [0, Validators.required]
      }),
      quantity: [0, Validators.required],
      imageName: ['', Validators.required],
      imageFile: ['']
    })

    this.loadCategories();

    this.loadOffers();

  }

  loadOffers() {
    this.navigationService.getAllOffers().subscribe((res: any) => {
      this.offerList = res;
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
              navItem.subCategories.push(({
                id: item.id,
                subCategory: item.subCategory
              }));
              present = true;
            }
          }
          if (!present) {
            this.categoryList.push({
              id: item.id,
              category: item.category,
              subCategories: [{
                id: item.id,
                subCategory: item.subCategory
              }],
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
      this.productForm.get('offer.id')?.setValue(selectedOffer.id);
      this.productForm.get('offer.discount')?.setValue(selectedOffer.discount);
    } else {
      this.productForm.get('offer.id')?.setValue(0);
      this.productForm.get('offer.discount')?.setValue(0);
    }
  }


  onCategoryChange() {
    // console.log(this.productForm.value);
    const selectedCategory = this.productForm.get('productCategory.category')?.value;

    const selectedCategoryObj = this.categoryList.find(category => category.category === selectedCategory);

    if (selectedCategoryObj) {
      // this.productForm.get('productCategory.id')?.setValue(selectedCategoryObj.id);
      this.subCategoryList = selectedCategoryObj?.subCategories;
    } else {
      // this.productForm.get('productCategory.id')?.setValue(0);
      this.subCategoryList = [];
    }
  }

  onSubCategoryChange() {
    const selectedSubCategory = this.productForm.get('productCategory.subCategory')?.value;
    // Access the selected category from the form
    const selectedCategory = this.productForm.get('productCategory.category')?.value;

    // Find the category object based on the selected category
    const selectedCategoryObj = this.categoryList.find(category => category.category === selectedCategory);

    if (selectedCategoryObj) {
      // Find the subcategory object based on the selected subcategory
      const selectedSubCategoryObj = selectedCategoryObj.subCategories.find(subCategory => subCategory.subCategory === selectedSubCategory);

      if (selectedSubCategoryObj) {
        // Access the subcategory ID and perform any desired action
        const selectedSubCategoryId = selectedSubCategoryObj.id;
        console.log("Selected subcategory ID:", selectedSubCategoryId);
        this.productForm.get('productCategory.id')?.setValue(selectedSubCategoryId);
      }
    }
  }

  uploadImage(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        return;
      }
      this.photoFile = file;
    }

    // if (event.target.files && event.target.files.length > 0) {
    //   const file = event.target.files[0];
    //   // console.log(file);

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

  async addProduct() {
    if (this.productForm.invalid) {
      alert("Validation Error");
      return;
    }

    const timestamp = new Date().getTime(); // Unique timestamp
    const fileName = `${timestamp}_${this.photoFile.name}`;
    const path = `products/${fileName}`;
    const uploadTask = await this.fireStorage.upload(path, this.photoFile);
    const url = await uploadTask.ref.getDownloadURL();

    const product = { ...this.productForm.value, imageFile: url } as Product;

    // console.log(product);

    this.navigationService.insertProduct(product).subscribe(res => {
      this.router.navigate(['admin'])
    },
      error => console.log("Error in inserting product in add product", error));
  }

}
