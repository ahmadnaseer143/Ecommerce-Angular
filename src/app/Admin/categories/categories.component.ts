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
      console.log("Edit Form");
      console.log(this.categoryForm.value);
    } else {
      console.log("Add Form");
      console.log(this.categoryForm.value);
    }
    this.resetForm();
  }

  resetForm() {
    this.edit = false;
    this.categoryForm.reset();
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
}