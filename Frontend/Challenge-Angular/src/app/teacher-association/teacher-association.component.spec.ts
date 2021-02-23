import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherAssociationComponent } from './teacher-association.component';

describe('TeacherAssociationComponent', () => {
  let component: TeacherAssociationComponent;
  let fixture: ComponentFixture<TeacherAssociationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeacherAssociationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeacherAssociationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
