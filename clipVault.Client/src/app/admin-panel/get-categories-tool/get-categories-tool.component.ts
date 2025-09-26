import { Component, OnInit } from '@angular/core';
import { VideoCategory } from '../../models/video.model';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { ApiService } from '../../api.service';

@Component({
  selector: 'app-get-categories-tool',
  templateUrl: './get-categories-tool.component.html',
  styleUrls: ['./get-categories-tool.component.scss'],
  imports: [MatProgressSpinnerModule, MatTableModule],
  standalone: true
})
export class GetCategoriesToolComponent implements OnInit {
  videoCategories: VideoCategory[] = [];
  getCategoriesLoading: boolean = false;
  getCategoriesError: string | null = null;
  tableColumns: string[] = ['id', 'categoryName', 'rating', 'imageId'];

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.getVideoCategories();
  }

  getVideoCategories(): void {
    this.getCategoriesLoading = true;
    this.getCategoriesError = null;
    this.apiService.getVideoCategories().subscribe({
      next: (res) => {
        this.videoCategories = res.sort((a, b) => a.id - b.id);
        this.getCategoriesLoading = false;
      },
      error: () => {
        this.getCategoriesError = 'Failed to load categories';
        this.getCategoriesLoading = false;
      }
    });
  }
}
