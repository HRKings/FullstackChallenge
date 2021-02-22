import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { NameData } from '../_models/NameData';
import { TEACHERS } from '../_models/Mock';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {

  getTeachers(): Observable<NameData[]> {
    const teachers = of(TEACHERS);
    return teachers;
  }

  getTeacher(id: number): Observable<NameData> {
    return of(TEACHERS.find(hero => hero.id === id));
  }

  constructor() { }
}
