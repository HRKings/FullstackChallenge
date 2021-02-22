import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NameData } from '../_models/NameData';
import { TeacherService } from '../_services/teacher.service';

@Component({
  selector: 'app-teacher',
  templateUrl: './teacher.component.html',
  styleUrls: ['./teacher.component.css']
})
export class TeacherComponent implements OnInit {
  teachers: NameData[];
  selectedTeacher?: NameData;

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  newTeacher = '';
  columnsToDisplay = ['id', 'name'];

  onSelect(teacher: NameData): void {
    this.selectedTeacher = teacher;
  }

  constructor(private teacherService: TeacherService) { }

  getTeachers(event?: PageEvent) {
    this.pageIndex = event ? event.pageIndex : 0;
    this.teacherService.getTeachersPaged(this.pageIndex + 1, 10).subscribe(teachers => this.teachers = teachers);
    return event;
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
