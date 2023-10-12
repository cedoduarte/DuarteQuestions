import { Injectable, inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export enum MessageType {
  Success,
  Information,
  Critical,
  Warning
}

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  private toastr: ToastrService = inject(ToastrService);

  constructor() {}

  public showMessage(messageType: MessageType, title: string, content: string): void {
    switch (messageType) {
      case MessageType.Success:
        this.toastr.success(content, title);
        break;
      case MessageType.Information:
        this.toastr.info(content, title);
        break;
      case MessageType.Critical:
        this.toastr.error(content, title);
        break;
      case MessageType.Warning:
        this.toastr.warning(content, title);
        break;
    }
  }
}
