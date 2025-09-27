import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { ApiService } from '../api.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-video-player-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatProgressSpinnerModule],
  template: `
    <div class="video-dialog-container">
      <ng-container *ngIf="isLoading">
        <mat-spinner></mat-spinner>
      </ng-container>
      <ng-container *ngIf="!isLoading && video">
        <video controls autoplay [src]="video.videoUrl" style="width:65vw;max-height:75vh; min-height: 40vh;"></video>
      </ng-container>
      <div *ngIf="error" class="error-message">{{ error }}</div>
    </div>
  `,
  styles: [`
    .video-dialog-container {
      min-width: 400px;
      max-width: 90vw;
      min-height: 40vh;
      background: transparent;
      box-shadow: 0 2px 16px rgba(0,0,0,0.4);
      margin: auto;
      display: flex;
      flex-direction: column;
      align-items: center;
      overflow: auto;
    }
    .video-player {
      background: #181a1b;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.2);
      border: none;
      display: block;
    }
    .error-message {
      color: #ff5252;
      margin-top: 16px;
    }
  `]
})
export class VideoPlayerDialogComponent {
  video: any = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private apiService: ApiService,
    private dialogRef: MatDialogRef<VideoPlayerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { videoId: string }
  ) {
    this.apiService.getVideo(data.videoId).subscribe({
      next: (video) => {
        this.video = video;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load video.';
        this.isLoading = false;
      }
    });
  }

  close() {
    this.dialogRef.close();
  }
}