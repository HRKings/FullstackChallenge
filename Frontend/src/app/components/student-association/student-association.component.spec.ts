import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentAssociationComponent } from './student-association.component';

describe('StudentAssociationComponent', () => {
  let component: StudentAssociationComponent;
  let fixture: ComponentFixture<StudentAssociationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StudentAssociationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentAssociationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
