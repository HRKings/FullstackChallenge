import { Component, OnInit } from '@angular/core';
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

  onSelect(teacher: NameData): void {
    this.selectedTeacher = teacher;
  }

  constructor(private teacherService: TeacherService) { }

  getTeachers(): void {
    this.teacherService.getTeachers().subscribe(teachers => this.teachers = teachers);
  }

  ngOnInit(): void {
    this.getTeachers()
  }

}
