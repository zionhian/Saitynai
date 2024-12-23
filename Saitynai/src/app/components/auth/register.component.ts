import { Component } from '@angular/core';
import {Client, RegisterUserDto} from '../../services/client.service';
import { HttpErrorResponse } from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [
    FormsModule,
    NgIf
  ]
})
export class RegisterComponent {
  username: string = '';
  email: string = '';
  password: string = '';
  errorMessage: string = ''; // For error handling

  constructor(private client: Client) {}

  onSubmit(): void {
    // Create the DTO object
    const registerDto = new RegisterUserDto();
    registerDto.userName = this.username;
    registerDto.email = this.email;
    registerDto.password = this.password;

    // Call the Client service's register method
    this.client.register(registerDto).subscribe({
      next: () => {
        console.log('Registration successful!');
        // Redirect or notify user
      },
      error: (err: HttpErrorResponse) => {
        console.error('Registration failed:', err);
        this.errorMessage = 'Registration failed. Please try again.';
      },
    });
  }
}
