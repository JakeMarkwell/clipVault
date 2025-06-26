import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';
import { development } from '../environments/development';
import { ThumbnailResponse } from './models/Thumbnail'; // Keep this import

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private api = development.apiUrl;

  constructor(private http: HttpClient) {}
  
  // New method to get multiple thumbnails (array)
  getThumbnails(thumbnailId: string): Observable<ThumbnailResponse> {
      return this.http.get<ThumbnailResponse>(`${this.api}/thumbnail/${thumbnailId}`)
  }

}
