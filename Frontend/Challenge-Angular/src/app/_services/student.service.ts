import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NameData } from '../_models/NameData';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

  }

  // Request all students
  getStudents(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Student`, { headers: headers });
  }

  // Request all the courses for a student
  getCourses(id: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Student/${id}/courses`, { headers: headers });
  }

  // Request the students but with pagination support
  getStudentsPaged(page: number, pageSize: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Student?${page}&pageSize=${pageSize}`, { headers: headers });
  }

  // Returns the amount of students, useful for pagination
  getStudentsCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>(`${environment.apiEndpoint}/Student/total`, { headers: headers });
  }

  // Returns a student
  getStudent(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`${environment.apiEndpoint}/Student/${id}`, { headers: headers });
  }

  // Creates a new students
  createStudent(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>('${environment.apiEndpoint}/Student', { name: newName }, { headers: headers });
  }

  // Updates a students
  updateStudent(student: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`${environment.apiEndpoint}/Student/${student.id}`, { name: student.name }, { headers: headers });
  }

  // Deletes a student
  deleteStudent(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`${environment.apiEndpoint}/Student/${id}`, { headers: headers });
  }
}
