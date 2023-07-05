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
    this.navigationService.getAllProducts().subscribe(products => {
      this.products = products;
    })
  }

  editProduct(id: number) {
    // Implement edit functionality
    this.router.navigate(["admin/editproduct", id]);
  }

  deleteProduct(productId: number) {
    // Implement delete functionality
  }

  addProduct() {
    this.router.navigate(["admin/addproduct"]);
  }
}
