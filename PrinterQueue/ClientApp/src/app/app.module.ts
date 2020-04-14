import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { PrintJobListComponent } from './print-job-list/print-job-list.component';
import { MessageDialog } from './message-dialog/message-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DurationPickerModule } from 'ngx-duration-picker'
import { MatButtonModule, MatFormFieldModule, MatInputModule, MatDialogModule } from '@angular/material'

@NgModule({
  entryComponents: [
    MessageDialog
  ],
  declarations: [
    AppComponent,
    PrintJobListComponent,
    MessageDialog
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    AngularFontAwesomeModule,
    MatDialogModule,
    DurationPickerModule,
    MatButtonModule, MatFormFieldModule, MatInputModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
