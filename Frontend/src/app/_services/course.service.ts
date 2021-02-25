import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NameData } from '../_models/NameData';
import { PagedData } from '../_models/PagedData';
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
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Course`, { headers: headers });
  }

  // Gets all courses but with pagination
  getCoursesPaged(page: number, pageSize: number): Observable<PagedData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<PagedData>(`${environment.apiEndpoint}/Course?page=${page}&pageSize=${pageSize}`, { headers: headers });
  }


  // Gets the amount of courses on the database
  getCoursesCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>(`${environment.apiEndpoint}/Course/total`, { headers: headers });
  }

  // Get a specific course
  getCourse(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`${environment.apiEndpoint}/Course/${id}`, { headers: headers });
  }

  // Creates a new couse
  createCourse(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>(`${environment.apiEndpoint}/Course`, { name: newName }, { headers: headers });
  }

  // Updates a course
  updateCourse(course: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`${environment.apiEndpoint}/Course/${course.id}`, { name: course.name }, { headers: headers });
  }

  // Deletes a course
  deleteCourse(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`${environment.apiEndpoint}/Course/${id}`, { headers: headers });
  }
}
