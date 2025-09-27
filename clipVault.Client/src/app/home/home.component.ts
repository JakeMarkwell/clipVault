import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { ApiService } from '../api.service';
import { Router } from '@angular/router';
import { HomeVideoCardComponent } from '../home-video-card/home-video-card.component';
import { GetThumbnailResponse } from '../models/get-thumbnail-response.model';
import { VideoCategory } from '../models/video.model';
import { VideoPlayerDialogComponent } from '../video-player-dialog/video-player-dialog.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatButtonModule,
    HomeVideoCardComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {
  thumbnails: GetThumbnailResponse[] = [];
  filteredThumbnails: GetThumbnailResponse[] = [];
  categories: VideoCategory[] = [];
  selectedCategoryId: number | null = null;
  isLoading = true;
  isLoadingFadingOut = false;
  error: string | null = null;
  
  @ViewChild('logoHeader') logoHeader!: ElementRef;
  
  constructor(
    private apiService: ApiService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadThumbnails();
    this.loadCategories();
  }
  
  ngAfterViewInit(): void {
    this.onScroll();
  }
  
  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (!this.logoHeader) return;
    
    const scrollPosition = window.scrollY;
    const fadeStart = 50;
    const fadeEnd = 200;
    const opacity = 1 - Math.min(Math.max((scrollPosition - fadeStart) / (fadeEnd - fadeStart), 0), 1);
    
    this.logoHeader.nativeElement.style.opacity = opacity.toString();
  }

  loadThumbnails(): void {
    this.isLoading = true;
    this.isLoadingFadingOut = false;
    this.error = null;
    this.apiService.getAllThumbnails(99).subscribe({
      next: (thumbnails) => {
        this.thumbnails = thumbnails
        this.applyFilter();
        
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

  loadCategories(): void {
    this.apiService.getVideoCategories().subscribe({
      next: (categories) => {
        this.categories = categories.sort((a, b) => a.categoryName.localeCompare(b.categoryName));
      },
      error: (err) => {
        console.error('Error loading categories', err);
      }
    });
  }

  selectCategory(categoryId: number | null): void {
    this.selectedCategoryId = categoryId;
    this.applyFilter();
  }

  applyFilter(): void {
    if (this.selectedCategoryId === null) {
      this.filteredThumbnails = this.thumbnails;
    } else {
      this.filteredThumbnails = this.thumbnails.filter(thumbnail => 
        thumbnail.categoryIds && thumbnail.categoryIds.includes(this.selectedCategoryId!)
      );
    }
    
    this.filteredThumbnails.sort((a, b) => {
      const aLowestRating = this.getLowestCategoryRating(a.categoryIds);
      const bLowestRating = this.getLowestCategoryRating(b.categoryIds);
      
      if (aLowestRating !== bLowestRating) {
        return aLowestRating - bLowestRating;
      }
      
      return (a.title || '').localeCompare(b.title || '', undefined, { sensitivity: 'base' });
    });
  }

  private getLowestCategoryRating(categoryIds?: number[]): number {
    if (!categoryIds || categoryIds.length === 0) {
      return Number.MAX_SAFE_INTEGER; 
    }
    
    const ratings = categoryIds
      .map(id => this.categories.find(cat => cat.id === id)?.rating)
      .filter(rating => rating !== undefined) as number[];
      
    return ratings.length > 0 ? Math.min(...ratings) : Number.MAX_SAFE_INTEGER;
  }

  navigateToVideo(thumbnailId?: string): void {
    this.router.navigate(['/video/', thumbnailId]);
  }

  openVideoPlayer(thumbnailId?: string): void {
    if (!thumbnailId) return;
    this.dialog.open(VideoPlayerDialogComponent, {
      data: { videoId: thumbnailId },
      width: '65vw',
      maxWidth: '75vw'
    });
  }
}