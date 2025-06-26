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
}