import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Category } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';
import { AngularFireStorage } from '@angular/fire/compat/storage';


@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent {

  categories: Category[] = [];
  categoryForm !: FormGroup;
  showFormValue: boolean = false;
  edit: boolean = false;
  itemsPerPage: number = 6;
  p: number = 1;
  photoFileError: string | null = null;
  photoFile !: File;

  constructor(private navigationService: NavigationService, private formBuilder: FormBuilder, private changeDetectorRef: ChangeDetectorRef, private fireStorage: AngularFireStorage) { }

  ngOnInit() {
    this.categoryForm = this.formBuilder.group({
      id: [1, Validators.required],
      category: ['', Validators.required],
      subCategory: ['', Validators.required],
      photoUrl: [''],
    });

    this.loadCategories();
  }

  loadCategories() {
    // get all categories
    this.navigationService.getCategoryList().subscribe((res: any) => {
      this.categories = res;
    })
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      // Clear any previous error message
      this.photoFileError = null;

      // Check file type (accept only image files)
      if (!file.type.startsWith('image/')) {
        this.photoFileError = 'Please select a valid image file.';
        return;
      }

      // Set the selected file in the categoryForm
      console.log(file);
      this.photoFile = file;
    }
  }

  async addCategory() {
    this.categoryForm.markAllAsTouched();
    if (this.categoryForm.invalid) {
      console.log("hello");
      return;
    }

    if (this.edit) {
      console.log("Edit Form");
      // console.log(this.categoryForm.value)

      this.navigationService.editCategory(this.categoryForm.value, this.photoFile).subscribe((res: any) => {
        console.log("Edited");
        this.loadCategories();
        this.resetForm();
      },
        error => console.log("Error in Editing Category", error)
      )

    } else {
      console.log("Add Form");
      // console.log(this.categoryForm.value);
      const path = `categories/${this.photoFile.name}`;
      const uploadTask = await this.fireStorage.upload(path, this.photoFile);
      const url = await uploadTask.ref.getDownloadURL();
      console.log(url)
      const updatedFormValue = { ...this.categoryForm.value, photoUrl: url };
      console.log(updatedFormValue);
      this.navigationService.insertCategory(updatedFormValue).subscribe((res: any) => {
        console.log("inserted");
        this.loadCategories();
        this.resetForm();
      },
        error => console.log("Error in Inserting Category", error)
      )
    }
  }

  resetForm() {
    this.edit = false;
    this.showFormValue = false;
    this.categoryForm.reset({
      id: 1,
      category: '',
      subCategory: ''
    });
  }

  editCategory(category: Category) {
    this.edit = true;
    this.showFormValue = true;
    this.categoryForm.patchValue({
      id: category.id,
      category: category.category,
      subCategory: category.subCategory,
      photoUrl: category.photoUrl
    });
    console.log(category.photoUrl)

    // this.getBannerImage(category.subCategory);
  }

  getBannerImage(subCategory: string) {
    this.navigationService.getBanner(subCategory).subscribe(
      (imageBlob: Blob) => {
        const fileOptions: FilePropertyBag = {
          type: imageBlob.type,
          lastModified: Date.now(),
        };
        this.photoFile = new File([imageBlob], subCategory, fileOptions);

        // Manually trigger change detection to update the view with the new photoFile URL
        this.changeDetectorRef.detectChanges();
      },
      (error) => console.log(`Error loading image for ${subCategory}:`, error)
    );
  }

  getPhotoFileURL(): any {
    // Check if the photoFile exists and return its URL
    // if (this.photoFile) {
    //   return URL.createObjectURL(this.photoFile);
    // }
    // return null;
    if (this.photoFile) {
      return URL.createObjectURL(this.photoFile);
    }
    const temp = this.categoryForm.get('photoUrl')?.value;
    if (temp) {
      return temp;
    }
    return null;
  }
  showForm() {

    this.showFormValue = !this.showFormValue;
  }

  deleteCategory(category: Category) {
    const decision = window.confirm("Are you sure you want to delete SubCategory " + category.subCategory + " and its products?");
    if (decision) {
      // Delete the image from Firebase Storage
      if (category.photoUrl) {
        this.fireStorage.storage.refFromURL(category.photoUrl).delete()
          .then(() => {
            console.log("Image deleted from Firebase Storage");
            // Proceed to delete the category from your service
            this.navigationService.deleteCategory(category.id).subscribe((res: any) => {
              console.log("Deleted");
              this.loadCategories();
            },
              error => console.log("Error in Deleting Category", error));
          })
          .catch(error => {
            console.log("Error deleting image from Firebase Storage:", error);
            // Proceed to delete the category even if there's an error deleting the image
            this.navigationService.deleteCategory(category.id).subscribe((res: any) => {
              console.log("Deleted");
              this.loadCategories();
            },
              error => console.log("Error in Deleting Category", error));
          });
      } else {
        // If no photoUrl, proceed to delete the category
        this.navigationService.deleteCategory(category.id).subscribe((res: any) => {
          console.log("Deleted");
          this.loadCategories();
        },
          error => console.log("Error in Deleting Category", error));
      }
    }
  }

}
