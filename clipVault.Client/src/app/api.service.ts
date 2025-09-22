import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { development } from '../environments/development';
import { VideoCategory } from './api-test/api-test.component'; 
import { GetThumbnailResponse } from './models/get-thumbnail-response.model';
import { UploadVideoResponse } from './models/upload-video-response.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private api = development.apiUrl;

  constructor(private http: HttpClient) {}

  getThumbnails(thumbnailId: string): Observable<GetThumbnailResponse> {
    return this.http.get<GetThumbnailResponse>(`${this.api}/thumbnail/${thumbnailId}`).pipe(
      map(response => {
        // Convert base64 to data URL for <img> src
        const imageUrl = `data:${response.fileType};base64,${response.imageData}`;
        return {
          imageData: imageUrl,
          fileType: response.fileType,
          title: response.title,
          friendTags: response.friendTags,
          categoryIds: response.categoryIds
        };
      })
    );
  }
  
  //Strongly type this better in future
  getAllThumbnails(limit: number = 16): Observable<GetThumbnailResponse[]> {
  return this.http.get<any>(`${this.api}/thumbnails?limit=${limit}`).pipe(
    map(response => {
      const items = Array.isArray(response) ? response : response.thumbnails;
      return items.map((item: GetThumbnailResponse) => {
        const imageUrl = `data:${item.fileType};base64,${item.imageData}`;
        return {
          id: item.id,
          imageData: imageUrl,
          fileType: item.fileType,
          title: item.title,
          friendTags: item.friendTags,
          categoryIds: item.categoryIds
        };
      });
    })
  );
}


  uploadVideo(formData: FormData): Observable<UploadVideoResponse> {
    return this.http.post<UploadVideoResponse>(`${this.api}/videos/upload`, formData);
  } 

  getVideo(id: string) {
    return this.http.get<any>(`${this.api}/video/${id}`);
  }

  getVideoCategories(): Observable<VideoCategory[]> {
    return this.http.get<any>(`${this.api}/video-categories`).pipe(
      map(response => response.categories as VideoCategory[])
    );
  }
}
