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
    return this.http.get<any>(`${this.api}/thumbnail/${thumbnailId}`).pipe(
      map(response => {
        // Convert base64 to data URL for <img> src
        const imageUrl = `data:${response.fileType};base64,${response.imageData}`;
        return {
          imageData: imageUrl,
          fileType: response.fileType,
          title: response.title,
          friendTags: response.friendTags,
          categoryTags: response.categoryTags
        };
      })
    );
  }
  
  getAllThumbnails(limit: number = 16): Observable<ThumbnailResponse[]> {
    return this.http.get<any>(`${this.api}/thumbnails?limit=${limit}`).pipe(
      map(response => {
        return response.thumbnails.map((item: any) => {
          const imageUrl = `data:${item.fileType};base64,${item.imageData}`;
          return {
            id: item.id,
            imageData: imageUrl,
            fileType: item.fileType,
            title: item.title,
            friendTags: item.friendTags,
            categoryTags: item.categoryTags
          };
        });
      })
    );
  }

  uploadVideo(formData: FormData): Observable<any> {
    return this.http.post(`${this.api}/videos/upload`, formData);
  } 

  getVideo(videoId: string): Observable<any> {
    return this.http.get<any>(`${this.api}/video/${videoId}`).pipe(
      map(response => {
        return {
          id: response.id,
          title: response.title,
          videoUrl: `${this.api}/video/stream/${videoId}`,
          friendTags: response.friendTags,
          categoryTags: response.categoryTags
        };
      })
    );
  }

}
