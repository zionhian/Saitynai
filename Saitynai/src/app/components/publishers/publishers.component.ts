import { Component, OnInit, ViewChild } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import {Client, PublisherGetDto, PublisherPostDto, PublisherPutDto} from '../../services/client.service';
import { NgForOf, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ModalComponent } from '../modal/modal.component';

@Component({
  selector: 'app-publishers',
  templateUrl: './publishers.component.html',
  imports: [
    RouterLink,
    NgForOf,
    RouterLinkActive,
    FormsModule,
    NgIf,
    ModalComponent
  ],
  styleUrls: ['./publishers.component.css']
})
export class PublishersComponent implements OnInit {
  publishers: PublisherGetDto[] = [];
  editingPublisher: Map<number, boolean> = new Map();
  newPublisher: PublisherPutDto = new PublisherPutDto();

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(private clientService: Client, protected authSerivce: AuthService) {}

  ngOnInit(): void {
    this.clientService.publishersAll().subscribe(data => {
      this.publishers = data;
    });
  }

  onSavePublisher(id: number | undefined): void {
    if (id !== undefined) {
      const publisher = this.publishers.find(pub => pub.id === id);
      if (publisher) {
        // Validate fields
        if (!publisher.name.trim() || !publisher.description.trim()) {
          this.modal.show('Both fields must be filled!', false);
          return;
        }

        // Prepare data for PUT request
        const updatedPublisher = new  PublisherPutDto;
        updatedPublisher.name = publisher.name;
        updatedPublisher. description = publisher.description;

        // Call the PUT method
        this.clientService.publishersPUT(id, updatedPublisher).subscribe({
          next: () => {
            this.editingPublisher.set(id, false);
            this.modal.show('Publisher updated successfully!', false);
          },
          error: (err) => {
            console.error('Error updating publisher:', err);
            this.modal.show('Failed to update the publisher. Please try again.', false);
          }
        });
      }
    }
  }

  onCancelEditPublisher(id: number | undefined): void {
    if (id !== undefined) {
      const originalPublisher = this.publishers.find(pub => pub.id === id);
      if (originalPublisher) {
        // Reset to original data
        this.clientService.publishersGET(id).subscribe({
          next: (data) => {
            originalPublisher.name = data.name;
            originalPublisher.description = data.description;
            this.editingPublisher.set(id, false);
          },
          error: (err) => {
            console.error('Error fetching original publisher data:', err);
            this.modal.show('Failed to reset the publisher data. Please try again.', false);
          }
        });
      }
    }
  }

  isEditingPublisher(id: number | undefined): boolean {
    return id !== undefined && this.editingPublisher.get(id) === true;
  }

  onEditPublisher(id: number | undefined): void {
    if (id !== undefined) {
      this.editingPublisher.set(id, true);
    }
  }

  onDeletePublisher(id: number | undefined): void {
    if (id !== undefined) {
      this.clientService.publishersDELETE(id).subscribe({
        next: () => {
          this.publishers = this.publishers.filter(pub => pub.id !== id);
          this.modal.show('Publisher deleted successfully!', false);
        },
        error: (err) => {
          console.error('Error deleting publisher:', err);
          this.modal.show('Failed to delete the publisher. Please try again.', false);
        }
      });
    }
  }

  onAddPublisher() {
    if (this.newPublisher.name == undefined || this.newPublisher.description == undefined) {
      this.modal.show('Both fields must be filled!', false);

    }
    if (!this.newPublisher.name.trim() || !this.newPublisher.description.trim()) {
      this.modal.show('Both fields must be filled!', false);
      return;
    }

    const publisherData = new PublisherPostDto();
    publisherData.name = this.newPublisher.name;
    publisherData.description = this.newPublisher.description;

    this.clientService.publishersPOST(publisherData).subscribe({
      next: (newPublisher) => {
        console.log("success")
        this.publishers.push(newPublisher);
        this.newPublisher = new PublisherPutDto();
        this.modal.show('Publisher added successfully!', false);
        window.location.reload();
        },
      error: (err) => {

        console.error('Error adding publisher:', err);
        this.modal.show('Failed to add the publisher. Please try again.' + err, false);
      }
    });
  }
}
