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
    var fileName = fileToUpload.name.replace(/\s/g, "_");
    formData.append('file', fileToUpload, fileName);
    let creDocument = {
      documentName: "bla bla bla",
      documentFile: fileToUpload
    }

    for (const prop in creDocument) {
      if (!creDocument.hasOwnProperty(prop)) { continue; }
      formData.append(prop, creDocument[prop]);
    }
    console.log("book===>", creDocument);

    this.http.post('https://localhost:44365/api/Files/UploadFile', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  }
}
