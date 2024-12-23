import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {GamePostDto} from '../../services/client.service';
import {GamesComponent} from '../games/games.component';
import {NgIf} from '@angular/common';
import {Route, Router} from '@angular/router';
import {ModalComponent} from '../modal/modal.component';

@Component({
  selector: 'app-game-create',
  imports: [
    FormsModule,
    NgIf,
    ModalComponent
  ],
  templateUrl: './game-create.component.html',
  styleUrl: './game-create.component.css'
})
export class GameCreateComponent {
  isEdit: boolean = false;
  newGame: GamePostDto = new GamePostDto();
  isVisible: boolean = true;
  constructor(    private router: Router,
  ) {
    this.isEdit = false;

  }

  onSubmit() {

  }

  onCancel() {

  }

  onCreateGame() {
    this.router.navigate(['publishers']);
  }

  onCloseModal() {

  }
}
