import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UtilityService } from './utility.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private utilityService: UtilityService, private router: Router) { }

  canActivate(): boolean {
    if (this.utilityService.isLoggedIn()) {
      // User is authenticated, allow access
      return true;
    } else {
      // User is not authenticated, redirect to login page
      alert("Please Login first")
      this.router.navigate(['/home']);
      return false;
    }
  }
}
