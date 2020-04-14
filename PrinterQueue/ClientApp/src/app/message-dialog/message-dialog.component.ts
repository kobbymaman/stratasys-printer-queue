import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-message-dialog',
  template: '{{ data.message }}',
})
export class MessageDialog {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }
}
