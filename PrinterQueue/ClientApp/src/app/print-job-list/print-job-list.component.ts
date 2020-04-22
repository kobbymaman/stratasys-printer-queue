import { OnInit, Component, Inject, ElementRef, ViewChild } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import * as moment from 'moment';
import { MessageDialog } from '../message-dialog/message-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-print-job-list',
  templateUrl: './print-job-list.component.html',
})
export class PrintJobListComponent implements OnInit {
  private hubConnection: signalR.HubConnection;

  private currentTime: moment.Moment = moment();
  private printJobs: IPrintJob[];
  private jobDurationOptions = {
    showYears: false,
    showMonths: false,
    showWeeks: false,
    showDays: false,
    showLetters: false,
    zeroValue: null,
    previewFormat: "{{h}} Hours : {{m}} Minutes : {{s}} Seconds"
  };
  private newJobDurationISO: string;
  @ViewChild('newJobNameInput', { static: true }) newJobNameInput: ElementRef;

  constructor(private readonly dialog: MatDialog, private readonly httpClient: HttpClient, @Inject('BASE_URL') private readonly baseUrl: string) {

  }

  public async ngOnInit(): Promise<void>  {
    this.setSignalREvents();
    await this.getAndDisplayAllJobs();
    await this.initPrinting();
  }

  public async getAndDisplayAllJobs() : Promise<void> {
    await this.httpClient.get<IPrintJob[]>(this.baseUrl + 'Job').subscribe(resultArr => {
      if (resultArr.length == 1) {
        this.currentTime = moment();
      }
      let jobTime = moment(this.currentTime);
      resultArr.forEach(job => {
        job.startDate = jobTime.toDate();
        jobTime = jobTime.add(moment.duration(job.duration));
        job.endDate = jobTime.toDate();
      });

      this.printJobs = resultArr;
    }, error => {
        let dialogRef = this.dialog.open(MessageDialog, {
          data: { message: error.message },
        });
    });
  }

  public async initPrinting() {
    this.httpClient.post<IPrintJob[]>(this.baseUrl + 'Job/InitPrinting', undefined).subscribe(async result => {

    }, err => {
      let dialogRef = this.dialog.open(MessageDialog, {
        data: { message: err.error },
      });
    });
  }

  public async addNewJob() : Promise<void> {
    const printJob = <IPrintJob>{
      name: this.newJobNameInput.nativeElement.value,
      duration: this.newJobDurationISO
    }

    await this.httpClient.post<IPrintJob[]>(this.baseUrl + 'Job/NewJob', printJob).subscribe(async result => {
      this.newJobNameInput.nativeElement.value = "";

      await this.getAndDisplayAllJobs();
    }, err => {
        let dialogRef = this.dialog.open(MessageDialog, {
          data: { message: err.error },
        });
    });
  }

  public async moveUp(id: number) {
    this.httpClient.post<IPrintJob[]>(this.baseUrl + 'Job/MoveUp?id=' + id, undefined).subscribe(async result => {
      await this.getAndDisplayAllJobs();
    }, err => {
        let dialogRef = this.dialog.open(MessageDialog, {
          data: { message: err.error },
        });
    });
  }

  public moveDown(id: number) {
    this.httpClient.post<IPrintJob[]>(this.baseUrl + 'Job/MoveDown?id=' + id, undefined).subscribe(async result => {
      await this.getAndDisplayAllJobs();
    }, err => {
        let dialogRef = this.dialog.open(MessageDialog, {
          data: { message: err.error },
        });
    });
  }

  public cancelPrintingJob() {
    this.httpClient.post<IPrintJob[]>(this.baseUrl + 'Job/CancelPrintingJob', undefined).subscribe(async result => {
      this.currentTime = moment();

      await this.getAndDisplayAllJobs();
    }, err => {
      let dialogRef = this.dialog.open(MessageDialog, {
        data: { message: err.error },
      });
    });
  }

  public delete(id: number) {
    this.httpClient.delete<IPrintJob[]>(this.baseUrl + 'Job?id=' + id, undefined).subscribe(async result => {
      await this.getAndDisplayAllJobs();
    }, err => {
      let dialogRef = this.dialog.open(MessageDialog, {
        data: { message: err.error },
      });
    });
  }

  public setSignalREvents() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.baseUrl + 'jobstatus').build();

    this.hubConnection.start().then(() => {
      console.log("connection started");
    }).catch(err => console.log(err));

    this.hubConnection.onclose(() => {
      setTimeout(() => {
        this.hubConnection.start().then(() => {
          console.log("connection restarted");
        }).catch(err => console.log(err));
      }, 5000);
    });

    this.hubConnection.on('printjobcompleted', async (info) => {
      console.log(info);
      await this.getAndDisplayAllJobs();
    });
  }
}
