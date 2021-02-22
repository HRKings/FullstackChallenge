import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  links = [{ name: 'Home', link: '/' },
  { name: 'Auth', link: '/protected' },
  { name: 'Teacher', link: '/teacher' },
  { name: 'Student', link: '/student' },
  { name: 'Course', link: '/course' }
  ];
  activeLink = this.links[0];

  title = 'Challenge-Angular';
}
