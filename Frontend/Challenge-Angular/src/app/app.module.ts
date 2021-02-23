import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProtectedComponent } from './protected/protected.component';
import { AuthGuardService } from './_services/auth/auth-guard.service';
import { AuthService } from './_services/auth/auth.service';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { TeacherComponent } from './teacher/teacher.component';
import { StudentComponent } from './student/student.component';
import { CourseComponent } from './course/course.component';
import { TeacherAssociationComponent } from './teacher-association/teacher-association.component';
import { StudentAssociationComponent } from './student-association/student-association.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    ProtectedComponent,
    AuthCallbackComponent,
    TeacherComponent,
    StudentComponent,
    CourseComponent,
    TeacherAssociationComponent,
    StudentAssociationComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatButtonModule,
    MatTabsModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatSnackBarModule
  ],
  providers: [AuthGuardService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
