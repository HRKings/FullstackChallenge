import { Injectable, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';

import { NameData } from '../_models/NameData';
import { AuthService } from './auth/auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TeacherService implements OnInit {

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

  }

  // Request all teachers
  getTeachers(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>("https://localhost:5001/Teacher", { headers: headers });
  }

  // Request the courses of a teacher
  getCourses(id: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`https://localhost:5001/Teacher/${id}/courses`, { headers: headers });
  }

  // Request all the teachers but with pagination
  getTeachersPaged(page: number, pageSize: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`https://localhost:5001/Teacher?page=${page}&pageSize=${pageSize}`, { headers: headers });
  }

  // Gets the amount of teachers on the database
  getTeachersCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>("https://localhost:5001/Teacher/total", { headers: headers });
  }

  // Gets a teacher
  getTeacher(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`https://localhost:5001/Teacher/${id}`, { headers: headers });
  }

  // Creates a new teacher
  createTeacher(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>('https://localhost:5001/Teacher', { name: newName }, { headers: headers });
  }

  // Updates a teacher
  updateTeacher(teacher: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`https://localhost:5001/Teacher/${teacher.id}`, { name: teacher.name }, { headers: headers });
  }


  // Deletes a teacher
  deleteTeacher(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`https://localhost:5001/Teacher/${id}`, { headers: headers });
  }
}
