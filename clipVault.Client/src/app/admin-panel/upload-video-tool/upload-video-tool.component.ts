import { Component, OnInit } from '@angular/core';
import { VideoCategory } from '../../models/video.model';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../api.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-upload-video-tool',
  templateUrl: './upload-video-tool.component.html',
  styleUrls: ['./upload-video-tool.component.css'],
  standalone: true,
  imports: [MatSelectModule, MatInputModule, MatProgressBarModule, MatFormFieldModule, FormsModule, MatIconModule],
})
export class UploadVideoToolComponent implements OnInit {
  uploadTitle: string = '';
  uploadFriendTagsInput: string = '';
  uploadCategoryIdsInput: string = '';
  uploadSelectedCategoryId: number | null = null;
  uploadCategories: VideoCategory[] = [];
  selectedFile: File | null = null;
  uploadVideoLoading: boolean = false;
  uploadVideoError: string | null = null;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadUploadCategories();
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
        next: () => {
          this.uploadVideoLoading = false;
          this.selectedFile = null;
          this.uploadTitle = '';
          this.uploadFriendTagsInput = '';
          this.uploadCategoryIdsInput = '';
          this.uploadSelectedCategoryId = null;
        },
        error: () => {
          this.uploadVideoError = 'Failed to upload video.';
          this.uploadVideoLoading = false;
        }
      });
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
}
