import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../api.service';

@Component({
  selector: 'app-add-video-category-tool',
  templateUrl: './add-video-category-tool.component.html',
  styleUrls: ['./add-video-category-tool.component.scss'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatButtonModule,
    FormsModule
  ],
})
export class AddVideoCategoryToolComponent {
  categoryName: string = '';
  categoryRating: number | null = null;
  categoryImageId: string = '';
  
  addCategoryLoading: boolean = false;
  addCategoryError: string | null = null;
  addCategorySuccess: string | null = null;

  constructor(private apiService: ApiService) {}

  addVideoCategory(): void {
    if (!this.categoryName.trim()) {
      this.addCategoryError = 'Please enter a category name.';
      return;
    }
    if (this.categoryRating === null || this.categoryRating < 0) {
      this.addCategoryError = 'Please enter a valid rating (0 or higher).';
      return;
    }

    this.addCategoryLoading = true;
    this.addCategoryError = null;
    this.addCategorySuccess = null;

    const categoryData = {
      categoryName: this.categoryName.trim(),
      rating: this.categoryRating,
      imageId: this.categoryImageId.trim() || null
    };

    this.apiService.addVideoCategory(categoryData).subscribe({
      next: (response) => {
        this.addCategoryLoading = false;
        if (response.success) {
          this.addCategorySuccess = `Category "${this.categoryName}" added successfully with ID: ${response.categoryId}`;
          this.resetForm();
        } else {
          this.addCategoryError = response.message || 'Failed to add video category.';
        }
      },
      error: (error) => {
        this.addCategoryError = error.error?.message || 'Failed to add video category.';
        this.addCategoryLoading = false;
      }
    });
  }

  private resetForm(): void {
    this.categoryName = '';
    this.categoryRating = null;
    this.categoryImageId = '';
  }
}