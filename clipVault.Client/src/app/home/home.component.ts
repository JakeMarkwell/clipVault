import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../api.service';
import { ThumbnailResponse } from '../models/Thumbnail';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  thumbnails: ThumbnailResponse[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadThumbnails();
  }

  loadThumbnails(): void {
    this.isLoading = true;
    this.error = null;
    this.apiService.getAllThumbnails(16).subscribe({
      next: (thumbnails) => {
        this.thumbnails = thumbnails;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading thumbnails', err);
        this.error = 'Failed to load thumbnails. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  navigateToVideo(thumbnailId?: string): void {
    // Navigate to individual video view (to be implemented)
    this.router.navigate(['/video', thumbnailId]);
  }
}
