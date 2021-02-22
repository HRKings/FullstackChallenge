import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { CourseComponent } from './course/course.component';
import { ProtectedComponent } from './protected/protected.component';
import { StudentComponent } from './student/student.component';
import { TeacherComponent } from './teacher/teacher.component';
import { AuthGuardService } from './_services/auth-guard.service';

const routes: Routes = [
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: 'protected', component: ProtectedComponent, canActivate: [AuthGuardService] },
  { path: 'teacher', component: TeacherComponent, canActivate: [AuthGuardService] },
  { path: 'student', component: StudentComponent, canActivate: [AuthGuardService] },
  { path: 'course', component: CourseComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
