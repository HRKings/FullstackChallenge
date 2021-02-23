import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NameData } from '../_models/NameData';
import { TeacherService } from '../_services/teacher.service';

@Component({
  selector: 'app-teacher',
  templateUrl: './teacher.component.html',
  styleUrls: ['./teacher.component.css']
})
export class TeacherComponent implements OnInit {
  teachers: NameData[];
  courses: NameData[];
  selectedTeacher?: NameData;

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  newTeacher = '';
  columnsToDisplay = ['id', 'name'];

  onSelect(teacher: NameData): void {
    this.courses = null;
    this.selectedTeacher = teacher;
    this.getCourses();
  }

  constructor(private teacherService: TeacherService, private _snackBar: MatSnackBar) { }

  getTeachers(event?: PageEvent) {
    this.pageIndex = event ? event.pageIndex : 0;
    this.teacherService.getTeachersPaged(this.pageIndex + 1, 10).subscribe(teachers => {
      if (teachers)
        this.teachers = teachers
      else
        this._snackBar.open('There were no teachers to display.');
    });
    return event;
  }

  getCourses(): void {
    if (this.selectedTeacher)
      this.teacherService.getCourses(this.selectedTeacher.id).subscribe(courses => {
        if (courses)
          this.courses = courses
        else
          this._snackBar.open('There were no courses to display.');
      });
  }

  getTeachersCount(): void {
    this.teacherService.getTeachersCount().subscribe(teachers => this.resultsLength = teachers);
  }

  update(): void {
    if (this.selectedTeacher) {
      this.teacherService.updateTeacher(this.selectedTeacher).subscribe(() => { this.getTeachers(); this.getTeachersCount(); });
      this.selectedTeacher = null;
    }
  }

  delete(): void {
    if (this.selectedTeacher) {
      this.teacherService.deleteTeacher(this.selectedTeacher.id).subscribe(() => { this.getTeachers(); this.getTeachersCount(); });
      this.selectedTeacher = null;
    }
  }

  create(): void {
    if (this.newTeacher) {
      this.teacherService.createTeacher(this.newTeacher).subscribe(() => { this.getTeachers(); this.getTeachersCount(); });
      this.newTeacher = null;
    }
  }

  ngOnInit(): void {
    this.getTeachers();
    this.getTeachersCount();
  }
}
