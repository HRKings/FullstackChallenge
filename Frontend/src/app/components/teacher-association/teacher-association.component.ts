import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../../_models/NameData';
import { AssociationService } from '../../_services/association.service';
import { TeacherAssociation } from '../../_dto/TeacherAssociation';

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

  hasError: boolean;
  errorMessage: string;

  error: boolean;

  columnsToDisplay = ['id', 'course'];

  onSelect(teacher: TeacherAssociation): void {
    this.selectedTeacher = teacher;
  }

  constructor(private associationService: AssociationService) { }

  // Populates the table
  getTeachers(): void {
    this.associationService.getTeachers().subscribe(teachers => {
      if (teachers) {
        this.teachers = teachers
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

  // Creates a new association between a teacher and a course
  create(): void {
    if (this.newTeacherId && this.newTeacherCourse) {
      this.associationService.teacherToCourse(+this.newTeacherId, +this.newTeacherCourse).subscribe(() => this.getTeachers());
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
