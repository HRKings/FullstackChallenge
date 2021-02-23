import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../_models/NameData';
import { AssociationService } from '../_services/association.service';
import { StudentAssociation } from '../_dto/StudentAssociation';
import { StudentService } from '../_services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-student-association',
  templateUrl: './student-association.component.html',
  styleUrls: ['./student-association.component.css']
})
export class StudentAssociationComponent implements OnInit {
  courses: NameData[];
  students: NameData[];

  selectedStudent?: StudentAssociation;
  newStudentId = '';
  newStudentCourse = '';

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  error: boolean;

  columnsToDisplay = ['id', 'course'];

  onSelect(student: StudentAssociation): void {
    this.selectedStudent = student;
  }

  constructor(private associationService: AssociationService, private studentService: StudentService, private _snackBar: MatSnackBar) { }

  // Populate the table
  getStudents(): void {
    this.associationService.getStudents().subscribe(students => {
      if (students) {
        this.students = students
      } else {
        this._snackBar.open('There were no associations to display.');
      }
    });
  }

  // Associates a student to a course
  create(): void {
    if (this.newStudentId && this.newStudentCourse) {
      this.associationService.studentToCourse(+this.newStudentId, +this.newStudentCourse).subscribe((response) => this.getStudents());
      this.newStudentId = null;
      this.newStudentCourse = null;
    }
  }

  // Removes a student from a course
  delete(): void {
    if (this.selectedStudent) {
      this.associationService.removeStudent(this.selectedStudent.studentId, this.selectedStudent.courseId).subscribe(() => this.getStudents());
      this.selectedStudent = null;
    }
  }


  ngOnInit(): void {
    this.getStudents();
  }

}
