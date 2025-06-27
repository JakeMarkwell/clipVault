import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, HostListener } from '@angular/core';
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
export class HomeComponent implements OnInit, AfterViewInit {
  thumbnails: ThumbnailResponse[] = [];
  isLoading = true;
  isLoadingFadingOut = false;
  error: string | null = null;
  
  @ViewChild('logoHeader') logoHeader!: ElementRef;
  
  constructor(
    private apiService: ApiService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadThumbnails();
  }
  
  ngAfterViewInit(): void {
    // Initial check for scroll position
    this.onScroll();
  }
  
  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (!this.logoHeader) return;
    
    const scrollPosition = window.scrollY;
    // Start fading from 50px scroll, completely transparent by 200px
    const fadeStart = 50;
    const fadeEnd = 200;
    const opacity = 1 - Math.min(Math.max((scrollPosition - fadeStart) / (fadeEnd - fadeStart), 0), 1);
    
    this.logoHeader.nativeElement.style.opacity = opacity.toString();
  }

  loadThumbnails(): void {
    this.isLoading = true;
    this.isLoadingFadingOut = false;
    this.error = null;
    this.apiService.getAllThumbnails(16).subscribe({
      next: (thumbnails) => {
        this.thumbnails = thumbnails;
        
        this.isLoadingFadingOut = true;
        
        setTimeout(() => {
          this.isLoading = false;
        }, 600); 
      },
      error: (err) => {
        console.error('Error loading thumbnails', err);
        this.error = 'Failed to load thumbnails. Please try again later.';
        
        this.isLoadingFadingOut = true;
        
        setTimeout(() => {
          this.isLoading = false;
        }, 600);
      }
    });
  }

  navigateToVideo(thumbnailId?: string): void {
    this.router.navigate(['/video/', thumbnailId]);
  }
}