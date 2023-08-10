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
  baseurlCategory = 'https://localhost:44376/api/Categories/';
  baseurlCarts = 'https://localhost:44376/api/Carts/';
  baseurlOffers = 'https://localhost:44376/api/Offers/';
  baseurlOrders = 'https://localhost:44376/api/Orders/';
  baseurlPayments = 'https://localhost:44376/api/Payments/';
  baseurlProducts = 'https://localhost:44376/api/Products/';
  baseurlReviews = 'https://localhost:44376/api/Reviews/';
  baseurlUsers = 'https://localhost:44376/api/Users/';

  constructor(private http: HttpClient) { }

  getCategoryList(): Observable<Category[]> {
    let url = this.baseurlCategory + 'GetCategoryList';
    return this.http.get<any[]>(url).pipe(
      map((categories) =>
        categories?.map((category) => {
          let mappedCategory: Category = {
            id: category.id,
            category: category.category,
            subCategory: category.subCategory,
            photoUrl: category.photoUrl,
          };
          return mappedCategory;
        })
      )
    );
  }

  getBanner(subCategory: string): Observable<Blob> {
    const url = `${this.baseurlCategory}GetBannerImage?name=${subCategory}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  insertCategory(category: Category): Observable<any> {
    console.log(category);
    return this.http.post(this.baseurlCategory + 'InsertCategory', category, { responseType: 'text' });
  }

  editCategory(category: Category): Observable<any> {
    return this.http.put(this.baseurlCategory + "EditCategory", category, { responseType: 'text' });
  }

  deleteCategory(id: number): Observable<any> {
    return this.http.delete(this.baseurlCategory + "DeleteCategory/" + id);
  }

  getProducts(category: string, subCategory: string, count: number): Observable<any[]> {
    return this.http.get<any[]>(this.baseurlProducts + 'GetProducts', {
      params: new HttpParams()
        .set('category', category)
        .set('subCategory', subCategory)
        .set('count', count),
    });
  }

  getImage(id: number): Observable<Blob> {
    const url = `${this.baseurlProducts}GetImage/${id}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  getAllProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.baseurlProducts + 'GetAllProducts');
  }

  getProduct(id: number): Observable<any> {
    let url = this.baseurlProducts + 'GetProduct/' + id;
    return this.http.get(url);
  }

  updateProduct(product: Product): Observable<any> {
    let url = this.baseurlProducts + "UpdateProduct";
    return this.http.put(url, product);
  }

  insertProduct(product: Product): Observable<any> {
    return this.http.post(this.baseurlProducts + "InsertProduct", product)
  }

  deleteProduct(id: number, category: string, subCategory: string): Observable<any> {
    const url = `${this.baseurlProducts}DeleteProduct/${id}?category=${category}&subCategory=${subCategory}`;
    return this.http.delete(url);
  }

  registerUser(user: User): Observable<any> {
    let url = this.baseurlUsers + 'RegisterUser';
    return this.http.post(url, user, { responseType: 'text' });
  }

  loginUser(email: string, password: string): Observable<any> {
    let url = this.baseurlUsers + 'LoginUser';
    return this.http.post(
      url,
      { Email: email, Password: password },
      { responseType: 'text' }
    );
  }

  getAllUsers(): Observable<any> {
    return this.http.get(this.baseurlUsers + 'GetAllUsers');
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

    let url = this.baseurlReviews + 'InsertReview';
    return this.http.post(url, obj, { responseType: 'text' });
  }

  getAllReviewsOfProduct(productId: number): Observable<any> {
    let url = this.baseurlReviews + 'GetProductReviews/' + productId;
    return this.http.get(url);
  }

  addToCart(userid: number, productid: number): Observable<any> {
    let url = this.baseurlCarts + 'InsertCartItem/' + userid + '/' + productid;
    return this.http.post(url, null, { responseType: 'text' });
  }

  removeFromCart(userid: number, productid: number): Observable<any> {
    let url = this.baseurlCarts + 'RemoveCartItem/' + userid + '/' + productid;
    return this.http.post(url, null, { responseType: 'text' });
  }

  getActiveCartOfUser(userid: number): Observable<any> {
    let url = this.baseurlCarts + 'GetActiveCartOfUser/' + userid;
    return this.http.get(url);
  }

  getAllPreviousCarts(userid: number): Observable<any> {
    let url = this.baseurlCarts + 'GetAllPreviousCartsOfUser/' + userid;
    return this.http.get(url);
  }

  getPaymentMethods(): Observable<any[]> {
    let url = this.baseurlPayments + 'GetPaymentMethods';
    return this.http.get<PaymentMethod[]>(url);
  }

  insertPayment(body: any): Observable<any> {
    const url = this.baseurlPayments + 'InsertPayment';

    return this.http.post(url, body);
  }

  processStripePayment(data: any): Observable<any> {
    return this.http.post(this.baseurlPayments + 'ProcessStripePayment', data);
  }

  insertOrder(order: any): Observable<any> {
    return this.http.post(this.baseurlOrders + 'InsertOrder', order);
  }

  getAllOrders(): Observable<any> {
    return this.http.get(this.baseurlOrders + 'GetAllOrders');
  }

  getAllOffers(): Observable<any> {
    return this.http.get(this.baseurlOffers + "GetAllOffers");
  }

  insertOffer(offer: any): Observable<any> {
    return this.http.post(this.baseurlOffers + 'InsertOffer', offer);
  }
}
