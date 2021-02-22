import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { ProtectedComponent } from './protected/protected.component';
import { TeacherComponent } from './teacher/teacher.component';
import { AuthGuardService } from './_services/auth-guard.service';

const routes: Routes = [
  { path: 'protected', component: ProtectedComponent, canActivate: [AuthGuardService] },
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: 'teacher', component: TeacherComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
