import { Component, OnInit, Input } from '@angular/core';
import { NameData } from '../_models/NameData';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { TeacherService } from '../_services/teacher.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css']
})

export class DetailComponent implements OnInit {
  @Input() selectedData: NameData;

  constructor(
    private route: ActivatedRoute,
    private teacherService: TeacherService,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.getData();
  }

  getData(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.teacherService.getTeacher(id)
      .subscribe(teacher => this.selectedData = teacher);
  }

  goBack(): void {
    this.location.back();
  }

}
