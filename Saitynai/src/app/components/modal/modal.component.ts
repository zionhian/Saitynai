import { Component } from '@angular/core';
import { trigger, style, animate, transition } from '@angular/animations';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
  animations: [
    trigger('slideIn', [
      transition(':enter', [
        style({transform: 'translateX(100%)', opacity: 0}),
        animate('300ms ease-out', style({transform: 'translateX(0)', opacity: 1})),
      ]),
      transition(':leave', [
        animate('300ms ease-in', style({transform: 'translateX(100%)', opacity: 0})),
      ]),
    ]),
  ],
  imports: [
    NgIf
  ]
})
export class ModalComponent {
  isVisible = false;
  message = '';
  isConfirmation = false;

  show(message: string, isConfirmation: boolean): void {
    this.message = message;
    this.isConfirmation = isConfirmation;
    this.isVisible = true;
  }

  onConfirm(): void {
    this.isVisible = false;
  }

  onCancel(): void {
    this.isVisible = false;
  }
}
