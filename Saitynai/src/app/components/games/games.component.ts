import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Client, GameGetDto, GamePostDto, GamePutDto } from '../../services/client.service';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { NgForOf, NgIf } from '@angular/common';
import { ModalComponent } from '../modal/modal.component';

@Component({
  selector: 'app-games',
  templateUrl: './games.component.html',
  imports: [
    FormsModule,
    NgIf,
    RouterLink,
    NgForOf,
    ModalComponent
  ],
  styleUrls: ['./games.component.css']
})
export class GamesComponent implements OnInit {
  games: GameGetDto[] = [];
  publisherId!: number;
  currentUserId!: string;
  editingGameId: number | null = null;
  newGame: GamePostDto = new GamePostDto();

  @ViewChild(ModalComponent) modal!: ModalComponent;

  constructor(
    private route: ActivatedRoute,
    private client: Client,
    protected authService: AuthService
  ) {}

  ngOnInit(): void {
    this.publisherId = +this.route.snapshot.paramMap.get('publisherId')!;
    this.client.gamesAll(this.publisherId).subscribe(data => {
      this.games = data;
    });

    const jwtToken = localStorage.getItem('access_token');
    if (jwtToken) {
      const decodedToken = JSON.parse(atob(jwtToken.split('.')[1]));
      this.currentUserId = decodedToken['sub'];
    }
  }

  myGames(): boolean {
    return this.games.length > 0 && this.games[0].publisherUserId === this.currentUserId;
  }

  ownsGame(game: GameGetDto): boolean {
    return game.userOwnsGame!;
  }

  isPublisher(): boolean {
    return this.games.length > 0 && this.games[0].publisherUserId === this.currentUserId;
  }

  onAddGame(): void {
    if (!this.newGame.title.trim() || !this.newGame.description.trim()) {
      this.modal.show('Title and description are required.', false);
      return;
    }

    const gameData = new GamePostDto();
    gameData.title = this.newGame.title;
    gameData.description = this.newGame.description;

    this.client.gamesPOST(this.publisherId, gameData).subscribe({
      next: (createdGame) => {
        this.games.push(createdGame);
        this.newGame = new GamePostDto();
        this.modal.show('Game created successfully!', false);
        window.location.reload();
      },
      error: (err) => {
        console.error('Error creating game:', err);
        this.modal.show('Failed to create the game. Please try again.', false);
      }
    });
  }

  onEditGame(id: number): void {
    this.editingGameId = id;
  }

  onCancelEditGame(): void {
    this.editingGameId = null;
    window.location.reload();
  }

  onSaveGame(id: number): void {
    const game = this.games.find(g => g.id === id);
    if (game) {
      if (!game.title.trim() || !game.description.trim()) {
        this.modal.show('Title and description are required.', false);
        return;
      }

      const updatedGame = new GamePutDto();
      updatedGame.title = game.title;
      updatedGame.description = game.description;

      this.client.gamesPUT(this.publisherId, id, updatedGame).subscribe({
        next: () => {
          this.modal.show('Game updated successfully!', false);
          this.editingGameId = null;
          window.location.reload();
        },
        error: (err) => {
          console.error('Error updating game:', err);
          this.modal.show('Failed to update the game. Please try again.', false);
        }
      });
    }
  }

  onDeleteGame(id: number): void {
    this.client.gamesDELETE(this.publisherId, id).subscribe({
      next: () => {
        this.games = this.games.filter(game => game.id !== id);
        this.modal.show('Game deleted successfully!', false);
        window.location.reload();
      },
      error: (err) => {
        console.error(`Failed to delete game ${id}:`, err);
        this.modal.show('Failed to delete the game. Please try again.', false);
      }
    });
  }
}
