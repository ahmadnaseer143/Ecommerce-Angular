import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {
  products: any = [
    {
      id: 1, title: "ABC", description: "hello",
      productCategory: {
        id: 1,
        category: "string",
        subCategory: "string",
      },
      offer: {
        id: 1,
        title: "string",
        discount: 20,
      },
      price: 200, quantity: 2, imageName: "Hello.jpg"
    },
    {
      id: 1, title: "ABC", description: "hello",
      productCategory: {
        id: 1,
        category: "string",
        subCategory: "string",
      },
      offer: {
        id: 1,
        title: "string",
        discount: 20,
      },
      price: 200, quantity: 2, imageName: "Hello.jpg"
    },
    {
      id: 1, title: "ABC", description: "hello",
      productCategory: {
        id: 1,
        category: "string",
        subCategory: "string",
      },
      offer: {
        id: 1,
        title: "string",
        discount: 20,
      },
      price: 200, quantity: 2, imageName: "Hello.jpg"
    },
    {
      id: 1, title: "ABC", description: "hello",
      productCategory: {
        id: 1,
        category: "string",
        subCategory: "string",
      },
      offer: {
        id: 1,
        title: "string",
        discount: 20,
      },
      price: 200, quantity: 2, imageName: "Hello.jpg"
    },
    {
      id: 1, title: "ABC", description: "hello",
      productCategory: {
        id: 1,
        category: "string",
        subCategory: "string",
      },
      offer: {
        id: 1,
        title: "string",
        discount: 20,
      },
      price: 200, quantity: 2, imageName: "Hello.jpg"
    },
  ]

  constructor(private router: Router) { }

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
