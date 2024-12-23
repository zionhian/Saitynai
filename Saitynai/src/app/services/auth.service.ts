import {Injectable} from '@angular/core';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor() { }
  public getRoles(): string[]
  {
  let jwtToken = localStorage.getItem("access_token");

  if (jwtToken) {
    const helper = new JwtHelperService();
    const decoded = helper.decodeToken(jwtToken);
    const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

    return decoded[roleClaim];
  }
  return [];
  }
  public setLoginJwt(jwtToken: string)
  {
    localStorage.setItem("access_token", jwtToken);
  }

  isPublisher(): boolean {
    const roles = this.getRoles();
    return roles.includes('Publisher');

  }
  isAdmin(): boolean {
    const roles = this.getRoles();
    return roles.includes('Admin');

  }
  isUser(): boolean {
    const roles = this.getRoles();
    return roles.includes('User');

  }
  isLoggedin(): boolean
  {
    const roles = this.getRoles();
    return roles.length > 0
  }
  logout(): void
  {
    localStorage.removeItem("access_token");
  }
}
