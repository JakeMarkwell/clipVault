import { Component, OnInit } from '@angular/core';
import { AzureBlobService } from '../../services/azure-blob.service';
import { VideoCategory } from '../../models/video.model';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../api.service';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { GenerateThumbnailService} from '../../services/generate-thumbnail.service'

@Component({
  selector: 'app-upload-video-tool',
  templateUrl: './upload-video-tool.component.html',
  styleUrls: ['./upload-video-tool.component.scss'],
  standalone: true,
  imports: [
    MatAutocompleteModule,
    MatSelectModule,
    MatInputModule,
    MatProgressBarModule,
    MatFormFieldModule,
    FormsModule,
    MatIconModule
  ],
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
  categoryInput: string = '';
  filteredCategories: VideoCategory[] = [];

  constructor(
    private apiService: ApiService,
    private azureBlobService: AzureBlobService,
    private generateThumbnailService: GenerateThumbnailService
  ) {}
  async uploadVideoDirectToAzure(): Promise<void> {
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

    try {
      const sasResult: any = await this.apiService
        .getSasToken(this.selectedFile.name)
        .toPromise();
      const sasUrl = sasResult.sasUrl;

      await this.azureBlobService.uploadFileToBlob(sasUrl, this.selectedFile, {
        title: this.uploadTitle,
        friendTags: this.uploadFriendTagsInput,
        categoryIds: String(this.uploadSelectedCategoryId)
      });

      await this.generateThumbnailService.generateAndUploadThumbnail(
        this.selectedFile,
        this.uploadTitle,
        this.uploadFriendTagsInput,
        [this.uploadSelectedCategoryId],
        this.apiService,
        this.azureBlobService
      );

      this.uploadVideoLoading = false;
      this.selectedFile = null;
      this.uploadTitle = '';
      this.uploadFriendTagsInput = '';
      this.uploadSelectedCategoryId = null;
      this.categoryInput = '';
    } catch (err: any) {
      this.uploadVideoError = err.message || 'Failed to upload video.';
      this.uploadVideoLoading = false;
    }
  }

  ngOnInit(): void {
    this.loadUploadCategories();
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
    this.uploadVideoError = null;
    this.uploadVideoLoading = false;
  }

  loadUploadCategories(): void {
    this.apiService.getVideoCategories().subscribe({
      next: (categories) => {
        this.uploadCategories = categories;
        this.filteredCategories = categories;
      },
      error: () => {
        this.uploadCategories = [];
        this.filteredCategories = [];
      }
    });
  }

  filterCategories(): void {
    const filterValue = this.categoryInput?.toLowerCase() || '';
    this.filteredCategories = this.uploadCategories.filter(cat =>
      cat.categoryName.toLowerCase().includes(filterValue)
    );
  }

  onCategorySelected(selectedName: string): void {
    const selected = this.uploadCategories.find(cat => cat.categoryName === selectedName);
    this.uploadSelectedCategoryId = selected ? selected.id : null;
    this.categoryInput = selected ? selected.categoryName : '';
  }

  // uploadVideo(): void {
  //   if (!this.selectedFile) {
  //     this.uploadVideoError = 'Please select a video file.';
  //     return;
  //   }
  //   if (!this.uploadSelectedCategoryId) {
  //     this.uploadVideoError = 'Please select a category.';
  //     return;
  //   }
  //   this.uploadVideoLoading = true;
  //   this.uploadVideoError = null;
  //   const formData = new FormData();
  //   formData.append('file', this.selectedFile, this.selectedFile.name);
  //   formData.append('title', this.uploadTitle);
  //   formData.append('friendTags', this.uploadFriendTagsInput);
  //   formData.append('categoryIds', this.uploadCategoryIdsInput);
  //   formData.append('categoryId', this.uploadSelectedCategoryId.toString());

  //   this.apiService.uploadVideo(formData)
  //     .subscribe({
  //       next: () => {
  //         this.uploadVideoLoading = false;
  //         this.selectedFile = null;
  //         this.uploadTitle = '';
  //         this.uploadFriendTagsInput = '';
  //         this.uploadCategoryIdsInput = '';
  //         this.uploadSelectedCategoryId = null;
  //         this.categoryInput = '';
  //       },
  //       error: () => {
  //         this.uploadVideoError = 'Failed to upload video.';
  //         this.uploadVideoLoading = false;
  //       }
  //     });
  // }
}
