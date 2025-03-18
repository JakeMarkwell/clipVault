// src/app/api-test/api-test.component.ts
import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser'; // Import DomSanitizer and SafeUrl
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-api-test',
  templateUrl: './api-test.component.html',
  styleUrls: ['./api-test.component.css'],
  imports: [NgIf],
  standalone: true,
})
export class ApiTestComponent {
  thumbnailId: string = '5cd66812-a7ed-4821-94fc-c5ca90c6e789';
  thumbnailUrl: SafeUrl | null = null; 

  constructor(private apiService: ApiService, private sanitizer: DomSanitizer) {}

  loadThumbnails() {
    this.apiService.getThumbnails(this.thumbnailId).subscribe({
      next: (blob: Blob) => {
        const url = URL.createObjectURL(blob);
        this.thumbnailUrl = this.sanitizer.bypassSecurityTrustUrl(url);
        console.log(this.thumbnailUrl);
      },
      error: (error) => {
        console.error('There was an error!', error);
      }
    });
  }
}
