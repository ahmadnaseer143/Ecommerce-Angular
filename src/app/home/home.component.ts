import { Component } from '@angular/core';
import { Category } from '../models/models';
import { NavigationService } from '../services/navigation.service';
import { DomSanitizer } from '@angular/platform-browser';
import { AngularFireStorage } from '@angular/fire/compat/storage';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  suggestedProducts: any[] = [];

  constructor(private navigationService: NavigationService, private sanitizer: DomSanitizer, private fireStorage: AngularFireStorage) { }

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    // get all categories and their images
    this.navigationService.getCategoryList().subscribe((res: any) => {
      console.log(res);
      this.suggestedProducts = res;
      // this.loadImages(res);
    }, error => console.log("Error in getting categroies list", error))
  }

  loadImages(res: any) {
    // Extract the subcategories from the response data (res) using the map function
    let arr = res.map((obj: any) => {
      return obj.subCategory;
    });

    console.log(arr); // ["Mobiles" , "Laptops", "Chairs"]

    // Loop through each subcategory to load its corresponding banner image
    arr.forEach((subCategory: string) => {
      // fetch the image for the current subcategory
      this.navigationService.getBanner(subCategory).subscribe(
        (imageBlob: Blob) => {
          // Use the blobToBase64 function to convert the imageBlob to a base64 string
          this.blobToBase64(imageBlob).then((base64String: string) => {
            // Find the category in the suggestedProducts array that matches the current subcategory
            const category = this.suggestedProducts.find(
              (cat: any) => cat.subCategory === subCategory
            );

            // If the category is found, update its 'image' property with the base64 encoded image URL
            if (category) {
              // validate the URL and  it can be trusted now as safe to be displayed
              category.image = this.sanitizer.bypassSecurityTrustUrl(
                'data:image/*;base64,' + base64String
              );
            }
          });
        },
        (error) => console.log(`Error loading image for ${subCategory}:`, error)
      );
    });
  }


  blobToBase64(blob: Blob): Promise<string> {
    return new Promise((resolve) => {
      const reader = new FileReader();
      reader.onloadend = () => {
        const base64String = reader.result as string;
        resolve(base64String.split(',')[1]);
      };
      reader.readAsDataURL(blob);
    });
  }
}
