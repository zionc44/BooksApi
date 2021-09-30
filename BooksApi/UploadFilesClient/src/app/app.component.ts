import { UserToCreate } from './_interfaces/userToCreate.model';
import { User } from './_interfaces/user.model';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public isCreate: boolean;
  public name: string;
  public address: string;
  public user: UserToCreate;
  public users: User[] = [];
  public response: { dbPath: '' };
  public fileURL: SafeResourceUrl;
  public isFileReady: boolean = false;
  public document: any;

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.isCreate = true;
  }

  public onCreate = () => {
    this.user = {
      name: this.name,
      address: this.address,
      imgPath: this.response.dbPath
    }

    this.http.post('https://localhost:5001/api/users', this.user)
      .subscribe(res => {
        this.getUsers();
        this.isCreate = false;
      });
  }

  private getUsers = () => {
    this.http.get('https://localhost:5001/api/users')
      .subscribe(res => {
        this.users = res as User[];
      });
  }

  public returnToCreate = () => {
    this.isCreate = true;
    this.name = '';
    this.address = '';
  }

  public uploadFinished = (event) => {
    this.response = event;
    console.log("uploadFinished====>", event);
  }

  public createImgPath = (serverPath: string) => {
    return `https://localhost:5001/${serverPath}`;
  }

  public downloadFile1(): void {
    this.http.get(
      'https://localhost:44365/api/Files/DownloadFile?documentId=' + this.name,
      { observe: 'response', responseType: 'blob' as 'json' }).subscribe(
        (response: HttpResponse<Blob>) => {
          // let filename: string = this.getFileName(response)
          var contentDisposition = response.headers.get('content-disposition');
          var filename = contentDisposition.split(';')[1].split('filename')[1].split('=')[1].trim();
          
          // filename = filename.replace(/\s/g, "");
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



  public downloadFile() {
    this.isFileReady = false;
    console.log("downloadFile=====>", this.name);
    console.log("sanitizer=====>", this.sanitizer);

    this.http.get('https://localhost:44365/api/Files/DownloadFile?documentId=' + this.name)
      .subscribe(response => {
        console.log("response====>", response);
        this.document = response;
        const reportBlob = this.documentToBlob(response);
        this.fileURL = this.sanitizer.bypassSecurityTrustResourceUrl(window.URL.createObjectURL(reportBlob));
        this.isFileReady = true;
      })
  }

  public documentToBlob(doc: any) {
    // const byteString = window.atob(base64String);
    const arrayBuffer = new ArrayBuffer(doc.documentFileBytes.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < doc.documentFileBytes.length; i++) {
      int8Array[i] = doc.documentFileBytes.charCodeAt(i);
    }
    const blob = new Blob([int8Array], { type: doc.documentFileType });
    return blob;
  }

}
