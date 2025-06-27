import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../api.service';
import { ThumbnailResponse } from '../models/Thumbnail';
import { Router } from '@angular/router';
import { HomeVideoCardComponent } from '../home-video-card/home-video-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    HomeVideoCardComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  thumbnails: ThumbnailResponse[] = [];
  isLoading = true;
  isLoadingFadingOut = false;
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
    this.isLoadingFadingOut = false;
    this.error = null;
    this.apiService.getAllThumbnails(16).subscribe({
      next: (thumbnails) => {
        this.thumbnails = thumbnails;
        
        // Start the fade out transition
        this.isLoadingFadingOut = true;
        
        // Remove loading container after animation completes
        setTimeout(() => {
          this.isLoading = false;
        }, 600); // Match this with the CSS transition timing
      },
      error: (err) => {
        console.error('Error loading thumbnails', err);
        this.error = 'Failed to load thumbnails. Please try again later.';
        
        // Start the fade out transition on error too
        this.isLoadingFadingOut = true;
        
        // Remove loading container after animation completes
        setTimeout(() => {
          this.isLoading = false;
        }, 600);
      }
    });
  }

  navigateToVideo(thumbnailId?: string): void {
    // Navigate to individual video view (to be implemented)
    this.router.navigate(['/video/', thumbnailId]);
  }
}