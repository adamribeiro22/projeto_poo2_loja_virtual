import { Component, Input } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { NavLink } from '../../../core/models/NavLink';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  @Input() navLinks: NavLink[] = [];
  @Input() title: string = 'App';
  @Input() titleLink: string = '/';

  constructor(private authService: AuthService) {}

  logout(): void {
    this.authService.logout();
  }
}