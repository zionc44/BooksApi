import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public name: string;
  public response: any;
  public document: any;

  constructor(
    private http: HttpClient) { }

  ngOnInit() {
  }

  public uploadFinished = (event) => {
    this.response = event;
    console.log("uploadFinished====>", event);
  }

  public downloadFile(): void {
    this.http.get(
      'https://localhost:44365/api/Files/DownloadFile?documentId=' + this.name,
      { observe: 'response', responseType: 'blob' as 'json' }).subscribe(
        (response: HttpResponse<Blob>) => {
          var contentDisposition = response.headers.get('content-disposition');
          
          console.log("contentDisposition===>", contentDisposition.split(';')[2].split('filename*')[1].split('=')[1].trim());  

          var utf8FileName = contentDisposition.split(';')[2].split('filename*')[1].split('=UTF-8\'\'')[1].trim();
          var filename = decodeURIComponent(utf8FileName.replace(/\+/g," "));
          
          console.log("filename====>",filename);

          let binaryData = [];
          binaryData.push(response.body);
          let downloadLink = document.createElement('a');
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: 'blob' }));
          downloadLink.setAttribute('download', filename);
          document.body.appendChild(downloadLink);
          downloadLink.click();
        }
      )
  }
}
