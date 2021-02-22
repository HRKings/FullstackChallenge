import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { NameData } from '../_models/NameData';
import { CourseService } from '../_services/course.service';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit {
  courses: NameData[];
  selectedCourse?: NameData;

  pageIndex: number;
  pageEvent: PageEvent;
  resultsLength: number;

  newCourse = '';
  columnsToDisplay = ['id', 'name'];

  onSelect(course: NameData): void {
    this.selectedCourse = course;
  }

  constructor(private courseService: CourseService) { }

  getCourses(event?: PageEvent) {
    this.pageIndex = event ? event.pageIndex : 0;
    this.courseService.getCoursesPaged(this.pageIndex + 1, 10).subscribe(courses => this.courses = courses);
    return event;
  }

  getCoursesCount(): void {
    this.courseService.getCoursesCount().subscribe(courses => this.resultsLength = courses);
  }

  update(): void {
    if (this.selectedCourse) {
      this.courseService.updateCourse(this.selectedCourse).subscribe(() => { this.getCourses(); this.getCoursesCount() });
      this.selectedCourse = null;
    }
  }

  delete(): void {
    if (this.selectedCourse) {
      this.courseService.deleteCourse(this.selectedCourse.id).subscribe(() => { this.getCourses(); this.getCoursesCount() });
      this.selectedCourse = null;
    }
  }

  create(): void {
    if (this.newCourse) {
      this.courseService.createCourse(this.newCourse).subscribe(() => { this.getCourses(); this.getCoursesCount() });
      this.newCourse = null;
    }
  }

  ngOnInit(): void {
    this.getCourses();
    this.getCoursesCount();
  }

}
