import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Category } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

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

  constructor(private navigationService: NavigationService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.categoryForm = this.formBuilder.group({
      id: [1, Validators.required],
      category: ['', Validators.required],
      subCategory: ['', Validators.required],
    });

    this.loadCategories();
  }

  loadCategories() {
    // get all categories
    this.navigationService.getCategoryList().subscribe((res: any) => {
      this.categories = res;
    })
  }

  addCategory() {
    this.categoryForm.markAllAsTouched();
    if (this.categoryForm.invalid) return;

    if (this.edit) {
      // console.log("Edit Form");
      // console.log(this.categoryForm.value);

      this.navigationService.editCategory(this.categoryForm.value).subscribe((res: any) => {
        console.log("Edited");
        this.loadCategories();
        this.resetForm();
      },
        error => console.log("Error in Editing Category", error)
      )
    } else {
      // console.log("Add Form");
      // console.log(this.categoryForm.value);
      this.navigationService.insertCategory(this.categoryForm.value).subscribe((res: any) => {
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
    })
  }

  showForm() {
    this.showFormValue = !this.showFormValue;
  }

  deleteCategory(category: Category) {
    const decision = window.confirm("Are you sure you want to delete SubCategory " + category.subCategory + " and its products?");
    if (decision) {
      // console.log("Deleting", category);

      this.navigationService.deleteCategory(category.id).subscribe((res: any) => {
        console.log("Deleted");
        this.loadCategories();
      },
        error => console.log("Error in Deleting Category", error)
      )
    }
  }
}
