import {Component, HostListener} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  imports: [
    RouterLink,
    NgIf
  ]
})
export class NavbarComponent {
  private router: Router;
  constructor(AuthService: AuthService, router: Router, authService: AuthService) {
    this.authService = AuthService,
    this.router =  router;
  }
  isMenuOpen: boolean = false;

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu(): void {
    this.isMenuOpen = false;
  }

  // Listen for clicks outside the menu to close it
  authService: AuthService;
  @HostListener('document:click', ['$event.target'])
  onClickOutside(target: HTMLElement): void {
    const clickedInside = target.closest('.navbar-links') || target.closest('.hamburger');
    if (!clickedInside) {
      this.isMenuOpen = false;
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/publishers']);

  }
}
