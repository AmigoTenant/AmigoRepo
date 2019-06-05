import { ResponseListDTO } from './../dto/response-list-dto';
import { Component, OnInit, Input } from '@angular/core';
import { UploadFileService } from './upload-file-service';

@Component({
  selector: 'at-upload-file',
  templateUrl: './upload-file.component.html',
  //styleUrls: ['./upload-file.component.css'],
  providers: [UploadFileService]
})
export class UploadFileComponent implements OnInit {

  imageUrl: string = "/assets/img/image-default.jpeg";
  fileToUpload : File= null;
  @Input() entityCode: string;
  @Input() parentId: string;
  fileRepositoryData: any[];
  public isSaving = false;

  constructor(private imageService: UploadFileService) { }

  ngOnInit() {
    this.getFileRepositories();
  }

  getFileRepositories()
  {
    this.imageService.getFileRepositories(this.entityCode, this.parentId).subscribe(
      res => {
          let datagrid = new ResponseListDTO(res);
          this.fileRepositoryData = datagrid.items;
      }
    );
  }

  handleFileInput(file: FileList){
    this.fileToUpload = file.item(0);

    let reader = new FileReader();
    reader.onload = (event: any) =>{
      this.imageUrl = event.target.result;
    }
    reader.readAsDataURL(this.fileToUpload);

  }

  onSubmit(Additional, Image){

    this.isSaving = true;
    this.imageService.postFile(this.entityCode, this.parentId, Additional.value, this.fileToUpload).subscribe(
      data=> {
        console.log('done');
        Additional.value = null;
        Image.value = null;
        this.imageUrl =  "/assets/img/image-default.jpeg";
      }
    )
    .add(
      r=> {
        this.getFileRepositories();
        this.isSaving = false;
      }
    )
    
    ;
  }

  onDownload(id: any){
    debugger;
    this.imageService.downloadFile(id)
      .subscribe(response => {
        const contentDisp = response.headers.get('Content-Disposition');
        debugger;
        const fileName = this.GetFileNameFromContentDisp(contentDisp);
        this.downloadFile([response.body], fileName);
      });
  }

  public downloadFile(blob: (ArrayBuffer | ArrayBufferView | Blob | string)[], fileName: string) {
    const file = new File(blob, fileName);
    if (window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(file);
    } else {
      const elem = window.document.createElement('a');
      const url = window.URL.createObjectURL(file);
      elem.href = url;
      elem.download = fileName;
      document.body.appendChild(elem);
      elem.click();
      document.body.removeChild(elem);
      window.URL.revokeObjectURL(url);
    }
  }

  public GetFileNameFromContentDisp(content: string) {
    const defaultName = 'download.dat';
    let filename = defaultName;
    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
    if (content && content.indexOf('attachment') !== -1) {
      const matches = filenameRegex.exec(content);
      if (matches != null && matches[1]) {
        filename = matches[1].replace(/['"]/g, '');
      }
    } else if (content && content.indexOf('inline') !== -1) {
      const matches = filenameRegex.exec(content);
      if (matches != null && matches[1]) {
        filename = matches[1].replace(/['"]/g, '');
      }
    }
    return filename;
  }

  public onDelete(data: any)
  {
    this.isSaving = true;
    this.imageService.deleteFile(data.fileRepositoryId)
    .subscribe()
    .add(
      r=>{
        this.getFileRepositories();
        this.isSaving = false;
      }
    );

  }

}
