import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NameData } from '../_models/NameData';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

  }

  // Gets all courses
  getCourses(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>("https://localhost:5001/Course", { headers: headers });
  }

  // Gets all courses but with pagination
  getCoursesPaged(page: number, pageSize: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`https://localhost:5001/Course?page${page}&pageSize=${pageSize}`, { headers: headers });
  }


  // Gets the amount of courses on the database
  getCoursesCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>("https://localhost:5001/Course/total", { headers: headers });
  }

  // Get a specific course
  getCourse(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`https://localhost:5001/Course/${id}`, { headers: headers });
  }

  // Creates a new couse
  createCourse(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>('https://localhost:5001/Course', { name: newName }, { headers: headers });
  }

  // Updates a course
  updateCourse(course: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`https://localhost:5001/Course/${course.id}`, { name: course.name }, { headers: headers });
  }

  // Deletes a course
  deleteCourse(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`https://localhost:5001/Course/${id}`, { headers: headers });
  }
}
