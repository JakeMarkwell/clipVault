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
    FormsModule],
  templateUrl: './api-test.component.html',
  styleUrl: './api-test.component.css',
})
export class ApiTestComponent implements OnInit {
  title: string = '';
  imageDataUrl: string = '';
  friendTags: string = '';
  categoryTags: string = '';

  getThumbnailLoading: boolean = true;
  getThumbnailError: string | null = null;
  thumbnailId: string = '9ee9051d-738e-4fb1-9619-49cc774c1f5e';

  uploadVideoLoading: boolean = false; 
  uploadVideoError: string | null = null;
  selectedFile: File | null = null;
  uploadTitle: string = '';
  uploadFriendTagsInput: string = ''; 
  uploadCategoryTagsInput: string = ''; 
  videoUploadProgress: number = 0; 

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
        this.categoryTags = response.categoryTags ?? '';
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
    this.uploadVideoLoading = true;
    this.uploadVideoError = null;
    const formData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);
    formData.append('title', this.uploadTitle);

    formData.append('friendTags', this.uploadFriendTagsInput); 
    formData.append('categoryTags', this.uploadCategoryTagsInput); 



    this.apiService.uploadVideo(formData)
      .subscribe({
        next: (response) => {
          console.log('Upload successful', response);
          this.uploadVideoLoading = false;
          this.selectedFile = null;
          this.uploadTitle = '';
          this.uploadFriendTagsInput = '';
          this.uploadCategoryTagsInput = '';
        },
        error: (error) => {
          console.error('Upload error', error);
          this.uploadVideoError = 'Failed to upload video.';
          this.uploadVideoLoading = false;
        }
      });
  }
}
