import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ApiService } from '../../api.service';

@Component({
  selector: 'app-get-thumbnail-tool',
  templateUrl: './get-thumbnail-tool.component.html',
  styleUrls: ['./get-thumbnail-tool.component.css'],
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule
  ],
})
export class GetThumbnailToolComponent implements OnInit {
  title: string = '';
  imageDataUrl: string = '';
  friendTags: string = '';
  thumbnailId: string = '9ee9051d-738e-4fb1-9619-49cc774c1f5e';
  getThumbnailLoading: boolean = false;
  getThumbnailError: string | null = null;
  categoryIds: number[] = [];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.getThumbnail(this.thumbnailId);
  }

  getThumbnail(thumbnailId: string): void {
    this.getThumbnailLoading = true;
    this.getThumbnailError = null;
    this.apiService.getThumbnails(thumbnailId).subscribe({
      next: (response) => {
        this.title = response.title;
        this.imageDataUrl = response.imageData;
        this.friendTags = response.friendTags ?? '';
        this.categoryIds = response.categoryIds ?? [];
        this.getThumbnailLoading = false;
      },
      error: () => {
        this.getThumbnailError = 'Failed to load thumbnail';
        this.getThumbnailLoading = false;
      }
    });
  }
}
