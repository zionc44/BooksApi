import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  public progress: number;
  public message: string;
  public documentId: string;
  public progress2: number;
  public message2: string;
  public documentId2: string;
  @Output() public onUploadFinished = new EventEmitter();
  constructor(private http: HttpClient) { }
  ngOnInit() {
  }
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('documentFile', fileToUpload, fileToUpload.name);
    formData.append('documentName', "שם מסמך עבור")


    this.http.post('https://localhost:44365/api/Files/UploadFile', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.documentId = (<any>event.body).documentId;
          this.onUploadFinished.emit(event.body);
        }
      });
  }

  public copyFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post('https://localhost:44365/api/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress2 = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message2 = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
}
