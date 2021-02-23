import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { CourseComponent } from './course/course.component';
import { HomeComponent } from './home/home.component';
import { ProtectedComponent } from './protected/protected.component';
import { StudentAssociationComponent } from './student-association/student-association.component';
import { StudentComponent } from './student/student.component';
import { TeacherAssociationComponent } from './teacher-association/teacher-association.component';
import { TeacherComponent } from './teacher/teacher.component';
import { AuthGuardService } from './_services/auth/auth-guard.service';

const routes: Routes = [
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: '', component: HomeComponent },
  { path: 'protected', component: ProtectedComponent, canActivate: [AuthGuardService] },
  { path: 'teacher', component: TeacherComponent, canActivate: [AuthGuardService] },
  { path: 'student', component: StudentComponent, canActivate: [AuthGuardService] },
  { path: 'course', component: CourseComponent, canActivate: [AuthGuardService] },
  { path: 'teacher-association', component: TeacherAssociationComponent, canActivate: [AuthGuardService] },
  { path: 'student-association', component: StudentAssociationComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
