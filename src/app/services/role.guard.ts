import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor() { }

  canActivate(): boolean {
    let token = localStorage.getItem('user');
    if (token) {
      // Decode the token to get the role
      const jwtHelper = new JwtHelperService();
      const decodedToken = jwtHelper.decodeToken(token);
      const role = decodedToken.role;

      // Check if the user has the required role (e.g., 'admin')
      if (role === 'admin') {
        console.log(role)
        return true;
      } else {
        alert("You dont have admin rights");
        return false;
      }
    }
    alert("Please Login");
    return false;
  }
}
