import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../api.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-api-test',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './api-test.component.html',
  styleUrl: './api-test.component.css',
})
export class ApiTestComponent implements OnInit {
  title: string = '';
  imageDataUrl: string = '';
  loading: boolean = true;
  error: string | null = null;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.getThumbnail('9ee9051d-738e-4fb1-9619-49cc774c1f5e');
  }

  getThumbnail(thumbnailId: string): void {
    this.apiService.getThumbnails(thumbnailId).subscribe(
      (response) => { console.log(response)})
    }

}
