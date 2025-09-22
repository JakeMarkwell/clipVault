import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import {MatSelectModule} from '@angular/material/select';

export interface VideoCategory {
  id: number;
  categoryName: string;
  rating: number;
  imageId?: string;
}

@Component({
  selector: 'app-api-test',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    FormsModule,
    MatSidenavModule,
    MatTableModule,
    MatSelectModule,
  ],
  templateUrl: './api-test.component.html',
  styleUrl: './api-test.component.css',
})
export class ApiTestComponent implements OnInit {
  title: string = '';
  imageDataUrl: string = '';
  friendTags: string = '';
  categoryIds: number[] = [];

  getThumbnailLoading: boolean = true;
  getThumbnailError: string | null = null;
  thumbnailId: string = '9ee9051d-738e-4fb1-9619-49cc774c1f5e';

  uploadVideoLoading: boolean = false; 
  uploadVideoError: string | null = null;
  selectedFile: File | null = null;
  uploadTitle: string = '';
  uploadFriendTagsInput: string = ''; 
  uploadCategoryIdsInput: string = ''; 
  videoUploadProgress: number = 0; 

  selectedApi: string = 'getThumbnail';

  videoCategories: VideoCategory[] = [];
  getCategoriesLoading: boolean = false;
  getCategoriesError: string | null = null;

  uploadSelectedCategoryId: number | null = null;
  uploadCategories: VideoCategory[] = [];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.getThumbnail('9ee9051d-738e-4fb1-9619-49cc774c1f5e');
  }

  getThumbnail(thumbnailId: string): void {
    this.getThumbnailLoading = true;
    this.getThumbnailError = null;
    this.apiService.getThumbnails(thumbnailId).subscribe({
      next: (response) => {
        this.title = response.title;
        this.imageDataUrl = response.imageData;
        this.friendTags = response.friendTags ?? '';
        this.categoryIds = response.categoryIds ?? '';
        this.getThumbnailLoading = false;
      },
      error: (err) => {
        this.getThumbnailError = 'Failed to load thumbnail';
        this.getThumbnailLoading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
    this.uploadVideoError = null;
    this.uploadVideoLoading = false;
  }

  uploadVideo(): void {
    if (!this.selectedFile) {
      this.uploadVideoError = 'Please select a video file.';
      return;
    }
    if (!this.uploadSelectedCategoryId) {
      this.uploadVideoError = 'Please select a category.';
      return;
    }
    this.uploadVideoLoading = true;
    this.uploadVideoError = null;
    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);
    formData.append('title', this.uploadTitle);
    formData.append('friendTags', this.uploadFriendTagsInput);
    formData.append('categoryIds', this.uploadCategoryIdsInput);
    formData.append('categoryId', this.uploadSelectedCategoryId.toString());

    this.apiService.uploadVideo(formData)
      .subscribe({
        next: (response) => {
          this.uploadVideoLoading = false;
          this.selectedFile = null;
          this.uploadTitle = '';
          this.uploadFriendTagsInput = '';
          this.uploadCategoryIdsInput = '';
          this.uploadSelectedCategoryId = null;
        },
        error: (error) => {
          this.uploadVideoError = 'Failed to upload video.';
          this.uploadVideoLoading = false;
        }
      });
  }

  selectApi(api: string): void {
    this.selectedApi = api;
    if (api === 'getVideoCategories') {
      this.videoCategories = [];
      this.getCategoriesError = null;
      this.getCategoriesLoading = false;
    }
    if (api === 'uploadVideo') {
      this.loadUploadCategories();
    }
  }

  loadUploadCategories(): void {
    this.apiService.getVideoCategories().subscribe({
      next: (categories) => {
        this.uploadCategories = categories;
      },
      error: () => {
        this.uploadCategories = [];
      }
    });
  }
 
  getVideoCategories(): void {
    this.getCategoriesLoading = true;
    this.getCategoriesError = null;
    this.apiService.getVideoCategories().subscribe({
      next: (res) => {
        this.videoCategories = res;
        this.getCategoriesLoading = false;
      },
      error: (err) => {
        this.getCategoriesError = 'Failed to load categories';
        this.getCategoriesLoading = false;
      }
    });
  }

  
}
