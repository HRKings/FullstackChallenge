import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-protected',
  templateUrl: './protected.component.html',
  styleUrls: ['./protected.component.css']
})
export class ProtectedComponent implements OnInit {
  oidClaims: string;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.oidClaims = this.authService.getClaims().given_name;
  }

  logoutClick() {
    this.authService.startLogout();
  }

}
