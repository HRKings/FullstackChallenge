import { Component, OnInit } from '@angular/core';
import { NameData } from '../../_models/NameData';
import { AssociationService } from '../../_services/association.service';
import { StudentAssociation } from '../../_dto/StudentAssociation';

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

  hasError: boolean;
  errorMessage: string;

  error: boolean;

  columnsToDisplay = ['id', 'course'];

  onSelect(student: StudentAssociation): void {
    this.selectedStudent = student;
  }

  constructor(private associationService: AssociationService) { }

  // Populate the table
  getStudents(): void {
    this.associationService.getStudents().subscribe(students => {
      if (students) {
        this.students = students
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
  }

  // Associates a student to a course
  create(): void {
    if (this.newStudentId && this.newStudentCourse) {
      this.associationService.studentToCourse(+this.newStudentId, +this.newStudentCourse).subscribe(() => this.getStudents(), error => {
        if (error) {
          this.hasError = true;
          this.errorMessage = error.error;
          console.log(error);
        }
      });
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
