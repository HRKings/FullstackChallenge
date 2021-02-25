import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { CourseComponent } from './components/course/course.component';
import { HomeComponent } from './components/home/home.component';
import { ProtectedComponent } from './components/protected/protected.component';
import { StudentAssociationComponent } from './components/student-association/student-association.component';
import { StudentComponent } from './components/student/student.component';
import { TeacherAssociationComponent } from './components/teacher-association/teacher-association.component';
import { TeacherComponent } from './components/teacher/teacher.component';
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
