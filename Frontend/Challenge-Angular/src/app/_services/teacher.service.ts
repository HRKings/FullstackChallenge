import { Injectable, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';

import { NameData } from '../_models/NameData';
import { AuthService } from './auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TeacherService implements OnInit {

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit() {

  }

  getTeachers(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>("https://localhost:5001/Teacher", { headers: headers });
  }

  getTeachersPaged(page: number, pageSize: number): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`https://localhost:5001/Teacher?page=${page}&pageSize=${pageSize}`, { headers: headers });
  }

  getTeachersCount(): Observable<number> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<number>("https://localhost:5001/Teacher/total", { headers: headers });
  }

  getTeacher(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData>(`https://localhost:5001/Teacher/${id}`, { headers: headers });
  }

  createTeacher(newName: string): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<NameData>('https://localhost:5001/Teacher', { name: newName }, { headers: headers });
  }

  updateTeacher(teacher: NameData): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.put<NameData>(`https://localhost:5001/Teacher/${teacher.id}`, { name: teacher.name }, { headers: headers });
  }

  deleteTeacher(id: number): Observable<NameData> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.delete<NameData>(`https://localhost:5001/Teacher/${id}`, { headers: headers });
  }
}
