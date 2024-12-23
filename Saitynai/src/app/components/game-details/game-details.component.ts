import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  Client,
  GameGetDto,
  ReviewGetDto,
  ReviewPostDto,
  ReviewPutDto
} from '../../services/client.service';
import {FormsModule} from '@angular/forms';
import {NgForOf, NgIf} from '@angular/common';
import {AuthService} from '../../services/auth.service';
import {routes} from '../../app.routes';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  imports: [
    FormsModule,
    NgIf,
    NgForOf
  ],
  styleUrls: ['./game-details.component.css']
})
export class GameDetailsComponent implements OnInit {
  game: GameGetDto = new GameGetDto();
  comments: ReviewGetDto[] = [];
  userComment: ReviewPostDto = new ReviewPostDto();
  currentUserId: string = '';
  existingCommentId: number | null = null;

  constructor(private client: Client, private route: ActivatedRoute, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    const gameId = +this.route.snapshot.paramMap.get('gameId')!;
    const publisherId = +this.route.snapshot.paramMap.get('publisherId')!;

    // Fetch game details
    this.client.gamesGET(publisherId, gameId).subscribe({
      next: (data) => {
        this.game = data;
      },
      error: (err) => {
        console.error('Failed to fetch game details:', err);
      }
    });

    // Fetch reviews
    this.client.reviewsAll(publisherId, gameId).subscribe({
      next: (data) => {
        this.comments = data;
        // Check if the user already has a review
        const existingComment = this.comments.find(c => c.myRating);
        if (existingComment) {
          this.existingCommentId = existingComment.id!;
          this.userComment.comment = existingComment.comment;
          this.userComment.rating = existingComment.rating!;
        }
      },
      error: (err) => {
        console.error('Failed to fetch reviews:', err);
      }
    });

    // Decode current user ID from JWT
    const jwtToken = localStorage.getItem('access_token');
    if (jwtToken) {
      const decodedToken = JSON.parse(atob(jwtToken.split('.')[1]));
      this.currentUserId = decodedToken['sub'];
    }
  }

  onSubmitComment(): void {
    if (!this.authService.isLoggedin())
    {
      this.router.navigate(['login']);
    }
    if (!this.userComment.comment!.trim() || !this.userComment.rating) {
      console.error('Both comment and rating are required.');
      return;
    }

    const publisherId = +this.route.snapshot.paramMap.get('publisherId')!;
    const gameId = +this.route.snapshot.paramMap.get('gameId')!;

    if (this.existingCommentId) {
      // Update existing comment
      const updatedComment = new ReviewPutDto();
      updatedComment.comment = this.userComment.comment;
      updatedComment.rating = this.userComment.rating;

      this.client.reviewsPUT(publisherId, gameId, this.existingCommentId, updatedComment).subscribe({
        next: () => {
          console.log('Review updated successfully.');
          window.location.reload();
        },
        error: (err) => {
          console.error('Failed to update review:', err);
        }
      });
    } else {
      // Create new comment
      this.client.reviewsPOST(publisherId, gameId, this.userComment).subscribe({
        next: () => {
          console.log('Review submitted successfully.');
          window.location.reload();
        },
        error: (err) => {
          console.error('Failed to submit review:', err);
        }
      });
    }
  }

  onEditComment(comment: ReviewGetDto): void {
    this.userComment.comment = comment.comment;
    this.userComment.rating = comment.rating!;
    this.existingCommentId = comment.id!;
  }

  onDeleteComment(commentId: number): void {
    const publisherId = +this.route.snapshot.paramMap.get('publisherId')!;
    const gameId = +this.route.snapshot.paramMap.get('gameId')!;

    this.client.reviewsDELETE(publisherId, gameId, commentId).subscribe({
      next: () => {
        console.log('Review deleted successfully.');
        this.comments = this.comments.filter(c => c.id !== commentId);
        this.userComment = new ReviewPostDto(); // Reset form
        this.existingCommentId = null;
      },
      error: (err) => {
        console.error('Failed to delete review:', err);
      }
    });
  }
}
