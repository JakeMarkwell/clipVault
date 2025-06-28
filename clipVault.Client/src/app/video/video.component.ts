import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../api.service';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-video',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {
  videoId: string = '';
  video: any = null;
  isLoading: boolean = true;
  error: string | null = null;
  thumbnail: any = null;

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.videoId = params['id'];
      if (this.videoId) {
        this.loadVideo();
        this.loadThumbnail(); // Also load the thumbnail for metadata
      } else {
        this.error = 'No video ID provided';
        this.isLoading = false;
      }
    });
  }

  loadVideo(): void {
    this.isLoading = true;
    this.error = null;

    this.apiService.getVideo(this.videoId).subscribe({
      next: (data) => {
        this.video = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading video', err);
        this.error = 'Failed to load video. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  // Load thumbnail to get metadata (optional, but useful for tags, etc.)
  loadThumbnail(): void {
    this.apiService.getThumbnails(this.videoId).subscribe({
      next: (data) => {
        this.thumbnail = data;
      },
      error: (err) => {
        console.error('Error loading thumbnail metadata', err);
      }
    });
  }

  formatTags(tags: string | null): string[] {
  if (!tags) return [];
  // Remove all double quotes before splitting
  return tags.replace(/"/g, '').split(',').map(tag => tag.trim()).filter(tag => tag.length > 0);
}
}