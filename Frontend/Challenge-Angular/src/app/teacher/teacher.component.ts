import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-teacher',
  templateUrl: './teacher.component.html',
  styleUrls: ['./teacher.component.css']
})
export class TeacherComponent implements OnInit {

  response: Object;
  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue(), 'Access-Control-Allow-Origin': 'Content-Type, Authorization' });

    this.http.get("https://localhost:5001/Teacher", { headers: headers })
      .subscribe(response => this.callback(response));

  }

  callback(request) {
    this.response = request;
    console.log(request);
  }

}
