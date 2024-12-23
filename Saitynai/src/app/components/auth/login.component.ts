
import {Component, ViewChild} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {Client, LoginDto} from '../../services/client.service';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {ModalComponent} from '../modal/modal.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  imports: [
    FormsModule,
    ModalComponent
  ],
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  @ViewChild(ModalComponent) modal!: ModalComponent;
  constructor(private authService: AuthService, private client: Client, private router: Router ) {}
  onSubmit() {
    let loginDto = new LoginDto();
    loginDto.userName = this.username;
    loginDto.password = this.password;
    this.client.login(loginDto).subscribe({
      next: (successLogin) => {
        if (successLogin.accessToken)
        {
          this.authService.setLoginJwt(successLogin.accessToken);
          this.router.navigate(['/publishers']);
        }

      },
      error: err =>
      {
        this.modal.show('Wrong login info', false);
        console.log(err);
      }
    })

  }
}
