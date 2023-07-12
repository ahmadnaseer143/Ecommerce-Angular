import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpResponse,
} from '@angular/common/http';
import { Observable, map } from 'rxjs';
import {
  Category,
  Order,
  Payment,
  PaymentMethod,
  Product,
  User,
} from '../models/models';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  baseurl = 'https://localhost:44376/api/Shopping/';

  constructor(private http: HttpClient) { }

  getCategoryList(): Observable<Category[]> {
    let url = this.baseurl + 'GetCategoryList';
    return this.http.get<any[]>(url).pipe(
      map((categories) =>
        categories.map((category) => {
          let mappedCategory: Category = {
            id: category.id,
            category: category.category,
            subCategory: category.subCategory,
          };
          return mappedCategory;
        })
      )
    );
  }

  insertCategory(category: Category): Observable<any> {
    return this.http.post(this.baseurl + "InsertCategory", category);
  }

  editCategory(category: Category): Observable<any> {
    return this.http.put(this.baseurl + "EditCategory", category);
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(this.baseurl + "DeleteCategory/" + id);
  }

  getProducts(category: string, subCategory: string, count: number): Observable<any[]> {
    return this.http.get<any[]>(this.baseurl + 'GetProducts', {
      params: new HttpParams()
        .set('category', category)
        .set('subCategory', subCategory)
        .set('count', count),
    });
  }

  getAllProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.baseurl + 'GetAllProducts');
  }

  getProduct(id: number): Observable<any> {
    let url = this.baseurl + 'GetProduct/' + id;
    return this.http.get(url);
  }

  updateProduct(product: Product): Observable<any> {
    let url = this.baseurl + "UpdateProduct";
    return this.http.put(url, product);
  }

  registerUser(user: User): Observable<any> {
    let url = this.baseurl + 'RegisterUser';
    return this.http.post(url, user, { responseType: 'text' });
  }

  loginUser(email: string, password: string): Observable<any> {
    let url = this.baseurl + 'LoginUser';
    return this.http.post(
      url,
      { Email: email, Password: password },
      { responseType: 'text' }
    );
  }

  submitReview(userid: number, productid: number, review: string): Observable<any> {
    let obj: any = {
      User: {
        Id: userid,
      },
      Product: {
        Id: productid,
      },
      Value: review,
    };

    let url = this.baseurl + 'InsertReview';
    return this.http.post(url, obj, { responseType: 'text' });
  }

  getAllReviewsOfProduct(productId: number): Observable<any> {
    let url = this.baseurl + 'GetProductReviews/' + productId;
    return this.http.get(url);
  }

  addToCart(userid: number, productid: number): Observable<any> {
    let url = this.baseurl + 'InsertCartItem/' + userid + '/' + productid;
    return this.http.post(url, null, { responseType: 'text' });
  }

  removeFromCart(userid: number, productid: number): Observable<any> {
    let url = this.baseurl + 'RemoveCartItem/' + userid + '/' + productid;
    return this.http.post(url, null, { responseType: 'text' });
  }

  getActiveCartOfUser(userid: number): Observable<any> {
    let url = this.baseurl + 'GetActiveCartOfUser/' + userid;
    return this.http.get(url);
  }

  getAllPreviousCarts(userid: number): Observable<any> {
    let url = this.baseurl + 'GetAllPreviousCartsOfUser/' + userid;
    return this.http.get(url);
  }

  getPaymentMethods(): Observable<any[]> {
    let url = this.baseurl + 'GetPaymentMethods';
    return this.http.get<PaymentMethod[]>(url);
  }

  insertPayment(body: any): Observable<any> {
    const url = this.baseurl + 'InsertPayment';

    return this.http.post(url, body);
  }

  insertOrder(order: any): Observable<any> {
    return this.http.post(this.baseurl + 'InsertOrder', order);
  }

  getAllOffers(): Observable<any> {
    return this.http.get(this.baseurl + "GetAllOffers");
  }

  insertProduct(product: Product): Observable<any> {
    return this.http.post(this.baseurl + "InsertProduct", product)
  }

  deleteProduct(id: number): Observable<any> {
    return this.http.delete(this.baseurl + "DeleteProduct/" + id);
  }

  getAllOrders(): Observable<any> {
    return this.http.get(this.baseurl + 'GetAllOrders');
  }

  getAllUsers(): Observable<any> {
    return this.http.get(this.baseurl + 'GetAllUsers');
  }
}
