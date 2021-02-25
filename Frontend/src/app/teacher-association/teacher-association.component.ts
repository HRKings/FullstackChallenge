import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../_models/NameData';
import { AssociationService } from '../_services/association.service';
import { TeacherAssociation } from '../_dto/TeacherAssociation';
import { TeacherService } from '../_services/teacher.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-teacher-association',
  templateUrl: './teacher-association.component.html',
  styleUrls: ['./teacher-association.component.css']
})
export class TeacherAssociationComponent implements OnInit {
  courses: NameData[];
  teachers: NameData[];

  selectedTeacher?: TeacherAssociation;
  newTeacherId = '';
  newTeacherCourse = '';

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  error: boolean;

  columnsToDisplay = ['id', 'course'];

  onSelect(teacher: TeacherAssociation): void {
    this.selectedTeacher = teacher;
  }

  constructor(private associationService: AssociationService, private teacherService: TeacherService, private _snackBar: MatSnackBar) { }

  // Populates the table
  getTeachers(): void {
    this.associationService.getTeachers().subscribe(teachers => {
      if (teachers) {
        this.teachers = teachers
      } else {
        this._snackBar.open('There were no associations to display.');
      }
    });
  }

  // Creates a new association between a teacher and a course
  create(): void {
    if (this.newTeacherId && this.newTeacherCourse) {
      this.associationService.teacherToCourse(+this.newTeacherId, +this.newTeacherCourse).subscribe((response) => this.getTeachers());
      this.newTeacherId = null;
      this.newTeacherCourse = null;
    }
  }

  // Removes a teacher from a course
  delete(): void {
    if (this.selectedTeacher) {
      this.associationService.removeTeacher(this.selectedTeacher.teacherId, this.selectedTeacher.courseId).subscribe(() => this.getTeachers());
      this.selectedTeacher = null;
    }
  }


  ngOnInit(): void {
    this.getTeachers();
  }

}
