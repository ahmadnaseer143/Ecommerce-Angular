import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Product } from 'src/app/models/models';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {
  products: Product[] = []

  constructor(private router: Router, private navigationService: NavigationService) { }

  ngOnInit() {
    this.getAllProducts();
  }

  getAllProducts() {
    this.navigationService.getAllProducts().subscribe(products => {
      this.products = products;
    })
  }

  editProduct(id: number) {
    this.router.navigate(["admin/editproduct", id]);
  }

  deleteProduct(product: Product) {
    const id = product.id;
    const category = product.productCategory.category;
    const subCategory = product.productCategory.subCategory;
    const decision = window.confirm("Are you sure you want to delete this product?");
    if (decision) {
      this.navigationService.deleteProduct(id, category, subCategory).subscribe((res: any) => {
        if (res) {
          this.getAllProducts();
        } else {
          console.log("Product deletion unsuccessful");
        }
      },
        error => console.log("Error in deleting product", error));
    }
  }

  addProduct() {
    this.router.navigate(["admin/addproduct"]);
  }
}
