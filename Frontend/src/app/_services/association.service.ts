import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { StudentAssociation } from '../_dto/StudentAssociation';
import { TeacherAssociation } from '../_dto/TeacherAssociation';
import { NameData } from '../_models/NameData';
import { AuthService } from './auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AssociationService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  // Assign a teacher to a course
  teacherToCourse(teacher: number, course: number): Observable<TeacherAssociation> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<TeacherAssociation>(`${environment.apiEndpoint}/Association/teacher`, { teacherId: teacher, courseId: course }, { headers: headers });
  }

  // Gets all teacher-course associations
  getTeachers(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Association/teacher`, { headers: headers });
  }

  // Assign a student to a course
  studentToCourse(student: number, course: number): Observable<StudentAssociation> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.post<StudentAssociation>(`${environment.apiEndpoint}/Association/student`, { studentId: student, courseId: course }, { headers: headers });
  }

  // Gets all student-course associations
  getStudents(): Observable<NameData[]> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    return this.http.get<NameData[]>(`${environment.apiEndpoint}/Association/student`, { headers: headers });
  }

  // Removes a teacher from a course
  removeTeacher(teacher: number, course: number): Observable<TeacherAssociation> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    const options = {
      headers: headers,
      body: { teacherId: teacher, courseId: course },
    };

    return this.http.delete<TeacherAssociation>(`${environment.apiEndpoint}/Association/teacher`, options);
  }

  // Removes a student from a course
  removeStudent(student: number, course: number): Observable<StudentAssociation> {
    let headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
    const options = {
      headers: headers,
      body: { studentId: student, courseId: course },
    };

    return this.http.delete<StudentAssociation>(`${environment.apiEndpoint}/Association/student`, options);
  }


}
