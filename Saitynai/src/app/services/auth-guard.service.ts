import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import {AuthService} from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private authService: AuthService;
  private router: Router;

  constructor(AuthService: AuthService, Router: Router) {
    this.authService = AuthService
    this.router = Router;
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const isLoggedIn = this.authService.isLoggedin();
    const allowWhenLoggedOut = route.data['allowWhenLoggedOut'];
    const isPublisher = this.authService.isPublisher();
    const isCreating = route.data['createGame'];
    if (isCreating && !isPublisher) {
      return false;
    }
    else if (isCreating) {
      return true;
    }

    if (allowWhenLoggedOut && isLoggedIn) {
      // Redirect to home if trying to access login/register while logged in
      this.router.navigate(['/publishers']);
      return false;
    }

    return true;
  }
}
