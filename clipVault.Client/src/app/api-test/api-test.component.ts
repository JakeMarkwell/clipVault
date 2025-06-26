import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';

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
    FormsModule],
  templateUrl: './api-test.component.html',
  styleUrl: './api-test.component.css',
})
export class ApiTestComponent implements OnInit {
  title: string = '';
  imageDataUrl: string = '';
  friendTags: string[] = [];
  categoryTags: string[] = [];
  loading: boolean = true;
  thumbnailId: string = '9ee9051d-738e-4fb1-9619-49cc774c1f5e';
  error: string | null = null;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.getThumbnail('9ee9051d-738e-4fb1-9619-49cc774c1f5e');
  }

  getThumbnail(thumbnailId: string): void {
    this.loading = true;
    this.error = null;
    this.apiService.getThumbnails(thumbnailId).subscribe({
      next: (response) => {
        this.title = response.title;
        this.imageDataUrl = response.imageData;
        this.friendTags = response.friendTags ?? [];
        this.categoryTags = response.categoryTags ?? [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load thumbnail';
        this.loading = false;
      }
    });
  }
}
