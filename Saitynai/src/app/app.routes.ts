import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login.component';
import { RegisterComponent } from './components/auth/register.component';
import { PublishersComponent } from './components/publishers/publishers.component';
import { GamesComponent } from './components/games/games.component';
import { GameDetailsComponent } from './components/game-details/game-details.component';
import { AuthGuard } from './services/auth-guard.service';
import {GameCreateComponent} from './components/game-create/game-create.component';


export const routes: Routes = [
  { path: '', redirectTo: '/publishers', pathMatch: 'full' }, // Default route

  // List of publishers
  { path: 'publishers', component: PublishersComponent },

  // List of games for a specific publisher
  { path: 'publishers/:publisherId/games', component: GamesComponent },

  // Details of a specific game
  { path: 'publishers/:publisherId/games/:gameId', component: GameDetailsComponent },

  // Authentication routes
  { path: 'login', component: LoginComponent, canActivate: [AuthGuard], data: { allowWhenLoggedOut: true } },
  { path: 'register', component: RegisterComponent, canActivate: [AuthGuard], data: { allowWhenLoggedOut: true } },
  {path: 'createGame', component: GameCreateComponent, canActivate: [AuthGuard], data: { createGame: true}},

  // Fallback route
  { path: '**', redirectTo: '/publishers' },
];
