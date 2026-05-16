import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ContactMessageDto } from '../../../../core/models/contact.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-messages-modal',
  imports: [DatePipe],
  templateUrl: './messages-modal.html',
  styleUrl: './messages-modal.css',
})
export class MessagesModal {

    @Input() message!: ContactMessageDto;

  constructor(public activeModal: NgbActiveModal) {}
}
