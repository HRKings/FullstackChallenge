import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // Create all links that will appear on the navigation bar
  links = [{ name: 'Home', link: '/' },
  { name: 'Auth', link: '/protected' },
  { name: 'Teacher', link: '/teacher' },
  { name: 'Student', link: '/student' },
  { name: 'Course', link: '/course' },
  { name: 'Associate Teacher', link: '/teacher-association' },
  { name: 'Association Student', link: '/student-association' }
  ];
  activeLink = this.links[0];

  title = 'Fullstack Challenge (Angular Frontend)';
}
