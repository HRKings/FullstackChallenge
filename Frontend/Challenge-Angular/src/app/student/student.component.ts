import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../_models/NameData';
import { StudentService } from '../_services/student.service';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {
  students: NameData[];
  selectedStudent?: NameData;

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  newStudent = '';
  columnsToDisplay = ['id', 'name'];

  onSelect(student: NameData): void {
    this.selectedStudent = student;
  }

  constructor(private studentService: StudentService) { }

  getStudents(event?: PageEvent) {
    this.pageIndex = event ? event.pageIndex : 0;
    this.studentService.getStudentsPaged(this.pageIndex + 1, 10).subscribe(students => this.students = students);
    return event;
  }

  getStudentsCount(): void {
    this.studentService.getStudentsCount().subscribe(students => this.resultsLength = students);
  }

  update(): void {
    if (this.selectedStudent) {
      this.studentService.updateStudent(this.selectedStudent).subscribe(() => { this.getStudents(); this.getStudentsCount() });
      this.selectedStudent = null;
    }
  }

  delete(): void {
    if (this.selectedStudent) {
      this.studentService.deleteStudent(this.selectedStudent.id).subscribe(() => { this.getStudents(); this.getStudentsCount() });
      this.selectedStudent = null;
    }
  }

  create(): void {
    if (this.newStudent) {
      this.studentService.createStudent(this.newStudent).subscribe(() => { this.getStudents(); this.getStudentsCount() });
      this.newStudent = null;
    }
  }

  ngOnInit(): void {
    this.getStudents();
    this.getStudentsCount();
  }
}
