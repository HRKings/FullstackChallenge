import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NameData } from '../_models/NameData';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

  }

  getStudents(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>("https://localhost:5001/Student", { headers: headers });
  }

  getStudentsPaged(page: number, pageSize: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`https://localhost:5001/Student?${page}&pageSize=${pageSize}`, { headers: headers });
  }

  getStudentsCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>("https://localhost:5001/Student/total", { headers: headers });
  }

  getStudent(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`https://localhost:5001/Student/${id}`, { headers: headers });
  }

  createStudent(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>('https://localhost:5001/Student', { name: newName }, { headers: headers });
  }

  updateStudent(student: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`https://localhost:5001/Student/${student.id}`, { name: student.name }, { headers: headers });
  }

  deleteStudent(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`https://localhost:5001/Student/${id}`, { headers: headers });
  }
}
