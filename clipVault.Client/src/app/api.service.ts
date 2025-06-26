// src/app/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { development } from '../environments/development';
import { ThumbnailResponse } from './models/Thumbnail';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private api = development.apiUrl;

  constructor(private http: HttpClient) {}
  
  getThumbnails(thumbnailId: string): Observable<ThumbnailResponse> {
    return this.http.get(`${this.api}/thumbnail/${thumbnailId}`, { 
      responseType: 'blob' 
    }).pipe(
      map(blob => {
        // Create URL for the blob
        const imageUrl = URL.createObjectURL(blob);
        
        // Return as ThumbnailResponse
        return {
          imageData: imageUrl,
          fileType: blob.type,
          title: `Thumbnail ${thumbnailId}`
        };
      })
    );
  }
}
