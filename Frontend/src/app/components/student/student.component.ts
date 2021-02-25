import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../../_models/NameData';
import { PagedData } from '../../_models/PagedData';
import { StudentService } from '../../_services/student.service';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {
  students: PagedData;
  courses: NameData[];
  selectedStudent?: NameData;

  pageIndex: number;
  pageEvent: PageEvent;

  hasError: boolean;
  errorMessage: string;

  newStudent = '';
  columnsToDisplay = ['id', 'name'];

  onSelect(student: NameData): void {
    this.courses = null;
    this.selectedStudent = student;
    this.getCourses();
  }

  constructor(private studentService: StudentService) { }

  getStudents(event?: PageEvent) {
    this.pageIndex = event ? event.pageIndex : 0;
    this.studentService.getStudentsPaged(this.pageIndex + 1, 10).subscribe(students => {
      if (students) {
        this.students = students;
        this.hasError = false;
        this.errorMessage = null;
      }
    }, error => {
      if (error) {
        this.hasError = true;
        this.errorMessage = error.error;
        console.log(error);
      }
    });
    return event;
  }

  getCourses(): void {
    if (this.selectedStudent)
      this.studentService.getCourses(this.selectedStudent.id).subscribe(courses => {
        if (courses)
          this.courses = courses;
      }, error => {
        if (error) {
          console.log(error);
        }
      });
  }

  update(): void {
    if (this.selectedStudent) {
      this.studentService.updateStudent(this.selectedStudent).subscribe(() => this.getStudents());
      this.selectedStudent = null;
    }
  }

  delete(): void {
    if (this.selectedStudent) {
      this.studentService.deleteStudent(this.selectedStudent.id).subscribe(() => this.getStudents());
      this.selectedStudent = null;
    }
  }

  create(): void {
    if (this.newStudent) {
      this.studentService.createStudent(this.newStudent).subscribe(() => this.getStudents());
      this.newStudent = null;
    }
  }

  ngOnInit(): void {
    this.getStudents();
  }
}
